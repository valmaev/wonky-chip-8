using NUnit.Framework;
using WonkyChip8.Interpreter.Commands;

namespace WonkyChip8.Interpreter.UnitTests.Commands
{
    [TestFixture]
    public class CommandFixture
    {
        public class CommandStub : Command
        {
            public CommandStub(int? address, int? operationCode) : base(address, operationCode) { }

            public override void Execute() { }
        }

        public static CommandStub CreateCommandStub(int? address = 0, int? operationCode = 0)
        {
            return new CommandStub(address, operationCode);
        }

        [Test]
        public void Constructor_ExpectNotThrowsException()
        {
            // Arrange
            CommandStub commandStub = null;
            const int expectedOperationCode = 99999;

            // Act & Assert
            Assert.DoesNotThrow(() => commandStub = CreateCommandStub(operationCode: expectedOperationCode));
            Assert.AreEqual(0, commandStub.Address);
            Assert.AreEqual(expectedOperationCode, commandStub.OperationCode);
        }

        [Test]
        public void NextCommandAddress_ExpectReturnsCommandAddressPlusTwoByte()
        {
            // Arrange
            const int nextCommandExpectedAddress = 2;

            // Act & Assert
            Assert.AreEqual(nextCommandExpectedAddress, CreateCommandStub().NextCommandAddress);
        }

        [Test]
        public void NextCommandAddress_WithNullAddress_ExpectReturnsNull()
        {
            // Act & Assert
            Assert.IsNull(CreateCommandStub(address: null).NextCommandAddress);
        }

        [Test]
        public void Execute_ExpectNotThrowsException()
        {
            // Act & Assert
            Assert.DoesNotThrow(() => CreateCommandStub().Execute());
        }

        [Test]
        public void Bits_WithNotNullOperationCode_ExpectReturnsProperValues()
        {
            // Arrange
            var commandStub = CreateCommandStub(operationCode: 0x1234);

            // Assert
            Assert.AreEqual(0x1, commandStub.FirstOperationCodeHalfBit);
            Assert.AreEqual(0x2, commandStub.SecondOperationCodeHalfBit);
            Assert.AreEqual(0x3, commandStub.ThirdOperationCodeHalfBit);
            Assert.AreEqual(0x4, commandStub.FourthOperationCodeHalfBit);

            Assert.AreEqual(0x12, commandStub.FirstOperationCodeBit);
            Assert.AreEqual(0x34, commandStub.SecondOperationCodeBit);
        }

        [Test]
        public void Bits_WithNullOperationCode_ExpectReturnsNullValues()
        {
            // Arrange
            var commandStub = CreateCommandStub(operationCode: null);

            // Assert
            Assert.IsNull(commandStub.FirstOperationCodeHalfBit);
            Assert.IsNull(commandStub.SecondOperationCodeHalfBit);
            Assert.IsNull(commandStub.ThirdOperationCodeHalfBit);
            Assert.IsNull(commandStub.FourthOperationCodeHalfBit);
            Assert.IsNull(commandStub.FirstOperationCodeBit);
            Assert.IsNull(commandStub.SecondOperationCodeBit);
        }
    }
}