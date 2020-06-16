using System;

namespace CovidSafe.DAL.Helpers
{
	/// <summary>
	/// Helper functions for working with rounding numbers with controlled precision.
	/// </summary>
	public static class PrecisionHelper
	{
		/// <summary>
		/// Rounds given number and precision parameter.
		/// </summary>
		/// <param name="d">Any integer</param>
		/// <param name="precision">Precision parameter, any integer</param>
		/// <returns>Nearest number aligned with precision grid, equal to d or closer to 0.
		/// Precision=0 maps any int from [-256, 256] to 0.
		/// Precision>=9 maps any int to itself</returns>
		public static int Round(int d, int precision)
        {
			int bits = Math.Max(9 - precision, 0);
			return d >= 0 ? d >> bits << bits : -(-d >> bits << bits);
        }

		/// <summary>
		/// Rounds given number and precision parameter. Method for legacy API that supports float values.
		/// </summary>
		/// <param name="d">Any double value</param>
		/// <param name="precision">Precision parameter, any integer</param>
		/// <returns>Nearest number aligned with precision grid, equal to d or closer to 0.
		/// Precision=0 maps any double from [-256, 256] to 0.
		/// Precision=9 maps any double to its int lower bound</returns>
		public static int Round(double d, int precision)
		{
			return Round((int)d, precision);
		}
	}
}
