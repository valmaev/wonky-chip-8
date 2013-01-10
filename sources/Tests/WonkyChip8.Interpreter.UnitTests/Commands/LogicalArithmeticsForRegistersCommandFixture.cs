using System;
using NUnit.Framework;
using WonkyChip8.Interpreter.Commands;
using WonkyChip8.Interpreter.UnitTests.TestUtilities;
using NSubstitute;

namespace WonkyChip8.Interpreter.UnitTests.Commands
{
    [TestFixture]
    public class LogicalArithmeticsForRegistersCommandFixture
    {
        private static LogicalArithmeticsForRegistersCommand CreateLogicalArithmeticsForRegistersCommand(int? address = 0,
                                                                                       int operationCode = 0x8001,
                                                                                       IRegisters registers = null)
        {
            return new LogicalArithmeticsForRegistersCommand(address, operationCode, registers ?? Substitute.For<IRegisters>());
        }

        [TestCase(0x8000)]
        [TestCase(0x99999)]
        public void Constructor_WithInvalidOperationCode_ExpectedThrowsArgumentOutOfRangeException(int invalidOperationCode)
        {
            NUnitExtensions.AssertThrowsArgumentExceptionWithParamName<ArgumentOutOfRangeException>(
                () => CreateLogicalArithmeticsForRegistersCommand(operationCode: invalidOperationCode), "operationCode");
        }

        [Test]
        public void Constructor_WithNullRegisters_ExpectedThrowsArgumentNullException()
        {
            NUnitExtensions.AssertThrowsArgumentExceptionWithParamName<ArgumentNullException>(
                () => new LogicalArithmeticsForRegistersCommand(0, 0x8001, null), "registers");
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
            TestLogicalArithmeticsOperations(operationCode, firstRegisterIndex, firstRegisterInitialValue,
                                             secondRegisterIndex, secondRegisterInitialValue, firstRegisterExpectedValue);
        }

        private void TestLogicalArithmeticsOperations(int operationCode,
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

            var logicalOrCommand = CreateLogicalArithmeticsForRegistersCommand(operationCode: operationCode,
                                                                               registers: registersStub);

            // Act
            logicalOrCommand.Execute();

            // Assert
            Assert.AreEqual(firstRegisterExpectedValue, firstRegisterActualValue);

        }

        [TestCase(0x8002, 0x0, 0x0, 0x0, 0x0, 0x0)]
        [TestCase(0x8002, 0x0, 0xF, 0x0, 0xF, 0xF)]
        [TestCase(0x8012, 0x0, 0x0, 0x1, 0x0, 0x0)]
        [TestCase(0x8012, 0x0, 0x0, 0x1, 0x1, 0x0)]
        [TestCase(0x8012, 0x0, 0x1, 0x1, 0x1, 0x1)]
        [TestCase(0x8A02, 0xA, 0xB, 0x0, 0xD, 0x9)]
        public void Execute_WithNotNullRegistersValue_ExpectedPerfomLogicalAnd(int operationCode,
                                                                               int firstRegisterIndex,
                                                                               byte firstRegisterInitialValue,
                                                                               int secondRegisterIndex,
                                                                               byte secondRegisterInitialValue,
                                                                               byte firstRegisterExpectedValue)
        {
            TestLogicalArithmeticsOperations(operationCode, firstRegisterIndex, firstRegisterInitialValue,
                                             secondRegisterIndex, secondRegisterInitialValue, firstRegisterExpectedValue);
        }

        [TestCase(0x8003, 0x0, 0x0, 0x0, 0x0, 0x0)]
        [TestCase(0x8003, 0x0, 0xF, 0x0, 0xF, 0x0)]
        [TestCase(0x8013, 0x0, 0x0, 0x1, 0x0, 0x0)]
        [TestCase(0x8013, 0x0, 0x0, 0x1, 0x1, 0x1)]
        [TestCase(0x8013, 0x0, 0x1, 0x1, 0x1, 0x0)]
        [TestCase(0x8A03, 0xA, 0xB, 0x0, 0xD, 0x6)]
        public void Execute_WithNotNullRegistersValue_ExpectedPerfomLogicalXor(int operationCode,
                                                                               int firstRegisterIndex,
                                                                               byte firstRegisterInitialValue,
                                                                               int secondRegisterIndex,
                                                                               byte secondRegisterInitialValue,
                                                                               byte firstRegisterExpectedValue)
        {
            TestLogicalArithmeticsOperations(operationCode, firstRegisterIndex, firstRegisterInitialValue,
                                             secondRegisterIndex, secondRegisterInitialValue, firstRegisterExpectedValue);
        }

        [TestCase(0x8001)]
        [TestCase(0x8002)]
        [TestCase(0x8003)]
        public void Execute_WithNullRegistersValue_ExpectedNull(int operationCode)
        {
            // Arrange
            var registersStub = Substitute.For<IRegisters>();
            byte? firstRegisterActualValue = null;
            registersStub[0] = Arg.Do<byte>(value => firstRegisterActualValue = value);
            registersStub[0].Returns(firstRegisterActualValue);

            var logicalOrCommand = CreateLogicalArithmeticsForRegistersCommand(operationCode: operationCode);

            // Act
            logicalOrCommand.Execute();

            // Assert
            Assert.IsNull(firstRegisterActualValue);
        }
    }
}