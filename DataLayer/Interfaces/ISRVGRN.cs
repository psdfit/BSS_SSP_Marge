using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Security.Cryptography;
using System.Text;
using DataLayer.JobScheduler.Scheduler;
using DataLayer.Models;
using Microsoft.Data.SqlClient;

namespace DataLayer.Interfaces
{
    public interface ISRVGRN
    {
        DataTable FetchClassesForGRNCompletion(GuruClassModel model);
    }
}
