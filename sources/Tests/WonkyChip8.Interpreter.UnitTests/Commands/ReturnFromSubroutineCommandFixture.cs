﻿using System;
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

        [Test]
        public void Constructor_WithInvalidOperationCode_ExpectedThrowsArgumentOutOfRangeException()
        {
            NUnitExtensions.AssertThrowsArgumentExceptionWithParamName<ArgumentOutOfRangeException>(
                () => CreateReturnFromSubroutineCommand(operationCode: 0xEEEE), "operationCode");
        }

        [Test]
        public void Constructor_WithNullCallStack_ExpectedThrowsArgumentNullException()
        {
            NUnitExtensions.AssertThrowsArgumentExceptionWithParamName<ArgumentNullException>(
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
            var callStackStub = Substitute.For<ICallStack>();
            const int topOfStackCommandAddress = 2;
            callStackStub.Peek().Returns(topOfStackCommandAddress);
            ReturnFromSubroutineCommand returnFromSubroutineCommand =
                CreateReturnFromSubroutineCommand(callStack: callStackStub);

            // Act & Assert
            Assert.AreEqual(topOfStackCommandAddress + 2, returnFromSubroutineCommand.NextCommandAddress);
        }
    }
}