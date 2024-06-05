using System;
using System.Collections.Generic;
using System.Text;

namespace DataLayer.Models
{
    public class Get_TRN_Classes_Model
    {
        public int ClassID { get; set; }
        public int SchemeID { get; set; }
        public string ClassCode { get; set; }
        public int? CertAuthID { get; set; }
        public int? TradeID { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public int Duration { get; set; }
        public int TraineesPerClass { get; set; }
        public double? PerTraineeTestCertCost { get; set; }
        public int? TotalTrainee { get; set; }
        public int? TotalTraineeResult { get; set; }
        public int CNICVerified { get; set; }
        public int CNICVExcesses { get; set; }
        public int CNICUnVerified { get; set; }
        public int CNICUnVExcesses { get; set; }
        public int PassVerified { get; set; }
        public int PassUnverified { get; set; }
        public int FailedVerified { get; set; }
        public int FailedUnverified { get; set; }
        public int AbsentVerified { get; set; }
        public int AbsentUnverified { get; set; }
        
    }
}
