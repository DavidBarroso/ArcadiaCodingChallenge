//using Arcadia.ArcadiaBackend.Utils;
using Arcadia.ArcadiaBackend.Helpers;
using Arcadia.Model;
using Arcadia.RestClientUtils;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
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
        private IMemoryCache _cache;

        public AirportsController(ILogger<AirportsController> logger, IMemoryCache memoryCache)
        {
            _cache = memoryCache;
            _logger = logger;
        }

        [HttpGet]
        public IEnumerable<Airport> Get()
        {
            Airport[] airports = _cache.Get<Airport[]>(ArcadiaUtils.AIRPORT_CACHE_KEY);
            if(airports == null)
            {
                airports = ArcadiaUtils.GetWorldAirports();
                _cache.Set(ArcadiaUtils.AIRPORT_CACHE_KEY, airports);
            }
            return airports;
        }
    }
}
