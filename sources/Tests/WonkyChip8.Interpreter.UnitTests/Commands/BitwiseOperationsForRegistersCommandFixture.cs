﻿using System;
using NUnit.Framework;
using WonkyChip8.Interpreter.Commands;
using WonkyChip8.Interpreter.UnitTests.TestUtilities;
using NSubstitute;

namespace WonkyChip8.Interpreter.UnitTests.Commands
{
    [TestFixture]
    public class BitwiseOperationsForRegistersCommandFixture
    {
        private static BitwiseOperationsForRegistersCommand CreateBitwiseOperationsForRegistersCommand(
            int operationCode = 0x8001, IGeneralRegisters generalRegisters = null)
        {
            return new BitwiseOperationsForRegistersCommand(0, operationCode,
                                                            generalRegisters ?? Substitute.For<IGeneralRegisters>());
        }

        [TestCase(0x8000)]
        [TestCase(0x99999)]
        public void Constructor_WithInvalidOperationCode_ExpectedThrowsArgumentOutOfRangeException(int invalidOperationCode)
        {
            NUnitUtilities.AssertThrowsArgumentExceptionWithParamName<ArgumentOutOfRangeException>(
                () => CreateBitwiseOperationsForRegistersCommand(invalidOperationCode), "operationCode");
        }

        [Test]
        public void Constructor_WithNullGeneralRegisters_ExpectedThrowsArgumentNullException()
        {
            NUnitUtilities.AssertThrowsArgumentExceptionWithParamName<ArgumentNullException>(
                () => new BitwiseOperationsForRegistersCommand(0, 0x8001, null), "generalRegisters");
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
            TestBitwiseOperations(operationCode, firstRegisterIndex, firstRegisterInitialValue,
                                             secondRegisterIndex, secondRegisterInitialValue, firstRegisterExpectedValue);
        }

        private void TestBitwiseOperations(int operationCode,
                                           int firstRegisterIndex,
                                           byte firstRegisterInitialValue,
                                           int secondRegisterIndex,
                                           byte secondRegisterInitialValue,
                                           byte firstRegisterExpectedValue)
        {
            // Arrange
            var registersStub = Substitute.For<IGeneralRegisters>();
            byte firstRegisterActualValue = firstRegisterInitialValue;
            registersStub[firstRegisterIndex] = Arg.Do<byte>(value => firstRegisterActualValue = value);
            registersStub[firstRegisterIndex].Returns(firstRegisterActualValue);

            byte secondRegisterActualValue = secondRegisterInitialValue;
            registersStub[secondRegisterIndex] = Arg.Do<byte>(value => secondRegisterActualValue = value);
            registersStub[secondRegisterIndex].Returns(secondRegisterActualValue);

            var logicalArithmeticsForRegistersCommand =
                CreateBitwiseOperationsForRegistersCommand(operationCode, registersStub);

            // Act
            logicalArithmeticsForRegistersCommand.Execute();

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
            TestBitwiseOperations(operationCode, firstRegisterIndex, firstRegisterInitialValue,
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
            TestBitwiseOperations(operationCode, firstRegisterIndex, firstRegisterInitialValue,
                                             secondRegisterIndex, secondRegisterInitialValue, firstRegisterExpectedValue);
        }
    }
}