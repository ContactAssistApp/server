using System;
using TraceDefense.Entities.Protos;

namespace TraceDefense.DAL.Providers
{
    public static class PayloadSizeHelper
    {
        public static long GetSize(MatchMessage message)
        {
            //TODO: calculate real size
            return 1;
        }
    }
}
