using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Arcadia.ArcadiaBackend.Helpers
{
    /// <summary>
    /// GISUtils
    /// </summary>
    public static class GISUtils
    {
        /// <summary>
        /// The earth radius
        /// </summary>
        public const double EARTH_RADIUS = 6371;

        /// <summary>
        /// To RAD
        /// </summary>
        public const double TO_RAD = Math.PI / 180;

        /// <summary>
        /// Gets the distance.
        /// Using Haversine formula to calculate distance and assuming that the longitudes and latitudes are in geographic system (WGS84/EPSG:4623)
        /// </summary>
        /// <param name="lat1">The lat1.</param>
        /// <param name="lon1">The lon1.</param>
        /// <param name="lat2">The lat2.</param>
        /// <param name="lon2">The lon2.</param>
        /// <returns></returns>
        public static double GetDistanceInGeographicSystem(double lat1, double lon1, double lat2, double lon2)
        {
            double deltaLatRad = (lat2 - lat1) * TO_RAD;
            double deltaLonRad = (lon2 - lon1) * TO_RAD;
            double a = Math.Sin(deltaLatRad / 2) * Math.Sin(deltaLatRad / 2) + Math.Cos(lat1 * (Math.PI / 180)) * Math.Cos(lat2 * (Math.PI / 180)) * Math.Sin(deltaLonRad / 2) * Math.Sin(deltaLonRad / 2);
            double c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));
            double distance = EARTH_RADIUS * c;
            return distance;
        }
    }
}
