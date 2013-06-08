using System;
using NSubstitute;
using NUnit.Framework;
using WonkyChip8.Interpreter.Commands;
using WonkyChip8.Interpreter.UnitTests.TestUtilities;

namespace WonkyChip8.Interpreter.UnitTests.Commands
{
    [TestFixture]
    public class SaveBinaryCodedRegisterValueInMemoryCommandFixture
    {
        private const int ValidOperationCode = 0xF033;

        private static SaveBinaryCodedRegisterValueInMemoryCommand CreateCommand(int operationCode = ValidOperationCode,
                                                                                 IGeneralRegisters generalRegisters =
                                                                                     null,
                                                                                 IAddressRegister addressRegister = null,
                                                                                 IMemory memory = null)
        {
            return new SaveBinaryCodedRegisterValueInMemoryCommand(0, operationCode,
                                                                   generalRegisters ??
                                                                   Substitute.For<IGeneralRegisters>(),
                                                                   addressRegister ?? Substitute.For<IAddressRegister>(),
                                                                   memory ?? Substitute.For<IMemory>());
        }

        [TestCase(0x99999)]
        [TestCase(0xF000)]
        [TestCase(0x0033)]
        public void Constructor_WithInvalidOperationCode_ExpectedThrowsArgumentOutOfRangeException(
            int invalidOperationCode)
        {
            NUnitUtilities.AssertThrowsArgumentExceptionWithParamName<ArgumentOutOfRangeException>(
                () => new SaveBinaryCodedRegisterValueInMemoryCommand(0, invalidOperationCode,
                                                                      Substitute.For<IGeneralRegisters>(),
                                                                      Substitute.For<IAddressRegister>(),
                                                                      Substitute.For<IMemory>()),
                "operationCode");
        }

        [Test]
        public void Constructor_WithNullGeneralRegisters_ExpectedThrowsArgumentNullException()
        {
            NUnitUtilities.AssertThrowsArgumentExceptionWithParamName<ArgumentNullException>(
                () => new SaveBinaryCodedRegisterValueInMemoryCommand(0, ValidOperationCode, null,
                                                                      Substitute.For<IAddressRegister>(),
                                                                      Substitute.For<IMemory>()),
                "generalRegisters");
        }

        [Test]
        public void Constructor_WithNullAddressRegister_ExpectedThrowsArgumentNullException()
        {
            NUnitUtilities.AssertThrowsArgumentExceptionWithParamName<ArgumentNullException>(
                () => new SaveBinaryCodedRegisterValueInMemoryCommand(0, ValidOperationCode,
                                                                      Substitute.For<IGeneralRegisters>(),
                                                                      null, Substitute.For<IMemory>()),
                "addressRegister");
        }

        [Test]
        public void Constructor_WithNullMemory_ExpectedThrowsArgumentNullException()
        {
            NUnitUtilities.AssertThrowsArgumentExceptionWithParamName<ArgumentNullException>(
                () => new SaveBinaryCodedRegisterValueInMemoryCommand(0, ValidOperationCode,
                                                                      Substitute.For<IGeneralRegisters>(),
                                                                      Substitute.For<IAddressRegister>(),
                                                                      null),
                "memory");
        }

        [TestCase(0xF033, 0x00, 0x00)]
        [TestCase(0xF033, 0x10, 0x00)]
        [TestCase(0xFA33, 0x00, 0x12)]
        [TestCase(0xFF33, 0xFF, 0x30)]
        [TestCase(0xFF33, 0xFF, 0xFFF)]
        public void Execute_ExpectedSaveBinaryCodedDecimalValueFromGeneralRegisterToMemoryAtAddressFromAddressRegister(
            int operationCode, byte registerValue, short addressRegisterValue)
        {
            // Arrange
            var generalRegistersStub = Substitute.For<IGeneralRegisters>();
            var registerIndex = (operationCode & 0x0F00) >> 8;
            generalRegistersStub[registerIndex].Returns(registerValue);

            var addressRegisterStub = Substitute.For<IAddressRegister>();
            addressRegisterStub.AddressValue.Returns(addressRegisterValue);

            var memoryStub = Substitute.For<IMemory>();
            var firstMemoryCellActualValue = 0;
            var secondMemoryCellActualValue = 0;
            var thirdMemoryCellActualValue = 0;
            memoryStub[addressRegisterValue] = Arg.Do<byte>(value => firstMemoryCellActualValue = value);
            memoryStub[addressRegisterValue + 1] = Arg.Do<byte>(value => secondMemoryCellActualValue = value);
            memoryStub[addressRegisterValue + 2] = Arg.Do<byte>(value => thirdMemoryCellActualValue = value);

            var firstMemoryCellExpectedValue = (byte) (registerValue/100);
            var secondMemoryCellExpectedValue = (byte) ((registerValue%100)/10);
            var thirdMemoryCellExpectedValue = (byte) ((registerValue%100)%10);
            var command = CreateCommand(operationCode, generalRegistersStub, addressRegisterStub, memoryStub);

            // Act
            command.Execute();

            // Assert
            Assert.AreEqual(firstMemoryCellExpectedValue, firstMemoryCellActualValue);
            Assert.AreEqual(secondMemoryCellExpectedValue, secondMemoryCellActualValue);
            Assert.AreEqual(thirdMemoryCellExpectedValue, thirdMemoryCellActualValue);
        }
    }
}