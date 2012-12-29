using System;
using System.Linq;
using NUnit.Framework;

namespace WonkyChip8.Interpreter.UnitTests
{
    [TestFixture]
    public class RandomAccessMemoryFixture
    {
        [Test]
        public void LoadProgram_WithNullProgramBytes_ExpectArgumentNullException()
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
        public void LoadProgram_WithProperProgramBytes_ExpectProgramLoadsInMemory()
        {
            // Arrange
            var randomAccessMemory = new RandomAccessMemory();
            var programBytes = new byte?[] {0xAA, 0x01, 0xBB, 0x11};

            // Act
            randomAccessMemory.LoadProgram(programBytes);

            // Assert
            for (var address = 0; address < programBytes.Count(); address++)
                Assert.AreEqual(programBytes[address], randomAccessMemory[RandomAccessMemory.ProgramStartAddress + address]);
            Assert.AreEqual(programBytes[0], randomAccessMemory.ProgramStartByte);
        }

        [Test]
        public void UnloadProgram_WithLoadedProgram_ExpectCleansMemory()
        {
            // Arrange
            var randomAccessMemory = new RandomAccessMemory();
            for (var i = RandomAccessMemory.ProgramStartAddress; i < RandomAccessMemory.EndAddress; i++)
                randomAccessMemory[i] = 1;

            // Act
            randomAccessMemory.UnloadProgram();

            // Assert
            for (var i = RandomAccessMemory.ProgramStartAddress; i < RandomAccessMemory.EndAddress; i++)
                Assert.AreEqual(null, randomAccessMemory[i]);
            Assert.AreEqual(null, randomAccessMemory.ProgramStartByte);
        }

        [TestCase(0x000, 1, ExpectedResult = 1)]
        [TestCase(0x200, 1, ExpectedResult = 1)]
        [TestCase(0xFFF, 1, ExpectedResult = 1)]
        public byte? ThisIndexer_WithProperCellAddress_ExpectEqualValue(int cellAddress, byte value)
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
        public void ThisIndexer_WithInvalidCellAddress_ExpectedArgumentOutOfRangeException(int cellAddress, byte value)
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