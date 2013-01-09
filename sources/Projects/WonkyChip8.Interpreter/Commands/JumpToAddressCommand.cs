using System;

namespace WonkyChip8.Interpreter.Commands
{
    public class JumpToAddressCommand : Command
    {
        private readonly int _nextCommandAddress;

        public JumpToAddressCommand(int? address, int operationCode) : base(address, operationCode)
        {
            if (FirstOperationCodeHalfByte != 0x1)
                throw new ArgumentOutOfRangeException("operationCode");

            _nextCommandAddress = operationCode & 0x0FFF;
        }

        public override int? NextCommandAddress
        {
            get { return _nextCommandAddress; }
        }
    }
}