using DataLayer.JobScheduler.Scheduler;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataLayer.Models
{
    public class TARLiveDataModel
    {
        public TARLiveDataModel() : base()
        {
        }


        public string TraineeCode { get; set; }
        public int TraineeID { get; set; }
        public string TraineeName { get; set; }
        public string TraineeCNIC { get; set; }
        public string FatherName { get; set; }
        public string SchemeName { get; set; }
        public string ClassCode { get; set; }
        public Boolean CheckIn { get; set; }
        public Boolean CheckOut { get; set; }
        public DateTime? TimeStamp { get; set; }


    }
}