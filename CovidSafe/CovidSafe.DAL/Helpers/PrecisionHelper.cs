using System;

namespace CovidSafe.DAL.Helpers
{
	/// <summary>
	/// Helper functions for working with rounding numbers with controlled precision.
	/// </summary>
	public static class PrecisionHelper
	{
		public static int GetStep(int precision)
        {
			int bits = Math.Max(8 - precision, 0);
			return 1 << bits;
        }

		/// <summary>
		/// Rounds given number and precision parameter.
		/// </summary>
		/// <param name="d">Any integer</param>
		/// <param name="precision">Precision parameter, any integer</param>
		/// <returns>Nearest number aligned with precision grid, equal to d or closer to 0.
		/// Precision=0 maps any int from [-256, 256] to 0.
		/// Precision>=8 maps any int to itself</returns>
		public static int Round(int d, int precision)
        {
			int bits = Math.Max(8 - precision, 0);
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

		/// <summary>
		/// For given number and precision parameter, returns range [xmin, xmax] containing the number,
		/// where xmin &lt; xmax, xmin and xmax are to numbers next to each other on the grid aligned with given precision
		/// Rounds given number and precision parameter. Method for legacy API that supports float values.
		/// </summary>
		/// <param name="d">Any double number</param>
		/// <param name="precision">Precision parameter, any integer</param>
		/// <returns>Tuple of ints (xmin, xmax)</returns>
		public static Tuple<int, int> GetRange(int d, int precision)
        {
			int rounded = Round(d, precision);
			int step = GetStep(precision);

			if (rounded == 0)
				return Tuple.Create(-step, step);
			else if (rounded > 0)
				return Tuple.Create(rounded, rounded + step);
			return Tuple.Create(rounded - step, rounded);
		}
	}
}
