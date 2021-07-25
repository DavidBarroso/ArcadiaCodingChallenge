using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading.Tasks;

namespace Arcadia.ArcadiaFrontend.Extensions
{
    /// <summary>
    /// SessionExtension
    /// </summary>
    public static class SessionExtension
    {
        /// <summary>
        /// Gets from session.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="that">The that.</param>
        /// <param name="key">The key.</param>
        /// <param name="logger">The logger.</param>
        /// <returns></returns>
        public static T GetFromSession<T>(this ISession that, string key, ILogger logger = null) where T : class
        {
            try
            {
                byte[] value = null;
                if (that.TryGetValue(key, out value))
                {
                    using (MemoryStream stream = new MemoryStream(value))
                    {
                        BinaryFormatter serializer = new BinaryFormatter();
                        T typedValue = (T)serializer.Deserialize(stream);
                        return typedValue;
                    }
                }
            }
            catch (Exception ex)
            {
                if (logger != null)
                    logger.LogError(string.Format("Error GetFromSession key:{0}", key), ex);
            }
            return null;
        }

        /// <summary>
        /// Sets the in session.
        /// </summary>
        /// <param name="that">The that.</param>
        /// <param name="key">The key.</param>
        /// <param name="value">The value.</param>
        /// <param name="logger">The logger.</param>
        public static void SetInSession(this ISession that, string key, object value, ILogger logger = null)
        {
            try
            {
                using (MemoryStream stream = new MemoryStream())
                {
                    BinaryFormatter serializer = new BinaryFormatter();
                    serializer.Serialize(stream, value);
                    that.Set(key, stream.GetBuffer());
                }
            }
            catch (Exception ex)
            {
                if (logger != null)
                    logger.LogError(string.Format("Error SaveInSession key:{0}", key), ex);
            }
        }
    }
}
