using System;

namespace WonkyChip8.Interpreter.Commands
{
    public class AddValueToRegisterCommand : Command
    {
        private readonly IRegisters _registers;

        public AddValueToRegisterCommand(int? address, int operationCode, IRegisters registers)
            : base(address, operationCode)
        {
            if ((operationCode & 0xF000) != 0x7000)
                throw new ArgumentOutOfRangeException("operationCode");
            if (registers == null)
                throw new ArgumentNullException("registers");

            _registers = registers;
        }

        public override void Execute()
        {
            byte registerValue = _registers[SecondOperationCodeHalfByte] ?? 0;
            registerValue += (byte) SecondOperationCodeByte;
            _registers[SecondOperationCodeHalfByte] = registerValue;
        }
    }
}