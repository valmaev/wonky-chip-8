using System.Collections.Generic;

namespace WonkyChip8.Interpreter
{
    public interface IMemory
    {
        int ProgramStartAddress { get; set; }
        void LoadBytes(int cellAddress, IEnumerable<byte> bytes);
        void UnloadBytes(int firstCellAddress, int lastCellAddress);
        byte this[int cellAddress] { get; set; }
    }
}