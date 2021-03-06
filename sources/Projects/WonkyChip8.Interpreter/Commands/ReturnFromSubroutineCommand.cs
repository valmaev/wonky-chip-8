﻿using System;

namespace WonkyChip8.Interpreter.Commands
{
    public sealed class ReturnFromSubroutineCommand : Command
    {
        private readonly ICallStack _callStack;
        private readonly int _initialTopOfStack ;

        public ReturnFromSubroutineCommand(int address, int operationCode, ICallStack callStack)
            : base(address, operationCode)
        {
            if (operationCode != 0x00EE)
                throw new ArgumentOutOfRangeException("operationCode");
            if (callStack == null)
                throw new ArgumentNullException("callStack");

            _callStack = callStack;
            _initialTopOfStack = callStack.Peek();
        }

        public override int NextCommandAddress
        {
            get { return _initialTopOfStack + CommandLength; }
        }

        public override void Execute()
        {
            _callStack.Pop();
        }
    }
}