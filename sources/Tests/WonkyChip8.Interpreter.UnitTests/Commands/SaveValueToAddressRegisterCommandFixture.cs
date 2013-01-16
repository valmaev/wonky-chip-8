using System;
using NSubstitute;
using NUnit.Framework;
using WonkyChip8.Interpreter.Commands;
using WonkyChip8.Interpreter.UnitTests.TestUtilities;

namespace WonkyChip8.Interpreter.UnitTests.Commands
{
    [TestFixture]
    public class SaveValueToAddressRegisterCommandFixture
    {
        private static SaveValueToAddressRegisterCommand CreateSaveValueToAddressRegisterCommand(
            int? address = 0, int operationCode = 0xA000, IAddressRegister addressRegister = null)
        {
            return new SaveValueToAddressRegisterCommand(address, operationCode,
                                                         addressRegister ?? Substitute.For<IAddressRegister>());
        }

        [TestCase(0x99999)]
        public void Constructor_WithInvalidOperationCode_ExpectedThrowsArgumentOutOfRangeException(
            int invalidOperationCode)
        {
            NUnitExtensions.AssertThrowsArgumentExceptionWithParamName<ArgumentOutOfRangeException>(
                () => CreateSaveValueToAddressRegisterCommand(operationCode: invalidOperationCode), "operationCode");
        }

        [Test]
        public void Constructor_WithNullAddressRegister_ExpectedThrowsArgumentNullException()
        {
            NUnitExtensions.AssertThrowsArgumentExceptionWithParamName<ArgumentNullException>(
                () => new SaveValueToAddressRegisterCommand(0, 0xA000, null), "addressRegister");
        }

        [TestCase(0xA000, 0x000)]
        [TestCase(0xA123, 0x123)]
        public void Execute_ExpectedSaveLastThreeHalfByteInAddressRegister(int operationCode,
                                                                           short addressRegisterExpectedValue)
        {
            // Arrange
            var addressRegister = Substitute.For<IAddressRegister>();

            short? addressRegisterActualValue = null;
            addressRegister.AddressValue = Arg.Do<short?>(value => addressRegisterActualValue = value);
            addressRegister.AddressValue.Returns(addressRegisterActualValue);

            var saveValueToAddressCommand = CreateSaveValueToAddressRegisterCommand(operationCode: operationCode,
                                                                                    addressRegister: addressRegister);
            // Act
            saveValueToAddressCommand.Execute();

            // Assert
            Assert.AreEqual(addressRegisterExpectedValue, addressRegisterActualValue);
        }
    }
}