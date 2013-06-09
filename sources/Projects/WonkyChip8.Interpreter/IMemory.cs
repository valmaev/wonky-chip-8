namespace WonkyChip8.Interpreter
{
    public interface IMemory
    {
        void LoadProgram(int programStartAddress, byte[] programBytes);
        void UnloadProgram(int programStartAddress);
        byte this[int cellAddress] { get; set; }
    }
}