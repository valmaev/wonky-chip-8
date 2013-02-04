using System;
using WonkyChip8.Interpreter.Commands;

namespace WonkyChip8.Interpreter
{
    public class CommandFactory : ICommandFactory
    {
        private readonly IGraphicsProcessingUnit _graphicsProcessingUnit;
        private readonly ICallStack _callStack;
        private readonly IGeneralRegisters _generalRegisters;
        private readonly IAddressRegister _addressRegister;
        private readonly IRandomGenerator _randomGenerator;
        private readonly IMemory _memory;
        private readonly IKeyboard _keyboard;

        public CommandFactory(IGraphicsProcessingUnit graphicsProcessingUnit, ICallStack callStack,
                              IGeneralRegisters generalRegisters, IAddressRegister addressRegister,
                              IRandomGenerator randomGenerator, IMemory memory, IKeyboard keyboard)
        {
            if (graphicsProcessingUnit == null)
                throw new ArgumentNullException("graphicsProcessingUnit");
            if (callStack == null)
                throw new ArgumentNullException("callStack");
            if (generalRegisters == null)
                throw new ArgumentNullException("generalRegisters");
            if (addressRegister == null)
                throw new ArgumentNullException("addressRegister");
            if (randomGenerator == null)
                throw new ArgumentNullException("randomGenerator");
            if (memory == null)
                throw new ArgumentNullException("memory");
            if (keyboard == null)
                throw new ArgumentNullException("keyboard");

            _graphicsProcessingUnit = graphicsProcessingUnit;
            _callStack = callStack;
            _generalRegisters = generalRegisters;
            _addressRegister = addressRegister;
            _randomGenerator = randomGenerator;
            _memory = memory;
            _keyboard = keyboard;
        }

        public ICommand Create(int address, int operationCode)
        {
            if (operationCode == 0x0000)
                return new NullCommand();
            if (operationCode == 0x00E0)
                return new ClearScreenCommand(address, _graphicsProcessingUnit);
            if (operationCode == 0x00EE)
                return new ReturnFromSubroutineCommand(address, operationCode, _callStack);
            switch (operationCode & 0xF000)
            {
                case 0x1000:
                case 0xB000:
                    return new JumpToAddressCommand(address, operationCode, _generalRegisters);
                case 0x2000:
                    return new CallSubroutineCommand(address, operationCode, _callStack);
                case 0x3000:
                case 0x4000:
                    return new SkipNextOperationCommand(address, operationCode, _generalRegisters);
                case 0x5000:
                case 0x9000:
                    if ((operationCode & 0x000F) == 0x0000)
                        return new SkipNextOperationCommand(address, operationCode, _generalRegisters);
                    break;
                case 0x6000:
                    return new SaveValueToRegisterCommand(address, operationCode, _generalRegisters);
                case 0x7000:
                    return new AddValueToRegisterCommand(address, operationCode, _generalRegisters);
                case 0x8000:
                    switch (operationCode & 0x000F)
                    {
                        case 0x0000:
                            return new CopyRegisterValueCommand(address, operationCode, _generalRegisters);
                        case 0x0001:
                        case 0x0002:
                        case 0x0003:
                            return new BitwiseOperationsForRegistersCommand(address, operationCode, _generalRegisters);
                        case 0x0004:
                        case 0x0005:
                        case 0x0007:
                            return new BinaryOperationsForRegistersCommand(address, operationCode, _generalRegisters);
                        case 0x0006:
                        case 0x000E:
                            return new ShiftOperationsForRegistersCommand(address, operationCode, _generalRegisters);
                    }
                    break;
                case 0xA000:
                    return new SaveValueToAddressRegisterCommand(address, operationCode, _addressRegister);
                case 0xC000:
                    return new SaveRandomValueToRegisterCommand(address, operationCode, _generalRegisters, _randomGenerator);
                case 0xD000:
                    return new DrawSpriteCommand(address, operationCode, _graphicsProcessingUnit, _generalRegisters,
                                                 _addressRegister, _memory);
                case 0xE000:
                    switch (operationCode & 0x00FF)
                    {
                        case 0x009E:
                        case 0x00A1:
                            return new KeyboardDrivenSkipNextOperationCommand(address, operationCode, _generalRegisters,
                                                                              _keyboard);
                    }
                    break;
            }

            throw new ArgumentOutOfRangeException("operationCode");
        }
    }
}