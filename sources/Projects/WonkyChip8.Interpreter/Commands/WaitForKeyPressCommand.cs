using System;

namespace WonkyChip8.Interpreter.Commands
{
    public sealed class WaitForKeyPressCommand : RegisterCommand
    {
        private readonly IKeyboard _keyboard;
        private int _nextCommandAddress;

        public WaitForKeyPressCommand(int address, int operationCode, IGeneralRegisters generalRegisters,
                                      IKeyboard keyboard)
            : base(address, operationCode, generalRegisters)
        {
            if (FirstOperationCodeHalfByte != 0xF || SecondOperationCodeByte != 0x0A)
                throw new ArgumentOutOfRangeException("operationCode");
            if (keyboard == null)
                throw new ArgumentNullException("keyboard");

            _keyboard = keyboard;
            _nextCommandAddress = address;
        }

        public override void Execute()
        {
            for (byte keyIndex = 0; keyIndex < _keyboard.KeysCount; keyIndex++)
                if (_keyboard.IsKeyPressed(keyIndex))
                {
                    GeneralRegisters[SecondOperationCodeHalfByte] = keyIndex;
                    _nextCommandAddress += CommandLength;
                    return;
                }
        }

        public override int NextCommandAddress
        {
            get { return _nextCommandAddress; }
        }
    }
}