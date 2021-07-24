using System;

namespace Arcadia.Model
{
    /// <summary>
    /// Arrivals
    /// </summary>
    [Serializable]
    public class Arrivals
    {
        /// <summary>
        /// Gets or sets the icao24.
        /// </summary>
        /// <value>
        /// The icao24.
        /// </value>
        public string Icao24 { get; set; }

        /// <summary>
        /// Gets or sets the first seen.
        /// </summary>
        /// <value>
        /// The first seen.
        /// </value>
        public int? FirstSeen { get; set; }

        /// <summary>
        /// Gets or sets the est departure airport.
        /// </summary>
        /// <value>
        /// The est departure airport.
        /// </value>
        public string EstDepartureAirport { get; set; }

        /// <summary>
        /// Gets or sets the last seen.
        /// </summary>
        /// <value>
        /// The last seen.
        /// </value>
        public int? LastSeen { get; set; }

        /// <summary>
        /// Gets or sets the est arrival airport.
        /// </summary>
        /// <value>
        /// The est arrival airport.
        /// </value>
        public string EstArrivalAirport { get; set; }

        /// <summary>
        /// Gets or sets the callsign.
        /// </summary>
        /// <value>
        /// The callsign.
        /// </value>
        public string Callsign { get; set; }

        /// <summary>
        /// Gets or sets the est departure airport horiz distance.
        /// </summary>
        /// <value>
        /// The est departure airport horiz distance.
        /// </value>
        public int? EstDepartureAirportHorizDistance { get; set; }

        /// <summary>
        /// Gets or sets the est departure airport vert distance.
        /// </summary>
        /// <value>
        /// The est departure airport vert distance.
        /// </value>
        public int? EstDepartureAirportVertDistance { get; set; }

        /// <summary>
        /// Gets or sets the est arrival airport horiz distance.
        /// </summary>
        /// <value>
        /// The est arrival airport horiz distance.
        /// </value>
        public int? EstArrivalAirportHorizDistance { get; set; }

        /// <summary>
        /// Gets or sets the est arrival airport vert distance.
        /// </summary>
        /// <value>
        /// The est arrival airport vert distance.
        /// </value>
        public int? EstArrivalAirportVertDistance { get; set; }

        /// <summary>
        /// Gets or sets the departure airport candidates count.
        /// </summary>
        /// <value>
        /// The departure airport candidates count.
        /// </value>
        public int? DepartureAirportCandidatesCount { get; set; }

        /// <summary>
        /// Gets or sets the arrival airport candidates count.
        /// </summary>
        /// <value>
        /// The arrival airport candidates count.
        /// </value>
        public int? ArrivalAirportCandidatesCount { get; set; }

    }
}
