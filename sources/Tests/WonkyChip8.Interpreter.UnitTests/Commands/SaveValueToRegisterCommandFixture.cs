using System;
using NSubstitute;
using NUnit.Framework;
using WonkyChip8.Interpreter.Commands;

namespace WonkyChip8.Interpreter.UnitTests.Commands
{
    [TestFixture]
    public class SaveValueToRegisterCommandFixture
    {
        public static SaveValueToRegisterCommand CreateSaveValueToRegisterCommand(int? address = 0,
                                                                                  int operationCode = 0x6000,
                                                                                  IRegisters registers = null)
        {
            return new SaveValueToRegisterCommand(address, operationCode, registers ?? Substitute.For<IRegisters>());
        }

        [Test]
        public void Constructor_WithInvalidOperationCode_ExpectedThrowsArgumentOutOfRangeException()
        {
            // Act & Assert
            var argumentOutOfRangeException =
                Assert.Throws<ArgumentOutOfRangeException>(
                    () => new SaveValueToRegisterCommand(0, 0x99999, Substitute.For<IRegisters>()));
            Assert.AreEqual("operationCode", argumentOutOfRangeException.ParamName);
        }

        [Test]
        public void Constructor_WithNullRegisters_ExpectecThrowsArgumentNullException()
        {
            // Act & Assert
            var argumentNullException =
                Assert.Throws<ArgumentNullException>(() => new SaveValueToRegisterCommand(0, 0x6000, null));
            Assert.AreEqual("registers", argumentNullException.ParamName);
        }

        [TestCase(0x610A, 0x0A)]
        [TestCase(0x6202, 0x02)]
        public void Execute_WithProperOperationCode_ExpectedSaveValueToRegister(int operationCode, byte expectedValueToSave)
        {
            // Arrange
            var registersStub = Substitute.For<IRegisters>();
            byte? registerValue = null;
            registersStub[Arg.Any<int>()] = Arg.Do<byte?>(arg => registerValue = arg);

            var saveValueToRegisterCommand = CreateSaveValueToRegisterCommand(0, operationCode, registersStub);

            // Act
            saveValueToRegisterCommand.Execute();

            // Assert
            Assert.AreEqual(expectedValueToSave, registerValue);
        }
    }
}