using System;

namespace WonkyChip8.Interpreter.Commands
{
    public abstract class RegisterCommand : Command
    {
        protected RegisterCommand(int? address, int operationCode, IGeneralRegisters generalRegisters)
            : base(address, operationCode)
        {
            if (generalRegisters == null)
                 throw new ArgumentNullException("generalRegisters");

            GeneralRegisters = generalRegisters;
        }

        protected IGeneralRegisters GeneralRegisters { get; private set; }
    }
}