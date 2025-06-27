using System;
using System.Collections.Generic;
using System.Text;

namespace DataLayer.Models
{
    public class RD_ClassForTSPModel
    {
        public int ClassID { get; set; }
        public string ClassCode { get; set; }
        public int? ClassStatusID { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public DateTime? EmploymentSubmitedDate { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string TSPName { get; set; }
        public string SchemeName { get; set; }
        public int? UserID { get; set; }
        public int TradeID { get; set; }
        public bool EmploymentSubmited { get; set; }
        public bool VerificationSubmited { get; set; }
        public bool CallCentreVerificationSubmitted { get; set; }
        public int EmploymentCommittedTrainees { get; set; }
        public int TraineesPerClass { get; set; }
        public int OverallEmploymentCommitment { get; set; }
        public int EmploymentReported { get; set; }
        public int VerifiedEmployment { get; set; }
        public int VerifiedToContractualCommitment { get; set; }
        public string SourceOfVerification { get; set; }
        public string ClassStatusName { get; set; }
        public int? TSPID { get; set; }
        public bool OJTSubmited { get; set; }
        public bool OJTVerificationSubmited { get; set; }
    }
    
    
    public class RD_TSPForEmploymentVerificationModel
    {
        public int TSPID { get; set; }
        public string TSPName { get; set; }
        public string TSPCode { get; set; }
      
    }

    public class RD_ClassForTSPModelExportExcelVerifedEmploymentReport
    {
        public int ClassID { get; set; }
        public string ClassCode { get; set; }
        public int? ClassStatusID { get; set; }
        public int CompletedTrainees { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string TSPName { get; set; }
        public string SchemeName { get; set; }
        public int? UserID { get; set; }
        public int TradeID { get; set; }
        public bool EmploymentSubmited { get; set; }
        public bool VerificationSubmited { get; set; }
        public int EmploymentCommittedTrainees { get; set; }
        public int TraineesPerClass { get; set; }
        public int OverallEmploymentCommitment { get; set; }
        public int EmploymentReported { get; set; }
        public int VerifiedEmployment { get; set; }
        public int VerifiedToContractualCommitment { get; set; }
        public string SourceOfVerification { get; set; }
        public string ClassStatusName { get; set; }
        public int? TSPID { get; set; }
        public string KAM { get; set; }

    }
}
