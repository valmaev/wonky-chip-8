using System;
using System.Collections.Generic;

namespace WonkyChip8.Interpreter.Commands
{
    public class CommandFactory : ICommandFactory
    {
        private readonly ICollection<ICommandFactory> _commandFactories = new List<ICommandFactory>();
 
        public CommandFactory(IEnumerable<ICommandFactory> commandFactories)
        {
            if (commandFactories != null)
                foreach (var commandFactory in commandFactories)
                    _commandFactories.Add(commandFactory);
        }

        public ICommand Create(int address, int operationCode)
        {
            if (operationCode != 0x0000)
            {
                foreach (var commandFactory in _commandFactories)
                {
                    var command = commandFactory.Create(address, operationCode);
                    if (command != null && command.GetType() != typeof (NullCommand))
                        return command;
                }

                throw new ArgumentOutOfRangeException("operationCode");
            }
            return new NullCommand();
        }
    }
}