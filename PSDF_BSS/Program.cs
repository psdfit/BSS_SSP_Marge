using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using DataLayer.Classes;
using DataLayer.Interfaces;
using DataLayer.JobScheduler.Scheduler;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NLog.Web;

namespace PSDF_BSS
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
                    webBuilder
                    .ConfigureServices(serviceCollection =>
                    {
                        serviceCollection.AddSingleton<IScheduler>(serviceProvider => new Scheduler(serviceProvider.GetService<ISRVSendEmail>(), serviceProvider.GetService<ISRVTraineeProfile>(), serviceProvider.GetService<ISRVDistrict>(),
                            serviceProvider.GetService<ISRVOrgConfig>(), serviceProvider.GetService<ISRVClass>(), serviceProvider.GetService<ISRVTraineeStatus>())); ; ;
                    })
                    .UseStartup<Startup>();
                })
                .ConfigureLogging(logging =>
                {
                    //var config = new ConfigurationBuilder()
                    //                .SetBasePath(Directory.GetCurrentDirectory())
                    //                .AddJsonFile("appsetting.json", optional: true)
                    //                .AddCommandLine(args)
                    //                .Build();
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