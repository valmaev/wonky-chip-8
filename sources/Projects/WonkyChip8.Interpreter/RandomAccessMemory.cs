using System;
using System.Collections.Generic;

namespace WonkyChip8.Interpreter
{
    public class RandomAccessMemory : IMemory
    {
        public static readonly int ProgramStartAddress = 0x200;
        public static readonly int EndAddress = 0xFFF;

        private readonly List<byte?> _memory = new List<byte?>(new byte?[EndAddress]);

        public byte? this[int cellAddress]
        {
            get { return _memory[cellAddress]; }
            set { _memory.Insert(cellAddress, value); }
        }

        public byte? ProgramStartByte
        {
            get { return this[ProgramStartAddress]; }
        }

        public void LoadProgram(IEnumerable<byte?> programBytes)
        {
            if (programBytes == null)
                throw new ArgumentNullException("programBytes");

            _memory.InsertRange(ProgramStartAddress, programBytes);
        }

        public void UnloadProgram()
        {
            for (var address = ProgramStartAddress; address < EndAddress; address++)
                _memory[address] = null;
        }
    }
}