using System;
using System.Collections.Generic;
using NSubstitute;
using NUnit.Framework;
using WonkyChip8.Interpreter.Commands;
using WonkyChip8.Interpreter.UnitTests.TestUtilities;

namespace WonkyChip8.Interpreter.UnitTests.Commands
{
    [TestFixture]
    public class CommandFactoryFixture
    {
        private const int NonZeroOperationCode = 0x0001;

        private static CommandFactory CreateCommandFactory(IEnumerable<ICommandFactory> commandFactories = null)
        {
            return new CommandFactory(commandFactories ?? Substitute.For<IEnumerable<ICommandFactory>>());
        }

        private static CommandFactory CreateConfiguredCommandFactory()
        {
            var factories = new List<ICommandFactory>
                {
                    DisplayCommandFactoryFixture.CreateDisplayCommandFactory(),
                    KeyboardCommandFactoryFixture.CreateKeyboardCommandFactory(),
                    RegisterCommandFactoryFixture.CreateRegisterCommandFactory(),
                    SubroutineCommandFactoryFixture.CreateSubroutineCommandFactory(),
                    TimerCommandFactoryFixture.CreateTimerCommandFactory()
                };
            return new CommandFactory(factories);
        }

        private static ICommandFactory CreateCommandFactoryStub(Func<ICommand> createFunction = null)
        {
            var stub = Substitute.For<ICommandFactory>();
            var createFunc = createFunction ?? (() => new NullCommand());
            stub.Create(Arg.Any<int>(), Arg.Any<int>()).Returns(createFunc.Invoke());
            return stub;
        }

        [Test]
        public void Constructor_WithNullCommandFactories_ExpectedNotThrowsExceptions()
        {
            Assert.DoesNotThrow(() => new CommandFactory(null));
        }

        [Test]
        public void Create_WithChildCommandFactories_ExpectedReturnsFirstNotNullCommand()
        {
            // Arrange
            var commandStub = Substitute.For<ICommand>();
            var factories = new List<ICommandFactory>
                {
                    CreateCommandFactoryStub(),
                    CreateCommandFactoryStub(() => commandStub),
                    CreateCommandFactoryStub()
                };

            var commandFactory = CreateCommandFactory(factories);
            const int nonZeroOperationCode = 0x0001;

            // Act
            var command = commandFactory.Create(0, nonZeroOperationCode);

            // Assert
            Assert.AreEqual(commandStub, command);
        }

        [Test]
        public void Create_WhenChildFactoryReturnsNullInCreateMethod_ExpectedIgnoresNull()
        {
            // Arrange
            var commandStub = Substitute.For<ICommand>();
            var factories = new List<ICommandFactory>
                {
                    CreateCommandFactoryStub(() => null),
                    CreateCommandFactoryStub(() => commandStub)
                };

            var commandFactory = CreateCommandFactory(factories);

            // Act
            var command = commandFactory.Create(0, NonZeroOperationCode);

            // Assert
            Assert.AreEqual(commandStub, command);
        }

        [Test]
        public void Create_WhenAllChildFactoriesReturnsNullOrNullCommand_ExpectedThrowArgumentOutOfRangeException()
        {
            // Arrange
            var commandFactory = CreateCommandFactory(new List<ICommandFactory>
                {
                    CreateCommandFactoryStub(() => null),
                    CreateCommandFactoryStub(() => new NullCommand())
                });

            // Act & Assert
            NUnitUtilities.AssertThrowsArgumentExceptionWithParamName<ArgumentOutOfRangeException>(
                () => commandFactory.Create(0, NonZeroOperationCode), "operationCode");
        }

        [Test]
        public void Create_WithoutChildCommandFactoriesAndNonZeroOperationCode_ExpectedThrowsArgumentOutOfRangeException()
        {
            NUnitUtilities.AssertThrowsArgumentExceptionWithParamName<ArgumentOutOfRangeException>(
                () => CreateCommandFactory().Create(0, NonZeroOperationCode), "operationCode");
        }

