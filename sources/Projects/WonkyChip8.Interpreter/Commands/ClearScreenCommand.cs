using System;

namespace WonkyChip8.Interpreter.Commands
{
    public class ClearScreenCommand : ICommand
    {
        private const int CommandLength = 0x2;

        private readonly int? _address;
        private readonly IGraphicsProcessingUnit _graphicsProcessingUnit;

        public ClearScreenCommand(int? address, IGraphicsProcessingUnit graphicsProcessingUnit)
        {
            if (graphicsProcessingUnit == null)
                throw new ArgumentNullException("graphicsProcessingUnit");

            _address = address;
            _graphicsProcessingUnit = graphicsProcessingUnit;
        }

        public int? Address { get { return _address; } }
        public int? NextCommandAddress { get { return _address + CommandLength; } }

        public void Execute()
        {
            _graphicsProcessingUnit.ClearScreen();
        }
    }
}