using DataLayer.JobScheduler.Scheduler;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataLayer.Models
{
    public class TARCWLiveDataModel
    {
        public TARCWLiveDataModel() : base() { }

        public string Scheme { get; set; }
        public string TSP { get; set; }
        public string ClassCode { get; set; }
        public string District { get; set; }
        public DateTime ClassStartDate { get; set; }
        public DateTime ClassEndDate { get; set; }
        public DateTime AttendanceDate { get; set; }
        public int NumberOfTrainees { get; set; }
        public int TotalTraineesPerClass { get; set; }
        public int OnRollCompletedTraineesPresent { get; set; }
        public int OnRollCompletedTraineesAbsent { get; set; }
        public string DataSource { get; set; }
        public int TotalOnRollCompletedTrainees { get; set; }
        public float OnRollCompletedTraineesRatio { get; set; }
    }
}
