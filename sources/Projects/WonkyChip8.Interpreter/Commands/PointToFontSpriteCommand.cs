using System;

namespace WonkyChip8.Interpreter.Commands
{
    public sealed class PointToFontSpriteCommand : RegisterCommand
    {
        private const int FontMemoryOffset = 0x050;
        private const int FontSpriteHeight = 5;

        private readonly IAddressRegister _addressRegister;

        public PointToFontSpriteCommand(int address, int operationCode, IGeneralRegisters generalRegisters,
                                        IAddressRegister addressRegister)
            : base(address, operationCode, generalRegisters)
        {
            if (FirstOperationCodeHalfByte != 0xF || SecondOperationCodeByte != 0x29)
                throw new ArgumentOutOfRangeException("operationCode");
            if (generalRegisters == null)
                throw new ArgumentNullException("generalRegisters");
            if (addressRegister == null)
                throw new ArgumentNullException("addressRegister");

            _addressRegister = addressRegister;
        }

        public override void Execute()
        {
            byte fontDigit = GeneralRegisters[SecondOperationCodeHalfByte];
            _addressRegister.AddressValue = (short) (FontMemoryOffset + FontSpriteHeight*fontDigit);
        }
    }
}