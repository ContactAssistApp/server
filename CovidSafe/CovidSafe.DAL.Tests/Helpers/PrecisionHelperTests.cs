using CovidSafe.DAL.Helpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CovidSafe.Tests.Helpers
{
    [TestClass]
    public class PrecisionHelperTests
    {
        [TestMethod]
        public void StepTest()
        {
            Assert.AreEqual(1.0, PrecisionHelper.GetStep(0));
            Assert.AreEqual(0.5, PrecisionHelper.GetStep(1));
            Assert.AreEqual(0.25, PrecisionHelper.GetStep(2));
            Assert.AreEqual(0.125, PrecisionHelper.GetStep(3));
            Assert.AreEqual(0.0625, PrecisionHelper.GetStep(4));
            Assert.AreEqual(2.0, PrecisionHelper.GetStep(-1));
            Assert.AreEqual(4.0, PrecisionHelper.GetStep(-2));
            Assert.AreEqual(8.0, PrecisionHelper.GetStep(-3));
            Assert.AreEqual(16.0, PrecisionHelper.GetStep(-4));
        }

        [TestMethod]
        public void RoundTest()
        {
	        Assert.AreEqual(0.0, PrecisionHelper.Round(0.0, 0));
	        Assert.AreEqual(0.0, PrecisionHelper.Round(0.0, 1));
	        Assert.AreEqual(0.0, PrecisionHelper.Round(0.0, 2));
	        Assert.AreEqual(0.0, PrecisionHelper.Round(0.0, 3));
	        Assert.AreEqual(0.0, PrecisionHelper.Round(0.0, 4));
	        Assert.AreEqual(0.0, PrecisionHelper.Round(0.0, -1));
	        Assert.AreEqual(0.0, PrecisionHelper.Round(0.0, -2));
	        Assert.AreEqual(0.0, PrecisionHelper.Round(0.0, -3));
	        Assert.AreEqual(0.0, PrecisionHelper.Round(0.0, -4));

	        Assert.AreEqual(0.0, PrecisionHelper.Round(0.5, 0));
	        Assert.AreEqual(0.5, PrecisionHelper.Round(0.5, 1));
	        Assert.AreEqual(0.5, PrecisionHelper.Round(0.5, 2));
	        Assert.AreEqual(0.5, PrecisionHelper.Round(0.5, 3));
	        Assert.AreEqual(0.5, PrecisionHelper.Round(0.5, 4));
	        Assert.AreEqual(0.0, PrecisionHelper.Round(0.5, -1));
	        Assert.AreEqual(0.0, PrecisionHelper.Round(0.5, -2));
	        Assert.AreEqual(0.0, PrecisionHelper.Round(0.5, -3));
	        Assert.AreEqual(0.0, PrecisionHelper.Round(0.5, -4));

	        Assert.AreEqual(-1.0, PrecisionHelper.Round(-0.5, 0));
	        Assert.AreEqual(-0.5, PrecisionHelper.Round(-0.5, 1));
	        Assert.AreEqual(-0.5, PrecisionHelper.Round(-0.5, 2));
	        Assert.AreEqual(-0.5, PrecisionHelper.Round(-0.5, 3));
	        Assert.AreEqual(-0.5, PrecisionHelper.Round(-0.5, 4));
	        Assert.AreEqual(-2.0, PrecisionHelper.Round(-0.5, -1));
	        Assert.AreEqual(-4.0, PrecisionHelper.Round(-0.5, -2));
	        Assert.AreEqual(-8.0, PrecisionHelper.Round(-0.5, -3));
	        Assert.AreEqual(-16.0, PrecisionHelper.Round(-0.5, -4));

	        Assert.AreEqual(1.0, PrecisionHelper.Round(1.0, 0));
	        Assert.AreEqual(1.0, PrecisionHelper.Round(1.0, 1));
	        Assert.AreEqual(1.0, PrecisionHelper.Round(1.0, 2));
	        Assert.AreEqual(1.0, PrecisionHelper.Round(1.0, 3));
	        Assert.AreEqual(1.0, PrecisionHelper.Round(1.0, 4));
	        Assert.AreEqual(0.0, PrecisionHelper.Round(1.0, -1));
	        Assert.AreEqual(0.0, PrecisionHelper.Round(1.0, -2));
	        Assert.AreEqual(0.0, PrecisionHelper.Round(1.0, -3));
	        Assert.AreEqual(0.0, PrecisionHelper.Round(1.0, -4));

	        Assert.AreEqual(-1.0, PrecisionHelper.Round(-1.0, 0));
	        Assert.AreEqual(-1.0, PrecisionHelper.Round(-1.0, 1));
	        Assert.AreEqual(-1.0, PrecisionHelper.Round(-1.0, 2));
	        Assert.AreEqual(-1.0, PrecisionHelper.Round(-1.0, 3));
	        Assert.AreEqual(-1.0, PrecisionHelper.Round(-1.0, 4));
	        Assert.AreEqual(-2.0, PrecisionHelper.Round(-1.0, -1));
	        Assert.AreEqual(-4.0, PrecisionHelper.Round(-1.0, -2));
	        Assert.AreEqual(-8.0, PrecisionHelper.Round(-1.0, -3));
	        Assert.AreEqual(-16.0, PrecisionHelper.Round(-1.0, -4));

			Assert.AreEqual(1.0, PrecisionHelper.Round(1.3, 0));
			Assert.AreEqual(1.0, PrecisionHelper.Round(1.3, 1));
			Assert.AreEqual(1.25, PrecisionHelper.Round(1.3, 2));
			Assert.AreEqual(1.25, PrecisionHelper.Round(1.3, 3));
			Assert.AreEqual(1.25, PrecisionHelper.Round(1.3, 4));
			Assert.AreEqual(0.0, PrecisionHelper.Round(1.3, -1));
			Assert.AreEqual(0.0, PrecisionHelper.Round(1.3, -2));
			Assert.AreEqual(0.0, PrecisionHelper.Round(1.3, -3));
			Assert.AreEqual(0.0, PrecisionHelper.Round(1.3, -4));

			Assert.AreEqual(-2.0, PrecisionHelper.Round(-1.3, 0));
			Assert.AreEqual(-1.5, PrecisionHelper.Round(-1.3, 1));
			Assert.AreEqual(-1.5, PrecisionHelper.Round(-1.3, 2));
			Assert.AreEqual(-1.375, PrecisionHelper.Round(-1.3, 3));
			Assert.AreEqual(-1.3125, PrecisionHelper.Round(-1.3, 4));
			Assert.AreEqual(-2.0, PrecisionHelper.Round(-1.3, -1));
			Assert.AreEqual(-4.0, PrecisionHelper.Round(-1.3, -2));
			Assert.AreEqual(-8.0, PrecisionHelper.Round(-1.3, -3));
			Assert.AreEqual(-16.0, PrecisionHelper.Round(-1.3, -4));

			Assert.AreEqual(37.0, PrecisionHelper.Round(37.9, 0));
	        Assert.AreEqual(37.5, PrecisionHelper.Round(37.9, 1));
	        Assert.AreEqual(37.75, PrecisionHelper.Round(37.9, 2));
	        Assert.AreEqual(37.875, PrecisionHelper.Round(37.9, 3));
	        Assert.AreEqual(37.875, PrecisionHelper.Round(37.9, 4));
	        Assert.AreEqual(36.0, PrecisionHelper.Round(37.9, -1));
	        Assert.AreEqual(36.0, PrecisionHelper.Round(37.9, -2));
	        Assert.AreEqual(32.0, PrecisionHelper.Round(37.9, -3));
	        Assert.AreEqual(32.0, PrecisionHelper.Round(37.9, -4));

	        Assert.AreEqual(-38.0, PrecisionHelper.Round(-37.9, 0));
	        Assert.AreEqual(-38.0, PrecisionHelper.Round(-37.9, 1));
	        Assert.AreEqual(-38.0, PrecisionHelper.Round(-37.9, 2));
	        Assert.AreEqual(-38.0, PrecisionHelper.Round(-37.9, 3));
	        Assert.AreEqual(-37.9375, PrecisionHelper.Round(-37.9, 4));
	        Assert.AreEqual(-38.0, PrecisionHelper.Round(-37.9, -1));
	        Assert.AreEqual(-40.0, PrecisionHelper.Round(-37.9, -2));
	        Assert.AreEqual(-40.0, PrecisionHelper.Round(-37.9, -3));
	        Assert.AreEqual(-48.0, PrecisionHelper.Round(-37.9, -4));
        }
    }
}