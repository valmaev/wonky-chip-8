namespace WonkyChip8.Interpreter
{
    public interface IRegisters
    {
        byte? this[int index] { get; set; }
        short? AddressRegister { get; set; }
    }
}