﻿using System.Collections.Generic;

namespace WonkyChip8.Interpreter
{
    public class Registers : IRegisters
    {
        private const int GeneralRegistersCount = 16;

        private readonly List<byte?> _generalRegisters = new List<byte?>(new byte?[GeneralRegistersCount]);

        public byte? this[int index]
        {
            get { return _generalRegisters[index]; }
            set { _generalRegisters[index] = value; }
        }

        public short? AddressRegister { get; set; }
    }
}