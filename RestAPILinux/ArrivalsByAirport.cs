using System;

namespace RestAPILinux
{
    /// <summary>
    /// ArrivalsByAirport
    /// </summary>
    public class ArrivalsByAirport
    {
        /// <summary>
        /// Gets or sets the icao24.
        /// </summary>
        /// <value>
        /// The icao24.
        /// </value>
        public string icao24 { get; set; }

        /// <summary>
        /// Gets or sets the first seen.
        /// </summary>
        /// <value>
        /// The first seen.
        /// </value>
        public int firstSeen { get; set; }

        /// <summary>
        /// Gets or sets the est departure airport.
        /// </summary>
        /// <value>
        /// The est departure airport.
        /// </value>
        public string estDepartureAirport { get; set; }

        /// <summary>
        /// Gets or sets the last seen.
        /// </summary>
        /// <value>
        /// The last seen.
        /// </value>
        public int lastSeen { get; set; }

        /// <summary>
        /// Gets or sets the est arrival airport.
        /// </summary>
        /// <value>
        /// The est arrival airport.
        /// </value>
        public string estArrivalAirport { get; set; }

        /// <summary>
        /// Gets or sets the callsign.
        /// </summary>
        /// <value>
        /// The callsign.
        /// </value>
        public string callsign { get; set; }

        /// <summary>
        /// Gets or sets the est departure airport horiz distance.
        /// </summary>
        /// <value>
        /// The est departure airport horiz distance.
        /// </value>
        public int estDepartureAirportHorizDistance { get; set; }

        /// <summary>
        /// Gets or sets the est departure airport vert distance.
        /// </summary>
        /// <value>
        /// The est departure airport vert distance.
        /// </value>
        public int estDepartureAirportVertDistance { get; set; }

        /// <summary>
        /// Gets or sets the est arrival airport horiz distance.
        /// </summary>
        /// <value>
        /// The est arrival airport horiz distance.
        /// </value>
        public int estArrivalAirportHorizDistance { get; set; }

        /// <summary>
        /// Gets or sets the est arrival airport vert distance.
        /// </summary>
        /// <value>
        /// The est arrival airport vert distance.
        /// </value>
        public int estArrivalAirportVertDistance { get; set; }

        /// <summary>
        /// Gets or sets the departure airport candidates count.
        /// </summary>
        /// <value>
        /// The departure airport candidates count.
        /// </value>
        public int departureAirportCandidatesCount { get; set; }

        /// <summary>
        /// Gets or sets the arrival airport candidates count.
        /// </summary>
        /// <value>
        /// The arrival airport candidates count.
        /// </value>
        public int arrivalAirportCandidatesCount { get; set; }

    }
}
