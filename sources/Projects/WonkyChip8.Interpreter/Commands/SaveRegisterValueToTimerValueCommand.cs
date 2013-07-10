using System;

namespace WonkyChip8.Interpreter.Commands
{
    public sealed class SaveRegisterValueToTimerValueCommand : RegisterCommand
    {
        private readonly ITimer _timer;

        public SaveRegisterValueToTimerValueCommand(int address, int operationCode, IGeneralRegisters generalRegisters,
                                                    ITimer timer)
            : base(address, operationCode, generalRegisters)
        {
            if (FirstOperationCodeHalfByte != 0xF ||
                (SecondOperationCodeByte != 0x15 && SecondOperationCodeByte != 0x18))
                throw new ArgumentOutOfRangeException("operationCode");
            if (timer == null)
                throw new ArgumentNullException("timer");

            _timer = timer;
        }

        public override void Execute()
        {
            _timer.Value = GeneralRegisters[SecondOperationCodeHalfByte];
        }
    }
}