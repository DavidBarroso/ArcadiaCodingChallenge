using Arcadia.ArcadiaFrontend.Models;
using Arcadia.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Mvc.Rendering;
using RestSharp;

namespace Arcadia.ArcadiaFrontend.Controllers
{
    public class HomeController : BaseClientController
    {
        private readonly ILogger<HomeController> _logger;
        private List<Airport> _airports;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;

            JsonSerializerOptions options = new JsonSerializerOptions() { PropertyNameCaseInsensitive = true };
            _airports = JsonSerializer.Deserialize<Airport[]>(System.IO.File.ReadAllText("./Resources/airportsSP_DE.json"), options).ToList();
        }

        public IActionResult Index(IndexViewModel model)
        {
            try
            {
                if (model == null)
                    model = new IndexViewModel();
                if (model.Airports == null)
                    model.Airports = _airports;
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
                model.Airports = _airports;

            string host = "http://arcadia.arcadiabackend";
            RestClient client = new RestClient(host);
            RestRequest rq = new RestRequest("arrivals", Method.GET, DataFormat.Json);
            IRestResponse rs = client.Get(rq);

            return PartialView("~/Views/Controls/ArrivalsFiltered.cshtml");

            //return PartialView("~/Views/Controls/ArrivalsFilter.cshtml", indexModel);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
