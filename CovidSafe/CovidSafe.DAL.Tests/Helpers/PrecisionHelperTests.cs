using CovidSafe.DAL.Helpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CovidSafe.Tests.Helpers
{
    [TestClass]
    public class PrecisionHelperTests
    {
        [TestMethod]
        public void RoundTest()
        {
	        Assert.AreEqual(0, PrecisionHelper.Round(0, 0));
	        Assert.AreEqual(0, PrecisionHelper.Round(0, 1));
	        Assert.AreEqual(0, PrecisionHelper.Round(0, 2));
	        Assert.AreEqual(0, PrecisionHelper.Round(0, 3));
	        Assert.AreEqual(0, PrecisionHelper.Round(0, 4));
			Assert.AreEqual(0, PrecisionHelper.Round(0, 5));
			Assert.AreEqual(0, PrecisionHelper.Round(0, 6));
			Assert.AreEqual(0, PrecisionHelper.Round(0, 7));
			Assert.AreEqual(0, PrecisionHelper.Round(0, 8));
			Assert.AreEqual(0, PrecisionHelper.Round(0, 9));

			Assert.AreEqual(0, PrecisionHelper.Round(1, 0));
	        Assert.AreEqual(0, PrecisionHelper.Round(1, 1));
	        Assert.AreEqual(0, PrecisionHelper.Round(1, 2));
	        Assert.AreEqual(0, PrecisionHelper.Round(1, 3));
	        Assert.AreEqual(0, PrecisionHelper.Round(1, 4));
	        Assert.AreEqual(0, PrecisionHelper.Round(1, 5));
	        Assert.AreEqual(0, PrecisionHelper.Round(1, 6));
	        Assert.AreEqual(0, PrecisionHelper.Round(1, 7));
	        Assert.AreEqual(0, PrecisionHelper.Round(1, 8));
			Assert.AreEqual(1, PrecisionHelper.Round(1, 9));

			Assert.AreEqual(0, PrecisionHelper.Round(-1, 0));
			Assert.AreEqual(0, PrecisionHelper.Round(-1, 1));
			Assert.AreEqual(0, PrecisionHelper.Round(-1, 2));
			Assert.AreEqual(0, PrecisionHelper.Round(-1, 3));
			Assert.AreEqual(0, PrecisionHelper.Round(-1, 4));
			Assert.AreEqual(0, PrecisionHelper.Round(-1, 5));
			Assert.AreEqual(0, PrecisionHelper.Round(-1, 6));
			Assert.AreEqual(0, PrecisionHelper.Round(-1, 7));
			Assert.AreEqual(0, PrecisionHelper.Round(-1, 8));
			Assert.AreEqual(-1, PrecisionHelper.Round(-1, 9));

			Assert.AreEqual(0, PrecisionHelper.Round(37, 0));
	        Assert.AreEqual(0, PrecisionHelper.Round(37, 1));
	        Assert.AreEqual(0, PrecisionHelper.Round(37, 2));
	        Assert.AreEqual(0, PrecisionHelper.Round(37, 3));
	        Assert.AreEqual(32, PrecisionHelper.Round(37, 4));
	        Assert.AreEqual(32, PrecisionHelper.Round(37, 5));
	        Assert.AreEqual(32, PrecisionHelper.Round(37, 6));
	        Assert.AreEqual(36, PrecisionHelper.Round(37, 7));
	        Assert.AreEqual(36, PrecisionHelper.Round(37, 8));
			Assert.AreEqual(37, PrecisionHelper.Round(37, 9));

			Assert.AreEqual(0, PrecisionHelper.Round(-37, 0));
			Assert.AreEqual(0, PrecisionHelper.Round(-37, 1));
			Assert.AreEqual(0, PrecisionHelper.Round(-37, 2));
			Assert.AreEqual(0, PrecisionHelper.Round(-37, 3));
			Assert.AreEqual(-32, PrecisionHelper.Round(-37, 4));
			Assert.AreEqual(-32, PrecisionHelper.Round(-37, 5));
			Assert.AreEqual(-32, PrecisionHelper.Round(-37, 6));
			Assert.AreEqual(-36, PrecisionHelper.Round(-37, 7));
			Assert.AreEqual(-36, PrecisionHelper.Round(-37, 8));
			Assert.AreEqual(-37, PrecisionHelper.Round(-37, 9));
		}
    }
}