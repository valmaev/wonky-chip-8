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
        public void Constructor_WithNullMemory_ExpectThrowsArgumentNullException()
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
        public void Constructor_WithNullCommandFactory_ExpectThrowsArgumentNullException()
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
        public void ExecuteProgram_WithNullProgramStartByte_ExpectNotThrowsException()
        {
            // Arrange
            var centralProcessingUnit = CreateCentralProcessingUnit();

            // Act & Assert
            Assert.DoesNotThrow(centralProcessingUnit.ExecuteProgram);
        }

        [Test]
        public void ExecuteProgram_WithNotNullProgramStartByte_ExpectExecutesCommandOneTime()
        {
            // Arrange
            const int programStartAddress = 0x200;
            var memoryStub = Substitute.For<IMemory>();
            memoryStub.ProgramStartAddress.Returns(programStartAddress);
            
            const byte commandOperationCode = 0x00E0;
            memoryStub[memoryStub.ProgramStartAddress].Returns(commandOperationCode);

            var commandMock = Substitute.For<ICommand>();
            var commandFactoryStub = Substitute.For<ICommandFactory>();
            commandFactoryStub.Create(programStartAddress, commandOperationCode).Returns(commandMock);

            var centralProcessingUnit = CreateCentralProcessingUnit(memoryStub, commandFactoryStub);

            // Act
            centralProcessingUnit.ExecuteProgram();

            // Assert
            commandMock.Received(1).Execute();
        }

        [Test]
        public void ExecuteProgram_WithTwoCommandsInMemory_ExpectExecutesEachCommandOneTime()
        {
            // Arrange
            const int programStartAddress = 0x200;
            var memoryStub = Substitute.For<IMemory>();
            memoryStub.ProgramStartAddress.Returns(programStartAddress);

            const byte firstCommandOperationCode = 0x00E0;
            memoryStub[memoryStub.ProgramStartAddress].Returns(firstCommandOperationCode);

            const int secondCommandAddress = programStartAddress + 0x2;
            const byte secondCommandOperationCode = 0x00EE;
            memoryStub[secondCommandAddress].Returns(secondCommandOperationCode);

            var firstCommandMock = Substitute.For<ICommand>();
            firstCommandMock.NextCommandAddress.Returns(secondCommandAddress);

            var secondCommandMock = Substitute.For<ICommand>();
            
            var commandFactoryStub = Substitute.For<ICommandFactory>();
            commandFactoryStub.Create(programStartAddress, firstCommandOperationCode).Returns(firstCommandMock);
            commandFactoryStub.Create(secondCommandAddress, secondCommandOperationCode).Returns(secondCommandMock);

            var centralProcessingUnit = CreateCentralProcessingUnit(memoryStub, commandFactoryStub);

            // Act
            centralProcessingUnit.ExecuteProgram();

            // Assert
            firstCommandMock.Received(1).Execute();
            secondCommandMock.Received(1).Execute();
        }
    }
}