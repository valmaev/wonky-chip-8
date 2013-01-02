using System;
using WonkyChip8.Interpreter.Commands;

namespace WonkyChip8.Interpreter
{
    public class CommandFactory : ICommandFactory
    {
        private readonly IGraphicsProcessingUnit _graphicsProcessingUnit;

        public CommandFactory(IGraphicsProcessingUnit graphicsProcessingUnit)
        {
            if (graphicsProcessingUnit == null)
                throw new ArgumentNullException("graphicsProcessingUnit");

            _graphicsProcessingUnit = graphicsProcessingUnit;
        }

        public ICommand Create(int? address, int? operationCode)
        {
            if (operationCode == null)
                return new NullCommand(address);
            if (operationCode == 0x00E0)
                return new ClearScreenCommand(address, _graphicsProcessingUnit);
            throw new ArgumentOutOfRangeException("operationCode");
        }
    }
}