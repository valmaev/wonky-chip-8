using System;

namespace WonkyChip8.Interpreter.Commands
{
    public class SaveValueToRegisterCommand : Command
    {
        private readonly IRegisters _registers;

        public SaveValueToRegisterCommand(int? address, int operationCode, IRegisters registers)
            : base(address, operationCode)
        {
            if ((operationCode & 0xF000) != 0x6000)
                throw new ArgumentOutOfRangeException("operationCode");
            if (registers == null)
                throw new ArgumentNullException("registers");

            _registers = registers;
        }

        public override void Execute()
        {
            if (SecondOperationCodeHalfByte != null)
                _registers[SecondOperationCodeHalfByte.Value] = (byte?) SecondOperationCodeByte;
        }
    }
}