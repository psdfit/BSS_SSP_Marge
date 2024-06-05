using DataLayer.JobScheduler.Jobs;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using PSDF_BSS.Logging.Interface;
using System;
using System.Threading;
using System.Threading.Tasks;
using System.IO;
namespace PSDF_BSS.BackgroundServices
{
    public class BGSSendEmail : BackgroundService
    {
        private readonly IJobSendEmail jobSendEmail;
        private readonly IConfiguration configuration;
        private readonly INLogManager logManager;
        public BGSSendEmail(IJobSendEmail jobSendEmail, IConfiguration configuration, INLogManager logManager)
        {
            this.jobSendEmail = jobSendEmail;
            this.configuration = configuration;
            this.logManager = logManager;
        }
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {

            bool isDisabled = configuration.GetSection("AppSettings:Jobs:SendEmail:IsDisabled").Get<bool>();
            int interval = configuration.GetSection("AppSettings:Jobs:SendEmail:Interval").Get<int>();
            int initialDelay = 60 * 1000;// 1 minute
            DateTime? nextInvocation = null;

            if (isDisabled)
            {
                await Task.CompletedTask;
            }
            else
            {
                try
                {
                    //await Task.Delay(initialDelay);
                    while (!stoppingToken.IsCancellationRequested)
                    {
                        //await Task.Delay(TimeSpan.FromMinutes(5));
                        if (nextInvocation != null)
                        {
                            TimeSpan delay = nextInvocation.Value.Subtract(DateTime.Now);
                            if (delay.Ticks > 0)
                            {
                                await Task.Delay(delay);
                            }
                        }
                        await jobSendEmail.SendAsync(stoppingToken);
                        nextInvocation = DateTime.Now.AddMinutes(interval);
                        //logManager.LogInformation($"Info: JobSendEmail has been executed successfully.");
                    }
                }
                catch (Exception ex)
                {
                    string message = string.Format("Time: {0}", DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss tt"));
                    message += "-------------------------BGSSendEmail----------------------------------";
                    message += Environment.NewLine;
                    message += string.Format("Message: {0}", ex.Message);
                    string path = @"C:\temp\ErrorLog.txt";
                    using (StreamWriter writer = new StreamWriter(path, true))
                    {
                        writer.WriteLine(message);
                        writer.Close();
                    }
                    logManager.LogError($"Error: JobSendEmail error details : {ex.Message}", new System.Collections.Generic.Dictionary<string, object>());
                }
            }
        }
    }
}
