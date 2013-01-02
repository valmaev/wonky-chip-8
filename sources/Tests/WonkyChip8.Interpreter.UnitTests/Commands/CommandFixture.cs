using NUnit.Framework;
using WonkyChip8.Interpreter.Commands;

namespace WonkyChip8.Interpreter.UnitTests.Commands
{
    [TestFixture]
    public class CommandFixture
    {
        public class CommandStub : Command
        {
            public CommandStub(int? address, int? operationCode) : base(address, operationCode) { }

            public override void Execute() { }
        }

        public static CommandStub CreateCommandStub(int? address = 0, int? operationCode = 0)
        {
            return new CommandStub(address, operationCode);
        }

        [Test]
        public void Constructor_ExpectNotThrowsException()
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
        public void NextCommandAddress_ExpectReturnsCommandAddressPlusTwoByte()
        {
            // Arrange
            const int nextCommandExpectedAddress = 2;

            // Act & Assert
            Assert.AreEqual(nextCommandExpectedAddress, CreateCommandStub().NextCommandAddress);
        }

        [Test]
        public void NextCommandAddress_WithNullAddress_ExpectReturnsNull()
        {
            // Act & Assert
            Assert.IsNull(CreateCommandStub(address: null).NextCommandAddress);
        }

        [Test]
        public void Execute_ExpectNotThrowsException()
        {
            // Act & Assert
            Assert.DoesNotThrow(() => CreateCommandStub().Execute());
        }
    }
}