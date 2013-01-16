using System;

namespace WonkyChip8.Interpreter.Commands
{
    public class ShiftOperationsForRegistersCommand : RegisterCommand
    {
        private const int LeastSignificantBitRegisterIndex = 0xF;
        private const int MostSignificantBitRegisterIndex = 0xF;

        public ShiftOperationsForRegistersCommand(int? address, int operationCode, IGeneralRegisters generalRegisters)
            : base(address, operationCode, generalRegisters)
        {
            if (FirstOperationCodeHalfByte != 0x8 ||
                (FourthOperationCodeHalfByte != 0x6 && FourthOperationCodeHalfByte != 0xE))
                throw new ArgumentOutOfRangeException("operationCode");
        }

        public override void Execute()
        {
            switch (FourthOperationCodeHalfByte)
            {
                case 0x6:
                    RightShiftSecondRegister();
                    break;
                case 0xE:
                    LeftShiftSecondRegister();
                    break;
            }
        }

        private void RightShiftSecondRegister()
        {
            var mostSignificantBit = (byte?) (GeneralRegisters[ThirdOperationCodeHalfByte] >> 7 & 1);
            GeneralRegisters[MostSignificantBitRegisterIndex] = mostSignificantBit;
            GeneralRegisters[SecondOperationCodeHalfByte] = (byte?) (GeneralRegisters[ThirdOperationCodeHalfByte] >> 1);
        }

        private void LeftShiftSecondRegister()
        {
            var leastSignificantBit = (byte?) (GeneralRegisters[ThirdOperationCodeHalfByte] & 1);
            GeneralRegisters[LeastSignificantBitRegisterIndex] = leastSignificantBit;
            GeneralRegisters[SecondOperationCodeHalfByte] = (byte?) (GeneralRegisters[ThirdOperationCodeHalfByte] << 1);
        }
    }
}