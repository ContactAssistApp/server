using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using CovidSafe.API.Controllers;

namespace CovidSafe.API.Tests
{
    class LoadTest
    {
        [TestMethod]
        public void TestMethod()
        {
        }

        [DataTestMethod]
        [assembly: Parallelize(Workers = 0, Scope = ExecutionScope.MethodLevel)]
        [DynamicData(nameof(GetData), DynamicDataSourceType.Method)]
        public void TestAddDynamicDataMethod(int a, int b, int expected)
        {
            var actual = a + b; //TODO: unit test functions under Controllers.MessageController
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
