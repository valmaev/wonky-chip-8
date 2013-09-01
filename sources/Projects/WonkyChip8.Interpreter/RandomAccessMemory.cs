using System;
using System.Collections.Generic;

namespace WonkyChip8.Interpreter
{
    public sealed class RandomAccessMemory : IMemory
    {
        private readonly List<byte> _memory;

        public RandomAccessMemory(int capacity)
        {
            _memory = new List<byte>(new byte[capacity]);
        }

        public byte this[int cellAddress]
        {
            get { return _memory[cellAddress]; }
            set { _memory[cellAddress] = value; }
        }

        public int Capacity
        {
            get { return _memory.Capacity; }
        }

        public int ProgramStartAddress { get; set; }

        public void LoadBytes(int cellAddress, IEnumerable<byte> bytes)
        {
            if (cellAddress < 0 || cellAddress >= _memory.Count)
                throw new ArgumentOutOfRangeException("cellAddress");
            if (bytes == null)
                throw new ArgumentNullException("bytes");

            var currentProgramAddress = cellAddress;
            foreach (var programByte in bytes)
            {
                _memory[currentProgramAddress] = programByte;
                currentProgramAddress++;
            }
        }

        public void UnloadBytes(int firstCellAddress, int lastCellAddress)
        {
            if (firstCellAddress < 0 || firstCellAddress >= _memory.Count)
                throw new ArgumentOutOfRangeException("firstCellAddress");
            if (lastCellAddress < 0 || lastCellAddress >= _memory.Count || lastCellAddress < firstCellAddress)
                throw new ArgumentOutOfRangeException("lastCellAddress");

            for (var address = firstCellAddress; address <= lastCellAddress; address++)
                _memory[address] = 0;
        }
    }
}