using DataLayer.JobScheduler.Scheduler;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataLayer.Models
{
    public class TARLiveDataModel
    {
        public TARLiveDataModel() : base() { }

        public string TraineeCode { get; set; }
        public int TraineeID { get; set; }
        public string TraineeName { get; set; }
        public string TraineeCNIC { get; set; }
        public string FatherName { get; set; }
        public string SchemeName { get; set; }
        public int ClassID { get; set; }
        public string ClassCode { get; set; }
        public DateTime? AttendanceDate { get; set; }
        public DateTime? CheckIn { get; set; }
        public DateTime? CheckOut { get; set; }
        public DateTime? ClassStartTime { get; set; }
        public DateTime? ClassEndTime { get; set; }
        public string ClassTotalHours { get; set; }
        public int EnrolledTrainees { get; set; }
        public string Shift { get; set; }
        public int TrainingDaysNo { get; set; }
        public string TrainingDays { get; set; }
        public string KAM { get; set; }
        public string TraineeStatusName { get; set; }
        public string ClassDistrictName { get; set; }
        public DateTime? ClassStartDate { get; set; }
        public DateTime? ClassEndDate { get; set; }
        public string ClassStatusName { get; set; }
        public string TSPName { get; set; }
        public string MPRTraineeStatus { get; set; }
        public DateTime? TPMVisitDateMarking1 { get; set; }
        public DateTime? TPMVisitDateMarking2 { get; set; }
    }
}
