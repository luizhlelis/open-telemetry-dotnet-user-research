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

        public HelloController(ILogger<HelloController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            await Task.Delay(30);
            using var currentActivity = Activity.Current;
            using TelemetrySpan telemetrySpan = new TelemetrySpan(currentActivity);
            if (currentActivity != null){
                currentActivity.AddTag("http.route", "GET Hello");
                telemetrySpan.UpdateName("Hello");
            }
            return Ok("Hello World");
        }
    }
}
