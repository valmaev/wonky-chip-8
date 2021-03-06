﻿using System;
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
                                                                 IDisplay display = null,
                                                                 IGeneralRegisters generalRegisters = null,
                                                                 IAddressRegister addressRegister = null,
                                                                 IMemory memory = null)
        {
            return new DrawSpriteCommand(0, operationCode,
                                         display ?? Substitute.For<IDisplay>(),
                                         generalRegisters ?? Substitute.For<IGeneralRegisters>(),
                                         addressRegister ?? Substitute.For<IAddressRegister>(),
                                         memory ?? Substitute.For<IMemory>());
        }

        private static IDisplay CreateDisplayMock(bool anyPixelFlipped = false)
        {
            var displayMock = Substitute.For<IDisplay>();
            displayMock.DrawSprite(Arg.Any<Tuple<int, int>>(), Arg.Any<byte[]>()).ReturnsForAnyArgs(anyPixelFlipped);
            return displayMock;
        }

        [Test]
        public void Constructor_WithInvalidOperationCode_ExpectedThrowsArgumentOutOfRangeException()
        {
            NUnitUtilities.AssertThrowsArgumentExceptionWithParamName<ArgumentOutOfRangeException>(
                () => CreateDrawSpriteCommand(operationCode: 0x99999), "operationCode");
        }

        [Test]
        public void Constructor_WithNullAddressRegister_ExpectedThrowsArgumentNullException()
        {
            NUnitUtilities.AssertThrowsArgumentExceptionWithParamName<ArgumentNullException>(
                () =>
                new DrawSpriteCommand(0, 0xD000, Substitute.For<IDisplay>(),
                                      Substitute.For<IGeneralRegisters>(), null, Substitute.For<IMemory>()),
                "addressRegister");
        }

        [Test]
        public void Constructor_WithNullGeneralRegisters_ExpectedThrowsArgumentNullException()
        {
            NUnitUtilities.AssertThrowsArgumentExceptionWithParamName<ArgumentNullException>(
                () =>
                new DrawSpriteCommand(0, 0xD000, Substitute.For<IDisplay>(), null,
                                      Substitute.For<IAddressRegister>(), Substitute.For<IMemory>()),
                "generalRegisters");
        }

        [Test]
        public void Constructor_WithNullDisplay_ExpectedThrowsArgumentNullException()
        {
            NUnitUtilities.AssertThrowsArgumentExceptionWithParamName<ArgumentNullException>(
                () =>
                new DrawSpriteCommand(0, 0xD000, null, Substitute.For<IGeneralRegisters>(),
                                      Substitute.For<IAddressRegister>(), Substitute.For<IMemory>()),
                "display");
        }

        [Test]
        public void Constructor_WithNullMemory_ExpectedThrowsArgumentNullException()
        {
            NUnitUtilities.AssertThrowsArgumentExceptionWithParamName<ArgumentNullException>(
                () =>
                new DrawSpriteCommand(0, 0xD000, Substitute.For<IDisplay>(),
                                      Substitute.For<IGeneralRegisters>(), Substitute.For<IAddressRegister>(), null),
                "memory");
        }

        [TestCase(0xD000, false)]
        [TestCase(0xD121, true)]
        public void Execute_ExpectedWriteResultOfDrawingToFlippingDetectorRegister(int operationCode, bool anyPixelFlipped)
        {
            // Arrange
            var displayMock = CreateDisplayMock(anyPixelFlipped);

            var generalRegistersStub = Substitute.For<IGeneralRegisters>();
            const int flippingDetectorRegisterIndex = 0xF;
            byte flippingDetectorRegisterActualValue = 0;
            generalRegistersStub[flippingDetectorRegisterIndex] =
                Arg.Do<byte>(value => flippingDetectorRegisterActualValue = value);
            generalRegistersStub[flippingDetectorRegisterIndex].Returns(flippingDetectorRegisterActualValue);

            var addressRegisterStub = Substitute.For<IAddressRegister>();
            const short addressRegisterValue = 0;
            addressRegisterStub.AddressValue.Returns(addressRegisterValue);

            DrawSpriteCommand command = CreateDrawSpriteCommand(operationCode, displayMock,
                                                                generalRegistersStub, addressRegisterStub);
            // Act
            command.Execute();

            // Assert
            Assert.AreEqual(Convert.ToByte(anyPixelFlipped), flippingDetectorRegisterActualValue);
        }

        [TestCase(0xD000, 0x00, 0x00)]
        [TestCase(0xDFFF, 0xFF, 0xFF)]
        [TestCase(0xD1A3, 0x1A, 0x32)]
        public void Execute_ExpectedCallsDisplayWithProperCoordinates(int operationCode, byte firstRegisterValue,
                                                                      byte secondRegisterValue)
        {
            // Arrange
            var firstRegisterIndex = (operationCode & 0x0F00) >> 8;
            var secondRegisterIndex = (operationCode & 0x00F0) >> 4;

            var generalRegistersStub = Substitute.For<IGeneralRegisters>();
            generalRegistersStub[firstRegisterIndex].Returns(firstRegisterValue);
            generalRegistersStub[secondRegisterIndex].Returns(secondRegisterValue);

            var displayMock = CreateDisplayMock(anyPixelFlipped: true);

            var command = CreateDrawSpriteCommand(operationCode, displayMock, generalRegistersStub);

            // Act
            command.Execute();

            // Assert
            displayMock.Received(1).DrawSprite(new Tuple<int, int>(firstRegisterValue, secondRegisterValue), 
                                               Arg.Any<byte[]>());
        }
    }
}