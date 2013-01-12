using System;

namespace WonkyChip8.Interpreter.Commands
{
    public class CopyRegisterValueCommand : RegisterCommand
    {
        public CopyRegisterValueCommand(int? address, int operationCode, IRegisters registers)
            : base(address, operationCode, registers)
        {
            if (FirstOperationCodeHalfByte != 0x8 || FourthOperationCodeHalfByte != 0x0)
                throw new ArgumentOutOfRangeException("operationCode");
        }

        public override void Execute()
        {
            Registers[SecondOperationCodeHalfByte] = Registers[ThirdOperationCodeHalfByte];
        }
    }
}