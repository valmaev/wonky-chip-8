using System;
using NUnit.Framework;
using WonkyChip8.Interpreter.Commands;
using WonkyChip8.Interpreter.UnitTests.TestUtilities;

namespace WonkyChip8.Interpreter.UnitTests.Commands
{
    [TestFixture]
    public class RegisterCommandFixture
    {
        private class RegisterCommandStub : RegisterCommand
        {
            public RegisterCommandStub(int address, int operationCode, IGeneralRegisters generalRegisters)
                : base(address, operationCode, generalRegisters) { }
        }

        [Test]
        public void Constructor_WithNullGeneralRegisters_ExpectedThrowsArgumentNullException()
        {
            NUnitUtilities.AssertThrowsArgumentExceptionWithParamName<ArgumentNullException>(
                () => new RegisterCommandStub(0, 0, null), "generalRegisters");
        }
    }
}