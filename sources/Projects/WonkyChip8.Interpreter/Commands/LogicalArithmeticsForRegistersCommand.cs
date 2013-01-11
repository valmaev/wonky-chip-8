using System;

namespace WonkyChip8.Interpreter.Commands
{
    public class LogicalArithmeticsForRegistersCommand : Command
    {
        private readonly IRegisters _registers;

        public LogicalArithmeticsForRegistersCommand(int? address, int operationCode, IRegisters registers)
            : base(address, operationCode)
        {
            if (FirstOperationCodeHalfByte != 0x8 || (FourthOperationCodeHalfByte != 0x1 &&
                FourthOperationCodeHalfByte != 0x2 && FourthOperationCodeHalfByte != 0x3))
                throw new ArgumentOutOfRangeException("operationCode");
            if (registers == null)
                throw new ArgumentNullException("registers");

            _registers = registers;
        }

        public override void Execute()
        {
            if (FourthOperationCodeHalfByte == 0x1)
                _registers[SecondOperationCodeHalfByte] |= _registers[ThirdOperationCodeHalfByte];
            if (FourthOperationCodeHalfByte == 0x2)
                _registers[SecondOperationCodeHalfByte] &= _registers[ThirdOperationCodeHalfByte];
            if (FourthOperationCodeHalfByte == 0x3)
                _registers[SecondOperationCodeHalfByte] ^= _registers[ThirdOperationCodeHalfByte];
        }
    }
}