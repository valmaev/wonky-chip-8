using System;
using NUnit.Framework;

namespace WonkyChip8.Interpreter.UnitTests.TestUtilities
{
    public static class NUnitUtilities
    {
        public static void AssertThrowsArgumentExceptionWithParamName<TArgumentException>(
            TestDelegate testCode, string expectedParamName) where TArgumentException : ArgumentException
        {
            if (testCode == null)
                throw new ArgumentNullException("testCode");
            if (expectedParamName == null)
                throw new ArgumentNullException("expectedParamName");

            var argumentException = Assert.Throws<TArgumentException>(testCode);
            Assert.AreEqual(expectedParamName, argumentException.ParamName);
        }
    }
}