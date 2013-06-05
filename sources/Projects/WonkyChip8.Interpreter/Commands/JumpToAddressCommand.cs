using System;

namespace WonkyChip8.Interpreter.Commands
{
    public class JumpToAddressCommand : RegisterCommand
    {
        public JumpToAddressCommand(int address, int operationCode, IGeneralRegisters generalRegisters)
            : base(address, operationCode, generalRegisters)
        {
            if (FirstOperationCodeHalfByte != 0x1 && FirstOperationCodeHalfByte != 0xB)
                throw new ArgumentOutOfRangeException("operationCode");
        }

        public override int NextCommandAddress
        {
            get
            {
                switch (FirstOperationCodeHalfByte)
                {
                    case 0x1:
                        return OperationCode & 0x0FFF;
                    case 0xB:
                        return (OperationCode & 0x0FFF) + GeneralRegisters[0];
                    default:
                        throw new InvalidOperationException(string.Format("Operation code {0:X4} isn't supported",
                                                                          OperationCode));
                }
            }
        }
    }
}