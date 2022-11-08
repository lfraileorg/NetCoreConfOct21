using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Enrichers.Span;
using Serilog.Sinks.Grafana.Loki;

namespace WebClient
{
    public class Program
    {
        public static void Main(string[] args)
        {
            // move this to application configuration file!

            Serilog.Log.Logger = new LoggerConfiguration()
                .WriteTo.GrafanaLoki("http://localhost:3100", outputTemplate: @"[{Timestamp:o} {Level:u3}] {TraceId:l} {Message:lj}{NewLine}{Exception}")
                .WriteTo.Console(Serilog.Events.LogEventLevel.Information)
                 //.WriteTo.ApplicationInsights("APP INSIGHTS KEY (Only key)", TelemetryConverter.Traces, Serilog.Events.LogEventLevel.Warning)
                .Enrich.FromLogContext()
                .Enrich.WithSpan()
                .CreateLogger();

            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                })
                .ConfigureLogging(loggingBuilder =>
                {
                    loggingBuilder.ClearProviders(); // debug, console ---
                    loggingBuilder.AddSerilog();
                });
    }
}
