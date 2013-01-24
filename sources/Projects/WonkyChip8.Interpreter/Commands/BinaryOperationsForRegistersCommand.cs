using System;

namespace WonkyChip8.Interpreter.Commands
{
    public class BinaryOperationsForRegistersCommand : RegisterCommand
    {
        private const int CarryRegisterIndex = 0xF;

        public BinaryOperationsForRegistersCommand(int address, int operationCode, IGeneralRegisters generalRegisters)
            : base(address, operationCode, generalRegisters)
        {
            if (!IsOperationCodeValid)
                throw new ArgumentOutOfRangeException("operationCode");
        }

        private bool IsOperationCodeValid
        {
            get
            {
                return FirstOperationCodeHalfByte == 0x8 &&
                       (FourthOperationCodeHalfByte == 0x4 || FourthOperationCodeHalfByte == 0x5 ||
                        FourthOperationCodeHalfByte == 0x7);
            }
        }

        public override void Execute()
        {
            switch (FourthOperationCodeHalfByte)
            {
                case 0x4:
                    AddFirstRegisterValueToSecondRegisterValue();
                    break;
                case 0x5:
                    SubtractSecondRegisterValueFromFirstRegisterValue();
                    break;
                case 0x7:
                    SubtractFirstRegisterValueFromSecondRegisterValue();
                    break;
                default:
                    throw new InvalidOperationException(string.Format("Operation code {0:X4} isn't supported",
                                                                      OperationCode));
            }
        }

        private void AddFirstRegisterValueToSecondRegisterValue()
        {
            int result = GeneralRegisters[SecondOperationCodeHalfByte] + GeneralRegisters[ThirdOperationCodeHalfByte];
            GeneralRegisters[CarryRegisterIndex] = (byte) ((result > byte.MaxValue) ? 0x1 : 0x0);
            SaveOperationResult((byte) result);
        }

        private void SaveOperationResult(byte binaryOperationResult)
        {
            GeneralRegisters[SecondOperationCodeHalfByte] = binaryOperationResult;
        }

        private void SubtractSecondRegisterValueFromFirstRegisterValue()
        {
            int result = GeneralRegisters[SecondOperationCodeHalfByte] - GeneralRegisters[ThirdOperationCodeHalfByte];
            GeneralRegisters[CarryRegisterIndex] = (byte) ((result < byte.MinValue) ? 0x1 : 0x0);
            SaveOperationResult((byte) result);
        }

        private void SubtractFirstRegisterValueFromSecondRegisterValue()
        {
            var result = GeneralRegisters[ThirdOperationCodeHalfByte] - GeneralRegisters[SecondOperationCodeHalfByte];
            GeneralRegisters[CarryRegisterIndex] = (byte) ((result < byte.MinValue) ? 0x1 : 0x0);
            SaveOperationResult((byte) result);
        }
    }
}