using System;
using NSubstitute;
using NUnit.Framework;
using WonkyChip8.Interpreter.Commands;
using WonkyChip8.Interpreter.UnitTests.TestUtilities;

namespace WonkyChip8.Interpreter.UnitTests.Commands
{
    [TestFixture]
    public class SkipNextOperationCommandFixture
    {
        private static SkipNextOperationCommand CreateSkipNextOperationCommand(int operationCode = 0x3000,
                                                                               IGeneralRegisters generalRegisters = null)
        {
            return new SkipNextOperationCommand(0, operationCode, generalRegisters ?? Substitute.For<IGeneralRegisters>());
        }

        [TestCase(0x99999)]
        [TestCase(0x5001)]
        [TestCase(0x9001)]
        public void Constructor_WithInvalidOperationCode_ExpectedThrowsArgumentOutOfRangeException(
            int invalidOoperationCode)
        {
            NUnitUtilities.AssertThrowsArgumentExceptionWithParamName<ArgumentOutOfRangeException>(
                () => CreateSkipNextOperationCommand(invalidOoperationCode), "operationCode");
        }

        [TestCase(0x3000)]
        [TestCase(0x4000)]
        [TestCase(0x5000)]
        [TestCase(0x9000)]
        public void Constructor_WithProperOperationCode_ExpectedNotThrowsException(int operationCode)
        {
            Assert.DoesNotThrow(() => CreateSkipNextOperationCommand(operationCode));
        }

        [Test]
        public void Constructor_WithNullGeneralRegisters_ExpectedThrowsArgumentNullException()
        {
            NUnitUtilities.AssertThrowsArgumentExceptionWithParamName<ArgumentNullException>(
                () => new SkipNextOperationCommand(0, 0x3000, null), "generalRegisters");
        }

        [TestCase(1, 0xA, 0x310A, 4)]
        [TestCase(1, 0xA, 0x3101, 2)]
        [TestCase(1, 0xA, 0x4101, 4)]
        [TestCase(1, 0x1, 0x4101, 2)]
        public void NextCommandAddress_WithProperDataFor3XNnAnd4XNn_ExpectedReturnsProperValue(int registerIndex,
                                                                                             byte registerValue,
                                                                                             int operationCode,
                                                                                             int expectedNextCommandAddress)
        {
            // Arrange
            var registersStub = Substitute.For<IGeneralRegisters>();
            registersStub[registerIndex].Returns(registerValue);

            var skipNextOperationCommand = CreateSkipNextOperationCommand(operationCode, registersStub);

            // Assert
            Assert.AreEqual(expectedNextCommandAddress, skipNextOperationCommand.NextCommandAddress);
        }

        [TestCase(1, 0x1, 2, 0x1, 0x5120, 4)]
        [TestCase(1, 0x1, 2, 0xA, 0x5120, 2)]
        public void NextCommandAddress_WithProperDataFor5Xy0_ExpectedReturnsProperValue(int registerXIndex,
                                                                                        byte registerXValue,
                                                                                        int registerYIndex,
                                                                                        byte registerYValue,
                                                                                        int operationCode,
                                                                                        int expectedNextCommandAddress)
        {
            TestNextCommandFor5Xy0And9Xy0(registerXIndex, registerXValue, registerYIndex, registerYValue, operationCode,
                                          expectedNextCommandAddress);
        }

        private void TestNextCommandFor5Xy0And9Xy0(int registerXIndex, byte registerXValue, int registerYIndex,
                                                   byte registerYValue, int operationCode,
                                                   int expectedNextCommandAddress)
        {
            // Arrange
            var registersStub = Substitute.For<IGeneralRegisters>();
            registersStub[registerXIndex].Returns(registerXValue);
            registersStub[registerYIndex].Returns(registerYValue);

            var skipNextOperationCommand = CreateSkipNextOperationCommand(operationCode, registersStub);

            // Assert
            Assert.AreEqual(expectedNextCommandAddress, skipNextOperationCommand.NextCommandAddress);
        }

        [TestCase(1, 0x1, 2, 0x1, 0x9120, 2)]
        [TestCase(1, 0x1, 2, 0xA, 0x9120, 4)]
        public void NextCommandAddress_WithProperDataFor9Xy0_ExpectedReturnsProperValue(int registerXIndex,
                                                                                        byte registerXValue,
                                                                                        int registerYIndex,
                                                                                        byte registerYValue,
                                                                                        int operationCode,
                                                                                        int expectedNextCommandAddress)
        {
            TestNextCommandFor5Xy0And9Xy0(registerXIndex, registerXValue, registerYIndex, registerYValue, operationCode,
                                          expectedNextCommandAddress);
        }
    }
}