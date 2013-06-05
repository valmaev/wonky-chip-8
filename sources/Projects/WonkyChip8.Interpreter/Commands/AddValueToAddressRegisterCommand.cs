using System;

namespace WonkyChip8.Interpreter.Commands
{
    public class AddValueToAddressRegisterCommand : RegisterCommand
    {
        private readonly IAddressRegister _addressRegister;

        public AddValueToAddressRegisterCommand(int address, int operationCode, IGeneralRegisters generalRegisters,
                                                IAddressRegister addressRegister)
            : base(address, operationCode, generalRegisters)
        {
            if (FirstOperationCodeHalfByte != 0xF && SecondOperationCodeByte != 0x1E)
                throw new ArgumentOutOfRangeException("operationCode");
            if (addressRegister == null)
                throw new ArgumentNullException("addressRegister");

            _addressRegister = addressRegister;
        }

        public override void Execute()
        {
            _addressRegister.AddressValue += GeneralRegisters[SecondOperationCodeHalfByte];

            if (_addressRegister.AddressValue > 0xFFF)
                GeneralRegisters[0xF] = 1;
            else
                GeneralRegisters[0xF] = 0;
        }
    }
}