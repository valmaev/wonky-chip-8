namespace WonkyChip8.Interpreter.Commands
{
    public abstract class Command : ICommand
    {
        protected static readonly int CommandLength = 0x2;
        private readonly int? _address;

        protected Command(int? address)
        {
            _address = address;
        }

        public virtual int? Address { get { return _address; } }

        public virtual int? NextCommandAddress
        {
            get { return Address + CommandLength; }
        }

        public virtual void Execute() { }
    }
}