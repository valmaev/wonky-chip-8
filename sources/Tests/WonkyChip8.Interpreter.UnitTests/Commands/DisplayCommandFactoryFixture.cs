using System;
using NSubstitute;
using NUnit.Framework;
using WonkyChip8.Interpreter.Commands;
using WonkyChip8.Interpreter.UnitTests.TestUtilities;

namespace WonkyChip8.Interpreter.UnitTests.Commands
{
    [TestFixture]
    public class DisplayCommandFactoryFixture
    {
        internal static DisplayCommandFactory CreateDisplayCommandFactory(IGeneralRegisters generalRegisters = null,
                                                                           IAddressRegister addressRegister = null,
                                                                           IMemory memory = null,
                                                                           IDisplay display = null)
        {
            return new DisplayCommandFactory(generalRegisters ?? Substitute.For<IGeneralRegisters>(),
                                             addressRegister ?? Substitute.For<IAddressRegister>(),
                                             memory ?? Substitute.For<IMemory>(),
                                             display ?? Substitute.For<IDisplay>());
        }

        [Test]
        public void Constructor_WithNullGeneralRegisters_ExpectedThrowsArgumentNullException()
        {
            NUnitUtilities.AssertThrowsArgumentExceptionWithParamName<ArgumentNullException>(
                () => new DisplayCommandFactory(null, Substitute.For<IAddressRegister>(),
                                                Substitute.For<IMemory>(), Substitute.For<IDisplay>()),
                "generalRegisters");
        }

        [Test]
        public void Constructor_WithNullAddressRegister_ExpectedThrowsArgumentNullException()
        {
            NUnitUtilities.AssertThrowsArgumentExceptionWithParamName<ArgumentNullException>(
                () => new DisplayCommandFactory(Substitute.For<IGeneralRegisters>(), null,
                                                Substitute.For<IMemory>(), Substitute.For<IDisplay>()),
                "addressRegister");
        }

        [Test]
        public void Constructor_WithNullMemory_ExpectedThrowsArgumentNullException()
        {
            NUnitUtilities.AssertThrowsArgumentExceptionWithParamName<ArgumentNullException>(
                () => new DisplayCommandFactory(Substitute.For<IGeneralRegisters>(),
                                                Substitute.For<IAddressRegister>(), null,
                                                Substitute.For<IDisplay>()),
                "memory");
        }

        [Test]
        public void Constructor_WithNullDisplay_ExpectedThrowsArgumentNullException()
        {
            NUnitUtilities.AssertThrowsArgumentExceptionWithParamName<ArgumentNullException>(
                () => new DisplayCommandFactory(Substitute.For<IGeneralRegisters>(),
                                                Substitute.For<IAddressRegister>(), Substitute.For<IMemory>(),
                                                null),
                "display");
        }

        [TestCase(0x00E0, typeof (ClearScreenCommand))]
        [TestCase(0xD000, typeof (DrawSpriteCommand))]
        public void Create_WithProperOperationCode_ExpectedReturnsCommandWithProperType(int operationCode,
                                                                                        Type commandType)
        {
            // Arrange
            var commandFactory = CreateDisplayCommandFactory();

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
            var commandFactory = CreateDisplayCommandFactory();

            // Act
            ICommand command = commandFactory.Create(0, notSupportedOperationCode);

            // Assert
            Assert.IsInstanceOf<NullCommand>(command);
        }
    }
}