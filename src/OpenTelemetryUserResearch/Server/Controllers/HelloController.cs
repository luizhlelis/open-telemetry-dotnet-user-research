using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Diagnostics;
using OpenTelemetry.Trace;
using System;

namespace OpenTelemetryUserResearch.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class HelloController : ControllerBase
    {
        private readonly ILogger<HelloController> _logger;
        private readonly TracerProvider _tracerProvider;
        private readonly ActivitySource _activitySource;

        public HelloController(
            ILogger<HelloController> logger,
            TracerProvider tracerProvider,
            ActivitySource activitySource)
        {
            _logger = logger;
            _tracerProvider = tracerProvider;
            _activitySource = activitySource;
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

                using (var childActivity = _activitySource.StartActivity(
                    "ChildActivity",
                    ActivityKind.Server,
                    currentActivity.Context))
                {
                    await Task.Delay(40);

                    try
                    {
                        throw new ArgumentNullException();
                    }
                    catch (Exception ex)
                    {
                        childActivity?.AddEvent(new ActivityEvent(ex.Data.GetType().ToString()));
                        childActivity?.SetTag("otel.status_code", OpenTelemetry.Trace.StatusCode.Error);
                        childActivity?.SetTag("otel.status_description", ex.Message);
                    }
                }
            }
            return Ok("Hello World");
        }
    }
}
