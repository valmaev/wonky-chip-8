using System;
using NUnit.Framework;
using WonkyChip8.Interpreter.Commands;
using WonkyChip8.Interpreter.UnitTests.TestUtilities;
using NSubstitute;

namespace WonkyChip8.Interpreter.UnitTests.Commands
{
    [TestFixture]
    public class LogicalOrCommandFixture
    {
        private static LogicalOrCommand CreateLogicalOrCommand(int? address = 0, int operationCode = 0x8001, IRegisters registers = null)
        {
            return new LogicalOrCommand(address, operationCode, registers ?? Substitute.For<IRegisters>());
        }

        [TestCase(0x8000)]
        [TestCase(0x99999)]
        public void Constructor_WithInvalidOperationCode_ExpectedThrowsArgumentOutOfRangeException(int invalidOperationCode)
        {
            NUnitExtensions.AssertThrowsArgumentExceptionWithParamName<ArgumentOutOfRangeException>(
                () => CreateLogicalOrCommand(operationCode: invalidOperationCode), "operationCode");
        }

        [Test]
        public void Constructor_WithNullRegisters_ExpectedThrowsArgumentNullException()
        {
            NUnitExtensions.AssertThrowsArgumentExceptionWithParamName<ArgumentNullException>(
                () => new LogicalOrCommand(0, 0x8001, null), "registers");
        }

        [TestCase(0x8001, 0x0, 0x0, 0x0, 0x0, 0x0)]
        [TestCase(0x8001, 0x0, 0xF, 0x0, 0xF, 0xF)]
        [TestCase(0x8011, 0x0, 0x0, 0x1, 0x0, 0x0)]
        [TestCase(0x8011, 0x0, 0x0, 0x1, 0x1, 0x1)]
        [TestCase(0x8011, 0x0, 0x1, 0x1, 0x1, 0x1)]
        [TestCase(0x8A01, 0xA, 0xB, 0x0, 0xD, 0xF)]
        public void Execute_WithNotNullRegistersValue_ExpectedPerfomLogicalOr(int operationCode,
                                                                              int firstRegisterIndex,
                                                                              byte firstRegisterInitialValue,
                                                                              int secondRegisterIndex,
                                                                              byte secondRegisterInitialValue,
                                                                              byte firstRegisterExpectedValue)
        {
            // Arrange
            var registersStub = Substitute.For<IRegisters>();
            byte firstRegisterActualValue = firstRegisterInitialValue;
            registersStub[firstRegisterIndex] = Arg.Do<byte>(value => firstRegisterActualValue = value);
            registersStub[firstRegisterIndex].Returns(firstRegisterActualValue);

            byte secondRegisterActualValue = secondRegisterInitialValue;
            registersStub[secondRegisterIndex] = Arg.Do<byte>(value => secondRegisterActualValue = value);
            registersStub[secondRegisterIndex].Returns(secondRegisterActualValue);

            var logicalOrCommand = CreateLogicalOrCommand(operationCode: operationCode, registers: registersStub);

            // Act
            logicalOrCommand.Execute();

            // Assert
            Assert.AreEqual(firstRegisterExpectedValue, firstRegisterActualValue);
        }

        [Test]
        public void Execute_WithNullRegistersValue_ExpectedNull()
        {
            // Arrange
            var registersStub = Substitute.For<IRegisters>();
            byte? firstRegisterActualValue = null;
            registersStub[0] = Arg.Do<byte>(value => firstRegisterActualValue = value);
            registersStub[0].Returns(firstRegisterActualValue);

            var logicalOrCommand = CreateLogicalOrCommand(operationCode: 0x8011);

            // Act
            logicalOrCommand.Execute();

            // Assert
            Assert.IsNull(firstRegisterActualValue);
        }
    }
}