using System;

namespace WonkyChip8.Interpreter.Commands
{
    public class SaveGeneralRegistersValuesInMemoryCommand : RegisterCommand
    {
        private readonly IAddressRegister _addressRegister;
        private readonly IMemory _memory;

        public SaveGeneralRegistersValuesInMemoryCommand(int address, int operationCode,
                                                         IGeneralRegisters generalRegisters,
                                                         IAddressRegister addressRegister, IMemory memory)
            : base(address, operationCode, generalRegisters)
        {
            if (FirstOperationCodeHalfByte != 0xF || SecondOperationCodeByte != 0x55)
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
                _memory[_addressRegister.AddressValue + registerIndex] = GeneralRegisters[registerIndex];
        }
    }
}