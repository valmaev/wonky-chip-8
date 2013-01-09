using System;

namespace WonkyChip8.Interpreter.Commands
{
    public class CopyRegisterValueCommand : Command
    {
        private readonly IRegisters _registers;

        public CopyRegisterValueCommand(int? address, int operationCode, IRegisters registers)
            : base(address, operationCode)
        {
            if (FirstOperationCodeHalfByte != 0x8 || FourthOperationCodeHalfByte != 0x0)
                throw new ArgumentOutOfRangeException("operationCode");
            if (registers == null)
                throw new ArgumentNullException("registers");

            _registers = registers;
        }

        public override void Execute()
        {
            _registers[SecondOperationCodeHalfByte] = _registers[ThirdOperationCodeHalfByte];
        }
    }
}