using System;

namespace WonkyChip8.Interpreter.Commands
{
    public sealed class DrawSpriteCommand : RegisterCommand
    {
        private const int PixelFlippingDetectorRegisterIndex = 0xF;

        private readonly IDisplay _display;
        private readonly IAddressRegister _addressRegister;
        private readonly IMemory _memory;

        public DrawSpriteCommand(int address, int operationCode, IDisplay display,
                                 IGeneralRegisters generalRegisters, IAddressRegister addressRegister, IMemory memory)
            : base(address, operationCode, generalRegisters)
        {
            if (FirstOperationCodeHalfByte != 0xD)
                throw new ArgumentOutOfRangeException("operationCode");
            if (display == null)
                throw new ArgumentNullException("display");
            if (addressRegister == null)
                throw new ArgumentNullException("addressRegister");
            if (memory == null)
                throw new ArgumentNullException("memory");

            _display = display;
            _addressRegister = addressRegister;
            _memory = memory;
        }

        public override void Execute()
        {
            var abscissa = GeneralRegisters[SecondOperationCodeHalfByte];
            var ordinate = GeneralRegisters[ThirdOperationCodeHalfByte];
            var firstPixelCoordinate = new Tuple<int, int>(abscissa, ordinate);

            var spriteHeight = FourthOperationCodeHalfByte;
            var pixels = GetPixelsFromMemory(spriteHeight);

            bool anyPixelFlipped = _display.DrawSprite(firstPixelCoordinate, pixels);
            GeneralRegisters[PixelFlippingDetectorRegisterIndex] = Convert.ToByte(anyPixelFlipped);
        }

        private byte[] GetPixelsFromMemory(int spriteHeight)
        {
            var pixels = new byte[spriteHeight];

            for (var currentRow = 0; currentRow < spriteHeight; currentRow++)
            {
                byte pixel = _memory[_addressRegister.AddressValue + currentRow];
                pixels[currentRow] = pixel;
            }

            return pixels;
        }
    }
}