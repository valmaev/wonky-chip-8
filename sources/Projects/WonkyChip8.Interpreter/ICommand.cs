namespace WonkyChip8.Interpreter
{
    public interface ICommand
    {
        int NextCommandAddress { get; }
        void Execute();
    }
}