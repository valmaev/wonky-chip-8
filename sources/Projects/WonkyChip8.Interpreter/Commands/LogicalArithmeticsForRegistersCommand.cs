using System;

namespace WonkyChip8.Interpreter.Commands
{
    public class LogicalArithmeticsForRegistersCommand : RegisterCommand
    {
        public LogicalArithmeticsForRegistersCommand(int? address, int operationCode, IRegisters registers)
            : base(address, operationCode, registers)
        {
            if (FirstOperationCodeHalfByte != 0x8 || (FourthOperationCodeHalfByte != 0x1 &&
                FourthOperationCodeHalfByte != 0x2 && FourthOperationCodeHalfByte != 0x3))
                throw new ArgumentOutOfRangeException("operationCode");
        }

        public override void Execute()
        {
            if (FourthOperationCodeHalfByte == 0x1)
                Registers[SecondOperationCodeHalfByte] |= Registers[ThirdOperationCodeHalfByte];
            if (FourthOperationCodeHalfByte == 0x2)
                Registers[SecondOperationCodeHalfByte] &= Registers[ThirdOperationCodeHalfByte];
            if (FourthOperationCodeHalfByte == 0x3)
                Registers[SecondOperationCodeHalfByte] ^= Registers[ThirdOperationCodeHalfByte];
        }
    }
}