using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DataLayer.JobScheduler.Jobs
{
    public interface IJobSendEmail
    {
        Task SendAsync(CancellationToken stoppingToken);
    }
}
