using System;
using ExampleWebApp;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Serilog;
using Serilog.Formatting.Json;
using Serilog.Sinks.RabbitMQ;
using Serilog.Sinks.RabbitMQ.Sinks.RabbitMQ;

namespace WebApp
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var config = new RabbitMQConfiguration
            {
                Hostname = "localhost",
                Username = "guest",
                Password = "guest",
                Exchange = "app-logging",
                ExchangeType = "direct",
                DeliveryMode = RabbitMQDeliveryMode.NonDurable,
                RouteKey = "Logs",
                Port = 5672
            };

            Log.Logger = new LoggerConfiguration()
                .WriteTo.RabbitMQ(config, new JsonFormatter())
                .Enrich.WithProperty("App Name", "WebApp")
                .MinimumLevel.Verbose()
                .CreateLogger();

            try
            {
                CreateWebHostBuilder(args).Build().Run();
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "Host terminated unexpectedly");
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>()
                .UseSerilog();
    }
}