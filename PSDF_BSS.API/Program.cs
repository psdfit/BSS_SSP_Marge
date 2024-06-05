using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NLog.Web;

namespace PSDF_BSS.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                })
            .ConfigureLogging(logging =>
            {
                var config = new ConfigurationBuilder()
                            .AddJsonFile("appsettings.json", optional: false)
                            .Build();
                logging.ClearProviders();
                NLog.Extensions.Logging.ConfigSettingLayoutRenderer.DefaultConfiguration = config;
                string level = config.GetSection("Logging:LogLevel:Default").Value;
                switch (level)
                {
                    case "Error":
                        logging.SetMinimumLevel(LogLevel.Error);
                        break;
                    case "Information":
                        logging.SetMinimumLevel(LogLevel.Information);
                        break;
                    case "Trace":
                        logging.SetMinimumLevel(LogLevel.Trace);
                        break;
                    case "Debug":
                        logging.SetMinimumLevel(LogLevel.Debug);
                        break;
                    case "Warning":
                        logging.SetMinimumLevel(LogLevel.Warning);
                        break;
                    default:
                        logging.SetMinimumLevel(LogLevel.Trace);
                        break;
                }
            }).UseNLog();
    }
}
