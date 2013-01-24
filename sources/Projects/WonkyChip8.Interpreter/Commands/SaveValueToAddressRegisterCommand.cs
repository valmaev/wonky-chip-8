using System;

namespace WonkyChip8.Interpreter.Commands
{
    public class SaveValueToAddressRegisterCommand : Command
    {
        private readonly IAddressRegister _addressRegister;

        public SaveValueToAddressRegisterCommand(int address, int operationCode, IAddressRegister addressRegister)
            : base(address, operationCode)
        {
            if (FirstOperationCodeHalfByte != 0xA)
                throw new ArgumentOutOfRangeException("operationCode");
            if (addressRegister == null)
                throw new ArgumentNullException("addressRegister");

            _addressRegister = addressRegister;
        }

        public override void Execute()
        {
            _addressRegister.AddressValue = (short) (OperationCode & 0x0FFF);
        }
    }
}