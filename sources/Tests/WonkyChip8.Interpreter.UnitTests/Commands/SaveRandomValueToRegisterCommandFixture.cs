using System;
using NSubstitute;
using NUnit.Framework;
using WonkyChip8.Interpreter.Commands;
using WonkyChip8.Interpreter.UnitTests.TestUtilities;

namespace WonkyChip8.Interpreter.UnitTests.Commands
{
    [TestFixture]
    public class SaveRandomValueToRegisterCommandFixture
    {
        private static SaveRandomValueToRegisterCommand CreateSaveRandomValueToRegisterCommand(
            int? address = 0, int operationCode = 0xC000, IGeneralRegisters generalRegisters = null,
            IRandomGenerator randomGenerator = null)
        {
            return new SaveRandomValueToRegisterCommand(address, operationCode,
                                                        generalRegisters ?? Substitute.For<IGeneralRegisters>(),
                                                        randomGenerator ?? Substitute.For<IRandomGenerator>());
        }

        [Test]
        public void Constructor_WithInvalidOperationCode_ExpectedThrowsArgumentOutOfRangeException()
        {
            NUnitExtensions.AssertThrowsArgumentExceptionWithParamName<ArgumentOutOfRangeException>(
                () => CreateSaveRandomValueToRegisterCommand(operationCode: 0x99999), "operationCode");
        }

        [Test]
        public void Constructor_WithNullGeneralRegisters_ExpectedThrowsArgumentNullException()
        {
            NUnitExtensions.AssertThrowsArgumentExceptionWithParamName<ArgumentNullException>(
                () => new SaveRandomValueToRegisterCommand(0, 0xC000, null, Substitute.For<IRandomGenerator>()),
                "generalRegisters");
        }

        [Test]
        public void Constructor_WithNullRandomGenerator_ExpectedThrowsArgumentNullException()
        {
            NUnitExtensions.AssertThrowsArgumentExceptionWithParamName<ArgumentNullException>(
                () => new SaveRandomValueToRegisterCommand(0, 0xC000, Substitute.For<IGeneralRegisters>(), null),
                "randomGenerator");
        }

        [TestCase(0xC000, 0x0, 0x0, 0x0)]
        [TestCase(0xC111, 0x1, 0x11, 0x0)]
        [TestCase(0xC1DA, 0x1, 0xDA, 0xCD)]
        [TestCase(0xCFFF, 0xF, 0xFF, 0xFF)]
        public void Execute_ExpectedSaveMaskedValueFromRandomGeneratorInRegister(int operationCode,
                                                                                 int registerIndex,
                                                                                 int mask,
                                                                                 int randomGeneratorValue)
        {
            // Arrange
            var generalRegistersStub = Substitute.For<IGeneralRegisters>();
            byte? actualRegisterValue = null;
            generalRegistersStub[registerIndex] = Arg.Do<byte?>(value => actualRegisterValue = value);
            generalRegistersStub[registerIndex].Returns(actualRegisterValue);

            var randomGeneratorStub = Substitute.For<IRandomGenerator>();
            randomGeneratorStub.Generate(Arg.Any<int>(), Arg.Any<int>()).Returns(randomGeneratorValue);

            var command = CreateSaveRandomValueToRegisterCommand(0, operationCode, generalRegistersStub,
                                                                 randomGeneratorStub);
            // Act
            command.Execute();

            // Assert
            Assert.AreEqual(actualRegisterValue, randomGeneratorValue & mask);
        }
    }
}