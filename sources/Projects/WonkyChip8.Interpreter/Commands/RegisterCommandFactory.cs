using System;

namespace WonkyChip8.Interpreter.Commands
{
    public sealed class RegisterCommandFactory : ICommandFactory
    {
        private readonly IGeneralRegisters _generalRegisters;
        private readonly IAddressRegister _addressRegister;
        private readonly IRandomGenerator _randomGenerator;
        private readonly IMemory _memory;

        public RegisterCommandFactory(IGeneralRegisters generalRegisters, IAddressRegister addressRegister,
                                      IMemory memory, IRandomGenerator randomGenerator)
        {
            if (generalRegisters == null)
                throw new ArgumentNullException("generalRegisters");
            if (addressRegister == null)
                throw new ArgumentNullException("addressRegister");
            if (randomGenerator == null)
                throw new ArgumentNullException("randomGenerator");
            if (memory == null)
                throw new ArgumentNullException("memory");

            _generalRegisters = generalRegisters;
            _addressRegister = addressRegister;
            _randomGenerator = randomGenerator;
            _memory = memory;
        }

        public ICommand Create(int address, int operationCode)
        {
            switch (operationCode & 0xF000)
            {
                case 0x1000:
                case 0xB000:
                    return new JumpToAddressCommand(address, operationCode, _generalRegisters);
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
                    return new SaveRandomValueToRegisterCommand(address, operationCode, _generalRegisters,
                                                                _randomGenerator);
                case 0XF000:
                    switch (operationCode & 0x00FF)
                    {
                        case 0x001E:
                            return new AddValueToAddressRegisterCommand(address, operationCode, _generalRegisters,
                                                                        _addressRegister);
                        case 0x0029:
                            return new PointToFontSpriteCommand(address, operationCode, _generalRegisters,
                                                                _addressRegister);
                        case 0x0033:
                            return new SaveBinaryCodedRegisterValueInMemoryCommand(address, operationCode,
                                                                                   _generalRegisters, _addressRegister,
                                                                                   _memory);
                        case 0x0055:
                            return new SaveGeneralRegistersValuesInMemoryCommand(address, operationCode,
                                                                                 _generalRegisters, _addressRegister,
                                                                                 _memory);
                        case 0x0065:
                            return new SaveMemoryCellValuesInGeneralRegistersCommand(address, operationCode,
                                                                                     _generalRegisters, _addressRegister,
                                                                                     _memory);
                    }
                    break;
            }

            return new NullCommand();
        }
    }
}