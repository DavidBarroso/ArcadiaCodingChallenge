using System;
using System.Collections.Generic;
using System.Text;

namespace Arcadia.Model
{
    /// <summary>
    /// Airport
    /// </summary>
    [Serializable]
    public class Airport
    {
        /// <summary>
        /// Gets or sets the icao.
        /// </summary>
        /// <value>
        /// The icao.
        /// </value>
        public string Icao { get; set; }

        /// <summary>
        /// Gets or sets the iata.
        /// </summary>
        /// <value>
        /// The iata.
        /// </value>
        public string Code { get; set; }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the alias.
        /// </summary>
        /// <value>
        /// The alias.
        /// </value>
        public string Alias { get; set; }

        /// <summary>
        /// Gets or sets the city.
        /// </summary>
        /// <value>
        /// The city.
        /// </value>
        public string City { get; set; }

        /// <summary>
        /// Gets or sets the state.
        /// </summary>
        /// <value>
        /// The state.
        /// </value>
        public string State { get; set; }

        /// <summary>
        /// Gets or sets the country.
        /// </summary>
        /// <value>
        /// The country.
        /// </value>
        public string Country { get; set; }

        /// <summary>
        /// Gets or sets the elevation.
        /// </summary>
        /// <value>
        /// The elevation.
        /// </value>
        public string Elev { get; set; }

        /// <summary>
        /// Gets or sets the lat.
        /// </summary>
        /// <value>
        /// The lat.
        /// </value>
        public string Lat { get; set; }

        /// <summary>
        /// Gets the latitude.
        /// </summary>
        /// <value>
        /// The latitude.
        /// </value>
        public double Latitude
        {
            get
            {
                double latitude;
                if (Double.TryParse(this.Lat, out latitude))
                    return latitude;
                return double.NaN;
            }
        }

        /// <summary>
        /// Gets or sets the lon.
        /// </summary>
        /// <value>
        /// The lon.
        /// </value>
        public string Lon { get; set; }

        /// <summary>
        /// Gets the longitude.
        /// </summary>
        /// <value>
        /// The longitude.
        /// </value>
        public double Longitude
        {
            get
            {
                double longitude;
                if (Double.TryParse(this.Lon, out longitude))
                    return longitude;
                return double.NaN;
            }
        }

        /// <summary>
        /// Gets or sets the tz.
        /// </summary>
        /// <value>
        /// The tz.
        /// </value>
        public string Tz { get; set; }
    }
}
