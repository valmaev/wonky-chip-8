using System;

namespace WonkyChip8.Interpreter.Commands
{
    public class DrawSpriteCommand : RegisterCommand
    {
        private const int PixelFlippingDetectorRegisterIndex = 0xF;
        private const int SpriteWidth = 8;

        private readonly IGraphicsProcessingUnit _graphicsProcessingUnit;
        private readonly IAddressRegister _addressRegister;
        private readonly IMemory _memory;

        public DrawSpriteCommand(int address, int operationCode, IGraphicsProcessingUnit graphicsProcessingUnit,
                                 IGeneralRegisters generalRegisters, IAddressRegister addressRegister, IMemory memory)
            : base(address, operationCode, generalRegisters)
        {
            if (FirstOperationCodeHalfByte != 0xD)
                throw new ArgumentOutOfRangeException("operationCode");
            if (graphicsProcessingUnit == null)
                throw new ArgumentNullException("graphicsProcessingUnit");
            if (addressRegister == null)
                throw new ArgumentNullException("addressRegister");
            if (memory == null)
                throw new ArgumentNullException("memory");

            _graphicsProcessingUnit = graphicsProcessingUnit;
            _addressRegister = addressRegister;
            _memory = memory;
        }

        public override void Execute()
        {
            var firstPixelCoordinate = new Tuple<int, int>(SecondOperationCodeHalfByte, ThirdOperationCodeHalfByte);
            var spriteHeight = FourthOperationCodeHalfByte;
            var pixels = GetPixelsFromMemory(SpriteWidth, spriteHeight);
            bool anyPixelFlipped = _graphicsProcessingUnit.DrawSprite(firstPixelCoordinate, spriteHeight, pixels);
            GeneralRegisters[PixelFlippingDetectorRegisterIndex] = Convert.ToByte(anyPixelFlipped);
        }

        private byte[,] GetPixelsFromMemory(int spriteWidth, int spriteHeight)
        {
            var pixels = new byte[spriteWidth, spriteHeight];

            for (var y = 0; y < spriteHeight; y++)
            {
                for (var x = 0; x < spriteWidth; x++)
                {
                    byte pixel = _memory[_addressRegister.AddressValue + spriteWidth * y + x];
                    pixels[x, y] = pixel;
                }
            }

            return pixels;
        }
    }
}