using System;

namespace WonkyChip8.Interpreter.Commands
{
    public sealed class SaveRandomValueToRegisterCommand : RegisterCommand
    {
        private readonly IRandomGenerator _randomGenerator;

        public SaveRandomValueToRegisterCommand(int address, int operationCode, IGeneralRegisters generalRegisters,
                                                IRandomGenerator randomGenerator)
            : base(address, operationCode, generalRegisters)
        {
            if (FirstOperationCodeHalfByte != 0xC)
                throw new ArgumentOutOfRangeException("operationCode");
            if (randomGenerator == null)
                throw new ArgumentNullException("randomGenerator");

            _randomGenerator = randomGenerator;
        }

        public override void Execute()
        {
            GeneralRegisters[SecondOperationCodeHalfByte] = (byte) (_randomGenerator.Generate(0x00, 0xFF) &
                                                                    SecondOperationCodeByte);
        }
    }
}