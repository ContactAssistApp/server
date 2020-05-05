using System;
using System.IO;

using ProtoBuf;

namespace CovidSafe.DAL.Helpers
{
    /// <summary>
    /// Helper for determining size (in bytes) of objects
    /// </summary>
    public static class PayloadSizeHelper
    {
        /// <summary>
        /// Calculates the size (in bytes) of an <see cref="object"/>
        /// </summary>
        /// <param name="message">Source <see cref="object"/></param>
        /// <returns><see cref="object"/> size, in bytes</returns>
        public static long GetSize(object message)
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
