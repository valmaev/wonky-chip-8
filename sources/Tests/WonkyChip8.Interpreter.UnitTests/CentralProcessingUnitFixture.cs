using System;
using NUnit.Framework;

namespace WonkyChip8.Interpreter.UnitTests
{
    [TestFixture]
    public class CentralProcessingUnitFixture 
    {
        [Test]
        public void Constructor_WithNullMemory_ExpectArgumentNullException()
        {
            // Act
            TestDelegate cpuConstructor = () => new CentralProcessingUnit(null);

            // Assert
            var argumentNullException = Assert.Throws<ArgumentNullException>(cpuConstructor);
            Assert.AreEqual("memory", argumentNullException.ParamName);
        }
    }
}