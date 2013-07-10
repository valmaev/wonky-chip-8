using System;

namespace WonkyChip8.Interpreter.Commands
{
    public sealed class SaveMemoryCellValuesInGeneralRegistersCommand : RegisterCommand
    {
        private readonly IAddressRegister _addressRegister;
        private readonly IMemory _memory;

        public SaveMemoryCellValuesInGeneralRegistersCommand(int address, int operationCode,
                                                             IGeneralRegisters generalRegisters,
                                                             IAddressRegister addressRegister, IMemory memory)
            : base(address, operationCode, generalRegisters)
        {
            if (FirstOperationCodeHalfByte != 0xF || SecondOperationCodeByte != 0x65)
                throw new ArgumentOutOfRangeException("operationCode");
            if (addressRegister == null)
                throw new ArgumentNullException("addressRegister");
            if (memory == null)
                throw new ArgumentNullException("memory");

            _addressRegister = addressRegister;
            _memory = memory;
        }

        public override void Execute()
        {
            var lastRegisterIndex = SecondOperationCodeHalfByte;

            for (int registerIndex = 0; registerIndex <= lastRegisterIndex; registerIndex++)
                GeneralRegisters[registerIndex] = _memory[_addressRegister.AddressValue + registerIndex];

            _addressRegister.AddressValue += (short) (lastRegisterIndex + 1);
        }
    }
}