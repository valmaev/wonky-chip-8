using System;
using System.Collections.Generic;
using NSubstitute;
using NUnit.Framework;
using WonkyChip8.Interpreter.Commands;
using WonkyChip8.Interpreter.UnitTests.TestUtilities;

namespace WonkyChip8.Interpreter.UnitTests.Commands
{
    [TestFixture]
    public class ReturnFromSubroutineCommandFixture
    {
        private static ReturnFromSubroutineCommand CreateReturnFromSubroutineCommand(int operationCode = 0x00EE,
                                                                                     ICallStack callStack = null)
        {
            return new ReturnFromSubroutineCommand(0, operationCode, callStack ?? Substitute.For<ICallStack>());
        }

        private static ICallStack CreateCallStackStub(int initialValue)
        {
            var stack = new Stack<int>();
            stack.Push(initialValue);

            var callStackStub = Substitute.For<ICallStack>();
            callStackStub.Peek().Returns(info => stack.Peek());
            callStackStub.Pop().Returns(info => stack.Pop());

            return callStackStub;
        }

        [Test]
        public void Constructor_WithInvalidOperationCode_ExpectedThrowsArgumentOutOfRangeException()
        {
            NUnitUtilities.AssertThrowsArgumentExceptionWithParamName<ArgumentOutOfRangeException>(
                () => CreateReturnFromSubroutineCommand(0xEEEE), "operationCode");
        }

        [Test]
        public void Constructor_WithNullCallStack_ExpectedThrowsArgumentNullException()
        {
            NUnitUtilities.AssertThrowsArgumentExceptionWithParamName<ArgumentNullException>(
                () => new ReturnFromSubroutineCommand(0, 0x00EE, null), "callStack");
        }

        [Test]
        public void Execute_ExpectedCallsCallStackPopOneTime()
        {
            // Arrange
            var callStackMock = Substitute.For<ICallStack>();
            ReturnFromSubroutineCommand returnFromSubroutineCommand =
                CreateReturnFromSubroutineCommand(callStack: callStackMock);

            // Act
            returnFromSubroutineCommand.Execute();

            // Assert
            callStackMock.Received(1).Pop();
        }

        [Test]
        public void NextCommandAddress_ExpectedReturnsNextForTopOfCallStackCommandAddress()
        {
            // Arrange
            const int topOfStackCommandAddress = 2;

            var callStackStub = Substitute.For<ICallStack>();
            callStackStub.Peek().Returns(topOfStackCommandAddress);

            ReturnFromSubroutineCommand returnFromSubroutineCommand =
                CreateReturnFromSubroutineCommand(callStack: callStackStub);
            int expectedNextCommandAddress = topOfStackCommandAddress + Command.CommandLength;

            // Act & Assert
            Assert.AreEqual(expectedNextCommandAddress, returnFromSubroutineCommand.NextCommandAddress);
        }

        [Test]
        public void NextCommandAddress_AfterExecuteCall_ExpectedReturnsProperValue()
        {
            // Arrange
            const int topOfStackCommandAddress = 2;

            var returnFromSubroutineCommand = CreateReturnFromSubroutineCommand(
                callStack: CreateCallStackStub(topOfStackCommandAddress));

            int expectedNextCommandAddress = topOfStackCommandAddress + Command.CommandLength;

            // Act
            returnFromSubroutineCommand.Execute();

            // Assert
            Assert.AreEqual(expectedNextCommandAddress, returnFromSubroutineCommand.NextCommandAddress);
        }
    }
}