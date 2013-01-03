using System;
using NSubstitute;
using NUnit.Framework;
using WonkyChip8.Interpreter.Commands;

namespace WonkyChip8.Interpreter.UnitTests
{
    [TestFixture]
    public class CommandFactoryFixture
    {
        private static CommandFactory CreateCommandFactory(IGraphicsProcessingUnit graphicsProcessingUnit = null,
                                                           ICallStack callStack = null)
        {
            return new CommandFactory(graphicsProcessingUnit ?? Substitute.For<IGraphicsProcessingUnit>(),
                                      callStack ?? Substitute.For<ICallStack>());
        }

        private static void AssertTypeOfCommand<TCommand>(int? operationCode) where TCommand : ICommand
        {
            // Arrange
            var commandFactory = CreateCommandFactory();

            // Act
            ICommand command = commandFactory.Create(0, operationCode);

            // Assert
            Assert.IsInstanceOf<TCommand>(command);
        }

        [Test]
        public void Constructor_WithNullGraphicsProcessingUnit_ExpectThrowsArgumentNullException()
        {
            // Act & Assert
            var argumentNullException =
                Assert.Throws<ArgumentNullException>(() => new CommandFactory(null, Substitute.For<ICallStack>()));
            Assert.AreEqual("graphicsProcessingUnit", argumentNullException.ParamName);
        }

        [Test]
        public void Constructor_WithNullCallStack_ExpectThrowsArgumentNullException()
        {
            // Act & Assert
            var argumentNullException =
                Assert.Throws<ArgumentNullException>(
                    () => new CommandFactory(Substitute.For<IGraphicsProcessingUnit>(), null));
            Assert.AreEqual("callStack", argumentNullException.ParamName);
        }

        [Test]
        public void Create_WithInvalidOperationCode_ExpectThrowsArgumentOutOfRangeException()
        {
            // Arrange
            var commandFactory = CreateCommandFactory();

            // Act & Assert
            Assert.Throws<ArgumentOutOfRangeException>(() => commandFactory.Create(0, 0x99999));
        }

        [Test]
        public void Create_WithNullOperationCode_ExpectReturnsNullCommand()
        {
            AssertTypeOfCommand<NullCommand>(null);
        }

        [Test]
        public void Create_WithOperationCodeEquals00E0_ExpectReturnsClearScreenCommand()
        {
            AssertTypeOfCommand<ClearScreenCommand>(0x00E0);
        }

        [Test]
        public void Create_WithOperationCodeEquals00Ee_ExpectReturnsReturnFromSubroutineCommand()
        {
            AssertTypeOfCommand<ReturnFromSubroutineCommand>(0x00EE);
        }

        [Test]
        public void Create_WithOperationCodeEquals1Nnn_ExpectReturnsJumpToAddressCommand()
        {
            AssertTypeOfCommand<JumpToAddressCommand>(0x1001);
        }

        [Test]
        public void Create_WithOperationCodeEquals2Nnn_ExpectReturnsCallSubroutineCommand()
        {
            AssertTypeOfCommand<CallSubroutineCommand>(0x2001);
        }
    }
}