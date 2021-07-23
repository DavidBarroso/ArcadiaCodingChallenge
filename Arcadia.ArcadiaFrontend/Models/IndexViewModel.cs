using Arcadia.Model;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Arcadia.ArcadiaFrontend.Models
{
    /// <summary>
    /// IndexViewModel
    /// </summary>
    public class IndexViewModel
    {
        /// <summary>
        /// Gets or sets the selected airport.
        /// </summary>
        /// <value>
        /// The selected airport.
        /// </value>
        public string SelectedAirport { get; set; }

        /// <summary>
        /// Gets or sets the airports.
        /// </summary>
        /// <value>
        /// The airports.
        /// </value>
        public List<Airport> Airports { get; set; }

        /// <summary>
        /// Gets or sets the world airports.
        /// </summary>
        /// <value>
        /// The world airports.
        /// </value>
        public List<Airport> WorldAirports { get; set; }

        /// <summary>
        /// Gets or sets the begin.
        /// </summary>
        /// <value>
        /// The begin.
        /// </value>
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-ddTHH:mm}")]
        public DateTime Begin { get; set; }

        /// <summary>
        /// Gets or sets the end.
        /// </summary>
        /// <value>
        /// The end.
        /// </value>
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-ddTHH:mm}")]
        public DateTime End { get; set; }

        /// <summary>
        /// Gets or sets the arrivals.
        /// </summary>
        /// <value>
        /// The arrivals.
        /// </value>
        public List<Arrivals> Arrivals { get; set; }
    }
}
