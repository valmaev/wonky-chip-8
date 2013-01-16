namespace WonkyChip8.Interpreter
{
    public interface IGeneralRegisters
    {
        byte? this[int index] { get; set; }
    }
}