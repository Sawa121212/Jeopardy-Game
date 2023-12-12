using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Cheaz.Common.Extensions.Tests
{
    [TestClass]
    public class StringExtensionsTests
    {
        [TestMethod]
        public void ToByteArrayTest()
        {
            var test = "This is a test string";
            var bytes = test.ToByteArray();
            var restored = bytes.FromByteArray();
            Assert.AreEqual(test, restored);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ToByteArrayExceptionTest()
        {
            string test = null; 
            var bytes = test.ToByteArray();
        }

        [TestMethod]
        public void ToUshortArrayTest()
        {
            var test = "This is a test string";
            var ushorts = test.ToUshortArray();
            var restored = ushorts.FromUshortArray();
            Assert.AreEqual(test, restored);
        }
        
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ToUshortArrayExceptionTest()
        {
            string test = null;
            var ushorts = test.ToUshortArray();
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void FromUshortArrayExceptionTest()
        {
            ushort[] ushorts = null;
            var s = ushorts.FromUshortArray();
        }
    }
}
