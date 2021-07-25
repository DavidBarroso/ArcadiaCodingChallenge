using Arcadia.Model;
using Arcadia.RestClientUtils;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Arcadia.ArcadiaBackend.Helpers
{
    /// <summary>
    ///ArcadiaUtils
    /// </summary>
    public static class ArcadiaUtils
    {
        /// <summary>
        /// The airport cache key
        /// </summary>
        public const string AIRPORT_CACHE_KEY = "AIRPORT_CACHE";

        /// <summary>
        /// Gets the world airports.
        /// </summary>
        /// <returns></returns>
        public static Airport[] GetWorldAirports()
        {
            //string json = System.IO.File.ReadAllText("./Resources/airportsSP_DE.json");
            string json = System.IO.File.ReadAllText("./Resources/airports.json");
            Airport[] airports = RestClientFactory.GetData<Airport[]>(json);
            return airports;
        }
    }
}
