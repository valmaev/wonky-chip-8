using System;
using WonkyChip8.Interpreter.Commands;

namespace WonkyChip8.Interpreter
{
    public class CommandFactory : ICommandFactory
    {
        private readonly IGraphicsProcessingUnit _graphicsProcessingUnit;
        private readonly ICallStack _callStack;

        public CommandFactory(IGraphicsProcessingUnit graphicsProcessingUnit, ICallStack callStack)
        {
            if (graphicsProcessingUnit == null)
                throw new ArgumentNullException("graphicsProcessingUnit");
            if (callStack == null)
                throw new ArgumentNullException("callStack");

            _graphicsProcessingUnit = graphicsProcessingUnit;
            _callStack = callStack;
        }

        public ICommand Create(int? address, int? operationCode)
        {
            if (operationCode == null)
                return new NullCommand(address);
            if (operationCode == 0x00E0)
                return new ClearScreenCommand(address, _graphicsProcessingUnit);
            if (operationCode == 0x00EE)
                return new ReturnFromSubroutineCommand(address, operationCode, _callStack);
            switch (operationCode & 0xF000)
            {
                case 0x1000:
                    return new JumpToAddressCommand(address, operationCode.Value);
                case 0x2000:
                    return new CallSubroutineCommand(address, operationCode.Value, _callStack);
            }

            throw new ArgumentOutOfRangeException("operationCode");
        }
    }
}