using System;

namespace WonkyChip8.Interpreter.Commands
{
    public class SaveTimerValueToRegisterCommand : RegisterCommand
    {
        private readonly ITimer _timer;

        public SaveTimerValueToRegisterCommand(int address, int operationCode,
                                               IGeneralRegisters generalRegisters, ITimer timer)
            : base(address, operationCode, generalRegisters)
        {
            if (FirstOperationCodeHalfByte != 0xF || SecondOperationCodeByte != 0x07)
                throw new ArgumentOutOfRangeException("operationCode");
            if (timer == null)
                throw new ArgumentNullException("timer");

            _timer = timer;
        }

        public override void Execute()
        {
            GeneralRegisters[SecondOperationCodeHalfByte] = _timer.Value;
        }
    }
}