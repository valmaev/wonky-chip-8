using System;

namespace WonkyChip8.Interpreter.Commands
{
    public class AddValueToRegisterCommand : RegisterCommand
    {
        public AddValueToRegisterCommand(int? address, int operationCode, IRegisters registers)
            : base(address, operationCode, registers)
        {
            if (FirstOperationCodeHalfByte != 0x7)
                throw new ArgumentOutOfRangeException("operationCode");
        }

        public override void Execute()
        {
            byte registerValue = Registers[SecondOperationCodeHalfByte] ?? 0;
            registerValue += (byte) SecondOperationCodeByte;
            Registers[SecondOperationCodeHalfByte] = registerValue;
        }
    }
}