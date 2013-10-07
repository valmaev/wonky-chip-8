using System;

namespace WonkyChip8.Interpreter.Commands
{
    public sealed class ShiftOperationsForRegistersCommand : RegisterCommand
    {
        private const int LeastSignificantBitRegisterIndex = 0xF;
        private const int MostSignificantBitRegisterIndex = 0xF;

        public ShiftOperationsForRegistersCommand(int address, int operationCode, IGeneralRegisters generalRegisters)
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
                default:
                    throw new InvalidOperationException(string.Format("Operation code {0:X4} isn't supported",
                                                                      OperationCode));
            }
        }

        private void RightShiftSecondRegister()
        {
            int mostSignificantBit = GeneralRegisters[SecondOperationCodeHalfByte] >> 7 & 1;
            GeneralRegisters[MostSignificantBitRegisterIndex] = (byte) mostSignificantBit;
            GeneralRegisters[SecondOperationCodeHalfByte] >>= 1;
        }

        private void LeftShiftSecondRegister()
        {
            int leastSignificantBit = GeneralRegisters[SecondOperationCodeHalfByte] & 1;
            GeneralRegisters[LeastSignificantBitRegisterIndex] = (byte) leastSignificantBit;
            GeneralRegisters[SecondOperationCodeHalfByte] <<= 1;
        }
    }
}