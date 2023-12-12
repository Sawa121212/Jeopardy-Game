using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Cheaz.Common.Extensions.Tests
{
    [TestClass]
    public class NumericExtensionsTests
    {
        [TestMethod]
        public void UshortReverseBytesTest()
        {
            ushort test = 0xFF00;
            var reversed = test.ReverseBytes();
            Assert.AreEqual(0X00FF, reversed);
        }

        [TestMethod]
        public void CastTest()
        {
            int defaultVal = -1;
            object sbMax = 1.154;
            if (sbMax is IConvertible convertible)
            {
                var i = convertible.CastTo<int>(defaultVal);

                Assert.AreNotEqual(i, defaultVal);
            }
        }
    }
}