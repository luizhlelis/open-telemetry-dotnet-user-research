using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Diagnostics;
using OpenTelemetry.Trace;
namespace OpenTelemetryUserResearch.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class HelloController : ControllerBase
    {
        private readonly ILogger<HelloController> _logger;
        private readonly TracerProvider _tracerProvider;

        public HelloController(ILogger<HelloController> logger, TracerProvider tracerProvider)
        {
            _logger = logger;
            _tracerProvider = tracerProvider;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            await Task.Delay(30);
            using var currentActivity = Activity.Current;
            if (currentActivity != null)
            {
                currentActivity.DisplayName = "Server Hello";
                currentActivity.SetTag("foo", "bar");
                currentActivity.SetTag("http.route", "GET Hello");
            }
            return Ok("Hello World");
        }
    }
}
