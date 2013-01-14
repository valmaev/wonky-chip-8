using System;
using NSubstitute;
using NUnit.Framework;
using WonkyChip8.Interpreter.Commands;
using WonkyChip8.Interpreter.UnitTests.TestUtilities;

namespace WonkyChip8.Interpreter.UnitTests.Commands
{
    [TestFixture]
    public class ShiftOperationsForRegistersCommandFixture
    {
        private static ShiftOperationsForRegistersCommand CreateShiftOperationsForRegistersCommand(
            int? address = 0, int operationCode = 0x8006, IRegisters registers = null)
        {
            return new ShiftOperationsForRegistersCommand(address, operationCode,
                                                          registers ?? Substitute.For<IRegisters>());
        }

        [TestCase(0x99999)]
        [TestCase(0x8FFF)]
        [TestCase(0x0006)]
        [TestCase(0x000E)]
        public void Constructor_WithInvalidOperationCode_ExpectedThrowsArgumentOutOfRangeException(
            int invalidOperationCode)
        {
            NUnitExtensions.AssertThrowsArgumentExceptionWithParamName<ArgumentOutOfRangeException>(
                () => CreateShiftOperationsForRegistersCommand(operationCode: invalidOperationCode), "operationCode");
        }

        [Test]
        public void Constructor_WithNullRegisters_ExpectedThrowsArgumentNullException()
        {
            NUnitExtensions.AssertThrowsArgumentExceptionWithParamName<ArgumentNullException>(
                () => new ShiftOperationsForRegistersCommand(0, 0x8006, null), "registers");
        }

        [TestCase(0x800E, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0)]
        [TestCase(0x800E, 0x0, 0x1, 0x0, 0x1, 0x2, 0x1)]
        [TestCase(0x812E, 0x1, 0xA, 0x2, 0xD, 0x1A, 0x1)]
        [TestCase(0x812E, 0x1, 0xA, 0x2, 0xDE, 0xBC, 0x0)]
        public void Execute_WithNotNullRegisters_ExpectedPerformRightShift(int operationCode,
                                                                           int firstRegisterIndex,
                                                                           byte firstRegisterInitialValue,
                                                                           int secondRegisterIndex,
                                                                           byte secondRegisterInitialValue,
                                                                           byte firstRegisterExpectedValue,
                                                                           byte carryRegisterExpectedValue)
        {
            TestShiftOperations(operationCode, firstRegisterIndex, firstRegisterInitialValue,
                                secondRegisterIndex, secondRegisterInitialValue, firstRegisterExpectedValue,
                                carryRegisterExpectedValue);
        }

        private static void TestShiftOperations(int operationCode,
                                                int firstRegisterIndex,
                                                byte firstRegisterInitialValue,
                                                int secondRegisterIndex,
                                                byte secondRegisterInitialValue,
                                                byte firstRegisterExpectedValue,
                                                byte carryRegisterExpectedValue)
        {
            // Arrange
            var registersStub = Substitute.For<IRegisters>();
            byte firstRegisterActualValue = firstRegisterInitialValue;
            registersStub[firstRegisterIndex] = Arg.Do<byte>(value => firstRegisterActualValue = value);
            registersStub[firstRegisterIndex].Returns(firstRegisterActualValue);

            byte secondRegisterActualValue = secondRegisterInitialValue;
            registersStub[secondRegisterIndex] = Arg.Do<byte>(value => secondRegisterActualValue = value);
            registersStub[secondRegisterIndex].Returns(secondRegisterActualValue);

            byte? carryRegisterActualValue = null;
            registersStub[0xF] = Arg.Do<byte?>(value => carryRegisterActualValue = value);
            registersStub[0xF].Returns(carryRegisterActualValue);

            var shiftOperationsForRegistersCommand =
                CreateShiftOperationsForRegistersCommand(operationCode: operationCode,
                                                         registers: registersStub);

            // Act
            shiftOperationsForRegistersCommand.Execute();

            // Assert
            Assert.AreEqual(firstRegisterExpectedValue, firstRegisterActualValue);
            Assert.AreEqual(carryRegisterExpectedValue, carryRegisterActualValue);
        }

        [TestCase(0x8006, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0)]
        [TestCase(0x8006, 0x0, 0x1, 0x0, 0x1, 0x0, 0x0)]
        [TestCase(0x8126, 0x1, 0xA, 0x2, 0xD, 0x6, 0x0)]
        [TestCase(0x8126, 0x1, 0xA, 0x2, 0xDE, 0x6F, 0x1)]
        public void Execute_WithNotNullRegisters_ExpectedPerformLeftShift(int operationCode,
                                                                          int firstRegisterIndex,
                                                                          byte firstRegisterInitialValue,
                                                                          int secondRegisterIndex,
                                                                          byte secondRegisterInitialValue,
                                                                          byte firstRegisterExpectedValue,
                                                                          byte carryRegisterExpectedValue)
        {
            TestShiftOperations(operationCode, firstRegisterIndex, firstRegisterInitialValue,
                                secondRegisterIndex, secondRegisterInitialValue, firstRegisterExpectedValue,
                                carryRegisterExpectedValue);
        }
    }
}