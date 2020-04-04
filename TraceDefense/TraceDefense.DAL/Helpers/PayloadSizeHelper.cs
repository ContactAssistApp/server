using System;
using System.IO;

using ProtoBuf;
using TraceDefense.Entities.Protos;

namespace TraceDefense.DAL.Helpers
{
    /// <summary>
    /// Helper for determining size (in bytes) of objects
    /// </summary>
    public static class PayloadSizeHelper
    {
        /// <summary>
        /// Calculates the size (in bytes) of a <see cref="MatchMessage"/>
        /// </summary>
        /// <param name="message">Source <see cref="MatchMessage"/></param>
        /// <returns><see cref="MatchMessage"/> size, in bytes</returns>
        public static long GetSize(MatchMessage message)
        {
            if(message != null)
            {
                // Serialize object into a byte array and return its size
                MemoryStream stream = new MemoryStream();
                Serializer.Serialize(stream, message);
                return stream.ToArray().Length;
            }
            else
            {
                throw new ArgumentNullException(nameof(message));
            }
        }
    }
}
