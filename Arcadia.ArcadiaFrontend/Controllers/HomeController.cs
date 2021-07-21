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

namespace Arcadia.ArcadiaFrontend.Controllers
{
    public class HomeController : BaseClientController
    {
        private readonly ILogger<HomeController> _logger;

        private const string HOST = "http://arcadia.arcadiabackend";
        private const string ARRIVALS_RESOURCE = "arrivals";
        private const string AIRPORTS_RESOURCE = "airports";

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index(IndexViewModel model)
        {
            try
            {
                if (model == null)
                    model = new IndexViewModel();
                if (model.Airports == null)
                    model.Airports = GetAirports();
                return View(model);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in HomeController - Index");
                return Error();
            }
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult FilterArrivals(IndexViewModel model)
        {
            if (model == null)
                model = new IndexViewModel();
            if (model.Airports == null)
                model.Airports = GetAirports();

            RestClient client = RestClientFactory.CreateRestClient(HOST);

            KeyValuePair<string, object> icao = new KeyValuePair<string, object>("icao", model.SelectedAirport);
            KeyValuePair<string, object> begin = new KeyValuePair<string, object>("begin", RestClientFactory.GetDateTimeAsUnixFormat(model.Begin));
            KeyValuePair<string, object> end = new KeyValuePair<string, object>("end", RestClientFactory.GetDateTimeAsUnixFormat(model.End));


            RestRequest rq = RestClientFactory.CreateRestRequest(ARRIVALS_RESOURCE, Method.GET, DataFormat.Json, icao, begin, end);
            IRestResponse rs = client.Get(rq);

            return PartialView("~/Views/Controls/ArrivalsFiltered.cshtml");

            //return PartialView("~/Views/Controls/ArrivalsFilter.cshtml", indexModel);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        private List<Airport> GetAirports()
        {
            RestClient client = RestClientFactory.CreateRestClient(HOST);
            RestRequest rq = RestClientFactory.CreateRestRequest(AIRPORTS_RESOURCE, Method.GET, DataFormat.Json);
            IRestResponse rs = client.Get(rq);
            if (rs.StatusCode == System.Net.HttpStatusCode.OK && rs.ContentType.ToLower().Contains("json"))
            {
                Airport[] airports = RestClientFactory.GetData<Airport[]>(rs.Content);
                if (airports == null)
                    return null;

                return airports.ToList();
            }
            return null;
        }
    }
}
