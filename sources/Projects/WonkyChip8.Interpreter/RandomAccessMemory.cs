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

        public int? ProgramStartAddress { get; private set; }

        public void LoadProgram(int programStartAddress, byte[] programBytes)
        {
            if (programBytes == null)
                throw new ArgumentNullException("programBytes");

            var currentProgramAddress = programStartAddress;
            foreach (var programByte in programBytes)
            {
                _memory[currentProgramAddress] = programByte;
                currentProgramAddress++;
            }

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