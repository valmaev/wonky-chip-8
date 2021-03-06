﻿using System;
using NSubstitute;
using NUnit.Framework;
using WonkyChip8.Interpreter.Commands;
using WonkyChip8.Interpreter.UnitTests.TestUtilities;

namespace WonkyChip8.Interpreter.UnitTests.Commands
{
    [TestFixture]
    public class BinaryOperationsForRegistersCommandFixture
    {
        private static BinaryOperationsForRegistersCommand CreateBinaryOperationsForRegistersCommand(
            int operationCode = 0x8004, IGeneralRegisters generalRegisters = null)
        {
            return new BinaryOperationsForRegistersCommand(0, operationCode,
                                                           generalRegisters ?? Substitute.For<IGeneralRegisters>());
        }

        [TestCase(0x8000)]
        [TestCase(0x0004)]
        [TestCase(0x0005)]
        [TestCase(0x0007)]
        [TestCase(0x99999)]
        public void Constructor_WithInvalidOperationCode_ExpectedThrowsArgumentOutOfRangeException(int invalidOperationCode)
        {
            NUnitUtilities.AssertThrowsArgumentExceptionWithParamName<ArgumentOutOfRangeException>(
                () => CreateBinaryOperationsForRegistersCommand(invalidOperationCode), "operationCode");
        }

        [Test]
        public void Constructor_WithNullGeneralRegisters_ExpectedThrowsArgumentNullException()
        {
            NUnitUtilities.AssertThrowsArgumentExceptionWithParamName<ArgumentNullException>(
                () => new BinaryOperationsForRegistersCommand(0, 0x8004, null), "generalRegisters");
        }

        [TestCase(0x8004, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0)]
        [TestCase(0x8004, 0x0, 0x1, 0x0, 0x1, 0x2, 0x0)]
        [TestCase(0x8124, 0x1, 0x0, 0x2, 0x0, 0x0, 0x0)]
        [TestCase(0x8124, 0x1, 0xA, 0x2, 0xD, 0x17, 0x0)]
        [TestCase(0x8124, 0x1, 0xFF, 0x2, 0xFF, 0xFE, 0x1)]
        [TestCase(0x8124, 0x1, 0xFE, 0x2, 0xFD, 0xFB, 0x1)]
        public void Execute_WithNotNullRegistersValues_ExpectedPerformAddingWithCarry(int operationCode,
                                                                                      int firstRegisterIndex,
                                                                                      byte firstRegisterInitialValue,
                                                                                      int secondRegisterIndex,
                                                                                      byte secondRegisterInitialValue,
                                                                                      byte firstRegisterExpectedValue,
                                                                                      byte carryRegisterExpectedValue)
        {
            TestBinaryOperations(operationCode, firstRegisterIndex, firstRegisterInitialValue,
                                 secondRegisterIndex, secondRegisterInitialValue, firstRegisterExpectedValue,
                                 carryRegisterExpectedValue);
        }

        private void TestBinaryOperations(int operationCode,
                                          int firstRegisterIndex,
                                          byte firstRegisterInitialValue,
                                          int secondRegisterIndex,
                                          byte secondRegisterInitialValue,
                                          byte firstRegisterExpectedValue,
                                          byte carryRegisterExpectedValue)
        {
            // Arrange
            var registersStub = Substitute.For<IGeneralRegisters>();
            byte firstRegisterActualValue = firstRegisterInitialValue;
            registersStub[firstRegisterIndex] = Arg.Do<byte>(value => firstRegisterActualValue = value);
            registersStub[firstRegisterIndex].Returns(firstRegisterActualValue);

            byte secondRegisterActualValue = secondRegisterInitialValue;
            registersStub[secondRegisterIndex] = Arg.Do<byte>(value => secondRegisterActualValue = value);
            registersStub[secondRegisterIndex].Returns(secondRegisterActualValue);

            byte carryRegisterActualValue = 0;
            registersStub[0xF] = Arg.Do<byte>(value => carryRegisterActualValue = value);
            registersStub[0xF].Returns(carryRegisterActualValue);

            var binaryOperationsForRegistersCommand = CreateBinaryOperationsForRegistersCommand(operationCode,
                                                                                                registersStub);
            // Act
            binaryOperationsForRegistersCommand.Execute();

            // Assert
            Assert.AreEqual(firstRegisterExpectedValue, firstRegisterActualValue);
            Assert.AreEqual(carryRegisterExpectedValue, carryRegisterActualValue);
        }

        [TestCase(0x8005, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0)]
        [TestCase(0x8005, 0x0, 0x1, 0x0, 0x1, 0x0, 0x0)]
        [TestCase(0x8125, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0)]
        [TestCase(0x8125, 0x1, 0xA, 0x2, 0x1, 0x9, 0x0)]
        [TestCase(0x8125, 0x1, 0x4, 0x2, 0x5, 0xFF, 0x1)]
        [TestCase(0x8125, 0x1, 0x2, 0x2, 0xF, 0xF3, 0x1)]
        public void Execute_WithNotNullRegistersValues_ExpectedPerformSubtractWithBorrow(int operationCode,
                                                                                         int firstRegisterIndex,
                                                                                         byte firstRegisterInitialValue,
                                                                                         int secondRegisterIndex,
                                                                                         byte secondRegisterInitialValue,
                                                                                         byte firstRegisterExpectedValue,
                                                                                         byte carryRegisterExpectedValue)
        {
            TestBinaryOperations(operationCode, firstRegisterIndex, firstRegisterInitialValue,
                                 secondRegisterIndex, secondRegisterInitialValue, firstRegisterExpectedValue,
                                 carryRegisterExpectedValue);
        }

        [TestCase(0x8007, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0)]
        [TestCase(0x8007, 0x0, 0x1, 0x0, 0x1, 0x0, 0x0)]
        [TestCase(0x8127, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0)]
        [TestCase(0x8127, 0x1, 0xA, 0x2, 0x1, 0xF7, 0x1)]
        [TestCase(0x8127, 0x1, 0x4, 0x2, 0x5, 0x1, 0x0)]
        [TestCase(0x8127, 0x1, 0x2, 0x2, 0xF, 0xD, 0x0)]
        public void Execute_WithNotNullRegistersValues_ExpectedPerformSubtractWithCarry(int operationCode,
                                                                                        int firstRegisterIndex,
                                                                                        byte firstRegisterInitialValue,
                                                                                        int secondRegisterIndex,
                                                                                        byte secondRegisterInitialValue,
                                                                                        byte firstRegisterExpectedValue,
                                                                                        byte carryRegisterExpectedValue)
        {
            TestBinaryOperations(operationCode, firstRegisterIndex, firstRegisterInitialValue,
                                 secondRegisterIndex, secondRegisterInitialValue, firstRegisterExpectedValue,
                                 carryRegisterExpectedValue);
        }
    }
}