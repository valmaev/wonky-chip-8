using System;
using NSubstitute;
using NUnit.Framework;
using WonkyChip8.Interpreter.Commands;
using WonkyChip8.Interpreter.UnitTests.TestUtilities;

namespace WonkyChip8.Interpreter.UnitTests.Commands
{
    [TestFixture]
    public class PointToFontSpriteCommandFixture
    {
        private const int ValidOperationCode = 0xF029;

        private static PointToFontSpriteCommand CreateCommand(int operationCode = ValidOperationCode,
                                                              IGeneralRegisters generalRegisters = null,
                                                              IAddressRegister addressRegister = null)
        {
            return new PointToFontSpriteCommand(0, operationCode,
                                                generalRegisters ?? Substitute.For<IGeneralRegisters>(),
                                                addressRegister ?? Substitute.For<IAddressRegister>());
        }

        [TestCase(0x99999)]
        [TestCase(0xF000)]
        [TestCase(0x0029)]
        public void Constructor_WithInvalidOperationCode_ExpectedThrowsArgumentOutOfRangeException(
            int invalidOperationCode)
        {
            NUnitUtilities.AssertThrowsArgumentExceptionWithParamName<ArgumentOutOfRangeException>(
                () => new PointToFontSpriteCommand(0, invalidOperationCode, Substitute.For<IGeneralRegisters>(),
                                                   Substitute.For<IAddressRegister>()),
                "operationCode");
        }


        [Test]
        public void Constructor_WithNullGeneralRegisters_ExpectedThrowsArgumentNullException()
        {
            NUnitUtilities.AssertThrowsArgumentExceptionWithParamName<ArgumentNullException>(
                () => new PointToFontSpriteCommand(0, ValidOperationCode, null, Substitute.For<IAddressRegister>()),
                "generalRegisters");
        }

        [Test]
        public void Constructor_WithNullAddressRegister_ExpectedThrowsArgumentNullException()
        {
            NUnitUtilities.AssertThrowsArgumentExceptionWithParamName<ArgumentNullException>(
                () => new PointToFontSpriteCommand(0, ValidOperationCode, Substitute.For<IGeneralRegisters>(), null),
                "addressRegister");
        }

        [TestCase(0xF029, 0x0)]
        [TestCase(0xF129, 0x1)]
        [TestCase(0xF429, 0xF)]
        [TestCase(0xFF29, 0xA)]
        public void Execute_ExpectedSaveToAddressRegisterValueOfFontSpriteAddress(int operationCode,
                                                                                  byte registerInitialValue)
        {
            // Arrange
            var generalRegistersStub = Substitute.For<IGeneralRegisters>();
            var registerIndex = (operationCode & 0x0F00) >> 8;
            generalRegistersStub[registerIndex].Returns(registerInitialValue);

            var addressRegisterStub = Substitute.For<IAddressRegister>();
            short addressRegisterActualValue = 0;
            addressRegisterStub.AddressValue.Returns(addressRegisterActualValue);
            addressRegisterStub.AddressValue = Arg.Do<short>(value => addressRegisterActualValue = value);

            var command = CreateCommand(operationCode, generalRegistersStub, addressRegisterStub);

            const int memoryOffset = 0x050;
            const byte fontSpriteHeight = 5;
            // Act
            command.Execute();

            // Assert
            Assert.AreEqual(memoryOffset + fontSpriteHeight*registerInitialValue, addressRegisterActualValue);
        }
    }
}