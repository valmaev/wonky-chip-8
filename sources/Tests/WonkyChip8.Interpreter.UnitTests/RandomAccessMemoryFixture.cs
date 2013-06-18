using System;
using System.Linq;
using NUnit.Framework;
using WonkyChip8.Interpreter.UnitTests.TestUtilities;

namespace WonkyChip8.Interpreter.UnitTests
{
    [TestFixture]
    public class RandomAccessMemoryFixture
    {
        private static RandomAccessMemory CreateMemory(int capacity = 0xFFF)
        {
            return new RandomAccessMemory(capacity);
        }

        private static byte[] CreateProgramBytes()
        {
            return new byte[] {0xAA, 0x01, 0xBB, 0x11};
        }

        [Test]
        public void LoadProgram_WithNullProgramBytes_ExpectedThrowsArgumentNullException()
        {
            NUnitUtilities.AssertThrowsArgumentExceptionWithParamName<ArgumentNullException>(
                () => CreateMemory().LoadProgram(0, null), "programBytes");
        }

        [Test]
        public void LoadProgram_WithProperProgramBytes_ExpectedLoadsProgramInMemory()
        {
            // Arrange
            var randomAccessMemory = CreateMemory();
            const int programStartAddress = 0x200;
            var programBytes = CreateProgramBytes();

            // Act
            randomAccessMemory.LoadProgram(programStartAddress, programBytes);

            // Assert
            for (var address = 0; address < programBytes.Count(); address++)
                Assert.AreEqual(programBytes[address], randomAccessMemory[programStartAddress + address]);
            Assert.AreEqual(programBytes[0], randomAccessMemory[programStartAddress]);
        }

        [Test]
        public void LoadProgram_ExpectedSetsProgramStartAddressFromParameter()
        {
            // Arrange
            var memory = CreateMemory();
            const int expectedProgramStartAddress = 0x200;
            
            // Act
            memory.LoadProgram(expectedProgramStartAddress, CreateProgramBytes());

            // Assert
            Assert.AreEqual(expectedProgramStartAddress, memory.ProgramStartAddress);
        }

        [Test]
        public void ProgramStartAddress_ExpectedDefualtValueIsNull()
        {
            Assert.IsNull(CreateMemory().ProgramStartAddress);
        }

        [Test]
        public void UnloadProgram_WithLoadedProgram_ExpectedCleansMemory()
        {
            // Arrange
            const int capacity = 100;
            var randomAccessMemory = CreateMemory();
            const int programStartAddress = 0x200;
            for (var i = programStartAddress; i < capacity; i++)
                randomAccessMemory[i] = 1;

            // Act
            randomAccessMemory.UnloadProgram(programStartAddress);

            // Assert
            for (var i = programStartAddress; i < capacity; i++)
                Assert.AreEqual(0, randomAccessMemory[i]);
            Assert.AreEqual(0, randomAccessMemory[programStartAddress]);
        }

        [Test]
        public void UnloadProgram_ExpectedSetsProgramStartAddressToNull()
        {
            // Arrange
            var memory = CreateMemory();
            const int programStartAddress = 0x200;
            memory.LoadProgram(programStartAddress, CreateProgramBytes());

            // Act
            memory.UnloadProgram(programStartAddress);

            // Assert
            Assert.IsNull(memory.ProgramStartAddress);
        }

        [TestCase(0x000, 1, ExpectedResult = 1)]
        [TestCase(0x200, 1, ExpectedResult = 1)]
        [TestCase(0xFFF, 1, ExpectedResult = 1)]
        public int ThisIndexer_WithProperCellAddress_ExpectedEqualsValue(int cellAddress, byte value)
        {
            // Arrange
            var randomAccessMemory = CreateMemory();

            // Act
            randomAccessMemory[cellAddress] = value;

            // Assert
            return randomAccessMemory[cellAddress];
        }

        [TestCase(-0x001, 1)]
        [TestCase(0x1000, 1)]
        public void ThisIndexer_WithInvalidCellAddress_ExpectedThrowsArgumentOutOfRangeException(int cellAddress, byte value)
        {
            NUnitUtilities.AssertThrowsArgumentExceptionWithParamName<ArgumentOutOfRangeException>(
                () => CreateMemory()[cellAddress] = value, "index");
        }
    }
}