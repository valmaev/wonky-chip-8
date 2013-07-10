using System;

namespace WonkyChip8.Interpreter
{
    public sealed class CentralProcessingUnit : ICentralProcessingUnit
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
            if (!_memory.ProgramStartAddress.HasValue)
                throw new InvalidOperationException(
                    "An attempt was made to execute program before loading it to memory.");

            int currentProgramByteAddress = _memory.ProgramStartAddress.Value;
            int currentOperationCode;
            do
            {
                currentOperationCode = GetCurrentOperationCodeFromMemory(currentProgramByteAddress);
                ICommand command = _commandFactory.Create(currentProgramByteAddress, currentOperationCode);
                command.Execute();
                currentProgramByteAddress = command.NextCommandAddress;
            } 
            while (currentOperationCode != 0);
        }

        private int GetCurrentOperationCodeFromMemory(int memoryCellAddress)
        {
            byte currentProgramByte = _memory[memoryCellAddress];
            byte nextProgramByte = _memory[memoryCellAddress + 1];
            return (currentProgramByte << 8) + nextProgramByte;
        }
    }
}