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
        public void Constructor_WithNullGraphicsProcessingUnit_ExpectedThrowsArgumentNullException()
        {
            NUnitUtilities.AssertThrowsArgumentExceptionWithParamName<ArgumentNullException>(
                () => new ClearScreenCommand(0, null), "graphicsProcessingUnit");
        }

        [Test]
        public void Execute_ExpectedCallsGraphicsProcessingUnitOneTime()
        {
            // Arrange
            var graphicsProcessingUnitMock = Substitute.For<IGraphicsProcessingUnit>();
            var clearScreenCommand = new ClearScreenCommand(0, graphicsProcessingUnitMock);

            // Act
            clearScreenCommand.Execute();

            // Assert
            graphicsProcessingUnitMock.Received(1).ClearScreen();
        }
    }
}