﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RestAPILinux.Controllers
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
        public IEnumerable<ArrivalsByAirport> Get()
        {
            var random = new Random();
            return Enumerable.Range(1, 5).Select(index => new ArrivalsByAirport
            {
                icao24 = Guid.NewGuid().ToString(),
                firstSeen = random.Next(),
                estDepartureAirport = Guid.NewGuid().ToString(),
                lastSeen = random.Next(),
                estArrivalAirport = Guid.NewGuid().ToString(),
                callsign = Guid.NewGuid().ToString(),
                estDepartureAirportHorizDistance = random.Next(),
                estDepartureAirportVertDistance = random.Next(),
                estArrivalAirportHorizDistance = random.Next(),
                estArrivalAirportVertDistance = random.Next(),
                departureAirportCandidatesCount = random.Next(),
                arrivalAirportCandidatesCount = random.Next()
            })
            .ToArray();
        }
    }
}
