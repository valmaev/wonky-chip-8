using System;

namespace WonkyChip8.Interpreter.Commands
{
    public class SaveValueToRegisterCommand : RegisterCommand
    {
        public SaveValueToRegisterCommand(int? address, int operationCode, IGeneralRegisters generalRegisters)
            : base(address, operationCode, generalRegisters)
        {
            if (FirstOperationCodeHalfByte != 0x6)
                throw new ArgumentOutOfRangeException("operationCode");
        }

        public override void Execute()
        {
            GeneralRegisters[SecondOperationCodeHalfByte] = (byte?) SecondOperationCodeByte;
        }
    }
}