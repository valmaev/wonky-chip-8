﻿namespace WonkyChip8.Interpreter
{
    public interface ICallStack
    {
        void Push(int? address);
        int? Pop();
        int? Peek();
    }
}