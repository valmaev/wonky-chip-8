namespace WonkyChip8.Interpreter
{
    public interface ICommand
    {
        int? Address { get; }
        int? NextCommandAddress { get; }
        void Execute();
    }
}