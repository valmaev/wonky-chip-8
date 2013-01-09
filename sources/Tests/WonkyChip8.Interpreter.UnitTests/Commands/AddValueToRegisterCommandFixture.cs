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
    }
}