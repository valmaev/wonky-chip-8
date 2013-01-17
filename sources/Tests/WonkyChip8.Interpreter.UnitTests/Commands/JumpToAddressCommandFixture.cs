using System;
using NSubstitute;
using NUnit.Framework;
using WonkyChip8.Interpreter.Commands;
using WonkyChip8.Interpreter.UnitTests.TestUtilities;

namespace WonkyChip8.Interpreter.UnitTests.Commands
{
    [TestFixture]
    public class JumpToAddressCommandFixture
    {
        private static JumpToAddressCommand CreateJumpToAddressCommand(int? address = 0, int operationCode = 0x1000,
                                                                 IGeneralRegisters generalRegisters = null)
        {
            return new JumpToAddressCommand(address, operationCode,
                                            generalRegisters ?? Substitute.For<IGeneralRegisters>());
        }

        [TestCase(0x99999)]
        public void Constructor_WithInvalidOperationCode_ExpectedThrowsArgumentOutOfRangeException(int invalidOperationCode)
        {
            NUnitExtensions.AssertThrowsArgumentExceptionWithParamName<ArgumentOutOfRangeException>(
                () => CreateJumpToAddressCommand(operationCode: invalidOperationCode), "operationCode");
        }

        [Test]
        public void Constructor_WithNullGeneralRegisters_ExpectedThrowsArgumentNullException()
        {
            NUnitExtensions.AssertThrowsArgumentExceptionWithParamName<ArgumentNullException>(
                () => new JumpToAddressCommand(0, 0x1000, null), "generalRegisters");
        }

        [Test]
        public void NextAddress_WithOperationCodeEquals1Nnn_ExpectedReturnsLastThreeHalfBitsOfOperationCode()
        {
            TestJumpToAddressCommandWithDefaultStubs(0x1111, 0x111);
        }

        private void TestJumpToAddressCommandWithDefaultStubs(int operationCode, int expectedNextCommandAddress)
        {
            // Arrange
            var jumpToAddressCommand = CreateJumpToAddressCommand(operationCode: operationCode);

            // Assert
            Assert.AreEqual(expectedNextCommandAddress, jumpToAddressCommand.NextCommandAddress);
        }

        [TestCase(0xB000, 0, 0)]
        [TestCase(0xB111, 0, 0x111)]
        [TestCase(0xB111, 0x11, 0x122)]
        public void NextCommandAddress_WithOperationCodeEqualsBnnn_ExpectedReturnsNnnPlusValueOfZeroRegister(
            int operationCode, byte zeroRegisterInitialValue, int expectedNextCommandAddress)
        {
            // Arrange
            var generalRegistersStub = Substitute.For<IGeneralRegisters>();
            byte? zeroRegisterActualValue = zeroRegisterInitialValue;
            generalRegistersStub[0] = Arg.Do<byte?>(value => zeroRegisterActualValue = value);
            generalRegistersStub[0].Returns(zeroRegisterActualValue);

            var jumpToAddressCommand = CreateJumpToAddressCommand(operationCode: operationCode,
                                                                  generalRegisters: generalRegistersStub);

            // Assert
            Assert.AreEqual(expectedNextCommandAddress, jumpToAddressCommand.NextCommandAddress);
        }

        [Test]
        public void NextCommandAddress_WithOperationCodeEqualBnnAndNullZeroRegisterValue_ExpectedReturnsNnn()
        {
            TestJumpToAddressCommandWithDefaultStubs(0xB123, 0x123);
        }
    }
}