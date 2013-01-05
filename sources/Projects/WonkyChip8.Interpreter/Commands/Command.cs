namespace WonkyChip8.Interpreter.Commands
{
    public abstract class Command : ICommand
    {
        protected static readonly int CommandLength = 0x2;

        private readonly int? _address;
        private readonly int? _operationCode;

        protected Command(int? address, int? operationCode)
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

        protected internal int? FirstOperationCodeHalfBit { get { return (OperationCode & 0xF000) >> 12; } }
        protected internal int? SecondOperationCodeHalfBit { get { return (OperationCode & 0x0F00) >> 8; } }
        protected internal int? ThirdOperationCodeHalfBit { get { return (OperationCode & 0x00F0) >> 4; } }
        protected internal int? FourthOperationCodeHalfBit { get { return OperationCode & 0x000F; } }

        protected internal int? FirstOperationCodeBit { get { return (OperationCode & 0xFF00) >> 8; } }
        protected internal int? SecondOperationCodeBit { get { return OperationCode & 0x00FF; } }

        public virtual void Execute() { }
    }
}