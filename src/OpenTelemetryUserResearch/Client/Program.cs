using System;
using System.Diagnostics;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using OpenTelemetry;
using OpenTelemetry.Exporter;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;

namespace Client
{
    class MainClass
    {
        /// <summary>
        ///     Builds the application's file settings that will be used to set the application up.
        /// </summary>
        private static IConfigurationRoot BuildAppConfiguration()
        {
            return new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: false)
                .AddEnvironmentVariables()
                .Build();
        }

        private static readonly ActivitySource _activitySource = new("OpenTelemetry.Instrumentation.Http", "1.0.0.0");

        public static async Task Main()
        {
            var configuration = BuildAppConfiguration();
            var zipkinConfig = new ZipkinExporterOptions();
            configuration.GetSection("Zipkin").Bind(zipkinConfig);

            using var tracerProvider = Sdk.CreateTracerProviderBuilder()
                .AddHttpClientInstrumentation()
                .SetResourceBuilder(ResourceBuilder.CreateDefault().AddService(configuration["Zipkin:ServiceName"]))
                .AddZipkinExporter(config => config = zipkinConfig)
                .Build();

            var httpClient = new HttpClient();
            httpClient.BaseAddress = new Uri(configuration.GetConnectionString("Server"));

            using var parentActivity = _activitySource.StartActivity(
                "ClientParentActivity",
                ActivityKind.Client);

            for (var i = 0; i < 5; i++)
                await httpClient.GetAsync(configuration["HelloEndpoint"]);
        }
    }
}
