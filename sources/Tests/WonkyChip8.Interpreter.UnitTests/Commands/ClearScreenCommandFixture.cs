using System;
using NSubstitute;
using NUnit.Framework;
using WonkyChip8.Interpreter.Commands;
using WonkyChip8.Interpreter.UnitTests.TestUtilities;

namespace WonkyChip8.Interpreter.UnitTests.Commands
{
    [TestFixture]
    public class ClearScreenCommandFixture
    {
        [Test]
        public void Constructor_WithNullDisplay_ExpectedThrowsArgumentNullException()
        {
            NUnitUtilities.AssertThrowsArgumentExceptionWithParamName<ArgumentNullException>(
                () => new ClearScreenCommand(0, null), "display");
        }

        [Test]
        public void Execute_ExpectedCallsDisplayOneTime()
        {
            // Arrange
            var displayMock = Substitute.For<IDisplay>();
            var clearScreenCommand = new ClearScreenCommand(0, displayMock);

            // Act
            clearScreenCommand.Execute();

            // Assert
            displayMock.Received(1).ClearScreen();
        }
    }
}