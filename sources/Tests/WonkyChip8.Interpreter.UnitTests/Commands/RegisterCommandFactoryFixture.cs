using System;
using NSubstitute;
using NUnit.Framework;
using WonkyChip8.Interpreter.Commands;
using WonkyChip8.Interpreter.UnitTests.TestUtilities;

namespace WonkyChip8.Interpreter.UnitTests.Commands
{
    [TestFixture]
    public class RegisterCommandFactoryFixture
    {
        internal static RegisterCommandFactory CreateRegisterCommandFactory(IGeneralRegisters generalRegisters = null,
                                                                            IAddressRegister addressRegister = null,
                                                                            IMemory memory = null,
                                                                            IRandomGenerator randomGenerator = null)
        {
            return new RegisterCommandFactory(generalRegisters ?? Substitute.For<IGeneralRegisters>(),
                                              addressRegister ?? Substitute.For<IAddressRegister>(),
                                              memory ?? Substitute.For<IMemory>(),
                                              randomGenerator ?? Substitute.For<IRandomGenerator>());
        }

        [Test]
        public void Constructor_WithNullGeneralRegisters_ExpectedThrowsArgumentNullException()
        {
            NUnitUtilities.AssertThrowsArgumentExceptionWithParamName<ArgumentNullException>(
                () => new RegisterCommandFactory(null, Substitute.For<IAddressRegister>(),
                                                 Substitute.For<IMemory>(), Substitute.For<IRandomGenerator>()),
                "generalRegisters");
        }

        [Test]
        public void Constructor_WithNullAddressRegister_ExpectedThrowsArgumentNullException()
        {
            NUnitUtilities.AssertThrowsArgumentExceptionWithParamName<ArgumentNullException>(
                () => new RegisterCommandFactory(Substitute.For<IGeneralRegisters>(), null,
                                                 Substitute.For<IMemory>(), Substitute.For<IRandomGenerator>()),
                "addressRegister");
        }

        [Test]
        public void Constructor_WithNullMemory_ExpectedThrowsArgumentNullException()
        {
            NUnitUtilities.AssertThrowsArgumentExceptionWithParamName<ArgumentNullException>(
                () => new RegisterCommandFactory(Substitute.For<IGeneralRegisters>(),
                                                 Substitute.For<IAddressRegister>(), null,
                                                 Substitute.For<IRandomGenerator>()),
                "memory");
        }

        [Test]
        public void Constructor_WithNullRandomGenerator_ExpectedThrowsArgumentNullException()
        {
            NUnitUtilities.AssertThrowsArgumentExceptionWithParamName<ArgumentNullException>(
                () => new RegisterCommandFactory(Substitute.For<IGeneralRegisters>(),
                                                 Substitute.For<IAddressRegister>(), Substitute.For<IMemory>(), null),
                "randomGenerator");
        }

        [TestCase(0x1000, typeof (JumpToAddressCommand))]
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
        [TestCase(0xF01E, typeof (AddValueToAddressRegisterCommand))]
        [TestCase(0xF029, typeof (PointToFontSpriteCommand))]
        [TestCase(0xF033, typeof (SaveBinaryCodedRegisterValueInMemoryCommand))]
        [TestCase(0xF055, typeof (SaveGeneralRegistersValuesInMemoryCommand))]
        public void Create_WithProperOperationCode_ExpectedReturnsCommandWithProperType(int operationCode,
                                                                                        Type commandType)
        {
            // Arrange
            var commandFactory = CreateRegisterCommandFactory();

            // Act
            ICommand command = commandFactory.Create(0, operationCode);

            // Assert
            Assert.IsInstanceOf(commandType, command);
        }

        [TestCase(0x0000)]
        [TestCase(0x99999)]
        [TestCase(0xF000)]
        public void Create_WithNotSupportedOperationCode_ExpectedReturnsNullCommand(int notSupportedOperationCode)
        {
            // Arrange
            var commandFactory = CreateRegisterCommandFactory();

            // Act
            ICommand command = commandFactory.Create(0, notSupportedOperationCode);

            // Assert
            Assert.IsInstanceOf<NullCommand>(command);
        }
    }
}