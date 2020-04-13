using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using CovidSafe.API.Controllers;


namespace CovidSafe.API.Tests
{
    [TestClass]
    public class UnitTest
    {
        [TestMethod]
        public void TestMethod()
        {
        }

        [DataTestMethod]
        [DynamicData(nameof(GetData), DynamicDataSourceType.Method)]
        public void TestAddDynamicDataMethod(int a, int b, int expected)
        {
            var actual = a+b; //TODO: unit test functions under Controllers.MessageController
            Assert.AreEqual(expected, actual);
        }
        public static IEnumerable<object[]> GetData()
        {
            yield return new object[] { 1, 1, 2 };
            yield return new object[] { 12, 30, 42 };
            yield return new object[] { 14, 1, 15 };
        }

    }
}
