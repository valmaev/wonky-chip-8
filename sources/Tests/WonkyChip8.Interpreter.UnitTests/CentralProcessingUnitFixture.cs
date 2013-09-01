using System;
using NSubstitute;
using NUnit.Framework;
using WonkyChip8.Interpreter.UnitTests.TestUtilities;

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

        private static IMemory CreateMemoryStub(int programStartAddress)
        {
            var memoryStub = Substitute.For<IMemory>();
            memoryStub.ProgramStartAddress.Returns(programStartAddress);
            return memoryStub;
        }

        [Test]
        public void Constructor_WithNullMemory_ExpectedThrowsArgumentNullException()
        {
            NUnitUtilities.AssertThrowsArgumentExceptionWithParamName<ArgumentNullException>(
                () => new CentralProcessingUnit(null, Substitute.For<ICommandFactory>()), "memory");
        }

        [Test]
        public void Constructor_WithNullCommandFactory_ExpectedThrowsArgumentNullException()
        {
            NUnitUtilities.AssertThrowsArgumentExceptionWithParamName<ArgumentNullException>(
                () => new CentralProcessingUnit(Substitute.For<IMemory>(), null), "commandFactory");
        }

        [Test]
        public void ExecuteProgram_ExpectedExecutesCommandOneTime()
        {
            // Arrange
            const int programStartAddress = 0x200;
            const int commandOperationCode = 0x00E0;
            
            var memoryStub = CreateMemoryStub(programStartAddress);
            memoryStub[programStartAddress].Returns((byte) 0x00);
            memoryStub[programStartAddress + 1].Returns((byte) 0xE0);

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
        public void ExecuteProgram_WithTwoCommandsInMemory_ExpectedExecutesEachCommandOneTime()
        {
            // Arrange
            const int programStartAddress = 0x200;
            var memoryStub = CreateMemoryStub(programStartAddress);

            const int firstCommandOperationCode = 0x00E0;
            memoryStub[programStartAddress].Returns((byte) 0x00);
            memoryStub[programStartAddress + 1].Returns((byte) 0xE0);

            const int secondCommandAddress = programStartAddress + 0x2;
            const int secondCommandOperationCode = 0x00EE;
            memoryStub[secondCommandAddress].Returns((byte)0x00);
            memoryStub[secondCommandAddress + 1].Returns((byte)0xEE);

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