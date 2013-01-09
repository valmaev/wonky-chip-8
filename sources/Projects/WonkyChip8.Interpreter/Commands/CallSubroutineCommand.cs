using System;

namespace WonkyChip8.Interpreter.Commands
{
    public class CallSubroutineCommand : Command
    {
        private readonly ICallStack _callStack;
        private readonly int? _nextCommandAddress;

        public CallSubroutineCommand(int? address, int operationCode, ICallStack callStack) : base(address, operationCode)
        {
            if (FirstOperationCodeHalfByte != 0x2)
                throw new ArgumentOutOfRangeException("operationCode"); 
            if (callStack == null)
                throw new ArgumentNullException("callStack");

            _callStack = callStack;
            _nextCommandAddress = operationCode & 0x0FFF;
        }

        public override int? NextCommandAddress
        {
            get { return _nextCommandAddress; }
        }

        public override void Execute()
        {
            _callStack.Push(Address);
        }
    }
}