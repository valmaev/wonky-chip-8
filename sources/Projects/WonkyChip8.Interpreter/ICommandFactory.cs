namespace WonkyChip8.Interpreter
{
    public interface ICommandFactory
    {
        ICommand Create(int? address, int? operationCode);
    }
}