using System.Collections.Generic;

namespace WonkyChip8.Interpreter
{
    public sealed class CallStack : ICallStack
    {
        private Stack<int> _stack;

        internal Stack<int> Stack
        {
            get { return (_stack ?? new Stack<int>()); }
            set { _stack = value; }
        }

        public void Push(int address)
        {
            Stack.Push(address);
        }

        public int Pop()
        {
            return Stack.Pop();
        }

        public int Peek()
        {
            return Stack.Count != 0 ? Stack.Peek() : 0;
        }
    }
}