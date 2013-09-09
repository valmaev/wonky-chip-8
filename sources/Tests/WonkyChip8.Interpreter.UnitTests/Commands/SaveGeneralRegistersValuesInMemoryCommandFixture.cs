using System;
using System.Collections.Generic;
using NSubstitute;
using NUnit.Framework;
using WonkyChip8.Interpreter.Commands;
using WonkyChip8.Interpreter.UnitTests.TestUtilities;

namespace WonkyChip8.Interpreter.UnitTests.Commands
{
    [TestFixture]
    public class SaveGeneralRegistersValuesInMemoryCommandFixture
    {
        private const int ValidOperationCode = 0xF055;

        private static SaveGeneralRegistersValuesInMemoryCommand CreateCommand(int operationCode = ValidOperationCode,
                                                                               IGeneralRegisters generalRegisters =
                                                                                   null,
                                                                               IAddressRegister addressRegister = null,
                                                                               IMemory memory = null)
        {
            return new SaveGeneralRegistersValuesInMemoryCommand(0, operationCode,
                                                                 generalRegisters ?? Substitute.For<IGeneralRegisters>(),
                                                                 addressRegister ?? Substitute.For<IAddressRegister>(),
                                                                 memory ?? Substitute.For<IMemory>());
        }

        [TestCase(0x99999)]
        [TestCase(0xF000)]
        [TestCase(0x0055)]
        public void Constructor_WithInvalidOperationCode_ExpectedThrowsArgumentOutOfRangeException(
            int invalidOperationCode)
        {
            NUnitUtilities.AssertThrowsArgumentExceptionWithParamName<ArgumentOutOfRangeException>(
                () => new SaveGeneralRegistersValuesInMemoryCommand(0, invalidOperationCode,
                                                                    Substitute.For<IGeneralRegisters>(),
                                                                    Substitute.For<IAddressRegister>(),
                                                                    Substitute.For<IMemory>()),
                "operationCode");
        }

        [Test]
        public void Constructor_WithNullGeneralRegisters_ExpectedThrowsArgumentNullException()
        {
            NUnitUtilities.AssertThrowsArgumentExceptionWithParamName<ArgumentNullException>(
                () => new SaveGeneralRegistersValuesInMemoryCommand(0, ValidOperationCode, null,
                                                                    Substitute.For<IAddressRegister>(),
                                                                    Substitute.For<IMemory>()),
                "generalRegisters");
        }

        [Test]
        public void Constructor_WithNullAddressRegister_ExpectedThrowsArgumentNullException()
        {
            NUnitUtilities.AssertThrowsArgumentExceptionWithParamName<ArgumentNullException>(
                () => new SaveGeneralRegistersValuesInMemoryCommand(0, ValidOperationCode,
                                                                    Substitute.For<IGeneralRegisters>(),
                                                                    null, Substitute.For<IMemory>()),
                "addressRegister");
        }

        [Test]
        public void Constructor_WithNullMemory_ExpectedThrowsArgumentNullException()
        {
            NUnitUtilities.AssertThrowsArgumentExceptionWithParamName<ArgumentNullException>(
                () => new SaveGeneralRegistersValuesInMemoryCommand(0, ValidOperationCode,
                                                                    Substitute.For<IGeneralRegisters>(),
                                                                    Substitute.For<IAddressRegister>(),
                                                                    null),
                "memory");
        }

        [TestCase(0xF055, 0x00, 0x00)]
        [TestCase(0xF155, 0x01, 0x01)]
        [TestCase(0xFA55, 0xFF, 0x12)]
        [TestCase(0xFF55, 0xAA, 0xA0)]
        public void Execute_ExpectedSaveGeneralRegistersValuesInMemory(int operationCode, byte registerValue,
                                                                       short addressValue)
        {
            // Arrange
            var generalRegistersStub = Substitute.For<IGeneralRegisters>();
            generalRegistersStub[Arg.Any<int>()].Returns(registerValue);

            var addressRegisterStub = Substitute.For<IAddressRegister>();
            addressRegisterStub.AddressValue.Returns(addressValue);

            var memoryStub = Substitute.For<IMemory>();
            var memoryCellValues = new Dictionary<int, byte>();
            var registerIndex = (operationCode & 0x0F00) >> 8;
            for (int memoryCellIndex = addressValue; memoryCellIndex <= addressValue + registerIndex; memoryCellIndex++)
            {
                int index = memoryCellIndex;
                memoryStub[memoryCellIndex] = Arg.Do<byte>(value => memoryCellValues.Add(index, value));
            }

            var command = CreateCommand(operationCode, generalRegistersStub, addressRegisterStub, memoryStub);

            // Act
            command.Execute();

            // Assert
            Assert.AreEqual(registerIndex + 1, memoryCellValues.Count);
            Assert.That(memoryCellValues.Values, Is.All.EqualTo(registerValue));
        }

        [TestCase(0xF055, 0x00)]
        [TestCase(0xF155, 0x01)]
        [TestCase(0xFA55, 0xFF)]
        [TestCase(0xFF55, 0xAA)]
        public void Execute_ExpectedIncrementAddressValueBySecondOperationHalfBytePlusOne(int operationCode,
                                                                                          short initialAddressValue)
        {
            // Arrange
            short addressValue = initialAddressValue;
            var addressRegisterStub = Substitute.For<IAddressRegister>();
            addressRegisterStub.AddressValue = Arg.Do<short>(value => addressValue = value);
            addressRegisterStub.AddressValue.Returns(addressValue);

            var command = CreateCommand(operationCode, addressRegister: addressRegisterStub);

            var expectedValue = (short) (initialAddressValue + command.SecondOperationCodeHalfByte + 1);

            // Act
            command.Execute();

            // Assert
            Assert.AreEqual(expectedValue, addressValue);
        }
    }
}