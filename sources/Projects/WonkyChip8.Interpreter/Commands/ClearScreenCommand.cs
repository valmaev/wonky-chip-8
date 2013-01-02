using System;

namespace WonkyChip8.Interpreter.Commands
{
    public class ClearScreenCommand : Command
    {
        private readonly IGraphicsProcessingUnit _graphicsProcessingUnit;

        public ClearScreenCommand(int? address, IGraphicsProcessingUnit graphicsProcessingUnit)
            : base(address, operationCode: 0x0E00)
        {
            if (graphicsProcessingUnit == null)
                throw new ArgumentNullException("graphicsProcessingUnit");

            _graphicsProcessingUnit = graphicsProcessingUnit;
        }

        public override void Execute()
        {
            _graphicsProcessingUnit.ClearScreen();
        }
    }
}