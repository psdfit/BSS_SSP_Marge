using System;
using System.Collections.Generic;
using System.Text;

namespace DataLayer.Models
{
    public class RD_CompletedTraineeByClassModel
    {
        public string TraineeCode { get; set; }
        public string TraineeName { get; set; }
        public string TraineeCNIC { get; set; }
        public string FatherName { get; set; }
        public string ContactNumber { get; set; }
        public int? TraineeDistrictID { get; set; }
        public string DistrictName { get; set; }
        public string ResultStatusName  { get; set; }
        public string TraineeEmploymentStatus { get; set; }
        public int? ResultStatusID { get; set; }
        public int ClassID { get; set; }
        public int TraineeID { get; set; }
        public bool IsVerified { get; set; }
    }
}
