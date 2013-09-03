using System;
using System.Collections.Generic;
using NUnit.Framework;

namespace WonkyChip8.Interpreter.UnitTests
{
    [TestFixture]
    public class CallStackFixture
    {
        private static CallStack CreateCallStack(Stack<int> stack = null)
        {
            return new CallStack {Stack = stack};
        }

        [Test]
        public void Push_ExpectedPushingValueToInternalStack()
        {
            // Arrange
            var stackStub = new Stack<int>();
            var callStack = CreateCallStack(stackStub);
            const int valueToPush = 1;

            // Act
            callStack.Push(valueToPush);

            // Assert
            Assert.AreEqual(valueToPush, stackStub.Peek());
        }

        [Test]
        public void Pop_WithEmptyStack_ExpectedThrowsInvalidOperationException()
        {
            Assert.Throws<InvalidOperationException>(() => CreateCallStack().Pop());
        }

        [Test]
        public void Pop_WithNonEmptyStack_ExpectedPoppingValueFromInternalStack()
        {
            // Arrange
            var stackStub = new Stack<int>();
            var callStack = CreateCallStack(stackStub);
            const int valueToPush = 1;
            callStack.Push(valueToPush);

            // Act
            var poppedValue = callStack.Pop();

            // Assert
            Assert.AreEqual(valueToPush, poppedValue);
            Assert.AreEqual(0, stackStub.Count);
        }

        [Test]
        public void Peek_WithEmptyStack_ExpectedReturnsZero()
        {
            Assert.AreEqual(0, CreateCallStack().Peek());
        }

        [Test]
        public void Peek_WithNonEmptyStack_ExpectedPeekingValueFromInternalStack()
        {
            // Arrange
            var stackStub = new Stack<int>();
            var callStack = CreateCallStack(stackStub);
            const int valueToPush = 1;
            callStack.Push(valueToPush);

            // Act
            var peekedValue = callStack.Peek();

            // Assert
            Assert.AreEqual(valueToPush, peekedValue);
            Assert.AreEqual(1, stackStub.Count);
        }

        [Test]
        public void Stack_AfterSettingToNull_ExpectedCreatingNewInstanceInGetter()
        {
            var callStack = new CallStack {Stack = null};
            Assert.IsNotNull(callStack.Stack, "Stack was null");
        }
    }
}