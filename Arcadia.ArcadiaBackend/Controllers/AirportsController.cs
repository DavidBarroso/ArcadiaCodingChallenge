//using Arcadia.ArcadiaBackend.Utils;
using Arcadia.ArcadiaBackend.Helpers;
using Arcadia.Model;
using Arcadia.RestClientUtils;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;

namespace Arcadia.ArcadiaBackend.Controllers
{
    /// <summary>
    /// AirportsController
    /// </summary>
    /// <seealso cref="Microsoft.AspNetCore.Mvc.ControllerBase" />
    [ApiController]
    [Route("[controller]")]
    public class AirportsController : ControllerBase
    {
        /// <summary>
        /// The configuration
        /// </summary>
        private readonly IConfiguration _configuration;

        /// <summary>
        /// The logger
        /// </summary>
        private readonly ILogger<AirportsController> _logger;

        /// <summary>
        /// The cache
        /// </summary>
        private readonly IMemoryCache _cache;

        /// <summary>
        /// Initializes a new instance of the <see cref="AirportsController"/> class.
        /// </summary>
        /// <param name="logger">The logger.</param>
        /// <param name="memoryCache">The memory cache.</param>
        /// <param name="configuration">The configuration.</param>
        public AirportsController(ILogger<AirportsController> logger, IMemoryCache memoryCache, IConfiguration configuration)
        {
            _cache = memoryCache;
            _logger = logger;
            _configuration = configuration;
        }

        /// <summary>
        /// Gets this instance.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IEnumerable<Airport> Get()
        {
            Airport[] airports = _cache.Get<Airport[]>(ArcadiaUtils.AIRPORT_CACHE_KEY);
            if(airports == null)
            {
                string filePath = _configuration.GetValue<string>("AirportsConfig:FilePath", "./Resources/airports.json");
                airports = ArcadiaUtils.GetWorldAirports(filePath);
                _cache.Set(ArcadiaUtils.AIRPORT_CACHE_KEY, airports);
            }
            return airports;
        }
    }
}
