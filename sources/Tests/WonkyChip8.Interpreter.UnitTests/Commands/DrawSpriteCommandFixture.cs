using System;
using NSubstitute;
using NUnit.Framework;
using WonkyChip8.Interpreter.Commands;
using WonkyChip8.Interpreter.UnitTests.TestUtilities;

namespace WonkyChip8.Interpreter.UnitTests.Commands
{
    [TestFixture]
    public class DrawSpriteCommandFixture
    {
        private static DrawSpriteCommand CreateDrawSpriteCommand(int operationCode = 0xD000,
                                                                 IGraphicsProcessingUnit graphicsProcessingUnit = null,
                                                                 IGeneralRegisters generalRegisters = null,
                                                                 IAddressRegister addressRegister = null,
                                                                 IMemory memory = null)
        {
            return new DrawSpriteCommand(0, operationCode,
                                         graphicsProcessingUnit ?? Substitute.For<IGraphicsProcessingUnit>(),
                                         generalRegisters ?? Substitute.For<IGeneralRegisters>(),
                                         addressRegister ?? Substitute.For<IAddressRegister>(),
                                         memory ?? Substitute.For<IMemory>());
        }

        [TestCase(0xD000, false)]
        public void Execute_ExpectedCallsGraphicsProcessingUnitOnce(int operationCode, bool anyPixelFlipped)
        {
            // Arrange
            var graphicsProcessingUnitMock = Substitute.For<IGraphicsProcessingUnit>();
            graphicsProcessingUnitMock.DrawSprite(Arg.Any<Tuple<int, int>>(), Arg.Any<int>(), Arg.Any<byte[,]>())
                                      .ReturnsForAnyArgs(anyPixelFlipped);

            var generalRegistersStub = Substitute.For<IGeneralRegisters>();
            const int flippingDetectorRegisterIndex = 0xF;
            byte flippingDetectorRegsiterActualValue = 0;
            generalRegistersStub[flippingDetectorRegisterIndex] =
                Arg.Do<byte>(value => flippingDetectorRegsiterActualValue = value);
            generalRegistersStub[flippingDetectorRegisterIndex].Returns(flippingDetectorRegsiterActualValue);

            var addressRegisterStub = Substitute.For<IAddressRegister>();
            const short addressRegisterValue = 0;
            addressRegisterStub.AddressValue.Returns(addressRegisterValue);

            DrawSpriteCommand command = CreateDrawSpriteCommand(graphicsProcessingUnit: graphicsProcessingUnitMock,
                                                                generalRegisters: generalRegistersStub,
                                                                addressRegister: addressRegisterStub);
            // Act
            command.Execute();

            // Assert
            graphicsProcessingUnitMock.Received(1)
                                      .DrawSprite(Arg.Any<Tuple<int, int>>(), Arg.Any<int>(), Arg.Any<byte[,]>());
            Assert.AreEqual(Convert.ToByte(anyPixelFlipped), flippingDetectorRegsiterActualValue);
        }

        [Test]
        public void Constructor_WithInvalidOperationCode_ExpectedThrowsArgumentOutOfRangeException()
        {
            NUnitExtensions.AssertThrowsArgumentExceptionWithParamName<ArgumentOutOfRangeException>(
                () => CreateDrawSpriteCommand(operationCode: 0x99999), "operationCode");
        }

        [Test]
        public void Constructor_WithNullAddressRegister_ExpectedThrowsArgumentNullException()
        {
            NUnitExtensions.AssertThrowsArgumentExceptionWithParamName<ArgumentNullException>(
                () =>
                new DrawSpriteCommand(0, 0xD000, Substitute.For<IGraphicsProcessingUnit>(),
                                      Substitute.For<IGeneralRegisters>(), null, Substitute.For<IMemory>()),
                "addressRegister");
        }

        [Test]
        public void Constructor_WithNullGeneralRegisters_ExpectedThrowsArgumentNullException()
        {
            NUnitExtensions.AssertThrowsArgumentExceptionWithParamName<ArgumentNullException>(
                () =>
                new DrawSpriteCommand(0, 0xD000, Substitute.For<IGraphicsProcessingUnit>(), null,
                                      Substitute.For<IAddressRegister>(), Substitute.For<IMemory>()), "generalRegisters");
        }

        [Test]
        public void Constructor_WithNullGraphicsProcessingUnit_ExpectedThrowsArgumentNullException()
        {
            NUnitExtensions.AssertThrowsArgumentExceptionWithParamName<ArgumentNullException>(
                () =>
                new DrawSpriteCommand(0, 0xD000, null, Substitute.For<IGeneralRegisters>(),
                                      Substitute.For<IAddressRegister>(), Substitute.For<IMemory>()),
                "graphicsProcessingUnit");
        }

        [Test]
        public void Constructor_WithNullMemory_ExpectedThrowsArgumentNullException()
        {
            NUnitExtensions.AssertThrowsArgumentExceptionWithParamName<ArgumentNullException>(
                () =>
                new DrawSpriteCommand(0, 0xD000, Substitute.For<IGraphicsProcessingUnit>(),
                                      Substitute.For<IGeneralRegisters>(),
                                      Substitute.For<IAddressRegister>(), null), "memory");
        }
    }
}