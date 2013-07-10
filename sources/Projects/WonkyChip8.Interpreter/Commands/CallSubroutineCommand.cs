using System;

namespace WonkyChip8.Interpreter.Commands
{
    public sealed class CallSubroutineCommand : Command
    {
        private readonly ICallStack _callStack;

        public CallSubroutineCommand(int address, int operationCode, ICallStack callStack)
            : base(address, operationCode)
        {
            if (FirstOperationCodeHalfByte != 0x2)
                throw new ArgumentOutOfRangeException("operationCode");
            if (callStack == null)
                throw new ArgumentNullException("callStack");

            _callStack = callStack;
        }

        public override int NextCommandAddress
        {
            get { return OperationCode & 0x0FFF; }
        }

        public override void Execute()
        {
            _callStack.Push(Address);
        }
    }
}