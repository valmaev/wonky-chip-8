﻿namespace WonkyChip8.Interpreter.Commands
{
    public abstract class Command : ICommand
    {
        protected static readonly int CommandLength = 0x2;

        private readonly int? _address;
        private readonly int _operationCode;

        protected Command(int? address, int operationCode)
        {
            _address = address;
            _operationCode = operationCode;
        }

        public virtual int? Address { get { return _address; } }

        public virtual int? NextCommandAddress
        {
            get { return Address + CommandLength; }
        }

        public int? OperationCode { get { return _operationCode; } }

        protected internal int FirstOperationCodeHalfByte { get { return (_operationCode & 0xF000) >> 12; } }
        protected internal int SecondOperationCodeHalfByte { get { return (_operationCode & 0x0F00) >> 8; } }
        protected internal int ThirdOperationCodeHalfByte { get { return (_operationCode & 0x00F0) >> 4; } }
        protected internal int FourthOperationCodeHalfByte { get { return _operationCode & 0x000F; } }

        protected internal int FirstOperationCodeByte { get { return (_operationCode & 0xFF00) >> 8; } }
        protected internal int SecondOperationCodeByte { get { return _operationCode & 0x00FF; } }

        public virtual void Execute() { }
    }
}