using System;
using NUnit.Framework;

namespace WonkyChip8.Interpreter.UnitTests.TestUtilities
{
    public static class NUnitExtensions
    {
        public static void AssertThrowsArgumentExceptionWithParamName<TArgumentException>(TestDelegate testCode,
                                                                                          string expectedParamName)
            where TArgumentException : ArgumentException
        {
            var argumentException = Assert.Throws<TArgumentException>(testCode);
            Assert.AreEqual(expectedParamName, argumentException.ParamName);
        }
    }
}