using System;
using NSubstitute;
using NUnit.Framework;
using WonkyChip8.Interpreter.Commands;
using WonkyChip8.Interpreter.UnitTests.TestUtilities;

namespace WonkyChip8.Interpreter.UnitTests.Commands
{
    [TestFixture]
    public class GraphicsCommandFactoryFixture
    {
        internal static GraphicsCommandFactory CreateGraphicsCommandFactory(IGeneralRegisters generalRegisters = null,
                                                                           IAddressRegister addressRegister = null,
                                                                           IMemory memory = null,
                                                                           IGraphicsProcessingUnit
                                                                               graphicsProcessingUnit = null)
        {
            return new GraphicsCommandFactory(generalRegisters ?? Substitute.For<IGeneralRegisters>(),
                                              addressRegister ?? Substitute.For<IAddressRegister>(),
                                              memory ?? Substitute.For<IMemory>(),
                                              graphicsProcessingUnit ?? Substitute.For<IGraphicsProcessingUnit>());
        }

        [Test]
        public void Constructor_WithNullGeneralRegisters_ExpectedThrowsArgumentNullException()
        {
            NUnitUtilities.AssertThrowsArgumentExceptionWithParamName<ArgumentNullException>(
                () => new GraphicsCommandFactory(null, Substitute.For<IAddressRegister>(),
                                                 Substitute.For<IMemory>(), Substitute.For<IGraphicsProcessingUnit>()),
                "generalRegisters");
        }

        [Test]
        public void Constructor_WithNullAddressRegister_ExpectedThrowsArgumentNullException()
        {
            NUnitUtilities.AssertThrowsArgumentExceptionWithParamName<ArgumentNullException>(
                () => new GraphicsCommandFactory(Substitute.For<IGeneralRegisters>(), null,
                                                 Substitute.For<IMemory>(), Substitute.For<IGraphicsProcessingUnit>()),
                "addressRegister");
        }

        [Test]
        public void Constructor_WithNullMemory_ExpectedThrowsArgumentNullException()
        {
            NUnitUtilities.AssertThrowsArgumentExceptionWithParamName<ArgumentNullException>(
                () => new GraphicsCommandFactory(Substitute.For<IGeneralRegisters>(),
                                                 Substitute.For<IAddressRegister>(), null,
                                                 Substitute.For<IGraphicsProcessingUnit>()),
                "memory");
        }

        [Test]
        public void Constructor_WithNullGraphicsProcessingUnit_ExpectedThrowsArgumentNullException()
        {
            NUnitUtilities.AssertThrowsArgumentExceptionWithParamName<ArgumentNullException>(
                () => new GraphicsCommandFactory(Substitute.For<IGeneralRegisters>(),
                                                 Substitute.For<IAddressRegister>(), Substitute.For<IMemory>(),
                                                 null),
                "graphicsProcessingUnit");
        }

        [TestCase(0x00E0, typeof (ClearScreenCommand))]
        [TestCase(0xD000, typeof (DrawSpriteCommand))]
        public void Create_WithProperOperationCode_ExpectedReturnsCommandWithProperType(int operationCode,
                                                                                        Type commandType)
        {
            // Arrange
            var commandFactory = CreateGraphicsCommandFactory();

            // Act
            ICommand command = commandFactory.Create(0, operationCode);

            // Assert
            Assert.IsInstanceOf(commandType, command);
        }

        [TestCase(0x0000)]
        [TestCase(0x99999)]
        [TestCase(0xF000)]
        public void Create_WithNotSupportedOperationCode_ExpectedReturnsNullCommand(int notSupportedOperationCode)
        {
            // Arrange
            var commandFactory = CreateGraphicsCommandFactory();

            // Act
            ICommand command = commandFactory.Create(0, notSupportedOperationCode);

            // Assert
            Assert.IsInstanceOf<NullCommand>(command);
        }

    }
}