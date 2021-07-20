using Arcadia.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Arcadia.ArcadiaFrontend.Models
{
    public class IndexViewModel
    {
        public string AirportSelected { get; set; }

        public List<Airport> Airports { get; set; }

        public DateTime Begin { get; set; }

        public DateTime End { get; set; }
    }
}
