using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace ApiOperacional.Controllers
{
    [ApiController]
    [Route("[controller]"), Authorize]
    public class ProtectedWeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<ProtectedWeatherForecastController> _logger;

        public ProtectedWeatherForecastController(ILogger<ProtectedWeatherForecastController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public string Get()
        {
            return "Recurso protegido";
        }
    }


    [ApiController]
    [Route("[controller]")]
    public class PublicWeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<PublicWeatherForecastController> _logger;

        public PublicWeatherForecastController(ILogger<PublicWeatherForecastController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public string Get()
        {
            return "recurso publico";
        }
    }
}
