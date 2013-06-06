using System;
using NSubstitute;
using NUnit.Framework;
using WonkyChip8.Interpreter.Commands;
using WonkyChip8.Interpreter.UnitTests.TestUtilities;

namespace WonkyChip8.Interpreter.UnitTests.Commands
{
    [TestFixture]
    public class SaveRegisterValueToTimerValueCommandFixture
    {
        private static SaveRegisterValueToTimerValueCommand CreateCommand(int operationCode = ValidOperationCode,
                                                                          IGeneralRegisters generalRegisters = null,
                                                                          ITimer timer = null)
        {
            return new SaveRegisterValueToTimerValueCommand(0, operationCode,
                                                            generalRegisters ?? Substitute.For<IGeneralRegisters>(),
                                                            timer ?? Substitute.For<ITimer>());
        }

        private const int ValidOperationCode = 0xF015;

        [Test]
        public void Constructor_WithNullGeneralRegisters_ExpectedThrowsArgumentNullException()
        {
            NUnitUtilities.AssertThrowsArgumentExceptionWithParamName<ArgumentNullException>(
                () => new SaveRegisterValueToTimerValueCommand(0, ValidOperationCode, null, Substitute.For<ITimer>()),
                "generalRegisters");
        }

        [Test]
        public void Constructor_WithNullTimer_ExpectedThrowsArgumentNullException()
        {
            NUnitUtilities.AssertThrowsArgumentExceptionWithParamName<ArgumentNullException>(
                () => new SaveRegisterValueToTimerValueCommand(0, ValidOperationCode,
                                                               Substitute.For<IGeneralRegisters>(), null),
                "timer");
        }

        [TestCase(0x99999)]
        [TestCase(0xF000)]
        [TestCase(0x0015)]
        [TestCase(0x0018)]
        public void Constructor_WithInvalidOperationCode_ExectedThrowsArgumentOutOfRangeException(
            int invalidOperationCode)
        {
            NUnitUtilities.AssertThrowsArgumentExceptionWithParamName<ArgumentOutOfRangeException>(
                () => new SaveRegisterValueToTimerValueCommand(0, invalidOperationCode,
                                                               Substitute.For<IGeneralRegisters>(),
                                                               Substitute.For<ITimer>()),
                "operationCode");
        }

        [TestCase(0xF015)]
        [TestCase(0xF018)]
        public void Constructor_WithProperOperationCode_ExpectedDoesNotThrowException(int properOperationCode)
        {
            Assert.DoesNotThrow(() => CreateCommand(properOperationCode));
        }

        [TestCase(0xF015)]
        [TestCase(0xF018)]
        [TestCase(0xF115)]
        [TestCase(0xF118)]
        [TestCase(0xFF15)]
        [TestCase(0xFF18)]
        public void Execute_ExpectedSetTimerValueToVxRegisterValue(int operationCode)
        {
            // Arrange
            var timerStub = Substitute.For<ITimer>();
            byte timerActualValue = 0;
            timerStub.Value.Returns(timerActualValue);
            timerStub.Value = Arg.Do<byte>(value => timerActualValue = value);

            var generalRegistersStub = Substitute.For<IGeneralRegisters>();
            const byte registerValue = 123;
            var registerIndex = (operationCode & 0x0F00) >> 8;
            generalRegistersStub[registerIndex].Returns(registerValue);

            var command = CreateCommand(operationCode, generalRegistersStub, timerStub);

            // Act
            command.Execute();

            // Assert
            Assert.AreEqual(registerValue, timerActualValue);
        }
    }
}