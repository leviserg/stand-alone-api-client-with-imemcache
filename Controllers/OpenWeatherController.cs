using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using stand_alone_api_client_with_imemcache.Services;
using System.Reflection.Metadata;

namespace stand_alone_api_client_with_imemcache.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OpenWeatherController : ControllerBase
    {

        private readonly CacheService _cacheService;
        private readonly IOpenWeatherService _openWeatherService;

        public OpenWeatherController(IOpenWeatherService openWeatherService, CacheService cacheService)
        {
            _openWeatherService = openWeatherService;
            _cacheService = cacheService;
        }

        [HttpGet]
        public async Task<ActionResult<dynamic>> GetWeather([FromQuery] string city)
        {
            string cacheKey = $"openweather-cache-{city}";

            var result = await _cacheService.GetOrAddAsync(
                key: cacheKey,
                dataSource: async () => await _openWeatherService.GetWeatherAsync(city),
                expirationTime: TimeSpan.FromMinutes(5)
            );

            return Ok(result);
        }


    }
}
