using System;

namespace WonkyChip8.Interpreter
{
    public class CentralProcessingUnit : ICentralProcessingUnit
    {
        private readonly IMemory _memory;
        private readonly ICommandFactory _commandFactory;

        public CentralProcessingUnit(IMemory memory, ICommandFactory commandFactory)
        {
            if (memory == null)
                throw new ArgumentNullException("memory");
            if (commandFactory == null)
                throw new ArgumentNullException("commandFactory");

            _memory = memory;
            _commandFactory = commandFactory;
        }

        public void ExecuteProgram()
        {
            byte? currentProgramByte = _memory.ProgramStartByte;

            while (currentProgramByte != null)
            {
                ICommand command = _commandFactory.Create(currentProgramByte);
                command.Execute();
                currentProgramByte = _memory[command.NextCommandAddress];
            }          
        }
    }
}