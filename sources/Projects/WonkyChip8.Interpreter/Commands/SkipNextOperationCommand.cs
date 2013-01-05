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
                return (FirstOperationCodeHalfBit == 0x3 || FirstOperationCodeHalfBit == 0x4 ||
                        (FirstOperationCodeHalfBit == 0x5 && FourthOperationCodeHalfBit == 0x0));
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
                if (SecondOperationCodeHalfBit != null && ThirdOperationCodeHalfBit != null &&
                    FourthOperationCodeHalfBit != null)
                {
                    if (FirstOperationCodeHalfBit == 0x3)
                        return _registers[SecondOperationCodeHalfBit.Value] == SecondOperationCodeBit;
                    if (FirstOperationCodeHalfBit == 0x4)
                        return _registers[SecondOperationCodeHalfBit.Value] != SecondOperationCodeBit;
                    if (FirstOperationCodeHalfBit == 0x5)
                        return _registers[SecondOperationCodeHalfBit.Value] == _registers[ThirdOperationCodeHalfBit.Value];
                    return false;
                }
                return null;
            }
        }
    }
}