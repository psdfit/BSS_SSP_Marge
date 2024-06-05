using DataLayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DataLayer.Models
{
    public class ProfileVerificationModel
    {
        public List<PVMList> PVMList { get; set; }
    }
    public class PVMList
    {
        public string IsLock { get; set; }
        public string SchemeName { get; set; }
        public string TSPName { get; set; }
        public string ClassCode { get; set; }
        public string TradeName { get; set; }
        public string TrainingCentreAddress { get; set; }
        public string VisitDateTime { get; set; }
        public int TraineesPerClass { get; set; }
        public int TotalTraineesPresentPerClass { get; set; }
        public string Name { get; set; }
        public string FatherName { get; set; }
        public string Cnic { get; set; }
        public string Status { get; set; }
        public string Remarks { get; set; }
        public string TraineeCountRemarks { get; set; }
        public string AttendanceStatus { get; set; }
        public string IsPresent { get; set; }
        public string VerificationStatus { get; set; }
    }
}