        [Test]
        public void Create_WithZeroOperationCode_ExpectedReturnsNullCommandWithoutCallsToChildFactories()
        {
            // Arrange
            var commandMock = Substitute.For<ICommand>();
            var commandFactory = CreateCommandFactory(
                new List<ICommandFactory> {CreateCommandFactoryStub(() => commandMock)});

            // Act
            var command = commandFactory.Create(0, 0x0000);

            // Assert
            Assert.IsInstanceOf<NullCommand>(command);
            commandMock.Received(0);
        }

        [TestCase(0x0000, typeof (NullCommand))]
        [TestCase(0x00E0, typeof (ClearScreenCommand))]
        [TestCase(0x00EE, typeof (ReturnFromSubroutineCommand))]
        [TestCase(0x1000, typeof (JumpToAddressCommand))]
        [TestCase(0x2000, typeof (CallSubroutineCommand))]
        [TestCase(0x3000, typeof (SkipNextOperationCommand))]
        [TestCase(0x4000, typeof (SkipNextOperationCommand))]
        [TestCase(0x5000, typeof (SkipNextOperationCommand))]
        [TestCase(0x6000, typeof (SaveValueToRegisterCommand))]
        [TestCase(0x7000, typeof (AddValueToRegisterCommand))]
        [TestCase(0x8000, typeof (CopyRegisterValueCommand))]
        [TestCase(0x8001, typeof (BitwiseOperationsForRegistersCommand))]
        [TestCase(0x8002, typeof (BitwiseOperationsForRegistersCommand))]
        [TestCase(0x8003, typeof (BitwiseOperationsForRegistersCommand))]
        [TestCase(0x8004, typeof (BinaryOperationsForRegistersCommand))]
        [TestCase(0x8005, typeof (BinaryOperationsForRegistersCommand))]
        [TestCase(0x8006, typeof (ShiftOperationsForRegistersCommand))]
        [TestCase(0x8007, typeof (BinaryOperationsForRegistersCommand))]
        [TestCase(0x800E, typeof (ShiftOperationsForRegistersCommand))]
        [TestCase(0x9000, typeof (SkipNextOperationCommand))]
        [TestCase(0xA000, typeof (SaveValueToAddressRegisterCommand))]
        [TestCase(0xB000, typeof (JumpToAddressCommand))]
        [TestCase(0xC000, typeof (SaveRandomValueToRegisterCommand))]
        [TestCase(0xD000, typeof (DrawSpriteCommand))]
        [TestCase(0xE09E, typeof (KeyboardDrivenSkipNextOperationCommand))]
        [TestCase(0xE0A1, typeof (KeyboardDrivenSkipNextOperationCommand))]
        [TestCase(0xF007, typeof (SaveTimerValueToRegisterCommand))]
        [TestCase(0xF00A, typeof (WaitForKeyPressCommand))]
        [TestCase(0xF015, typeof (SaveRegisterValueToTimerValueCommand))]
        [TestCase(0xF018, typeof (SaveRegisterValueToTimerValueCommand))]
        [TestCase(0xF01E, typeof (AddValueToAddressRegisterCommand))]
        [TestCase(0xF029, typeof (PointToFontSpriteCommand))]
        [TestCase(0xF033, typeof (SaveBinaryCodedRegisterValueInMemoryCommand))]
        [TestCase(0xF055, typeof (SaveGeneralRegistersValuesInMemoryCommand))]
        [TestCase(0xF065, typeof (SaveMemoryCellValuesInGeneralRegistersCommand))]
        public void Create_WithProperOperationCode_ExpectedReturnsCommandWithProperType(int operationCode,
                                                                                        Type commandType)
        {
            // Arrange
            var commandFactory = CreateConfiguredCommandFactory();

            // Act
            ICommand command = commandFactory.Create(0, operationCode);

            // Assert
            Assert.IsInstanceOf(commandType, command);
        }
    }
}