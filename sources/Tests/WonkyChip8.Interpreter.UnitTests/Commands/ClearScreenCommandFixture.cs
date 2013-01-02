using System;
using NSubstitute;
using NUnit.Framework;
using WonkyChip8.Interpreter.Commands;

namespace WonkyChip8.Interpreter.UnitTests.Commands
{
    [TestFixture]
    public class ClearScreenCommandFixture
    {
        [Test]
        public void Constructor_WithNullGraphicsProcessingUnit_ExpectThrowsArgumentNullException()
        {
            // Act & Assert
            var argumentNullException = Assert.Throws<ArgumentNullException>(() => new ClearScreenCommand(0, null));
            Assert.AreEqual("graphicsProcessingUnit", argumentNullException.ParamName);
        }

        [Test]
        public void Execute_ExpectCallsGraphicsProcessingUnitOneTime()
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