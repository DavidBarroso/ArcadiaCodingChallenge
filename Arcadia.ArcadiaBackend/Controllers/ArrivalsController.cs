using Arcadia.ArcadiaBackend.Helpers;
using Arcadia.Model;
using Arcadia.RestClientUtils;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Arcadia.ArcadiaBackend.Controllers
{
    /// <summary>
    /// ArrivalsController
    /// </summary>
    /// <seealso cref="Microsoft.AspNetCore.Mvc.ControllerBase" />
    [ApiController]
    [Route("[controller]")]
    public class ArrivalsController : ControllerBase
    {
        /// <summary>
        /// The host
        /// </summary>
        private const string HOST = "https://opensky-network.org/api";
        /// <summary>
        /// The arrivals resource
        /// </summary>
        private const string ARRIVALS_RESOURCE = "/flights/arrival";


        /// <summary>
        /// The logger
        /// </summary>
        private readonly ILogger<ArrivalsController> _logger;
        /// <summary>
        /// The cache
        /// </summary>
        private IMemoryCache _cache;

        /// <summary>
        /// Initializes a new instance of the <see cref="ArrivalsController" /> class.
        /// </summary>
        /// <param name="logger">The logger.</param>
        /// <param name="memoryCache">The memory cache.</param>
        public ArrivalsController(ILogger<ArrivalsController> logger, IMemoryCache memoryCache)
        {
            _cache = memoryCache;
            _logger = logger;
        }

        /// <summary>
        /// Gets the specified icao.
        /// </summary>
        /// <param name="icao">The icao.</param>
        /// <param name="begin">The begin.</param>
        /// <param name="end">The end.</param>
        /// <returns></returns>
        [HttpGet]
        public IEnumerable<Arrivals> Get(string icao, string begin, string end)
        {
            //Get all airports
            Airport[] airports = _cache.Get<Airport[]>(ArcadiaUtils.AIRPORT_CACHE_KEY);

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

                //Set distances from departure airports
                arrivals.ToList().ForEach(arrival => CalculateDistanceFromDepartureAirport(airports, arrival));
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


        /// <summary>
        /// Calculates the distance from departure airport.
        /// </summary>
        /// <param name="airports">The airports.</param>
        /// <param name="arrival">The arrival.</param>
        private void CalculateDistanceFromDepartureAirport(Airport[] airports, Arrivals arrival)
        {
            arrival.DistanceToDepartureAirport = null;
            if (airports == null || !airports.Any() ||
                arrival == null || string.IsNullOrWhiteSpace(arrival.EstArrivalAirport) || string.IsNullOrWhiteSpace(arrival.EstDepartureAirport))
                return;

            Airport arrivalAirport = airports.FirstOrDefault(x => x.Icao == arrival.EstArrivalAirport);
            Airport departureAirport = airports.FirstOrDefault(x => x.Icao == arrival.EstDepartureAirport);

            if (arrivalAirport == null || double.IsNaN(arrivalAirport.Longitude) || double.IsNaN(arrivalAirport.Latitude) ||
               departureAirport == null || double.IsNaN(departureAirport.Longitude) || double.IsNaN(departureAirport.Latitude))
                return;

            double distance = GISUtils.GetDistanceInGeographicSystem(arrivalAirport.Latitude, arrivalAirport.Longitude, departureAirport.Latitude, departureAirport.Longitude);
            arrival.DistanceToDepartureAirport = distance;
        }
    }
}
