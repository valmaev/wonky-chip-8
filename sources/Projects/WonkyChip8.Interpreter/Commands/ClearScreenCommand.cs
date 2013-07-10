using System;

namespace WonkyChip8.Interpreter.Commands
{
    public sealed class ClearScreenCommand : Command
    {
        private readonly IDisplay _display;

        public ClearScreenCommand(int address, IDisplay display)
            : base(address, operationCode: 0x0E00)
        {
            if (display == null)
                throw new ArgumentNullException("display");

            _display = display;
        }

        public override void Execute()
        {
            _display.ClearScreen();
        }
    }
}