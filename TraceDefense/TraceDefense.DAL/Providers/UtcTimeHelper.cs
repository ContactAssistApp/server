using System;
using System.Collections.Generic;
using System.Text;
using TraceDefense.Entities.Protos;

namespace TraceDefense.DAL.Providers
{
    /// <summary>
    /// Helper class which casts UNIX epoch (ms) to <see cref="UTCTime"/>
    /// </summary>
    public static class UtcTimeHelper
    {
        /// <summary>
        /// Casts a UNIX epoch timestamp to a <see cref="UTCTime"/> equivalent
        /// </summary>
        /// <param name="timestampMs">UNIX epoch time, in ms</param>
        /// <returns><see cref="UTCTime"/> equivalent</returns>
        public static UTCTime ToUtcTime(long timestampMs)
        {
            if(timestampMs > 0)
            {
                DateTimeOffset parsed = DateTimeOffset.FromUnixTimeMilliseconds(timestampMs);

                // Return new object
                return new UTCTime
                {
                    Year = parsed.Year,
                    Month = parsed.Month,
                    Day = parsed.Day,
                    Hour = parsed.Hour,
                    Minute = parsed.Minute,
                    Second = parsed.Second,
                    Millisecond = parsed.Millisecond
                };
            }
            else
            {
                throw new ArgumentOutOfRangeException(nameof(timestampMs));
            }
        }

        /// <summary>
        /// Casts a <see cref="UTCTime"/> to its UNIX epoch equivalent, in ms
        /// </summary>
        /// <param name="timestamp"><see cref="UTCTime"/> source</param>
        /// <returns>UNIX epoch equivalent, in ms</returns>
        public static long ToMilliseconds(UTCTime timestamp)
        {
            if(timestamp != null)
            {
                // Create new DateTimeOffset object
                DateTimeOffset parsed = new DateTimeOffset(
                    timestamp.Year,
                    timestamp.Month,
                    timestamp.Day,
                    timestamp.Hour,
                    timestamp.Minute,
                    timestamp.Second,
                    timestamp.Millisecond,
                    TimeSpan.Zero
                );

                return parsed.ToUnixTimeMilliseconds();
            }
            else
            {
                throw new ArgumentNullException(nameof(timestamp));
            }
        }
    }
}
