using System;
using NSubstitute;
using NUnit.Framework;
using WonkyChip8.Interpreter.Commands;

namespace WonkyChip8.Interpreter.UnitTests.Commands
{
    [TestFixture]
    public class ReturnFromSubroutineCommandFixture
    {
        private static ReturnFromSubroutineCommand CreateReturnFromSubroutineCommand(int? address = 0,
                                                                                     int operationCode = 0x00EE,
                                                                                     ICallStack callStack = null)
        {
            return new ReturnFromSubroutineCommand(address, operationCode, callStack ?? Substitute.For<ICallStack>());
        }

        [Test]
        public void Constructor_WithInvalidOperationCode_ExpectThrowsArgumentOutOfRangeException()
        {
            // Act & Assert
            var argumentOutOfRangeException =
                Assert.Throws<ArgumentOutOfRangeException>(
                    () => CreateReturnFromSubroutineCommand(operationCode: 0xEEEE));
            Assert.AreEqual("operationCode", argumentOutOfRangeException.ParamName);
        }

        [Test]
        public void Constructor_WithNullCallStack_ExpectThrowsArgumentNullException()
        {
            // Act & Assert
            var argumentNullException =
                Assert.Throws<ArgumentNullException>(() => new ReturnFromSubroutineCommand(0, 0x00EE, null));
            Assert.AreEqual("callStack", argumentNullException.ParamName);
        }

        [Test]
        public void NextCommandAddress_ExpectReturnsNextForTopOfCallStackCommandAddress()
        {
            // Arrange
            var callStackStub = Substitute.For<ICallStack>();
            const int topOfStackCommandAddress = 2;
            callStackStub.Peek().Returns(topOfStackCommandAddress);
            var returnFromSubroutineCommand = CreateReturnFromSubroutineCommand(callStack: callStackStub);

            // Act & Assert
            Assert.AreEqual(topOfStackCommandAddress + 2, returnFromSubroutineCommand.NextCommandAddress);
        }

        [Test]
        public void NextCommandAddress_WithNullTopOfStackCommandAddress_ExpectReturnsNull()
        {
            Assert.IsNull(CreateReturnFromSubroutineCommand().NextCommandAddress);
        }

        [Test]
        public void Execute_ExpectCallsCallStackPopOneTime()
        {
            // Arrange
            var callStackMock = Substitute.For<ICallStack>();
            var returnFromSubroutineCommand = CreateReturnFromSubroutineCommand(callStack: callStackMock);

            // Act
            returnFromSubroutineCommand.Execute();

            // Assert
            callStackMock.Received(1).Pop();
        }
    }
}