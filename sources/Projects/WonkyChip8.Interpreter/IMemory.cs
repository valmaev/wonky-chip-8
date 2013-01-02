using System.Collections.Generic;

namespace WonkyChip8.Interpreter
{
    public interface IMemory
    {
        int ProgramStartAddress { get; }
        int EndAddress { get; }
        void LoadProgram(IEnumerable<int?> programBytes);
        void UnloadProgram();
        int? this[int cellAddress] { get; set; }
    }
}