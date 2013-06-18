using System;
using System.Collections.Generic;

namespace WonkyChip8.Interpreter
{
    public class RandomAccessMemory : IMemory
    {
        private readonly List<byte> _memory;

        public RandomAccessMemory(int capacity)
        {
            _memory = new List<byte>(new byte[capacity]);
        }

        public byte this[int cellAddress]
        {
            get { return _memory[cellAddress]; }
            set { _memory.Insert(cellAddress, value); }
        }

        public int? ProgramStartAddress { get; private set; }

        public void LoadProgram(int programStartAddress, byte[] programBytes)
        {
            if (programBytes == null)
                throw new ArgumentNullException("programBytes");

            _memory.InsertRange(programStartAddress, programBytes);
            ProgramStartAddress = programStartAddress;
        }

        public void UnloadProgram(int programStartAddress)
        {
            for (var address = programStartAddress; address < _memory.Count; address++)
                _memory[address] = 0;

            ProgramStartAddress = null;
        }
    }
}