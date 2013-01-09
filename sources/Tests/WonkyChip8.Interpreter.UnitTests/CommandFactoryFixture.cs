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
                                                           ICallStack callStack = null, IRegisters registers = null)
        {
            return new CommandFactory(graphicsProcessingUnit ?? Substitute.For<IGraphicsProcessingUnit>(),
                                      callStack ?? Substitute.For<ICallStack>(),
                                      registers ?? Substitute.For<IRegisters>());
        }

        [Test]
        public void Constructor_WithNullGraphicsProcessingUnit_ExpectThrowsArgumentNullException()
        {
            // Act & Assert
            var argumentNullException =
                Assert.Throws<ArgumentNullException>(
                    () => new CommandFactory(null, Substitute.For<ICallStack>(), Substitute.For<IRegisters>()));
            Assert.AreEqual("graphicsProcessingUnit", argumentNullException.ParamName);
        }

        [Test]
        public void Constructor_WithNullCallStack_ExpectThrowsArgumentNullException()
        {
            // Act & Assert
            var argumentNullException =
                Assert.Throws<ArgumentNullException>(
                    () =>
                    new CommandFactory(Substitute.For<IGraphicsProcessingUnit>(), null, Substitute.For<IRegisters>()));
            Assert.AreEqual("callStack", argumentNullException.ParamName);
        }

        [Test]
        public void Constructor_WithNullRegisters_ExpectThrowsArgumentNullException()
        {
            // Act & Assert
            var argumentNullException =
                Assert.Throws<ArgumentNullException>(
                    () =>
                    new CommandFactory(Substitute.For<IGraphicsProcessingUnit>(), Substitute.For<ICallStack>(), null));
            Assert.AreEqual("registers", argumentNullException.ParamName);
        }

        [TestCase(0x99999)]
        [TestCase(0x5121)]
        [TestCase(0x800F)]
        public void Create_WithInvalidOperationCode_ExpectThrowsArgumentOutOfRangeException(int invalidOperationCode)
        {
            // Arrange
            var commandFactory = CreateCommandFactory();

            // Act & Assert
            Assert.Throws<ArgumentOutOfRangeException>(() => commandFactory.Create(0, invalidOperationCode));
        }

        [TestCase(null, typeof(NullCommand))]
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
        [TestCase(0x8001, typeof(LogicalOrCommand))]
        public void Create_WithProperOperationCode_ExpectedReturnsCommandWithProperType(int? operationCode,
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