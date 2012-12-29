using System;

namespace WonkyChip8.Interpreter
{
    public class CentralProcessingUnit
    {
        private readonly IMemory _memory;

        public CentralProcessingUnit(IMemory memory)
        {
            if (memory == null)
                throw new ArgumentNullException("memory");

            _memory = memory;
        }

        public void ExecuteProgram()
        {
            InterpretInstruction(_memory.ProgramStartByte);
        }

        private void InterpretInstruction(byte? instruction)
        {
            throw new NotImplementedException();
        }
    }
}