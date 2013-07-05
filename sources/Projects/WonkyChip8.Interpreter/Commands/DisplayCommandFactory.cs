using System;

namespace WonkyChip8.Interpreter.Commands
{
    public class DisplayCommandFactory : ICommandFactory
    {
        private readonly IGeneralRegisters _generalRegisters;
        private readonly IAddressRegister _addressRegister;
        private readonly IMemory _memory;
        private readonly IDisplay _display;

        public DisplayCommandFactory(IGeneralRegisters generalRegisters, IAddressRegister addressRegister,
                                     IMemory memory, IDisplay display)
        {
            if (generalRegisters == null)
                throw new ArgumentNullException("generalRegisters");
            if (addressRegister == null)
                throw new ArgumentNullException("addressRegister");
            if (memory == null)
                throw new ArgumentNullException("memory");
            if (display == null)
                throw new ArgumentNullException("display");

            _generalRegisters = generalRegisters;
            _addressRegister = addressRegister;
            _memory = memory;
            _display = display;
        }

        public ICommand Create(int address, int operationCode)
        {
            if (operationCode == 0x00E0)
                return new ClearScreenCommand(address, _display);
            if ((operationCode & 0xF000) == 0xD000)
                return new DrawSpriteCommand(address, operationCode, _display, _generalRegisters,
                                             _addressRegister, _memory);
            return new NullCommand();
        }
    }
}