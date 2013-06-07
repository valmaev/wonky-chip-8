using System;

namespace WonkyChip8.Interpreter.Commands
{
    public class KeyboardCommandFactory : ICommandFactory
    {
        private readonly IGeneralRegisters _generalRegisters;
        private readonly IKeyboard _keyboard;

        public KeyboardCommandFactory(IGeneralRegisters generalRegisters, IKeyboard keyboard)
        {
            if (generalRegisters == null)
                throw new ArgumentNullException("generalRegisters");
            if (keyboard == null)
                throw new ArgumentNullException("keyboard");

            _generalRegisters = generalRegisters;
            _keyboard = keyboard;
        }

        public ICommand Create(int address, int operationCode)
        {
            switch (operationCode & 0xF0FF)
            {
                case 0xE09E:
                case 0xE0A1:
                    return new KeyboardDrivenSkipNextOperationCommand(address, operationCode, _generalRegisters,
                                                                      _keyboard);
                case 0xF00A:
                    return new WaitForKeyPressCommand(address, operationCode, _generalRegisters, _keyboard);
                default:
                    return new NullCommand();
            }
        }
    }
}