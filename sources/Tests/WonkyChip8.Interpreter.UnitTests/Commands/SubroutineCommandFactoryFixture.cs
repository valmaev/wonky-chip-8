using System;
using NSubstitute;
using NUnit.Framework;
using WonkyChip8.Interpreter.Commands;
using WonkyChip8.Interpreter.UnitTests.TestUtilities;

namespace WonkyChip8.Interpreter.UnitTests.Commands
{
    [TestFixture]
    public class SubroutineCommandFactoryFixture
    {
        private static SubroutineCommandFactory CreateSubroutineCommandFactory(ICallStack callStack = null)
        {
            return new SubroutineCommandFactory(callStack ?? Substitute.For<ICallStack>());
        }

        [Test]
        public void Constructor_WithNullCallStack_ExpectedThrowsArgumentNullException()
        {
            NUnitExtensions.AssertThrowsArgumentExceptionWithParamName<ArgumentNullException>(
                () => new SubroutineCommandFactory(null), "callStack");
        }

        [TestCase(0x00EE, typeof (ReturnFromSubroutineCommand))]
        [TestCase(0x2000, typeof (CallSubroutineCommand))]
        public void Create_WithProperOperationCode_ExpectedReturnsCommandWithProperType(int operationCode,
                                                                                        Type commandType)
        {
            // Arrange
            var commandFactory = CreateSubroutineCommandFactory();

            // Act
            ICommand command = commandFactory.Create(0, operationCode);

            // Assert
            Assert.IsInstanceOf(commandType, command);
        }

        [TestCase(0x0000)]
        [TestCase(0x99999)]
        [TestCase(0xF000)]
        public void Create_WithNotSupportedOperationCode_ExpectedReturnsNullCommand(int notSupportedOpertationCode)
        {
            // Arrange
            var commandFactory = CreateSubroutineCommandFactory();

            // Act
            ICommand command = commandFactory.Create(0, notSupportedOpertationCode);

            // Assert
            Assert.IsInstanceOf<NullCommand>(command);
        }
    }
}