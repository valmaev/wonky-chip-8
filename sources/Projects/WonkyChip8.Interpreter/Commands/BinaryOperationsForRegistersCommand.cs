using System;

namespace WonkyChip8.Interpreter.Commands
{
    public class BinaryOperationsForRegistersCommand : RegisterCommand
    {
        private const int CarryRegisterIndex = 0xF;

        public BinaryOperationsForRegistersCommand(int? address, int operationCode, IRegisters registers)
            : base(address, operationCode, registers)
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
            if (FourthOperationCodeHalfByte == 0x4)
                AddFirstRegisterValueToSecondRegisterValue();
            if (FourthOperationCodeHalfByte == 0x5)
                SubtractSecondRegisterValueFromFirstRegisterValue();
            if (FourthOperationCodeHalfByte == 0x7)
                SubtractFirstRegisterValueFromSecondRegisterValue();
        }

        private void AddFirstRegisterValueToSecondRegisterValue()
        {
            var result = Registers[SecondOperationCodeHalfByte] + Registers[ThirdOperationCodeHalfByte];
            Registers[CarryRegisterIndex] = (byte?) ((result > byte.MaxValue) ? 0x1 : 0x0);
            SaveOperationResult((byte?) result);
        }

        private void SaveOperationResult(byte? binaryOperationResult)
        {
            Registers[SecondOperationCodeHalfByte] = binaryOperationResult;
        }

        private void SubtractSecondRegisterValueFromFirstRegisterValue()
        {
            var result = Registers[SecondOperationCodeHalfByte] - Registers[ThirdOperationCodeHalfByte];
            Registers[CarryRegisterIndex] = (byte?)((result < byte.MinValue) ? 0x1 : 0x0);
            SaveOperationResult((byte?)result);
        }

        private void SubtractFirstRegisterValueFromSecondRegisterValue()
        {
            var result = Registers[ThirdOperationCodeHalfByte] - Registers[SecondOperationCodeHalfByte];
            Registers[CarryRegisterIndex] = (byte?)((result < byte.MinValue) ? 0x1 : 0x0);
            SaveOperationResult((byte?)result);
        }
    }
}