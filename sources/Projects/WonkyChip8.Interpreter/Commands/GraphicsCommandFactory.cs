using System;

namespace WonkyChip8.Interpreter.Commands
{
    public class GraphicsCommandFactory : ICommandFactory
    {
        private readonly IGeneralRegisters _generalRegisters;
        private readonly IAddressRegister _addressRegister;
        private readonly IMemory _memory;
        private readonly IGraphicsProcessingUnit _graphicsProcessingUnit;

        public GraphicsCommandFactory(IGeneralRegisters generalRegisters, IAddressRegister addressRegister,
                                      IMemory memory, IGraphicsProcessingUnit graphicsProcessingUnit)
        {
            if (generalRegisters == null)
                throw new ArgumentNullException("generalRegisters");
            if (addressRegister == null)
                throw new ArgumentNullException("addressRegister");
            if (memory == null)
                throw new ArgumentNullException("memory");
            if (graphicsProcessingUnit == null)
                throw new ArgumentNullException("graphicsProcessingUnit");

            _generalRegisters = generalRegisters;
            _addressRegister = addressRegister;
            _memory = memory;
            _graphicsProcessingUnit = graphicsProcessingUnit;
        }

        public ICommand Create(int address, int operationCode)
        {
            if (operationCode == 0x00E0)
                return new ClearScreenCommand(address, _graphicsProcessingUnit);
            if ((operationCode & 0xF000) == 0xD000)
                return new DrawSpriteCommand(address, operationCode, _graphicsProcessingUnit, _generalRegisters,
                                             _addressRegister, _memory);
            return new NullCommand();
        }
    }
}