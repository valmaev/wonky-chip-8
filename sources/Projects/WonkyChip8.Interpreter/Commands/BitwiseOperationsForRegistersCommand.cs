using System;

namespace WonkyChip8.Interpreter.Commands
{
    public class BitwiseOperationsForRegistersCommand : RegisterCommand
    {
        public BitwiseOperationsForRegistersCommand(int? address, int operationCode, IGeneralRegisters generalRegisters)
            : base(address, operationCode, generalRegisters)
        {
            if (FirstOperationCodeHalfByte != 0x8 || (FourthOperationCodeHalfByte != 0x1 &&
                FourthOperationCodeHalfByte != 0x2 && FourthOperationCodeHalfByte != 0x3))
                throw new ArgumentOutOfRangeException("operationCode");
        }

        public override void Execute()
        {
            if (FourthOperationCodeHalfByte == 0x1)
                GeneralRegisters[SecondOperationCodeHalfByte] |= GeneralRegisters[ThirdOperationCodeHalfByte];
            if (FourthOperationCodeHalfByte == 0x2)
                GeneralRegisters[SecondOperationCodeHalfByte] &= GeneralRegisters[ThirdOperationCodeHalfByte];
            if (FourthOperationCodeHalfByte == 0x3)
                GeneralRegisters[SecondOperationCodeHalfByte] ^= GeneralRegisters[ThirdOperationCodeHalfByte];
        }
    }
}