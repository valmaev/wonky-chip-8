using System.Collections.Generic;

namespace WonkyChip8.Interpreter
{
    public interface IMemory
    {
        byte? ProgramStartByte { get; }
        void LoadProgram(IEnumerable<byte?> programBytes);
        void UnloadProgram();
        byte? this[int cellAddress] { get; set; }
    }
}