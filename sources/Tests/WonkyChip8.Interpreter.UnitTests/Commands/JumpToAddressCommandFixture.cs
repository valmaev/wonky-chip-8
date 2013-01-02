using System;
using NUnit.Framework;
using WonkyChip8.Interpreter.Commands;

namespace WonkyChip8.Interpreter.UnitTests.Commands
{
    [TestFixture]
    public class JumpToAddressCommandFixture
    {
        [Test]
        public void Constructor_WithInvalidOperationCode_ExpectArgumentOutOfRangeException()
        {
            // Act & Assert
            var argumentOutOfRangeException =
                Assert.Throws<ArgumentOutOfRangeException>(() => new JumpToAddressCommand(0, 0x2000));
            Assert.AreEqual("operationCode", argumentOutOfRangeException.ParamName);
        }

        [Test]
        public void NextAddress_ExpectReturnLastThreeHalfBitsOfOperationCode()
        {
            // Arrange
            var jumpToAddressCommand = new JumpToAddressCommand(0, 0x1111);

            // Assert
            Assert.AreEqual(0x111, jumpToAddressCommand.NextCommandAddress);
        }
    }
}