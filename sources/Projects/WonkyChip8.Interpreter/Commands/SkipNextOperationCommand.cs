using System;

namespace WonkyChip8.Interpreter.Commands
{
    public class SkipNextOperationCommand : Command
    {
        private readonly IRegisters _registers;

        public SkipNextOperationCommand(int? address, int operationCode, IRegisters registers)
            : base(address, operationCode)
        {
            if (!IsOperationCodeValid)
                throw new ArgumentOutOfRangeException("operationCode");
            
            if (registers == null)
                throw new ArgumentNullException("registers");

            _registers = registers;
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
                if (SkipNextOperation == null)
                    return null;
                if (SkipNextOperation.Value)
                    return base.NextCommandAddress + CommandLength;
                return base.NextCommandAddress;
            }
        }

        private bool? SkipNextOperation
        {
            get
            {
                if (SecondOperationCodeHalfByte != null && ThirdOperationCodeHalfByte != null &&
                    FourthOperationCodeHalfByte != null)
                {
                    if (FirstOperationCodeHalfByte == 0x3)
                        return _registers[SecondOperationCodeHalfByte.Value] == SecondOperationCodeByte;
                    if (FirstOperationCodeHalfByte == 0x4)
                        return _registers[SecondOperationCodeHalfByte.Value] != SecondOperationCodeByte;
                    if (FirstOperationCodeHalfByte == 0x5)
                        return _registers[SecondOperationCodeHalfByte.Value] == _registers[ThirdOperationCodeHalfByte.Value];
                    return false;
                }
                return null;
            }
        }
    }
}