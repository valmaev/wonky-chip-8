using System;
using System.Collections.Generic;
using NSubstitute;
using NUnit.Framework;
using WonkyChip8.Interpreter.Commands;
using WonkyChip8.Interpreter.UnitTests.TestUtilities;

namespace WonkyChip8.Interpreter.UnitTests.Commands
{
    [TestFixture]
    public class SaveMemoryCellValuesInGeneralRegistersCommandFixture
    {
        private const int ValidOperationCode = 0xF065;

        private static SaveMemoryCellValuesInGeneralRegistersCommand CreateCommand(
            int operationCode = ValidOperationCode, IGeneralRegisters generalRegisters = null,
            IAddressRegister addressRegister = null, IMemory memory = null)
        {
            return new SaveMemoryCellValuesInGeneralRegistersCommand(
                0, operationCode, generalRegisters ?? Substitute.For<IGeneralRegisters>(),
                addressRegister ?? Substitute.For<IAddressRegister>(),
                memory ?? Substitute.For<IMemory>());
        }

        [TestCase(0x99999)]
        [TestCase(0xF000)]
        [TestCase(0x0065)]
        public void Constructor_WithInvalidOperationCode_ExpectedThrowsArgumentOutOfRangeException(
            int invalidOperationCode)
        {
            NUnitUtilities.AssertThrowsArgumentExceptionWithParamName<ArgumentOutOfRangeException>(
                () => new SaveMemoryCellValuesInGeneralRegistersCommand(0, invalidOperationCode,
                                                                        Substitute.For<IGeneralRegisters>(),
                                                                        Substitute.For<IAddressRegister>(),
                                                                        Substitute.For<IMemory>()),
                "operationCode");
        }

        [Test]
        public void Constructor_WithNullGeneralRegisters_ExpectedThrowsArgumentNullException()
        {
            NUnitUtilities.AssertThrowsArgumentExceptionWithParamName<ArgumentNullException>(
                () => new SaveMemoryCellValuesInGeneralRegistersCommand(0, ValidOperationCode, null,
                                                                        Substitute.For<IAddressRegister>(),
                                                                        Substitute.For<IMemory>()),
                "generalRegisters");
        }

        [Test]
        public void Constructor_WithNullAddressRegister_ExpectedThrowsArgumentNullException()
        {
            NUnitUtilities.AssertThrowsArgumentExceptionWithParamName<ArgumentNullException>(
                () => new SaveMemoryCellValuesInGeneralRegistersCommand(0, ValidOperationCode,
                                                                        Substitute.For<IGeneralRegisters>(),
                                                                        null, Substitute.For<IMemory>()),
                "addressRegister");
        }

        [Test]
        public void Constructor_WithNullMemory_ExpectedThrowsArgumentNullException()
        {
            NUnitUtilities.AssertThrowsArgumentExceptionWithParamName<ArgumentNullException>(
                () => new SaveMemoryCellValuesInGeneralRegistersCommand(0, ValidOperationCode,
                                                                        Substitute.For<IGeneralRegisters>(),
                                                                        Substitute.For<IAddressRegister>(),
                                                                        null),
                "memory");
        }

        [TestCase(0xF065, 0x00, 0x00)]
        [TestCase(0xF165, 0xFF, 0x01)]
        [TestCase(0xFA65, 0x16, 0xFF)]
        [TestCase(0xFF65, 0xFF, 0xFF)]
        public void Execute_ExpectedSavesMemoryCellValuesInGeneralRegisters(int operationCode, byte memoryCellValue,
                                                                            short initialAddressValue)
        {
            // Arrange
            var generalRegistersStub = Substitute.For<IGeneralRegisters>();
            var generalRegistersValues = new Dictionary<int, byte>();
            var lastRegisterIndex = (operationCode & 0x0F00) >> 8;

            for (int registerIndex = 0; registerIndex <= lastRegisterIndex; registerIndex++)
            {
                int index = registerIndex;
                generalRegistersStub[index] = Arg.Do<byte>(value => generalRegistersValues.Add(index, value));
            }

            var addressRegisterStub = Substitute.For<IAddressRegister>();
            var addressActualValue = initialAddressValue;
            addressRegisterStub.AddressValue = Arg.Do<short>(value => addressActualValue = value);
            addressRegisterStub.AddressValue.Returns(addressActualValue);

            var memoryStub = Substitute.For<IMemory>();
            memoryStub[Arg.Any<int>()].Returns(memoryCellValue);

            var command = CreateCommand(operationCode, generalRegistersStub, addressRegisterStub, memoryStub);

            // Act
            command.Execute();

            // Assert
            Assert.AreEqual(lastRegisterIndex + 1, generalRegistersValues.Count);
            Assert.That(generalRegistersValues.Values, Is.All.EqualTo(memoryCellValue));
            Assert.AreEqual(initialAddressValue + lastRegisterIndex + 1, addressActualValue);
        }
    }
}