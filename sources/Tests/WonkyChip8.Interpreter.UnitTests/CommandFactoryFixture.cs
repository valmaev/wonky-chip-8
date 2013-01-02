using System;
using NSubstitute;
using NUnit.Framework;
using WonkyChip8.Interpreter.Commands;

namespace WonkyChip8.Interpreter.UnitTests
{
    [TestFixture]
    public class CommandFactoryFixture
    {
        public static CommandFactory CreateCommandFactory(IGraphicsProcessingUnit graphicsProcessingUnit = null)
        {
            return new CommandFactory(graphicsProcessingUnit ?? Substitute.For<IGraphicsProcessingUnit>());
        }

        [Test]
        public void Constructor_WithNullGraphicsProcessingUnit_ExpectThrowsArgumentNullException()
        {
            // Act & Assert
            var argumentNullException = Assert.Throws<ArgumentNullException>(() => new CommandFactory(null));
            Assert.AreEqual("graphicsProcessingUnit", argumentNullException.ParamName);
        }

        [Test]
        public void Create_WithNullOperationCode_ExpectReturnNullCommand()
        {
            // Arrange
            var commandFactory = CreateCommandFactory();

            // Act & Assert
            Assert.IsInstanceOf<NullCommand>(commandFactory.Create(0, null));
        }

        [Test]
        public void Create_WithInvalidOperationCode_ExpectThrowingArgumentOutOfRangeException()
        {
            // Arrange
            var commandFactory = CreateCommandFactory();

            // Act & Assert
            Assert.Throws<ArgumentOutOfRangeException>(() => commandFactory.Create(0, 4369));
        }

        [Test]
        public void Create_WithOperationCodeEquals00E0_ExpectReturnsClearScreenCommand()
        {
            // Arrange
            var commandFactory = CreateCommandFactory();

            // Act
            ICommand command = commandFactory.Create(0, 0x00E0);

            // Assert
            Assert.IsInstanceOf<ClearScreenCommand>(command);
        }
    }
}