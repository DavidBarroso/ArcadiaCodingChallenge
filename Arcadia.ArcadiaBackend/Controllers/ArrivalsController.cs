using Arcadia.ArcadiaBackend.Helpers;
using Arcadia.Model;
using Arcadia.RestClientUtils;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using RestSharp;
using System.Collections.Generic;
using System.Linq;

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
        /// The configuration
        /// </summary>
        private readonly IConfiguration _configuration;

        /// <summary>
        /// The logger
        /// </summary>
        private readonly ILogger<ArrivalsController> _logger;

        /// <summary>
        /// The cache
        /// </summary>
        private readonly IMemoryCache _cache;

        /// <summary>
        /// Initializes a new instance of the <see cref="ArrivalsController"/> class.
        /// </summary>
        /// <param name="logger">The logger.</param>
        /// <param name="memoryCache">The memory cache.</param>
        /// <param name="configuration">The configuration.</param>
        public ArrivalsController(ILogger<ArrivalsController> logger, IMemoryCache memoryCache, IConfiguration configuration)
        {
            _cache = memoryCache;
            _logger = logger;
            _configuration = configuration;
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

            //Open sky config settings
            string host = _configuration.GetValue<string>("OpenSkyRestAPIConfiguration:Host", "https://opensky-network.org/api");
            string userName = _configuration.GetValue<string>("OpenSkyRestAPIConfiguration:UserName", null);
            string pass = _configuration.GetValue<string>("OpenSkyRestAPIConfiguration:Pass", null);
            string arrivalResource = _configuration.GetValue<string>("OpenSkyRestAPIConfiguration:ArrivalResource", "/flights/arrival");

            //Request params
            KeyValuePair<string, object> param1 = new KeyValuePair<string, object>("airport", icao);
            KeyValuePair<string, object> param2 = new KeyValuePair<string, object>("begin", begin);
            KeyValuePair<string, object> param3 = new KeyValuePair<string, object>("end", end);

            //Request
            RestClient client = RestClientFactory.CreateRestClient(host, userName, pass);
            RestRequest rq = RestClientFactory.CreateRestRequest(arrivalResource, Method.GET, DataFormat.Json, param1, param2, param3);
            IRestResponse rs = client.Get(rq);

            //Response
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
