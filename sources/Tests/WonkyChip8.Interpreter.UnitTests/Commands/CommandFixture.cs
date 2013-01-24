using NUnit.Framework;
using WonkyChip8.Interpreter.Commands;

namespace WonkyChip8.Interpreter.UnitTests.Commands
{
    [TestFixture]
    public class CommandFixture
    {
        private class CommandStub : Command
        {
            public CommandStub(int address, int operationCode) : base(address, operationCode) { }
        }

        private static CommandStub CreateCommandStub(int address = 0, int operationCode = 0)
        {
            return new CommandStub(address, operationCode);
        }

        [Test]
        public void Constructor_ExpectedNotThrowsException()
        {
            // Arrange
            CommandStub commandStub = null;
            const int expectedOperationCode = 99999;

            // Act & Assert
            Assert.DoesNotThrow(() => commandStub = CreateCommandStub(operationCode: expectedOperationCode));
            Assert.AreEqual(0, commandStub.Address);
            Assert.AreEqual(expectedOperationCode, commandStub.OperationCode);
        }

        [Test]
        public void NextCommandAddress_ExpectedReturnsCommandAddressPlusTwoByte()
        {
            // Arrange
            const int nextCommandExpectedAddress = 2;

            // Act & Assert
            Assert.AreEqual(nextCommandExpectedAddress, CreateCommandStub().NextCommandAddress);
        }

        [Test]
        public void Execute_ExpectedNotThrowsException()
        {
            // Act & Assert
            Assert.DoesNotThrow(() => CreateCommandStub().Execute());
        }

        [Test]
        public void Bytes_ExpectedReturnsProperValues()
        {
            // Arrange
            var commandStub = CreateCommandStub(operationCode: 0x1234);

            // Assert
            Assert.AreEqual(0x1, commandStub.FirstOperationCodeHalfByte);
            Assert.AreEqual(0x2, commandStub.SecondOperationCodeHalfByte);
            Assert.AreEqual(0x3, commandStub.ThirdOperationCodeHalfByte);
            Assert.AreEqual(0x4, commandStub.FourthOperationCodeHalfByte);

            Assert.AreEqual(0x12, commandStub.FirstOperationCodeByte);
            Assert.AreEqual(0x34, commandStub.SecondOperationCodeByte);
        }
    }
}