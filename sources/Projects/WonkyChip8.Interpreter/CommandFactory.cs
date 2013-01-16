﻿using System;
using WonkyChip8.Interpreter.Commands;

namespace WonkyChip8.Interpreter
{
    public class CommandFactory : ICommandFactory
    {
        private readonly IGraphicsProcessingUnit _graphicsProcessingUnit;
        private readonly ICallStack _callStack;
        private readonly IGeneralRegisters _generalRegisters;

        public CommandFactory(IGraphicsProcessingUnit graphicsProcessingUnit, ICallStack callStack, IGeneralRegisters generalRegisters)
        {
            if (graphicsProcessingUnit == null)
                throw new ArgumentNullException("graphicsProcessingUnit");
            if (callStack == null)
                throw new ArgumentNullException("callStack");
            if (generalRegisters == null)
                throw new ArgumentNullException("generalRegisters");

            _graphicsProcessingUnit = graphicsProcessingUnit;
            _callStack = callStack;
            _generalRegisters = generalRegisters;
        }

        public ICommand Create(int? address, int? operationCode)
        {
            if (operationCode == null)
                return new NullCommand(address);
            if (operationCode == 0x00E0)
                return new ClearScreenCommand(address, _graphicsProcessingUnit);
            if (operationCode == 0x00EE)
                return new ReturnFromSubroutineCommand(address, operationCode.Value, _callStack);
            switch (operationCode & 0xF000)
            {
                case 0x1000:
                    return new JumpToAddressCommand(address, operationCode.Value);
                case 0x2000:
                    return new CallSubroutineCommand(address, operationCode.Value, _callStack);
                case 0x3000:
                case 0x4000:
                    return new SkipNextOperationCommand(address, operationCode.Value, _generalRegisters);
                case 0x5000:
                case 0x9000:
                    if ((operationCode & 0x000F) == 0x0000)
                        return new SkipNextOperationCommand(address, operationCode.Value, _generalRegisters);
                    break;
                case 0x6000:
                    return new SaveValueToRegisterCommand(address, operationCode.Value, _generalRegisters);
                case 0x7000:
                    return new AddValueToRegisterCommand(address, operationCode.Value, _generalRegisters);
                case 0x8000:
                    switch ((operationCode & 0x000F))
                    {
                        case 0x0000:
                            return new CopyRegisterValueCommand(address, operationCode.Value, _generalRegisters);
                        case 0x0001:
                        case 0x0002:
                        case 0x0003:
                            return new BitwiseOperationsForRegistersCommand(address, operationCode.Value, _generalRegisters);
                        case 0x0004:
                        case 0x0005:
                        case 0x0007:
                            return new BinaryOperationsForRegistersCommand(address, operationCode.Value, _generalRegisters);
                        case 0x0006:
                        case 0x000E:
                            return new ShiftOperationsForRegistersCommand(address, operationCode.Value, _generalRegisters);
                    }
                    break;
            }

            throw new ArgumentOutOfRangeException("operationCode");
        }
    }
}