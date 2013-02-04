using System;
using NSubstitute;
using NUnit.Framework;
using WonkyChip8.Interpreter.Commands;
using WonkyChip8.Interpreter.UnitTests.TestUtilities;

namespace WonkyChip8.Interpreter.UnitTests
{
    [TestFixture]
    public class CommandFactoryFixture
    {
        private static CommandFactory CreateCommandFactory(IGraphicsProcessingUnit graphicsProcessingUnit = null,
                                                           ICallStack callStack = null,
                                                           IGeneralRegisters generalRegisters = null,
                                                           IAddressRegister addressRegister = null,
                                                           IRandomGenerator randomGenerator = null,
                                                           IMemory memory = null,
                                                           IKeyboard keyboard = null)
        {
            return new CommandFactory(graphicsProcessingUnit ?? Substitute.For<IGraphicsProcessingUnit>(),
                                      callStack ?? Substitute.For<ICallStack>(),
                                      generalRegisters ?? Substitute.For<IGeneralRegisters>(),
                                      addressRegister ?? Substitute.For<IAddressRegister>(),
                                      randomGenerator ?? Substitute.For<IRandomGenerator>(),
                                      memory ?? Substitute.For<IMemory>(),
                                      keyboard ?? Substitute.For<IKeyboard>());
        }

        [Test]
        public void Constructor_WithNullGraphicsProcessingUnit_ExpectedThrowsArgumentNullException()
        {
            NUnitExtensions.AssertThrowsArgumentExceptionWithParamName<ArgumentNullException>(
                () => new CommandFactory(null, Substitute.For<ICallStack>(), Substitute.For<IGeneralRegisters>(),
                                         Substitute.For<IAddressRegister>(), Substitute.For<IRandomGenerator>(),
                                         Substitute.For<IMemory>(), Substitute.For<IKeyboard>()),
                "graphicsProcessingUnit");
        }

        [Test]
        public void Constructor_WithNullCallStack_ExpectedThrowsArgumentNullException()
        {
            NUnitExtensions.AssertThrowsArgumentExceptionWithParamName<ArgumentNullException>(
                () => new CommandFactory(Substitute.For<IGraphicsProcessingUnit>(), null,
                                         Substitute.For<IGeneralRegisters>(), Substitute.For<IAddressRegister>(),
                                         Substitute.For<IRandomGenerator>(), Substitute.For<IMemory>(),
                                         Substitute.For<IKeyboard>()),
                "callStack");
        }

        [Test]
        public void Constructor_WithNullGeneralRegisters_ExpectedThrowsArgumentNullException()
        {
            NUnitExtensions.AssertThrowsArgumentExceptionWithParamName<ArgumentNullException>(
                () => new CommandFactory(Substitute.For<IGraphicsProcessingUnit>(), Substitute.For<ICallStack>(), null,
                                         Substitute.For<IAddressRegister>(), Substitute.For<IRandomGenerator>(),
                                         Substitute.For<IMemory>(), Substitute.For<IKeyboard>()),
                "generalRegisters");
        }

        [Test]
        public void Constructor_WithNullAddressRegister_ExpectedThrowsArgumentNullException()
        {
            NUnitExtensions.AssertThrowsArgumentExceptionWithParamName<ArgumentNullException>(
                () => new CommandFactory(Substitute.For<IGraphicsProcessingUnit>(), Substitute.For<ICallStack>(),
                                         Substitute.For<IGeneralRegisters>(), null, Substitute.For<IRandomGenerator>(),
                                         Substitute.For<IMemory>(), Substitute.For<IKeyboard>()),
                "addressRegister");
        }

        [Test]
        public void Constructor_WithNullRandomGenerator_ExpectedThrowsArgumentNullException()
        {
            NUnitExtensions.AssertThrowsArgumentExceptionWithParamName<ArgumentNullException>(
                () => new CommandFactory(Substitute.For<IGraphicsProcessingUnit>(), Substitute.For<ICallStack>(),
                                         Substitute.For<IGeneralRegisters>(), Substitute.For<IAddressRegister>(), null,
                                         Substitute.For<IMemory>(), Substitute.For<IKeyboard>()),
                "randomGenerator");
        }

