using NUnit.Framework;
using WonkyChip8.Interpreter.Commands;

namespace WonkyChip8.Interpreter.UnitTests.Commands
{
    [TestFixture]
    public class NullCommandFixture
    {
        [Test]
        public void Constructor_WithProperAddress_ExpectNotThrowsException()
        {
            Assert.DoesNotThrow(() => new NullCommand(0));
        }

        [Test]
        public void Address_ExpectReturnsAddressFromConstructorParameter()
        {
            // Arrange
            const int nullCommandAddress = 10;
            var nullCommand = new NullCommand(nullCommandAddress);

            // Act & Assert
            Assert.AreEqual(nullCommandAddress, nullCommand.Address);
        }

        [Test]
        public void NextCommandAddress_ExpectReturnsNull()
        {
            // Arrange
            var nullCommand = new NullCommand(0);

            // Act & Assert
            Assert.IsNull(nullCommand.NextCommandAddress);
        }

        [Test]
        public void Execute_ExpectNotThrowsException()
        {
            Assert.DoesNotThrow(() => new NullCommand(0).Execute());
        }
    }
}