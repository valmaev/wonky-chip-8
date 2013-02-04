using NUnit.Framework;
using WonkyChip8.Interpreter.Commands;

namespace WonkyChip8.Interpreter.UnitTests.Commands
{
    [TestFixture]
    public class NullCommandFixture
    {
        [Test]
        public void Constructor_WithProperAddress_ExpectedNotThrowsException()
        {
            Assert.DoesNotThrow(() => new NullCommand());
        }

        [Test]
        public void Address_ExpectedReturnsZero()
        {
            Assert.AreEqual(0, new NullCommand().Address);
        }

        [Test]
        public void NextCommandAddress_ExpectedReturnsZero()
        {
            Assert.AreEqual(0, new NullCommand().NextCommandAddress);
        }

        [Test]
        public void OperationCode_ExpectedReturnsZero()
        {
            Assert.AreEqual(0, new NullCommand().OperationCode);
        }

        [Test]
        public void Execute_ExpectedNotThrowsException()
        {
            Assert.DoesNotThrow(() => new NullCommand().Execute());
        }
    }
}