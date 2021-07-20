using Arcadia.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Arcadia.ArcadiaBackend.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ArrivalsController : ControllerBase
    {
        private readonly ILogger<ArrivalsController> _logger;

        public ArrivalsController(ILogger<ArrivalsController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public IEnumerable<Arrivals> Get()
        {

            //OpenSkyClient.OpenSkyClient client = new OpenSkyClient.OpenSkyClient("https://opensky-network.org/api", "DavidBarroso", "AirbusTest");
            //OpenSkyClient.OpenSkyClient client = new OpenSkyClient.OpenSkyClient("https://opensky-network.org/api");
            //var rs = client.GetArrivals("EDDF", DateTime.Now, DateTime.Now.AddHours(12));
            //return rs;

            //MOCK DATA
            var random = new Random();
            return Enumerable.Range(1, 5).Select(index => new Arrivals
            {
                Icao24 = Guid.NewGuid().ToString(),
                FirstSeen = random.Next(),
                EstDepartureAirport = Guid.NewGuid().ToString(),
                LastSeen = random.Next(),
                EstArrivalAirport = Guid.NewGuid().ToString(),
                Callsign = Guid.NewGuid().ToString(),
                EstDepartureAirportHorizDistance = random.Next(),
                EstDepartureAirportVertDistance = random.Next(),
                EstArrivalAirportHorizDistance = random.Next(),
                EstArrivalAirportVertDistance = random.Next(),
                DepartureAirportCandidatesCount = random.Next(),
                ArrivalAirportCandidatesCount = random.Next()
            })
            .ToArray();
            //END MOCK DATA
        }
    }
}
