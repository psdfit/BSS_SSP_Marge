using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Drawing;
using System.Data;
using DataLayer.Services;
using DataLayer.Models;
using System.Net.Mail;
using System.Threading;
using System.Drawing.Imaging;
using DataLayer.Interfaces;
using DataLayer.Scheduler;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using DataLayer.Scheduler.Jobs;

namespace DataLayer.JobScheduler.Scheduler
{
    public class Scheduler : IScheduler
    {

        private readonly ISRVTraineeProfile _srvTraineeProfile;
        private readonly ISRVDistrict _srvDistrict;
        private readonly ISRVOrgConfig _srvorgconfig;
        private readonly ISRVClass _srvclass;
        private readonly ISRVTraineeStatus _srvTraineeStatus;
        private readonly ISRVSendEmail srvSendEmail;
        private IJob job;
        public Scheduler(ISRVSendEmail srvSendEmail,ISRVTraineeProfile srvTraineeProfile, ISRVDistrict srvDistrict, ISRVOrgConfig srvorgconfig,
            ISRVClass srvclass, ISRVTraineeStatus srvTraineeStatus)
        {
            _srvTraineeProfile = srvTraineeProfile;
            _srvDistrict = srvDistrict;
            _srvorgconfig = srvorgconfig;
            _srvclass = srvclass;
            _srvTraineeStatus= srvTraineeStatus;
            this.srvSendEmail = srvSendEmail;
        }


        public void Start()
        {
            //RUN ALL JOBS ON THERE
            new NADRA_Bulk_Uploader(this.srvSendEmail,_srvTraineeProfile, _srvDistrict, _srvorgconfig,_srvclass,_srvTraineeStatus).Run();
        }

    }

}
