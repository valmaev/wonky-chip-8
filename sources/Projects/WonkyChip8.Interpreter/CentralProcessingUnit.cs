using System;
using System.Collections.Generic;
using System.Linq;

namespace WonkyChip8.Interpreter
{
    public sealed class CentralProcessingUnit : ICentralProcessingUnit
    {
        private readonly IMemory _memory;
        private readonly ICommandFactory _commandFactory;
        private readonly IEnumerable<ITimer> _timers;

        public CentralProcessingUnit(IMemory memory, ICommandFactory commandFactory, IEnumerable<ITimer> timers)
        {
            if (memory == null)
                throw new ArgumentNullException("memory");
            if (commandFactory == null)
                throw new ArgumentNullException("commandFactory");
            if (timers == null)
                throw new ArgumentNullException("timers");

            _memory = memory;
            _commandFactory = commandFactory;
            _timers = timers;
        }

        public void ExecuteProgram()
        {
            int currentProgramByteAddress = _memory.ProgramStartAddress;
            int currentOperationCode;
            do
            {
                currentOperationCode = GetOperationCodeFromMemory(currentProgramByteAddress);
                currentProgramByteAddress = ExecuteCommand(currentProgramByteAddress, currentOperationCode);
                DecrementTimers();
            } 
            while (currentOperationCode != 0);
        }

        private int GetOperationCodeFromMemory(int memoryCellAddress)
        {
            byte currentProgramByte = _memory[memoryCellAddress];
            byte nextProgramByte = _memory[memoryCellAddress + 1];
            return (currentProgramByte << 8) + nextProgramByte;
        }

        private int ExecuteCommand(int currentProgramByteAddress, int currentOperationCode)
        {
            ICommand command = _commandFactory.Create(currentProgramByteAddress, currentOperationCode);
            command.Execute();
            return command.NextCommandAddress;
        }

        private void DecrementTimers()
        {
            foreach (var timer in _timers.Where(timer => timer.Value > 0))
                timer.Value--;
        }
    }
}