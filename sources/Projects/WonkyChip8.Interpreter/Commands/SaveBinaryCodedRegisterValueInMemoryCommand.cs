using System;

namespace WonkyChip8.Interpreter.Commands
{
    public sealed class SaveBinaryCodedRegisterValueInMemoryCommand : RegisterCommand
    {
        private readonly IAddressRegister _addressRegister;
        private readonly IMemory _memory;

        public SaveBinaryCodedRegisterValueInMemoryCommand(int address, int operationCode,
                                                           IGeneralRegisters generalRegisters,
                                                           IAddressRegister addressRegister,
                                                           IMemory memory)
            : base(address, operationCode, generalRegisters)
        {
            if (FirstOperationCodeHalfByte != 0xF || SecondOperationCodeByte != 0x33)
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
            var registerValue = GeneralRegisters[SecondOperationCodeHalfByte];
            _memory[_addressRegister.AddressValue] = (byte) (registerValue / 100);
            _memory[_addressRegister.AddressValue + 1] = (byte)((registerValue % 100) / 10);
            _memory[_addressRegister.AddressValue + 2] = (byte) ((registerValue%100)%10);
        }
    }
}