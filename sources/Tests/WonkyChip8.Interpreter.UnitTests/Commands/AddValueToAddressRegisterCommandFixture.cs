using System;
using NSubstitute;
using NUnit.Framework;
using WonkyChip8.Interpreter.Commands;
using WonkyChip8.Interpreter.UnitTests.TestUtilities;

namespace WonkyChip8.Interpreter.UnitTests.Commands
{
    [TestFixture]
    public class AddValueToAddressRegisterCommandFixture
    {
        private AddValueToAddressRegisterCommand CreateCommand(int operationCode = 0xF01E,
                                                               IGeneralRegisters generalRegisters = null,
                                                               IAddressRegister addressRegister = null)
        {
            return new AddValueToAddressRegisterCommand(0, operationCode,
                                                        generalRegisters ?? Substitute.For<IGeneralRegisters>(),
                                                        addressRegister ?? Substitute.For<IAddressRegister>());
        }

        [TestCase(0x99999)]
        public void Constructor_WithInvalidOperationCode_ExpectedThrowsArgumentOutOfRangeException(
            int invalidOperationCode)
        {
            NUnitUtilities.AssertThrowsArgumentExceptionWithParamName<ArgumentOutOfRangeException>(
                () => new AddValueToAddressRegisterCommand(0, invalidOperationCode, Substitute.For<IGeneralRegisters>(),
                                                           Substitute.For<IAddressRegister>()),
                "operationCode");
        }

        [Test]
        public void Constructor_WithNullGeneralRegisters_ExpectedThrowsArgumentNullException()
        {
            NUnitUtilities.AssertThrowsArgumentExceptionWithParamName<ArgumentNullException>(
                () => new AddValueToAddressRegisterCommand(0, 0xF029, null, Substitute.For<IAddressRegister>()),
                "generalRegisters");
        }

        [Test]
        public void Constructor_WithNullAddressRegister_ExpectedThrowsArgumentNullException()
        {
            NUnitUtilities.AssertThrowsArgumentExceptionWithParamName<ArgumentNullException>(
                () => new AddValueToAddressRegisterCommand(0, 0xF029, Substitute.For<IGeneralRegisters>(), null),
                "addressRegister");
        }

        [TestCase(0xF01E, 0, 0)]
        [TestCase(0xF01E, 1, 1)]
        [TestCase(0xF11E, 10, 20)]
        [TestCase(0xFF1E, 255, 4000)]
        public void Execute_ExpectedAddsValueOfRegisterVxToAddressRegister(int operationCode, byte registerInitialValue,
                                                                           short addressRegisterInitialValue)
        {
            // Arrange
            var generalRegistersStub = Substitute.For<IGeneralRegisters>();
            var registerIndex = (operationCode & 0x0F00) >> 8;
            byte registerActualValue = registerInitialValue;
            generalRegistersStub[registerIndex] = Arg.Do<byte>(value => registerActualValue = value);
            generalRegistersStub[registerIndex].Returns(registerActualValue);

            var addressRegisterStub = Substitute.For<IAddressRegister>();
            short addressActualValue = addressRegisterInitialValue;
            addressRegisterStub.AddressValue = Arg.Do<short>(value => addressActualValue = value);
            addressRegisterStub.AddressValue.Returns(addressActualValue);

            var command = CreateCommand(operationCode, generalRegistersStub, addressRegisterStub);

            // Act
            command.Execute();

            // Assert
            Assert.AreEqual(registerInitialValue + addressRegisterInitialValue, addressActualValue);
        }

        [TestCase(0xF01E, 0, 4096, ExpectedResult = 1)]
        [TestCase(0xF11E, 255, 4000, ExpectedResult = 1)]
        [TestCase(0xFF1E, 255, 4000, ExpectedResult = 1)]
        [TestCase(0xF01E, 0, 0, ExpectedResult = 0)]
        [TestCase(0xF01E, 1, 1, ExpectedResult = 0)]
        [TestCase(0xF11E, 10, 20, ExpectedResult = 0)]
        public byte Execute_WhenSumOfAddressValueAndRegisterValueGreaterThanFff_ExpectedSetsVfRegisterToOne(
            int operationCode, byte registerInitialValue, short addressRegisterInitialValue)
        {
            // Arrange
            var generalRegistersStub = Substitute.For<IGeneralRegisters>();
            var registerIndex = (operationCode & 0x0F00) >> 8;
            byte registerActualValue = registerInitialValue;
            generalRegistersStub[registerIndex] = Arg.Do<byte>(value => registerActualValue = value);
            generalRegistersStub[registerIndex].Returns(registerActualValue);

            var registerVfActualValue = (byte) (registerIndex == 0xF ? registerActualValue : 0);
            generalRegistersStub[0xF] = Arg.Do<byte>(value => registerVfActualValue = value);
            generalRegistersStub[0xF].Returns(registerVfActualValue);

            var addressRegisterStub = Substitute.For<IAddressRegister>();
            short addressActualValue = addressRegisterInitialValue;
            addressRegisterStub.AddressValue = Arg.Do<short>(value => addressActualValue = value);
            addressRegisterStub.AddressValue.Returns(addressActualValue);

            var command = CreateCommand(operationCode, generalRegistersStub, addressRegisterStub);

            // Act
            command.Execute();

            // Assert
            return registerVfActualValue;
        }
    }
}