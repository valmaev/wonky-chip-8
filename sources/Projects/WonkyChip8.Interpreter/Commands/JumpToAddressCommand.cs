using System;

namespace WonkyChip8.Interpreter.Commands
{
    public class JumpToAddressCommand : RegisterCommand
    {
        private readonly int _nextCommandAddress;

        public JumpToAddressCommand(int? address, int operationCode, IGeneralRegisters generalRegisters)
            : base(address, operationCode, generalRegisters)
        {
            if (FirstOperationCodeHalfByte != 0x1 && FirstOperationCodeHalfByte != 0xB)
                throw new ArgumentOutOfRangeException("operationCode");

            _nextCommandAddress = operationCode & 0x0FFF;
        }

        public override int? NextCommandAddress
        {
            get
            {
                if (FirstOperationCodeHalfByte == 0x1)
                    return _nextCommandAddress;
                if (FirstOperationCodeHalfByte == 0xB)
                    return _nextCommandAddress + (GeneralRegisters[0] ?? 0);
                throw new InvalidOperationException(string.Format("Operation code {0} isn't supported", OperationCode));
            }
        }
    }
}