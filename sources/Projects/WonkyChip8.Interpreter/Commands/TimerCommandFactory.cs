using System;

namespace WonkyChip8.Interpreter.Commands
{
    public class TimerCommandFactory : ICommandFactory
    {
        private readonly IGeneralRegisters _generalRegisters;
        private readonly ITimer _delayTimer;
        private readonly ITimer _soundTimer;

        public TimerCommandFactory(IGeneralRegisters generalRegisters, ITimer delayTimer, ITimer soundTimer)
        {
            if (generalRegisters == null)
                throw new ArgumentNullException("generalRegisters");
            if (delayTimer == null)
                throw new ArgumentNullException("delayTimer");
            if (soundTimer == null)
                throw new ArgumentNullException("soundTimer");

            _generalRegisters = generalRegisters;
            _delayTimer = delayTimer;
            _soundTimer = soundTimer;
        }

        public ICommand Create(int address, int operationCode)
        {
            switch (operationCode & 0xF0FF)
            {
                case 0xF007:
                    return new SaveTimerValueToRegisterCommand(address, operationCode, _generalRegisters, _delayTimer);
            }

            return new NullCommand();
        }
    }
}