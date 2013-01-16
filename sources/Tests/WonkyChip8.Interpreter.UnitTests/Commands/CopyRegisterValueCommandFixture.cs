using System;
using NSubstitute;
using NUnit.Framework;
using WonkyChip8.Interpreter.Commands;
using WonkyChip8.Interpreter.UnitTests.TestUtilities;

namespace WonkyChip8.Interpreter.UnitTests.Commands
{
    [TestFixture]
    public class CopyRegisterValueCommandFixture
    {
        private static CopyRegisterValueCommand CreateCopyRegisterValueCommand(int? address = 0,
                                                                               int operationCode = 0x8000,
                                                                               IGeneralRegisters generalRegisters = null)
        {
            return new CopyRegisterValueCommand(address, operationCode, generalRegisters ?? Substitute.For<IGeneralRegisters>());
        }

        [TestCase(0x8001)]
        [TestCase(0x99999)]
        public void Constructor_WithInvalidOperationCode_ExpectedThrowsArgumentOutOfRangeException(int operationCode)
        {
            NUnitExtensions.AssertThrowsArgumentExceptionWithParamName<ArgumentOutOfRangeException>(
                () => CreateCopyRegisterValueCommand(operationCode: operationCode), "operationCode");
        }

        [Test]
        public void Constructor_WithNullRegisters_ExpectedThrowsArgumentNullException()
        {
            NUnitExtensions.AssertThrowsArgumentExceptionWithParamName<ArgumentNullException>(
                () => new CopyRegisterValueCommand(0, 0x8000, null), "generalRegisters");
        }

        [TestCase(0x8000, 0, 0x0, 0, 0x0, 0)]
        [TestCase(0x8010, 0, 0x0, 1, 0x1, 1)]
        [TestCase(0x8AC0, 0xA, 0x0, 0XC, 0x0, 0)]
        [TestCase(0x8AC0, 0xA, 0x0, 0XC, 0x25, 0x25)]
        public void Execute_ExpectedCopyValueFromOneRegisterToAnother(int operationCode,
                                                                      int firstRegisterIndex,
                                                                      byte firstRegisterInitialValue,
                                                                      int secondRegisterIndex,
                                                                      byte secondRegisterInitialValue,
                                                                      byte firstRegisterExpectedValue)
        {
            // Arrange
            var registersStub = Substitute.For<IGeneralRegisters>();
            byte? firstRegisterActualValue = firstRegisterInitialValue;
            registersStub[firstRegisterIndex] = Arg.Do<byte?>(arg =>firstRegisterActualValue = arg);
            registersStub[firstRegisterIndex].Returns(firstRegisterActualValue);

            byte? secondRegisterActualValue = secondRegisterInitialValue;
            registersStub[secondRegisterIndex] = Arg.Do<byte?>(arg => secondRegisterActualValue = arg);
            registersStub[secondRegisterIndex].Returns(secondRegisterActualValue);

            var copyRegisterValueCommand = CreateCopyRegisterValueCommand(operationCode: operationCode,
                                                                          generalRegisters: registersStub);

            // Act
            copyRegisterValueCommand.Execute();

            // Assert
            Assert.AreEqual(firstRegisterExpectedValue, firstRegisterActualValue);
        }
    }
}