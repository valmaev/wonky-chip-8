using System;

namespace WonkyChip8.Interpreter.Commands
{
    public class LogicalOrCommand : Command
    {
        private readonly IRegisters _registers;

        public LogicalOrCommand(int? address, int operationCode, IRegisters registers)
            : base(address, operationCode)
        {
            if (FirstOperationCodeHalfByte != 0x8 || FourthOperationCodeHalfByte != 0x1)
                throw new ArgumentOutOfRangeException("operationCode");
            if (registers == null)
                throw new ArgumentNullException("registers");

            _registers = registers;
        }

        public override void Execute()
        {
            _registers[SecondOperationCodeHalfByte] |= _registers[ThirdOperationCodeHalfByte];
        }
    }
}