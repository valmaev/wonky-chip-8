using System;
using NSubstitute;
using NUnit.Framework;
using WonkyChip8.Interpreter.Commands;

namespace WonkyChip8.Interpreter.UnitTests.Commands
{
    [TestFixture]
    public class SkipNextOperationCommandFixture
    {
        private static SkipNextOperationCommand CreateSkipNextOperationCommand(int? address = 0,
                                                                               int operationCode = 0x3000,
                                                                               IRegisters registers = null)
        {
            return new SkipNextOperationCommand(address, operationCode, registers ?? Substitute.For<IRegisters>());
        }

        [TestCase(0x99999)]
        [TestCase(0x5001)]
        public void Constructor_WithInvalidOperationCode_ExpectThrowsArgumentOutOfRangeException(int operationCode)
        {
            // Act & Assert
            var argumentOutOfRangeException =
                Assert.Throws<ArgumentOutOfRangeException>(() => CreateSkipNextOperationCommand(0, operationCode));
            Assert.AreEqual("operationCode", argumentOutOfRangeException.ParamName);
        }

        [TestCase(0x3000)]
        [TestCase(0x4000)]
        [TestCase(0x5000)]
        public void Constructor_WithProperOperationCode_ExpectNotThrowsException(int operationCode)
        {
            Assert.DoesNotThrow(() => CreateSkipNextOperationCommand(0, operationCode));
        }

        [Test]
        public void Constructor_WithNullRegisters_ExpectThrowsArgumentNullException()
        {
            var argumentNullException =
                Assert.Throws<ArgumentNullException>(() => new SkipNextOperationCommand(0, 0x3000, null));
            Assert.AreEqual("registers", argumentNullException.ParamName);
        }

        [TestCase(1, 0xA, 0x310A, 4)]
        [TestCase(1, 0xA, 0x3101, 2)]
        [TestCase(1, 0xA, 0x4101, 4)]
        [TestCase(1, 0x1, 0x4101, 2)]
        public void NextCommandAddress_WithProperDataFor3XNnAnd4XNn_ExpectReturnsProperValue(int registerIndex,
                                                                                             byte registerValue,
                                                                                             int operationCode,
                                                                                             int expectedNextCommandAddress)
        {
            // Arrange
            var registersStub = Substitute.For<IRegisters>();
            registersStub[registerIndex].Returns(registerValue);

            var skipNextOperationCommand = CreateSkipNextOperationCommand(0, operationCode, registersStub);

            // Assert
            Assert.AreEqual(expectedNextCommandAddress, skipNextOperationCommand.NextCommandAddress);
        }

        [TestCase(1, 0x1, 2, 0x1, 0x5120, 4)]
        [TestCase(1, 0x1, 2, 0xA, 0x5120, 2)]
        public void NextCommandAddress_WithProperDataFor5Xyn_ExpectReturnsProperValue(int registerXIndex,
                                                                                      byte registerXValue,
                                                                                      int registerYIndex,
                                                                                      byte registerYValue,
                                                                                      int operationCode,
                                                                                      int expectedNextCommandAddress)
        {
            // Arrange
            var registersStub = Substitute.For<IRegisters>();
            registersStub[registerXIndex].Returns(registerXValue);
            registersStub[registerYIndex].Returns(registerYValue);

            var skipNextOperationCommand = CreateSkipNextOperationCommand(0, operationCode, registersStub);

            // Assert
            Assert.AreEqual(expectedNextCommandAddress, skipNextOperationCommand.NextCommandAddress);
        }
    }
}