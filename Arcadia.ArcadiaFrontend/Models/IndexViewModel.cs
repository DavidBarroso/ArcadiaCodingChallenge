using Arcadia.Model;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Arcadia.ArcadiaFrontend.Models
{
    public class IndexViewModel
    {
        public string SelectedAirport { get; set; }

        public List<Airport> Airports { get; set; }

        public DateTime Begin { get; set; }

        public DateTime End { get; set; }
    }
}
