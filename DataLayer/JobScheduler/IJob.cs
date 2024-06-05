using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Scheduler
{
    public interface IJob
    {
        void Run();
        Task JobTask();
    }
}
