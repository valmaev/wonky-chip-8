using System;

namespace WonkyChip8.Interpreter.Commands
{
    public class SubroutineCommandFactory : ICommandFactory
    {
        private readonly ICallStack _callStack;

        public SubroutineCommandFactory(ICallStack callStack)
        {
            if (callStack == null)
                throw new ArgumentNullException("callStack");

            _callStack = callStack;
        }

        public ICommand Create(int address, int operationCode)
        {
            if (operationCode == 0x00EE)
                return new ReturnFromSubroutineCommand(address, operationCode, _callStack);
            if ((operationCode & 0xF000) == 0x2000)
                return new CallSubroutineCommand(address, operationCode, _callStack);
            return new NullCommand();
        }
    }
}