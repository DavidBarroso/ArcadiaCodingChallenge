//using Arcadia.ArcadiaBackend.Utils;
using Arcadia.Model;
using Arcadia.RestClientUtils;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;

namespace Arcadia.ArcadiaBackend.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AirportsController : ControllerBase
    {
        private readonly ILogger<AirportsController> _logger;

        public AirportsController(ILogger<AirportsController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public IEnumerable<Airport> Get()
        {
            //string json = System.IO.File.ReadAllText("./Resources/airportsSP_DE.json");
            string json = System.IO.File.ReadAllText("./Resources/airports.json");
            Airport[] airports = RestClientFactory.GetData<Airport[]>(json);

            return airports.ToList().Where(x =>
            {
                return !string.IsNullOrWhiteSpace(x.Name) && (x.Country == "Germany" || x.Country == "Spain");
            }).OrderBy(x => x.Country).ThenBy(x => x.Name).ToArray();
        }
    }
}
