using System;
using System.Linq;
using NUnit.Framework;

namespace WonkyChip8.Interpreter.UnitTests
{
    [TestFixture]
    public class RandomAccessMemoryFixture
    {
        [Test]
        public void LoadProgram_WithNullProgramBytes_ExpectThrowsArgumentNullException()
        {
            // Arrange
            var randomAccessMemory = new RandomAccessMemory();

            // Act
            TestDelegate loadProgram = () => randomAccessMemory.LoadProgram(null);

            // Assert
            var argumentNullException = Assert.Throws<ArgumentNullException>(loadProgram);
            Assert.AreEqual("programBytes", argumentNullException.ParamName);
        }

        [Test]
        public void LoadProgram_WithProperProgramBytes_ExpectLoadsProgramInMemory()
        {
            // Arrange
            var randomAccessMemory = new RandomAccessMemory();
            var programBytes = new int?[] {0xAA, 0x01, 0xBB, 0x11};

            // Act
            randomAccessMemory.LoadProgram(programBytes);

            // Assert
            for (var address = 0; address < programBytes.Count(); address++)
                Assert.AreEqual(programBytes[address], randomAccessMemory[randomAccessMemory.ProgramStartAddress + address]);
            Assert.AreEqual(programBytes[0], randomAccessMemory[randomAccessMemory.ProgramStartAddress]);
        }

        [Test]
        public void UnloadProgram_WithLoadedProgram_ExpectCleansMemory()
        {
            // Arrange
            var randomAccessMemory = new RandomAccessMemory();
            for (var i = randomAccessMemory.ProgramStartAddress; i < randomAccessMemory.EndAddress; i++)
                randomAccessMemory[i] = 1;

            // Act
            randomAccessMemory.UnloadProgram();

            // Assert
            for (var i = randomAccessMemory.ProgramStartAddress; i < randomAccessMemory.EndAddress; i++)
                Assert.AreEqual(null, randomAccessMemory[i]);
            Assert.AreEqual(null, randomAccessMemory[randomAccessMemory.ProgramStartAddress]);
        }

        [TestCase(0x000, 1, ExpectedResult = 1)]
        [TestCase(0x200, 1, ExpectedResult = 1)]
        [TestCase(0xFFF, 1, ExpectedResult = 1)]
        public int? ThisIndexer_WithProperCellAddress_ExpectEqualsValue(int cellAddress, int value)
        {
            // Arrange
            var randomAccessMemory = new RandomAccessMemory();

            // Act
            randomAccessMemory[cellAddress] = value;

            // Assert
            return randomAccessMemory[cellAddress];
        }

        [TestCase(-0x001, 1)]
        [TestCase(0x1000, 1)]
        public void ThisIndexer_WithInvalidCellAddress_ExpectThrowsArgumentOutOfRangeException(int cellAddress, byte value)
        {
            // Arrange
            var randomAccessMemory = new RandomAccessMemory();

            // Act
            TestDelegate accessToRandomAccessMemoryCellValue = () => randomAccessMemory[cellAddress] = value;

            // Assert
            var argumentOutOfRangeException =
                Assert.Throws<ArgumentOutOfRangeException>(accessToRandomAccessMemoryCellValue);
            Assert.AreEqual("index", argumentOutOfRangeException.ParamName);
        }
    }
}