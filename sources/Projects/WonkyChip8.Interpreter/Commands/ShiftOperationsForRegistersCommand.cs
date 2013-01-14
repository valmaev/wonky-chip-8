using System;

namespace WonkyChip8.Interpreter.Commands
{
    public class ShiftOperationsForRegistersCommand : RegisterCommand
    {
        private const int LeastSignificantBitRegisterIndex = 0xF;
        private const int MostSignificantBitRegisterIndex = 0xF;

        public ShiftOperationsForRegistersCommand(int? address, int operationCode, IRegisters registers)
            : base(address, operationCode, registers)
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
            var mostSignificantBit = (byte?) (Registers[ThirdOperationCodeHalfByte] >> 7 & 1);
            Registers[MostSignificantBitRegisterIndex] = mostSignificantBit;
            Registers[SecondOperationCodeHalfByte] = (byte?) (Registers[ThirdOperationCodeHalfByte] >> 1);
        }

        private void LeftShiftSecondRegister()
        {
            var leastSignificantBit = (byte?) (Registers[ThirdOperationCodeHalfByte] & 1);
            Registers[LeastSignificantBitRegisterIndex] = leastSignificantBit;
            Registers[SecondOperationCodeHalfByte] = (byte?) (Registers[ThirdOperationCodeHalfByte] << 1);
        }
    }
}