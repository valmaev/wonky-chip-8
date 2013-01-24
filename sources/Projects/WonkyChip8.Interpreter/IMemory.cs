namespace WonkyChip8.Interpreter
{
    public interface IMemory
    {
        int ProgramStartAddress { get; }
        int EndAddress { get; }
        void LoadProgram(byte[] programBytes);
        void UnloadProgram();
        byte this[int cellAddress] { get; set; }
    }
}