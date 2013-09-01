using System;
using System.Collections.Generic;
using System.Linq;
using NSubstitute;
using NUnit.Framework;
using WonkyChip8.Interpreter.UnitTests.TestUtilities;

namespace WonkyChip8.Interpreter.UnitTests
{
    [TestFixture]
    public class RandomAccessMemoryFixture
    {
        private const int DefaultCapacity = 0x1000;

        private static RandomAccessMemory CreateMemory(int capacity = DefaultCapacity)
        {
            return new RandomAccessMemory(capacity);
        }

        private static byte[] CreateProgramBytes()
        {
            return new byte[] {0xAA, 0x01, 0xBB, 0x11};
        }

        [TestCase(-0x1)]
        [TestCase(DefaultCapacity)]
        [TestCase(DefaultCapacity + 1)]
        public void LoadBytes_WithInvalidCellAddress_ExpectedThrowsArgumentOutOfRangeException(int invalidCellAddress)
        {
            NUnitUtilities.AssertThrowsArgumentExceptionWithParamName<ArgumentOutOfRangeException>(
                () => CreateMemory().LoadBytes(invalidCellAddress, Arg.Any<IEnumerable<byte>>()), "cellAddress");
        }

        [Test]
        public void LoadBytes_WithNullProgramBytes_ExpectedThrowsArgumentNullException()
        {
            NUnitUtilities.AssertThrowsArgumentExceptionWithParamName<ArgumentNullException>(
                () => CreateMemory().LoadBytes(0, null), "bytes");
        }

        [Test]
        public void LoadBytes_WithProperBytes_ExpectedLoadsProgramInMemory()
        {
            // Arrange
            var randomAccessMemory = CreateMemory();
            const int programStartAddress = 0x200;
            var programBytes = CreateProgramBytes();

            // Act
            randomAccessMemory.LoadBytes(programStartAddress, programBytes);

            // Assert
            for (var address = 0; address < programBytes.Count(); address++)
                Assert.AreEqual(programBytes[address], randomAccessMemory[programStartAddress + address]);
            Assert.AreEqual(programBytes[0], randomAccessMemory[programStartAddress]);
        }

        [Test]
        public void LoadBytes_ExpectedCapacityNotChanged()
        {
            // Arrange
            var memory = CreateMemory();
            var expectedCapacity = memory.Capacity;

            // Act
            memory.LoadBytes(0, new byte[] { 0, 1, 1 });

            // Assert
            Assert.AreEqual(expectedCapacity, memory.Capacity);
        }

        [Test]
        public void ProgramStartAddress_ExpectedDefualtValueIsZero()
        {
            Assert.AreEqual(0, CreateMemory().ProgramStartAddress);
        }

        [TestCase(0x0, 0x0)]
        [TestCase(0x200, DefaultCapacity - 1)]
        public void UnloadBytes_WithProperArguments_ExpectedCleansMemory(int firstCellAddress, int lastCellAddress)
        {
            // Arrange
            var randomAccessMemory = CreateMemory();
            for (var i = firstCellAddress; i <= lastCellAddress; i++)
                randomAccessMemory[i] = 1;

            // Act
            randomAccessMemory.UnloadBytes(firstCellAddress, lastCellAddress);

            // Assert
            for (var i = firstCellAddress; i <= lastCellAddress; i++)
                Assert.AreEqual(0, randomAccessMemory[i]);
            Assert.AreEqual(0, randomAccessMemory[firstCellAddress]);
        }

        [TestCase(-0x1, 0x0, "firstCellAddress")]
        [TestCase(DefaultCapacity, 0x0, "firstCellAddress")]
        [TestCase(DefaultCapacity + 1, 0x0, "firstCellAddress")]
        [TestCase(0x0, -0x1, "lastCellAddress")]
        [TestCase(0x1, 0x0, "lastCellAddress")]
        [TestCase(0x0, DefaultCapacity, "lastCellAddress")]
        [TestCase(0x0, DefaultCapacity + 1, "lastCellAddress")]
        public void UnloadBytes_WithInvalidArguments_ExpectedThrowsArgumentOutOfRangeException(int firstCellAddress,
                                                                                               int lastCellAddress,
                                                                                               string exceptionParamName)
        {
            NUnitUtilities.AssertThrowsArgumentExceptionWithParamName<ArgumentOutOfRangeException>(
                () => CreateMemory().UnloadBytes(firstCellAddress, lastCellAddress), exceptionParamName);
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

        [Test]
        public void ThisIndexer_AfterSettingCellValue_ExpectedCapacityNotChanged()
        {
            // Arrange
            var memory = CreateMemory();
            var expectedCapacity = memory.Capacity;
            const byte someValueToSet = 1;

            // Act
            memory[0] = someValueToSet;

            // Assert
            Assert.AreEqual(expectedCapacity, memory.Capacity);
        }
    }
}