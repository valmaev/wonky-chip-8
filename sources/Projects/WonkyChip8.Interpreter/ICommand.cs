namespace WonkyChip8.Interpreter
{
    public interface ICommand
    {
        int? Address { get; }
        int? NextCommandAddress { get; }
        int? OperationCode { get; }
        void Execute();
    }
}