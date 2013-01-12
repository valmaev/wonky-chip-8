using System;

namespace WonkyChip8.Interpreter.Commands
{
    public class SkipNextOperationCommand : RegisterCommand
    {
        public SkipNextOperationCommand(int? address, int operationCode, IRegisters registers)
            : base(address, operationCode, registers)
        {
            if (!IsOperationCodeValid)
                throw new ArgumentOutOfRangeException("operationCode");
        }

        private bool IsOperationCodeValid
        {
            get
            {
                return (FirstOperationCodeHalfByte == 0x3 || FirstOperationCodeHalfByte == 0x4 ||
                        (FirstOperationCodeHalfByte == 0x5 && FourthOperationCodeHalfByte == 0x0));
            }
        }

        public override int? NextCommandAddress
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
                    return Registers[SecondOperationCodeHalfByte] == SecondOperationCodeByte;
                if (FirstOperationCodeHalfByte == 0x4)
                    return Registers[SecondOperationCodeHalfByte] != SecondOperationCodeByte;
                if (FirstOperationCodeHalfByte == 0x5)
                    return Registers[SecondOperationCodeHalfByte] == Registers[ThirdOperationCodeHalfByte];
                return false;
            }
        }
    }
}