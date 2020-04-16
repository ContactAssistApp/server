using System;

using CovidSafe.Entities.Protos;

namespace CovidSafe.DAL.Helpers
{
	/// <summary>
	/// Helper functions for working with rounding numbers with controlled precision.
	/// Used to interpret <see cref="Region"/>
	/// </summary>
	public static class PrecisionHelper
	{
		/// <summary>
		/// Returns difference between 2 closest numbers for given precision parameter
		/// </summary>
		/// <param name="precision">Precision parameter, any integer</param>
		/// <returns>1 / 2^precision</returns>
		public static double GetStep(int precision)
		{
			return precision < 0 ? (double)(1 << (-precision)) : 1.0 / (double)(1 << precision);
		}

		private static double RoundSymmetric(double d, int precision)
		{
			if (precision >= 0)
			{
				int exp = 1 << precision;
				return ((double)(int)(d * exp)) / exp;
			}
			else
			{
				int exp = 1 << (-precision);
				return (double)(((int)(d / exp)) * exp);
			}
		}
		/// <summary>
		/// Rounds given number and precision parameter.
		/// </summary>
		/// <param name="d">Any double number</param>
		/// <param name="precision">Precision parameter, any integer</param>
		/// <returns>Nearest number aligned with precision grid, equal to d or closer to zero</returns>
		/// <remarks>
		/// Reference: https://csharpindepth.com/articles/FloatingPoint
		/// Reference: https://jonskeet.uk/csharp/DoubleConverter.cs
		/// </remarks>
		public static double Round(double d, int precision)
		{
			double shift = (double)(1 << 16);   //16 is some number that 1 << 16 > 180 and bigger than maximum precision value that we are using
			return (RoundSymmetric(d + shift, precision) - shift);
		}

		/// <summary>
		/// For given number and precision parameter, returns range [xmin, xmax] containing the number,
		/// where xmin &lt; xmax, xmin and xmax are to numbers next to each other on the grid aligned with given precision
		/// </summary>
		/// <param name="d">Any double number</param>
		/// <param name="precision">Precision parameter, any integer</param>
		/// <returns>Tuple of doubles (xmin, xmax)</returns>
		public static Tuple<double, double> GetRange(double d, int precision)
		{
			double rounded = Round(d, precision);
			double step = GetStep(precision);

			return Tuple.Create(rounded, rounded + step);
		}
	}
}
