using System;

namespace WonkyChip8.Interpreter.Commands
{
    public class SaveValueToRegisterCommand : RegisterCommand
    {
        public SaveValueToRegisterCommand(int? address, int operationCode, IRegisters registers)
            : base(address, operationCode, registers)
        {
            if (FirstOperationCodeHalfByte != 0x6)
                throw new ArgumentOutOfRangeException("operationCode");
        }

        public override void Execute()
        {
            Registers[SecondOperationCodeHalfByte] = (byte?) SecondOperationCodeByte;
        }
    }
}