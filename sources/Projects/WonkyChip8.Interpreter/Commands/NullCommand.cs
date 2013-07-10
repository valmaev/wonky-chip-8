namespace WonkyChip8.Interpreter.Commands
{
    public sealed class NullCommand : ICommand
    {
        public int Address { get { return 0; } }
        public int NextCommandAddress { get { return 0; } }
        public int OperationCode { get { return 0; } }

        public void Execute() { }
    }
}