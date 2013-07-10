using System;

namespace WonkyChip8.Interpreter.Commands
{
    public sealed class CopyRegisterValueCommand : RegisterCommand
    {
        public CopyRegisterValueCommand(int address, int operationCode, IGeneralRegisters generalRegisters)
            : base(address, operationCode, generalRegisters)
        {
            if (FirstOperationCodeHalfByte != 0x8 || FourthOperationCodeHalfByte != 0x0)
                throw new ArgumentOutOfRangeException("operationCode");
        }

        public override void Execute()
        {
            GeneralRegisters[SecondOperationCodeHalfByte] = GeneralRegisters[ThirdOperationCodeHalfByte];
        }
    }
}