using System;
using NUnit.Framework;

namespace WonkyChip8.Interpreter.UnitTests
{
    [TestFixture]
    public class RegistersFixture
    {
        [TestCase(0x0, 1)]
        [TestCase(0x1, 1)]
        [TestCase(0x2, 1)]
        [TestCase(0xA, 1)]
        [TestCase(0xF, 1)]
        public void ThisIndexer_WithProperRegisterIndex_ExpectEqualsValue(int registerIndex, byte value)
        {
            // Arrange
            var registers = new Registers();

            // Act
            registers[registerIndex] = value;

            // Assert
            Assert.AreEqual(value, registers[registerIndex]);
        }

        [TestCase(-0x1, 1)]
        [TestCase(0x10, 1)]
        [TestCase(0xFF, 1)]
        public void ThisIndexer_WithInvalidRegisterIndex_ExpectThrowsArgumentOutOfRangeException(int registerIndex, byte value)
        {
            // Arrange
            var registers = new Registers();

            // Act & Assert
            var argumentOutOfRangeException =
                Assert.Throws<ArgumentOutOfRangeException>(() => registers[registerIndex] = value);
            Assert.AreEqual("index", argumentOutOfRangeException.ParamName);
        }
    }
}