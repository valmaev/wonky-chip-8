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
            int currentProgramByteAddress = _memory.ProgramStartAddress;
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

        private int GetCurrentOperationCodeFromMemory(int addressInMemory)
        {
            byte currentProgramByte = _memory[addressInMemory];
            byte nextProgramByte = _memory[addressInMemory + 1];
            return (currentProgramByte << 8) + nextProgramByte;
        }
    }
}