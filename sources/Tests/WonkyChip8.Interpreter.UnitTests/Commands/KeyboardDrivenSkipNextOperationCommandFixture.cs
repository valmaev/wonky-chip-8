using System;
using NSubstitute;
using NUnit.Framework;
using WonkyChip8.Interpreter.Commands;
using WonkyChip8.Interpreter.UnitTests.TestUtilities;

namespace WonkyChip8.Interpreter.UnitTests.Commands
{
    [TestFixture]
    public class KeyboardDrivenSkipNextOperationCommandFixture
    {
        private static KeyboardDrivenSkipNextOperationCommand CreateCommand(int operationCode = 0xE09E,
                                                                            IGeneralRegisters generalRegisters = null,
                                                                            IKeyboard keyboard = null)
        {
            return new KeyboardDrivenSkipNextOperationCommand(0, operationCode,
                                                              generalRegisters ?? Substitute.For<IGeneralRegisters>(),
                                                              keyboard ?? Substitute.For<IKeyboard>());
        }

        [TestCase(0xE000)]
        [TestCase(0x009E)]
        [TestCase(0x00A1)]
        public void Constructor_WithInvalidOperationCode_ExpectedThrowsArgumentOutOfRangeException(int invalidOperationCode)
        {
            NUnitExtensions.AssertThrowsArgumentExceptionWithParamName<ArgumentOutOfRangeException>(
                () => CreateCommand(invalidOperationCode), "operationCode");
        }

        [TestCase(0xE09E)]
        [TestCase(0xE0A1)]
        public void Constructor_WithProperOperationCode_ExpectedNotThrowsException(int operationCode)
        {
            Assert.DoesNotThrow(() => CreateCommand(operationCode));
        }

        [Test]
        public void Constructor_WithNullGeneralRegisters_ExpectedThrowsArgumentNullException()
        {
            NUnitExtensions.AssertThrowsArgumentExceptionWithParamName<ArgumentNullException>(
                () => new KeyboardDrivenSkipNextOperationCommand(0, 0xE09E, null, Substitute.For<IKeyboard>()),
                "generalRegisters");
        }

        [Test]
        public void Constructor_WithNullKeyboard_ExpectedThrowsArgumentNullException()
        {
            NUnitExtensions.AssertThrowsArgumentExceptionWithParamName<ArgumentNullException>(
                () => new KeyboardDrivenSkipNextOperationCommand(0, 0xE09E, Substitute.For<IGeneralRegisters>(), null),
                "keyboard");
        }

        [TestCase(0xE09E, 0x0, 0x0, 0x0, true, ExpectedResult = 0x4)]
        [TestCase(0xE09E, 0x0, 0x0, 0x0, false, ExpectedResult = 0x2)]
        [TestCase(0xEA9E, 0xA, 0xB, 0xB, true, ExpectedResult = 0x4)]
        [TestCase(0xEA9E, 0xA, 0xB, 0xB, false, ExpectedResult = 0x2)]
        [TestCase(0xE0A1, 0x0, 0x0, 0x0, false, ExpectedResult = 0x4)]
        [TestCase(0xE0A1, 0x0, 0x0, 0x0, true, ExpectedResult = 0x2)]
        [TestCase(0xEAA1, 0xA, 0xB, 0xB, true, ExpectedResult = 0x2)]
        [TestCase(0xEAA1, 0xA, 0xB, 0xB, false, ExpectedResult = 0x4)]
        public int NextCommandAddress_ExpectedReturnsProperValue(int operationCode, int registerIndex,
                                                                 byte registerValue, byte keyIndex, bool keyPressedStatus)
        {
            var generalRegistersStub = Substitute.For<IGeneralRegisters>();
            generalRegistersStub[registerIndex].Returns(registerValue);

            var keyboardStub = Substitute.For<IKeyboard>();
            keyboardStub.IsKeyPressed(keyIndex).Returns(keyPressedStatus);

            var command = CreateCommand(operationCode, generalRegistersStub, keyboardStub);

            return command.NextCommandAddress;
        }
    }
}