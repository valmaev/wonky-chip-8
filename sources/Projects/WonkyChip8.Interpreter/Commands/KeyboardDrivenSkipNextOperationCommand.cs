using System;

namespace WonkyChip8.Interpreter.Commands
{
    public class KeyboardDrivenSkipNextOperationCommand : RegisterCommand
    {
        private readonly IKeyboard _keyboard;

        public KeyboardDrivenSkipNextOperationCommand(int address, int operationCode, IGeneralRegisters generalRegisters,
                                                      IKeyboard keyboard)
            : base(address, operationCode, generalRegisters)
        {
            if (FirstOperationCodeHalfByte != 0xE ||
                (SecondOperationCodeByte != 0x9E && SecondOperationCodeByte != 0xA1))
                throw new ArgumentOutOfRangeException("operationCode");
            if (keyboard == null)
                throw new ArgumentNullException("keyboard");

            _keyboard = keyboard;
        }

        public override int NextCommandAddress
        {
            get
            {
                switch (SecondOperationCodeByte)
                {
                    case 0x9E:
                        if (_keyboard.IsKeyPressed(GeneralRegisters[SecondOperationCodeHalfByte]))
                            return base.NextCommandAddress + CommandLength;
                        return base.NextCommandAddress;
                    case 0xA1:
                        if (!_keyboard.IsKeyPressed(GeneralRegisters[SecondOperationCodeHalfByte]))
                            return base.NextCommandAddress + CommandLength;
                        return base.NextCommandAddress;
                    default:
                        throw new InvalidOperationException(string.Format("Operation code {0:X4} isn't supported",
                                                                          OperationCode));
                }
            }
        }
    }
}