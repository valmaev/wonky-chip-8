using NUnit.Framework;
using WonkyChip8.Interpreter.Commands;

namespace WonkyChip8.Interpreter.UnitTests.Commands
{
    [TestFixture]
    public class CommandFixture
    {
        public class CommandStub : Command
        {
            public CommandStub(int? address) : base(address) { }

            public override void Execute() { }
        }

        [Test]
        public void Constructor_ExpectNotThrowsException()
        {
            // Arrange
            CommandStub commandStub = null;

            // Act & Assert
            Assert.DoesNotThrow(() => commandStub = new CommandStub(0));
            Assert.AreEqual(0, commandStub.Address);
        }

        [Test]
        public void NextCommandAddress_ExpectReturnsCommandAddressPlusTwoByte()
        {
            // Arrange
            const int commandAddress = 0;
            const int nextCommandExpectedAddress = 2;

            // Act & Assert
            Assert.AreEqual(nextCommandExpectedAddress, new CommandStub(commandAddress).NextCommandAddress);
        }

        [Test]
        public void NextCommandAddress_WithNullAddress_ExpectReturnsNull()
        {
            // Act & Assert
            Assert.IsNull(new CommandStub(null).NextCommandAddress);
        }

        [Test]
        public void Execute_ExpectNotThrowsException()
        {
            // Act & Assert
            Assert.DoesNotThrow(() => new CommandStub(0).Execute());
        }
    }
}