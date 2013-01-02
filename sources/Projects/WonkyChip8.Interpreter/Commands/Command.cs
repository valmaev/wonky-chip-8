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

        public virtual void Execute() { }
    }
}