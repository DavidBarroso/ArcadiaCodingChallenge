using Arcadia.Model;
using Arcadia.RestClientUtils;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;

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
            string json = System.IO.File.ReadAllText("./Resources/airportsSP_DE.json");
            Airport[] airports = RestClientFactory.GetData<Airport[]>(json);

            return airports;
        }
    }
}
