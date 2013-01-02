﻿using System;
using System.Collections.Generic;

namespace WonkyChip8.Interpreter
{
    public class RandomAccessMemory : IMemory
    {
        private readonly List<int?> _memory;

        public RandomAccessMemory()
        {
            _memory = new List<int?>(new int?[EndAddress]);
        }

        public int? this[int cellAddress]
        {
            get { return _memory[cellAddress]; }
            set { _memory.Insert(cellAddress, value); }
        }

        public int ProgramStartAddress { get { return 0x200; } }
        public int EndAddress { get { return 0xFFF; } }

        public void LoadProgram(IEnumerable<int?> programBytes)
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