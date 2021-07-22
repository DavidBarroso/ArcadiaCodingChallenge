using Arcadia.Model;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Arcadia.ArcadiaFrontend.Models
{
    public class IndexViewModel
    {
        public string SelectedAirport { get; set; }

        public List<Airport> Airports { get; set; }

        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-ddTHH:mm}")]
        public DateTime Begin { get; set; }

        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-ddTHH:mm}")]
        public DateTime End { get; set; }

        public List<Arrivals> Arrivals { get; set; }
    }
}
