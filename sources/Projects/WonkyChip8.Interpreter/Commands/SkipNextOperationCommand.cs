using System;

namespace WonkyChip8.Interpreter.Commands
{
    public sealed class SkipNextOperationCommand : RegisterCommand
    {
        public SkipNextOperationCommand(int address, int operationCode, IGeneralRegisters generalRegisters)
            : base(address, operationCode, generalRegisters)
        {
            if (!IsOperationCodeValid)
                throw new ArgumentOutOfRangeException("operationCode");
        }

        private bool IsOperationCodeValid
        {
            get
            {
                return (FirstOperationCodeHalfByte == 0x3 ||
                        FirstOperationCodeHalfByte == 0x4 ||
                        FirstOperationCodeHalfByte == 0x5 && FourthOperationCodeHalfByte == 0x0 ||
                        FirstOperationCodeHalfByte == 0x9 && FourthOperationCodeHalfByte == 0x0);
            }
        }

        public override int NextCommandAddress
        {
            get
            {
                if (SkipNextOperation)
                    return base.NextCommandAddress + CommandLength;
                return base.NextCommandAddress;
            }
        }

        private bool SkipNextOperation
        {
            get
            {
                if (FirstOperationCodeHalfByte == 0x3)
                    return GeneralRegisters[SecondOperationCodeHalfByte] == SecondOperationCodeByte;
                if (FirstOperationCodeHalfByte == 0x4)
                    return GeneralRegisters[SecondOperationCodeHalfByte] != SecondOperationCodeByte;
                if (FirstOperationCodeHalfByte == 0x5)
                    return GeneralRegisters[SecondOperationCodeHalfByte] == GeneralRegisters[ThirdOperationCodeHalfByte];
                if (FirstOperationCodeHalfByte == 0x9)
                    return GeneralRegisters[SecondOperationCodeHalfByte] != GeneralRegisters[ThirdOperationCodeHalfByte];
                throw new InvalidOperationException(string.Format("Operation code {0:X4} isn't supported", OperationCode));
            }
        }
    }
}