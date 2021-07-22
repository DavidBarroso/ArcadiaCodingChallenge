using Arcadia.Model;
using Arcadia.RestClientUtils;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using RestSharp;
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
        private const string HOST = "https://opensky-network.org/api";
        private const string ARRIVALS_RESOURCE = "/flights/arrival";
        

        private readonly ILogger<ArrivalsController> _logger;

        public ArrivalsController(ILogger<ArrivalsController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public IEnumerable<Arrivals> Get(string icao, string begin, string end)
        {
            ////MockRQ
            //icao = "EDDF";
            //begin = "1517227200";
            //end = "1517230800";
            ////END MockRQ

            KeyValuePair<string, object> param1 = new KeyValuePair<string, object>("airport", icao);
            KeyValuePair<string, object> param2 = new KeyValuePair<string, object>("begin", begin);
            KeyValuePair<string, object> param3 = new KeyValuePair<string, object>("end", end);

            RestClient client = RestClientFactory.CreateRestClient(HOST, "DavidBarroso", "AirbusTest");
            RestRequest rq = RestClientFactory.CreateRestRequest(ARRIVALS_RESOURCE, Method.GET, DataFormat.Json, param1, param2, param3);
            IRestResponse rs = client.Get(rq);
            if (rs.StatusCode == System.Net.HttpStatusCode.OK && rs.ContentType.ToLower().Contains("json"))
            {
                Arrivals[] arrivals = RestClientFactory.GetData<Arrivals[]>(rs.Content);
                if (arrivals == null)
                    return null;

                return arrivals;
            }
            return null;

            ////MOCK DATA
            //var random = new Random();
            //return Enumerable.Range(1, 5).Select(index => new Arrivals
            //{
            //    Icao24 = Guid.NewGuid().ToString(),
            //    FirstSeen = random.Next(),
            //    EstDepartureAirport = Guid.NewGuid().ToString(),
            //    LastSeen = random.Next(),
            //    EstArrivalAirport = Guid.NewGuid().ToString(),
            //    Callsign = Guid.NewGuid().ToString(),
            //    EstDepartureAirportHorizDistance = random.Next(),
            //    EstDepartureAirportVertDistance = random.Next(),
            //    EstArrivalAirportHorizDistance = random.Next(),
            //    EstArrivalAirportVertDistance = random.Next(),
            //    DepartureAirportCandidatesCount = random.Next(),
            //    ArrivalAirportCandidatesCount = random.Next()
            //})
            //.ToArray();
            //END MOCK DATA
        }
    }
}
