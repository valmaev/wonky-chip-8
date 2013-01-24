using System;
using NUnit.Framework;
using WonkyChip8.Interpreter.UnitTests.TestUtilities;

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
        public void ThisIndexer_WithProperRegisterIndex_ExpectedEqualsValue(int registerIndex, byte value)
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
        public void ThisIndexer_WithInvalidRegisterIndex_ExpectedThrowsArgumentOutOfRangeException(int registerIndex, byte value)
        {
            NUnitExtensions.AssertThrowsArgumentExceptionWithParamName<ArgumentOutOfRangeException>(
                () => new Registers()[registerIndex] = value, "index");
        }
    }
}