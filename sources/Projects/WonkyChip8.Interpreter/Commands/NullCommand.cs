namespace WonkyChip8.Interpreter.Commands
{
    public class NullCommand : ICommand
    {
        private readonly int? _address;

        public NullCommand(int? address)
        {
            _address = address;
        }

        public int? Address { get { return _address; } }
        public int? NextCommandAddress { get { return null; } }
        public void Execute() { }
    }
}