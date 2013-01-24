using System;
using NSubstitute;
using NUnit.Framework;
using WonkyChip8.Interpreter.Commands;
using WonkyChip8.Interpreter.UnitTests.TestUtilities;

namespace WonkyChip8.Interpreter.UnitTests.Commands
{
    [TestFixture]
    public class CallSubroutineCommandFixture
    {
        private static CallSubroutineCommand CreateCallSubroutineCommand(int operationCode = 0x2001,
                                                                         ICallStack callStack = null)
        {
            return new CallSubroutineCommand(0, operationCode, callStack ?? Substitute.For<ICallStack>());
        }

        [Test]
        public void Constructor_WithInvalidOperationCode_ExpectedThrowsArgumentOutOfRangeException()
        {
            NUnitExtensions.AssertThrowsArgumentExceptionWithParamName<ArgumentOutOfRangeException>(
                () => CreateCallSubroutineCommand(operationCode: 0x9999), "operationCode");
        }

        [Test]
        public void Constructor_WithNullCallStack_ExpectedThrowsArgumentNullException()
        {
            NUnitExtensions.AssertThrowsArgumentExceptionWithParamName<ArgumentNullException>(
                () => new CallSubroutineCommand(0, 0x2001, null), "callStack");
        }

        [Test]
        public void NextCommandAddress_ExpectedReturnsLastThreeHalfBitsFromOperationCode()
        {
            // Arrange
            const int operationCode = 0x2111;
            const int expectedNextCommandAddress = 0x111;

            // Act & Assert
            Assert.AreEqual(expectedNextCommandAddress, CreateCallSubroutineCommand(operationCode).NextCommandAddress);
        }

        [Test]
        public void Execute_ExpectedCallsCallStackPushOneTime()
        {
            // Arrange
            var callStackMock = Substitute.For<ICallStack>();
            var callSubroutineCommand = CreateCallSubroutineCommand(callStack: callStackMock);

            // Act
            callSubroutineCommand.Execute();

            // Assert
            callStackMock.Received(1).Push(Arg.Any<int>());
        }
    }
}