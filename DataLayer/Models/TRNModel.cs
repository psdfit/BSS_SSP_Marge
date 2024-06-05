using System;
using System.Collections.Generic;
using System.Text;

namespace DataLayer.Models
{
    public class TRNModel
    {
        public int TRNID { get; set; }
        public int? ClassID { get; set; }
        public int? CertAuthID { get; set; }
        public string SchemeCode { get; set; }
        public string SchemeName { get; set; }
        public string ClassCode { get; set; }
        public string InvoiceNo { get; set; }
        public int? TradeID { get; set; }
        public string ClassStatus { get; set; }
        public DateTime? ClassStartDate { get; set; }
        public DateTime? ClassEndDate { get; set; }
        public int? Duration { get; set; }
        public int? ContractualTrainees { get; set; }
        public int? ClaimedTrainees { get; set; }
        public int? EnrolledTrainees { get; set; }
        public int? DropOut { get; set; }
        public int? DropoutsVerified { get; set; }
        public int? DropoutsUnverified { get; set; }
        public int? CNICVerified { get; set; }
        public int? CNICVExcesses { get; set; }
        public int? CNICUnverified { get; set; }
        public int? CNICUnVExcesses { get; set; }
        public int? ExpelledVerified { get; set; }
        public int? ExpelledUnverified { get; set; }
        public int? PassVerified { get; set; }
        public int? PassUnverified { get; set; }
        public int? FailedVerified { get; set; }
        public int? FailedUnverified { get; set; }
        public int? AbsentVerified { get; set; }
        public int? AbsentUnverified { get; set; }
        public int? DeductionExtraRegisteredForExam { get; set; }
        public int? CompletedTrainees { get; set; }
        public int? VerifiedTrainees { get; set; }
        public int? VerifiedToCompletedCommitment { get; set; }
        public bool? IsApproved { get; set; }
        public string ProcessKey { get; set; }
        public bool? IsRejected { get; set; }
        public int? ModifiedUserID { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public int? TRNMasterID { get; set; }
        public DateTime? CreatedDate { get; set; }
        public int? PaymentToBeReleased { get; set; }
        public int? TraineesPerClass { get; set; }
        public int? TraineesEnrolled { get; set; }
        public int? TotalClaimedTrainees { get; set; }
        public int? TraineesRegisteredForExam { get; set; }
        public string TradeName { get; set; }
    }
}
