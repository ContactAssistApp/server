using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

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
        /// <param name="message">Source object</param>
        /// <returns>Message size, in bytes</returns>
        public static long GetSize(object message)
        {
            if(message != null)
            {
                using (MemoryStream stream = new MemoryStream())
                {
                    BinaryFormatter formatter = new BinaryFormatter();
                    formatter.Serialize(stream, message);
                    return stream.ToArray().Length;
                }
            }
            else
            {
                throw new ArgumentNullException(nameof(message));
            }
        }
    }
}