        [Test]
        public void Constructor_WithNullMemory_ExpectedThrowsArgumentNullException()
        {
            NUnitExtensions.AssertThrowsArgumentExceptionWithParamName<ArgumentNullException>(
                () => new CommandFactory(Substitute.For<IGraphicsProcessingUnit>(), Substitute.For<ICallStack>(),
                                         Substitute.For<IGeneralRegisters>(), Substitute.For<IAddressRegister>(),
                                         Substitute.For<IRandomGenerator>(), null, Substitute.For<IKeyboard>()),
                "memory");
        }

        [Test]
        public void Constructor_WithKeyboard_ExpectedThrowsArgumentNullException()
        {
            NUnitExtensions.AssertThrowsArgumentExceptionWithParamName<ArgumentNullException>(
                () => new CommandFactory(Substitute.For<IGraphicsProcessingUnit>(), Substitute.For<ICallStack>(),
                                         Substitute.For<IGeneralRegisters>(), Substitute.For<IAddressRegister>(),
                                         Substitute.For<IRandomGenerator>(), Substitute.For<IMemory>(), null),
                "keyboard");
        }

        [TestCase(0x99999)]
        [TestCase(0x5121)]
        [TestCase(0x800F)]
        [TestCase(0xE000)]
        public void Create_WithInvalidOperationCode_ExpectedThrowsArgumentOutOfRangeException(int invalidOperationCode)
        {
            NUnitExtensions.AssertThrowsArgumentExceptionWithParamName<ArgumentOutOfRangeException>(
                () => CreateCommandFactory().Create(0, invalidOperationCode), "operationCode");
        }

        [TestCase(0x0000, typeof(NullCommand))]
        [TestCase(0x00E0, typeof(ClearScreenCommand))]
        [TestCase(0x00EE, typeof(ReturnFromSubroutineCommand))]
        [TestCase(0x1000, typeof(JumpToAddressCommand))]
        [TestCase(0x2000, typeof(CallSubroutineCommand))]
        [TestCase(0x3000, typeof(SkipNextOperationCommand))]
        [TestCase(0x4000, typeof(SkipNextOperationCommand))]
        [TestCase(0x5000, typeof(SkipNextOperationCommand))]
        [TestCase(0x6000, typeof(SaveValueToRegisterCommand))]
        [TestCase(0x7000, typeof(AddValueToRegisterCommand))]
        [TestCase(0x8000, typeof(CopyRegisterValueCommand))]
        [TestCase(0x8001, typeof(BitwiseOperationsForRegistersCommand))]
        [TestCase(0x8002, typeof(BitwiseOperationsForRegistersCommand))]
        [TestCase(0x8003, typeof(BitwiseOperationsForRegistersCommand))]
        [TestCase(0x8004, typeof(BinaryOperationsForRegistersCommand))]
        [TestCase(0x8005, typeof(BinaryOperationsForRegistersCommand))]
        [TestCase(0x8006, typeof(ShiftOperationsForRegistersCommand))]
        [TestCase(0x8007, typeof(BinaryOperationsForRegistersCommand))]
        [TestCase(0x800E, typeof(ShiftOperationsForRegistersCommand))]
        [TestCase(0x9000, typeof(SkipNextOperationCommand))]
        [TestCase(0xA000, typeof(SaveValueToAddressRegisterCommand))]
        [TestCase(0xB000, typeof(JumpToAddressCommand))]
        [TestCase(0xC000, typeof(SaveRandomValueToRegisterCommand))]
        [TestCase(0xD000, typeof(DrawSpriteCommand))]
        [TestCase(0xE09E, typeof(KeyboardDrivenSkipNextOperationCommand))]
        [TestCase(0xE0A1, typeof(KeyboardDrivenSkipNextOperationCommand))]
        public void Create_WithProperOperationCode_ExpectedReturnsCommandWithProperType(int operationCode,
                                                                                        Type commandType)
        {
            // Arrange
            var commandFactory = CreateCommandFactory();

            // Act
            ICommand command = commandFactory.Create(0, operationCode);

            // Assert
            Assert.IsInstanceOf(commandType, command);
        }
    }
}