using System;

namespace WonkyChip8.Interpreter.Commands
{
    public abstract class RegisterCommand : Command
    {
        protected RegisterCommand(int? address, int operationCode, IRegisters registers)
            : base(address, operationCode)
        {
            if (registers == null)
                 throw new ArgumentNullException("registers");

            Registers = registers;
        }

        protected IRegisters Registers { get; private set; }
    }
}