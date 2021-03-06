using Arcadia.ArcadiaFrontend.Extensions;
using Arcadia.ArcadiaFrontend.Models;
using Arcadia.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using RestSharp;
using Arcadia.RestClientUtils;
using Microsoft.Extensions.Configuration;

namespace Arcadia.ArcadiaFrontend.Controllers
{
    /// <summary>
    /// HomeController
    /// </summary>
    /// <seealso cref="Arcadia.ArcadiaFrontend.Controllers.BaseClientController" />
    public class HomeController : BaseClientController
    {
        private readonly IConfiguration _configuration;

        /// <summary>
        /// The logger
        /// </summary>
        private readonly ILogger<HomeController> _logger;

        /// <summary>
        /// The session key airports
        /// </summary>
        private const string SESSION_KEY_AIRPORTS = "AIRPORTS";

        /// <summary>
        /// The session key arrivals
        /// </summary>
        private const string SESSION_KEY_ARRIVALS = "ARRIVALS";

        /// <summary>
        /// Initializes a new instance of the <see cref="HomeController"/> class.
        /// </summary>
        /// <param name="logger">The logger.</param>
        /// <param name="configuration">The configuration.</param>
        public HomeController(ILogger<HomeController> logger, IConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;
        }

        /// <summary>
        /// Indexes the specified model.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        public IActionResult Index(IndexViewModel model)
        {
            try
            {
                if (model == null)
                    model = new IndexViewModel();
                if (string.IsNullOrWhiteSpace(model.Countries))
                    model.Countries = _configuration.GetValue<string>("ArcadiaBackend:AirportInitCountries", "Spain, Germany");
                model.Begin = DateTime.Now;
                model.End = DateTime.Now;
                model.WorldAirports = GetAirports();
                model.Airports = GetFilteredAirports(model.WorldAirports, model.Countries.Split(','));


                model.Arrivals = new List<Arrivals>();

                return View(model);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in HomeController - Index");
                return Error();
            }
        }

        /// <summary>
        /// Privacies this instance.
        /// </summary>
        /// <returns></returns>
        public IActionResult Privacy()
        {
            return View();
        }

        /// <summary>
        /// Gets the filtered arrivals.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        public List<Arrivals> GetFilteredArrivals(IndexViewModel model)
        {
            try
            {
                if (model == null)
                    model = new IndexViewModel();
                if (string.IsNullOrWhiteSpace(model.Countries))
                    model.Countries = _configuration.GetValue<string>("ArcadiaBackend:AirportInitCountries", "Spain, Germany");
                model.WorldAirports = GetAirports();
                model.Airports = GetFilteredAirports(model.WorldAirports, model.Countries.Split(','));
                List<Arrivals> arrivals = GetArrivals(model.SelectedAirport, model.Begin, model.End);
                model.Arrivals = arrivals;

                return arrivals;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in HomeController - GetFilteredArrivals");
                return null;
            }
        }

        /// <summary>
        /// Errors this instance.
        /// </summary>
        /// <returns></returns>
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        /// <summary>
        /// Gets the airports.
        /// </summary>
        /// <returns></returns>
        private List<Airport> GetAirports()
        {
            List<Airport> airportsInCache = HttpContext.Session.GetFromSession<List<Airport>>(SESSION_KEY_AIRPORTS);
            if (airportsInCache == null)
            {
                string backedHost = _configuration.GetValue<string>("ArcadiaBackend:Host", "http://arcadia.arcadiabackend");
                string airportResource = _configuration.GetValue<string>("ArcadiaBackend:AirportResource", "airports");

                RestClient client = RestClientFactory.CreateRestClient(backedHost);
                RestRequest rq = RestClientFactory.CreateRestRequest(airportResource, Method.GET, DataFormat.Json);
                IRestResponse rs = client.Get(rq);
                Airport[] airports = null;
                if (rs.StatusCode == System.Net.HttpStatusCode.OK && rs.ContentType.ToLower().Contains("json"))
                    airports = RestClientFactory.GetData<Airport[]>(rs.Content);
                if (airports != null)
                {
                    airportsInCache = airports.ToList();
                    HttpContext.Session.SetInSession(SESSION_KEY_AIRPORTS, airportsInCache);
                }
                else
                {
                    airportsInCache = new List<Airport>();
                }
            }
            return airportsInCache;
        }

        /// <summary>
        /// Gets the filtered airports.
        /// </summary>
        /// <param name="airports">The airports.</param>
        /// <param name="countries">The countries.</param>
        /// <returns></returns>
        private List<Airport> GetFilteredAirports(List<Airport> airports, params string[] countries)
        {
            if (countries == null || !countries.Any())
                return new List<Airport>();

            return airports.Where(x =>
            {
                return !string.IsNullOrWhiteSpace(x.Name) && countries.ToList().Exists(y => string.Equals(x.Country.Trim(), y.Trim(), StringComparison.InvariantCultureIgnoreCase));

            }).OrderBy(x => x.Country).ThenBy(x => x.Name).ToList();
        }

        /// <summary>
        /// Gets the arrivals.
        /// </summary>
        /// <param name="icao">The icao.</param>
        /// <param name="begin">The begin.</param>
        /// <param name="end">The end.</param>
        /// <returns></returns>
        private List<Arrivals> GetArrivals(string icao, DateTime begin, DateTime end)
        {
            
            int beginUnixFormat = RestClientFactory.GetDateTimeAsUnixFormat(begin);
            int endUnixFormat = RestClientFactory.GetDateTimeAsUnixFormat(end);
            ArrivalCollection arrivalsInCache = HttpContext.Session.GetFromSession<ArrivalCollection>(SESSION_KEY_ARRIVALS);

            bool sameArrivalsCollection = arrivalsInCache == null ? false : string.Equals(icao, arrivalsInCache.ICAO) &&
                                                                            string.Equals(beginUnixFormat, arrivalsInCache.Begin) &&
                                                                            string.Equals(endUnixFormat, arrivalsInCache.End);
            if (!sameArrivalsCollection)
            {
                string backedHost = _configuration.GetValue<string>("ArcadiaBackend:Host", "http://arcadia.arcadiabackend");
                string arrivalResource = _configuration.GetValue<string>("ArcadiaBackend:ArrivalResource", "arrivals");

                RestClient client = RestClientFactory.CreateRestClient(backedHost);

                KeyValuePair<string, object> icaoParam = new KeyValuePair<string, object>("icao", icao);
                KeyValuePair<string, object> beginParam = new KeyValuePair<string, object>("begin", beginUnixFormat);
                KeyValuePair<string, object> endParam = new KeyValuePair<string, object>("end", endUnixFormat);

                RestRequest rq = RestClientFactory.CreateRestRequest(arrivalResource, Method.GET, DataFormat.Json, icaoParam, beginParam, endParam);
                IRestResponse rs = client.Get(rq);

                List<Arrivals> arrivals = null;
                if (rs.StatusCode == System.Net.HttpStatusCode.OK && rs.ContentType.ToLower().Contains("json"))
                    arrivals = RestClientFactory.GetData<List<Arrivals>>(rs.Content);
                if (arrivals != null)
                {
                    arrivalsInCache = new ArrivalCollection(icao, beginUnixFormat, endUnixFormat, arrivals.ToList());
                    HttpContext.Session.SetInSession(SESSION_KEY_ARRIVALS, arrivalsInCache);
                }
                else
                {
                    arrivalsInCache = new ArrivalCollection(icao, beginUnixFormat, endUnixFormat, new List<Arrivals>()); ;
                }
            }

            return arrivalsInCache.ToList();
        }
    }
}
