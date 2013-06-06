using System;
using NSubstitute;
using NUnit.Framework;
using WonkyChip8.Interpreter.Commands;
using WonkyChip8.Interpreter.UnitTests.TestUtilities;

namespace WonkyChip8.Interpreter.UnitTests.Commands
{
    [TestFixture]
    public class TimerCommandFactoryFixture
    {
        internal static TimerCommandFactory CreateTimerCommandFactory(IGeneralRegisters generalRegisters = null,
                                                                      ITimer delayTimer = null, ITimer soundTimer = null)
        {
            return new TimerCommandFactory(generalRegisters ?? Substitute.For<IGeneralRegisters>(),
                                           delayTimer ?? Substitute.For<ITimer>(),
                                           soundTimer ?? Substitute.For<ITimer>());
        }

        [Test]
        public void Constructor_WithNullGeneralRegisters_ExpectedThrowsArgumentNullExceptions()
        {
            NUnitUtilities.AssertThrowsArgumentExceptionWithParamName<ArgumentNullException>(
                () => new TimerCommandFactory(null, Substitute.For<ITimer>(), Substitute.For<ITimer>()),
                "generalRegisters");
        }

        [Test]
        public void Constructor_WithNullDelayTimer_ExpectedThrowsArgumentNullException()
        {
            NUnitUtilities.AssertThrowsArgumentExceptionWithParamName<ArgumentNullException>(
                () => new TimerCommandFactory(Substitute.For<IGeneralRegisters>(), null, Substitute.For<ITimer>()),
                "delayTimer");
        }

        [Test]
        public void Constructor_WithNullSoundTimer_ExpectedThrowsArgumentNullException()
        {
            NUnitUtilities.AssertThrowsArgumentExceptionWithParamName<ArgumentNullException>(
                () => new TimerCommandFactory(Substitute.For<IGeneralRegisters>(), Substitute.For<ITimer>(), null),
                "soundTimer");
        }

        [TestCase(0xF007, typeof (SaveTimerValueToRegisterCommand))]
        [TestCase(0xF015, typeof (SaveRegisterValueToTimerValueCommand))]
        [TestCase(0xF018, typeof (SaveRegisterValueToTimerValueCommand))]
        public void Create_WithProperOperationCode_ExpectedReturnsCommandWithProperType(int operationCode,
                                                                                        Type commandType)
        {
            // Arrange
            var commandFactory = CreateTimerCommandFactory();

            // Act
            ICommand command = commandFactory.Create(0, operationCode);

            // Assert
            Assert.IsInstanceOf(commandType, command);
        }

        [TestCase(0x99999)]
        [TestCase(0xF000)]
        [TestCase(0x0007)]
        public void Create_WithNotSupportedOperationCode_ExpectedReturnsNullCommand(int notSupportedOperationCode)
        {
            // Arrange
            var commandFactory = CreateTimerCommandFactory();

            // Act
            ICommand command = commandFactory.Create(0, notSupportedOperationCode);

            // Assert
            Assert.IsInstanceOf<NullCommand>(command);
        }
    }
}