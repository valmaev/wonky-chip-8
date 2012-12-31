namespace WonkyChip8.Interpreter
{
    public interface ICommandFactory
    {
        ICommand Create(byte? operationCode);
    }
}