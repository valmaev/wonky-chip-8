using System;
using NSubstitute;
using NUnit.Framework;
using WonkyChip8.Interpreter.Commands;
using WonkyChip8.Interpreter.UnitTests.TestUtilities;

namespace WonkyChip8.Interpreter.UnitTests.Commands
{
    [TestFixture]
    public class AddValueToRegisterCommandFixture
    {
        private static AddValueToRegisterCommand CreateAddValueToRegisterCommand(int? address = 0,
                                                                                 int operationCode = 0x7000,
                                                                                 IRegisters registers = null)
        {
            return new AddValueToRegisterCommand(address, operationCode, registers ?? Substitute.For<IRegisters>());
        }

        [Test]
        public void Constructor_WithInvalidOperationCode_ExpectedThrowsArgumentOutOfRangeException()
        {
            NUnitExtensions.AssertThrowsArgumentExceptionWithParamName<ArgumentOutOfRangeException>(
                () => CreateAddValueToRegisterCommand(operationCode: 0x9999), "operationCode");
        }

        [Test]
        public void Constructor_WithNullRegisters_ExpectedThrowsArgumentNullException()
        {
            NUnitExtensions.AssertThrowsArgumentExceptionWithParamName<ArgumentNullException>(
                () => new AddValueToRegisterCommand(0, 0x7000, null), "registers");
        }

        [TestCase(0x7000, 0x0, 0x0, 0x0)]
        [TestCase(0x7001, 0x0, 0x0, 0x1)]
        [TestCase(0x7A50, 0xA, 0x25, 0x75)]
        public void Execute_WithProperOperationCode_ExpectedAddValueToRegister(int operationCode, int registerIndex,
                                                                               byte registerInitialValue,
                                                                               byte expectedResult)
        {
            // Arrange
            var registersStub = Substitute.For<IRegisters>();
            byte? registerValue = registerInitialValue;
            registersStub[registerIndex] = Arg.Do<byte?>(arg => registerValue = arg);
            registersStub[registerIndex].Returns(registerValue);

            var addValueToRegisterCommand = CreateAddValueToRegisterCommand(0, operationCode, registersStub);

            // Act
            addValueToRegisterCommand.Execute();

            // Assert
            Assert.AreEqual(registerValue, expectedResult);
        }

        [Test]
        public void Execute_WithRegisterInitialValueEqualsNull_ExpectedAddValueToRegister()
        {
            // Arrange
            var registersStub = Substitute.For<IRegisters>();
            byte? registerValue = null;
            registersStub[Arg.Any<int>()] = Arg.Do<byte?>(arg => registerValue = arg);
            registersStub[Arg.Any<int>()].Returns(registerValue);

            var addValueToRegisterCommand = CreateAddValueToRegisterCommand(0, 0x7010, registersStub);

            // Act
            addValueToRegisterCommand.Execute();

            // Assert
            Assert.AreEqual(registerValue, 0x10);
        }
    }
}