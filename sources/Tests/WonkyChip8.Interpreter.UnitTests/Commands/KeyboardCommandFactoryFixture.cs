using System;
using NSubstitute;
using NUnit.Framework;
using WonkyChip8.Interpreter.Commands;
using WonkyChip8.Interpreter.UnitTests.TestUtilities;

namespace WonkyChip8.Interpreter.UnitTests.Commands
{
    [TestFixture]
    public class KeyboardCommandFactoryFixture
    {
        internal static KeyboardCommandFactory CreateKeyboardCommandFactory(IGeneralRegisters generalRegisters = null,
                                                                           IKeyboard keyboard = null)
        {
            return new KeyboardCommandFactory(generalRegisters ?? Substitute.For<IGeneralRegisters>(),
                                              keyboard ?? Substitute.For<IKeyboard>());
        }

        [Test]
        public void Constructor_WithNullGeneralRegisters_ExpectedThrowsArgumentNullException()
        {
            NUnitUtilities.AssertThrowsArgumentExceptionWithParamName<ArgumentNullException>(
                () => new KeyboardCommandFactory(null, Substitute.For<IKeyboard>()), "generalRegisters");
        }

        [Test]
        public void Constructor_WithNullKeyboard_ExpectedThrowsArgumentNullException()
        {
            NUnitUtilities.AssertThrowsArgumentExceptionWithParamName<ArgumentNullException>(
                () => new KeyboardCommandFactory(Substitute.For<IGeneralRegisters>(), null), "keyboard");
        }

        [TestCase(0xE09E, typeof(KeyboardDrivenSkipNextOperationCommand))]
        [TestCase(0xE0A1, typeof(KeyboardDrivenSkipNextOperationCommand))]
        public void Create_WithProperOperationCode_ExpectedReturnsCommandWithProperType(int operationCode,
                                                                                        Type commandType)
        {
            // Arrange
            var commandFactory = CreateKeyboardCommandFactory();

            // Act
            ICommand command = commandFactory.Create(0, operationCode);

            // Assert
            Assert.IsInstanceOf(commandType, command);
        }

        [TestCase(0x0000)]
        [TestCase(0x99999)]
        [TestCase(0xF000)]
        public void Create_WithNotSupportedOperationCode_ExpectedReturnsNullCommand(int notSupportedOpertationCode)
        {
            // Arrange
            var commandFactory = CreateKeyboardCommandFactory();

            // Act
            ICommand command = commandFactory.Create(0, notSupportedOpertationCode);

            // Assert
            Assert.IsInstanceOf<NullCommand>(command);
        }
    }
}