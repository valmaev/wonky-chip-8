﻿using System;

namespace WonkyChip8.Interpreter
{
    public interface IGraphicsProcessingUnit
    {
        void ClearScreen();
        bool DrawSprite(Tuple<int, int> coordinate, byte[] pixels);
    }
}