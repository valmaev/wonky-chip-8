using System;
using NSubstitute;
using NUnit.Framework;
using WonkyChip8.Interpreter.Commands;
using WonkyChip8.Interpreter.UnitTests.TestUtilities;

namespace WonkyChip8.Interpreter.UnitTests.Commands
{
    [TestFixture]
    public class SaveTimerValueToRegisterCommandFixture
    {
        private static SaveTimerValueToRegisterCommand CreateCommand(int operationCode = 0xF007,
                                                                     IGeneralRegisters generalRegistersStub = null,
                                                                     ITimer timerStub = null)
        {
            return new SaveTimerValueToRegisterCommand(0, operationCode,
                                                       generalRegistersStub ?? Substitute.For<IGeneralRegisters>(),
                                                       timerStub ?? Substitute.For<ITimer>());
        }

        [TestCase(0x99999)]
        [TestCase(0xF000)]
        [TestCase(0x0007)]
        public void Constructor_WithInvalidOperationCode_ExpectedThrowsArgumentOutOfRangeException(
            int invalidOperationCode)
        {
            NUnitUtilities.AssertThrowsArgumentExceptionWithParamName<ArgumentOutOfRangeException>(
                () => new SaveTimerValueToRegisterCommand(0, invalidOperationCode, Substitute.For<IGeneralRegisters>(),
                                                          Substitute.For<ITimer>()),
                "operationCode");
        }

        [Test]
        public void Constructor_WithNullGeneralRegisters_ExpectedThrowsArgumentNullException()
        {
            NUnitUtilities.AssertThrowsArgumentExceptionWithParamName<ArgumentNullException>(
                () => new SaveTimerValueToRegisterCommand(0, 0xF007, null, Substitute.For<ITimer>()),
                "generalRegisters");
        }

        [Test]
        public void Constructor_WithNullTimer_ExpectedThrowsArgumentNullException()
        {
            NUnitUtilities.AssertThrowsArgumentExceptionWithParamName<ArgumentNullException>(
                () => new SaveTimerValueToRegisterCommand(0, 0xF007, Substitute.For<IGeneralRegisters>(), null),
                "timer");
        }

        [TestCase(0xF007)]
        [TestCase(0xF107)]
        [TestCase(0xFF07)]
        public void Execute_ExpectedSaveTimerValueInVxRegister(int operationCode)
        {
            // Arrange
            var timerStub = Substitute.For<ITimer>();
            const byte timerValue = 123;
            timerStub.Value.Returns(timerValue);

            var generalRegistersStub = Substitute.For<IGeneralRegisters>();
            byte registerActualValue = 0;
            var registerIndex = (operationCode & 0x0F00) >> 8;
            generalRegistersStub[registerIndex].Returns(registerActualValue);
            generalRegistersStub[registerIndex] = Arg.Do<byte>(value => registerActualValue = value);

            var command = CreateCommand(operationCode, generalRegistersStub, timerStub);

            // Act
            command.Execute();

            // Assert
            Assert.AreEqual(timerValue, registerActualValue);
        }
    }
}