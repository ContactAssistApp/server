using System;
using System.Collections.Generic;
using System.Text;

namespace TraceDefense.DAL.Repositories
{
    public static class TimestampProvider
    {
        public static int GetTimestamp()
        {
            return (Int32)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds;
        }
    }
}
