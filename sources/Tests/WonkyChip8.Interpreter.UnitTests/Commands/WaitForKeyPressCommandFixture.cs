using System;
using NSubstitute;
using NUnit.Framework;
using WonkyChip8.Interpreter.Commands;
using WonkyChip8.Interpreter.UnitTests.TestUtilities;

namespace WonkyChip8.Interpreter.UnitTests.Commands
{
    [TestFixture]
    public class WaitForKeyPressCommandFixture
    {
        private const int ValidOperationCode = 0xF00A;

        private static WaitForKeyPressCommand CreateCommand(int address = 0,
                                                            int operationCode = ValidOperationCode,
                                                            IGeneralRegisters generalRegisters = null,
                                                            IKeyboard keyboard = null)
        {
            return new WaitForKeyPressCommand(address, operationCode,
                                              generalRegisters ?? Substitute.For<IGeneralRegisters>(),
                                              keyboard ?? CreateKeyboardStub());
        }

        private static IKeyboard CreateKeyboardStub(byte keysCount = 16, byte pressedKeyIndex = 0)
        {
            var keyboardStub = Substitute.For<IKeyboard>();
            keyboardStub.KeysCount.Returns(keysCount);
            keyboardStub.IsKeyPressed(pressedKeyIndex).Returns(true);
            return keyboardStub;
        }

        [TestCase(0x99999)]
        [TestCase(0xF000)]
        [TestCase(0x000A)]
        public void Constructor_WithInvalidOperationCode_ExpectedThrowsArgumentNullException(
            int invalidOperationCode)
        {
            NUnitUtilities.AssertThrowsArgumentExceptionWithParamName<ArgumentOutOfRangeException>(
                () => new WaitForKeyPressCommand(0, invalidOperationCode, Substitute.For<IGeneralRegisters>(),
                                                 Substitute.For<IKeyboard>()),
                "operationCode");
        }

        [Test]
        public void Constructor_WithNullGeneralRegisters_ExpectedThrowsArgumentNullException()
        {
            NUnitUtilities.AssertThrowsArgumentExceptionWithParamName<ArgumentNullException>(
                () => new WaitForKeyPressCommand(0, ValidOperationCode, null, Substitute.For<IKeyboard>()),
                "generalRegisters");
        }

        [Test]
        public void Constructor_WithNullKeyboard_ExpectedThrowsArgumentNullException()
        {
            NUnitUtilities.AssertThrowsArgumentExceptionWithParamName<ArgumentNullException>(
                () => new WaitForKeyPressCommand(0, ValidOperationCode, Substitute.For<IGeneralRegisters>(), null),
                "keyboard");
        }

        [TestCase(0)]
        [TestCase(4)]
        [TestCase(100)]
        public void NextCommandAddress_ExpectedReturnsCommandAddressAsDefaultValue(int commandAddress)
        {
            var command = CreateCommand(commandAddress);
            Assert.AreEqual(commandAddress, command.NextCommandAddress);
        }

        [Test]
        public void Execute_ExpectedChangesNextCommandAddressWhenKeyPressed()
        {
            // Arrange
            var command = CreateCommand();

            // Act
            command.Execute();

            // Assert
            Assert.AreEqual(Command.CommandLength, command.NextCommandAddress);
        }

        [TestCase(0xF00A, 1)]
        [TestCase(0xF10A, 2)]
        [TestCase(0xFF0A, 6)]
        public void Execute_ExpectedSaveInRegisterVxPressedKeyIndex(int operationCode, byte pressedKeyIndex)
        {
            // Arrange
            var generalRegistersStub = Substitute.For<IGeneralRegisters>();
            byte registerActualValue = 0;
            var registerIndex = (operationCode & 0x0F00) >> 8;
            generalRegistersStub[registerIndex].Returns(registerActualValue);
            generalRegistersStub[registerIndex] = Arg.Do<byte>(value => registerActualValue = value);

            var keyboardStub = CreateKeyboardStub(pressedKeyIndex: pressedKeyIndex);

            var command = CreateCommand(operationCode: operationCode, generalRegisters: generalRegistersStub,
                                        keyboard: keyboardStub);
            
            // Act
            command.Execute();

            // Assert
            Assert.AreEqual(pressedKeyIndex, registerActualValue);
        }
    }
}