﻿using System;

namespace WonkyChip8.Interpreter.Commands
{
    public sealed class AddValueToRegisterCommand : RegisterCommand
    {
        public AddValueToRegisterCommand(int address, int operationCode, IGeneralRegisters generalRegisters)
            : base(address, operationCode, generalRegisters)
        {
            if (FirstOperationCodeHalfByte != 0x7)
                throw new ArgumentOutOfRangeException("operationCode");
        }

        public override void Execute()
        {
            byte registerValue = GeneralRegisters[SecondOperationCodeHalfByte];
            registerValue += SecondOperationCodeByte;
            GeneralRegisters[SecondOperationCodeHalfByte] = registerValue;
        }
    }
}