using System;
using NSubstitute;
using NUnit.Framework;

namespace WonkyChip8.Interpreter.UnitTests
{
    [TestFixture]
    public class CentralProcessingUnitFixture 
    {
        private static CentralProcessingUnit CreateCentralProcessingUnit(IMemory memory = null,
                                                                         ICommandFactory commandFactory = null)
        {
            return new CentralProcessingUnit(memory ?? Substitute.For<IMemory>(),
                                             commandFactory ?? Substitute.For<ICommandFactory>());
        }

        [Test]
        public void Constructor_WithNullMemory_ExpectArgumentNullException()
        {
            // Arrange
            var commandFactoryStub = Substitute.For<ICommandFactory>();

            // Act
            TestDelegate cpuConstructor = () => new CentralProcessingUnit(null, commandFactoryStub);

            // Assert
            var argumentNullException = Assert.Throws<ArgumentNullException>(cpuConstructor);
            Assert.AreEqual("memory", argumentNullException.ParamName);
        }

        [Test]
        public void Constructor_WithNullCommandFactory_ExpectArgumentNullException()
        {
            // Arrange
            var memoryStub = Substitute.For<IMemory>();

            // Act
            TestDelegate cpuConstructor = () => new CentralProcessingUnit(memoryStub, null);

            // Assert
            var argumentNullException = Assert.Throws<ArgumentNullException>(cpuConstructor);
            Assert.AreEqual("commandFactory", argumentNullException.ParamName);
        }

        [Test]
        public void ExecuteProgram_WithNullProgramStartByte_ExpectNotTrowingException()
        {
            // Arrange
            var centralProcessingUnit = CreateCentralProcessingUnit();

            // Act & Assert
            Assert.DoesNotThrow(centralProcessingUnit.ExecuteProgram);
        }

        [Test]
        public void ExecuteProgram_WithNotNullProgramStartByte_ExpectExecuteCommandOneTime()
        {
            // Arrange
            const byte commandOperationCode = 0x00E0;
            var memoryStub = Substitute.For<IMemory>();
            memoryStub.ProgramStartByte.Returns(commandOperationCode);

            var commandMock = Substitute.For<ICommand>();

            var commandFactoryStub = Substitute.For<ICommandFactory>();
            commandFactoryStub.Create(commandOperationCode).Returns(commandMock);

            var centralProcessingUnit = CreateCentralProcessingUnit(memoryStub, commandFactoryStub);

            // Act
            centralProcessingUnit.ExecuteProgram();

            // Assert
            commandMock.Received(1).Execute();
        }
    }
}