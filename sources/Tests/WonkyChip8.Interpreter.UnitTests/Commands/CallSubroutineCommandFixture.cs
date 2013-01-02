using System;
using NSubstitute;
using NUnit.Framework;
using WonkyChip8.Interpreter.Commands;

namespace WonkyChip8.Interpreter.UnitTests.Commands
{
    [TestFixture]
    public class CallSubroutineCommandFixture
    {
        private static CallSubroutineCommand CreateCallSubroutineCommand(int? address = 0, int? operationCode = 0x2001,
                                                                         ICallStack callStack = null)
        {
            return new CallSubroutineCommand(address, operationCode, callStack ?? Substitute.For<ICallStack>());
        }

        [Test]
        public void Constructor_WithInvalidOperationCode_ExpectThrowsArgumentOutOfRangeException()
        {
            // Act & Assert
            var argumentOutOfRagneException =
                Assert.Throws<ArgumentOutOfRangeException>(() => CreateCallSubroutineCommand(operationCode: 0x9999));
            Assert.AreEqual("operationCode", argumentOutOfRagneException.ParamName);
        }

        [Test]
        public void Constructor_WithNullCallStack_ExpectThrowsArgumentNullException()
        {
            // Act & Assert
            var argumentNullException =
                Assert.Throws<ArgumentNullException>(() => new CallSubroutineCommand(0, 0x2001, null));
            Assert.AreEqual("callStack", argumentNullException.ParamName);
        }

        [Test]
        public void NextCommandAddress_ExpectReturnsLastThreeHalfBitsFromOperationCode()
        {
            // Arrange
            const int operationCode = 0x2111;
            const int expectedNextCommandAddress = 0x111;

            // Act & Assert
            Assert.AreEqual(expectedNextCommandAddress,
                            CreateCallSubroutineCommand(operationCode: operationCode).NextCommandAddress);
        }

        [Test]
        public void Execute_ExpectCallsCallStackOneTime()
        {
            // Arrange
            var callStackMock = Substitute.For<ICallStack>();
            var callSubroutineCommand = CreateCallSubroutineCommand(callStack: callStackMock);

            // Act
            callSubroutineCommand.Execute();

            // Assert
            callStackMock.Received(1).Push(Arg.Any<int?>());
        }
    }
}