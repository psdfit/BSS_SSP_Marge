/* **** Aamer Rehman Malik *****/

using System;

namespace DataLayer.Models
{
    [Serializable]
    public class PRNModel
    {
        public int PRNID { get; set; }
        public int InvoiceNumber { get; set; }
        public int TradeID { get; set; }
        public int ClaimedTrainees { get; set; }
        public int EnrolledTrainees { get; set; }
        public int DropoutsVerified { get; set; }
        public int DropoutsUnverified { get; set; }
        public int CNICVerified { get; set; }
        public int CNICVExcesses { get; set; }
        public int CNICUnverified { get; set; }
        public int CNICUnVExcesses { get; set; }
        public int MaxAttendance { get; set; }
        public int DeductionMarginal { get; set; }
        public double Duration { get; set; }
        public int ContractualTrainees { get; set; }
        public int ExpelledVerified { get; set; }
        public int ExpelledUnverified { get; set; }
        public int PassVerified { get; set; }
        public int PassUnverified { get; set; }
        public int FailedVerified { get; set; }
        public int FailedUnverified { get; set; }
        public int AbsentVerified { get; set; }
        public int AbsentUnverified { get; set; }
        public int DropOut { get; set; }
        public int DeductionExtraRegisteredForExam { get; set; }
        public int DeductionFailedTrainees { get; set; }
        public int PenaltyImposedByME { get; set; }
        public int? DeductionUniformBagReceiving { get; set; }
        public int CompletedTrainees { get; set; }
        public int GraduatedCommitmentTrainees { get; set; }
        public int EmploymentReported { get; set; }
        public int VerifiedTrainees { get; set; }
        public int VerifiedToCompletedCommitment { get; set; }
        public int TraineesFoundInVISIT1 { get; set; }
        public int TraineesFoundInVISIT2 { get; set; }
        public int PRNMasterID { get; set; }
        public double DeductionSinIncepDropout { get; set; }
        public double PaymentWithheldPhysicalCount { get; set; }
        public double PaymentWithheldSinIncepUnVCNIC { get; set; }
        public double PenaltyTPMReports { get; set; }
        public double ReimbursementUnVTrainees { get; set; }
        public double ReimbursementAttandance { get; set; }
        public double PaymentToBeReleasedTrainees { get; set; }
        public double Payment100p { get; set; }
        public double Payment50p { get; set; }
        public string TradeName { get; set; }
        public string ClassCode { get; set; }
        public string ClassStatus { get; set; }
        public string NonFunctionalVisit1 { get; set; }
        public string NonFunctionalVisit2 { get; set; }
        public string NonFunctionalVisit3 { get; set; }
        public string EmploymentCommitmentPercentage { get; set; }
        public DateTime? NonFunctionalVisit1Date { get; set; }
        public DateTime? NonFunctionalVisit2Date { get; set; }
        public DateTime? NonFunctionalVisit3Date { get; set; }
        public DateTime ClassStartDate { get; set; }
        public DateTime ClassEndDate { get; set; }
        public bool? IsApproved { get; set; }
        public string ProcessKey { get; set; }
        public DateTime? Month { get; set; }
        public bool? IsRejected { get; set; }
        public int CurUserID { get; set; }
        public int ClassID { get; set; }
        public int? ExtraTraineeDeductCompletion { get; set; }
        public int? UnVDeductCompletion { get; set; }
        public int? DropOutDeductCompletion { get; set; }
        public int? AbsentDeductCompletion { get; set; }
        public int? DropoutPassFailAbsent { get; set; }
        public int? ExpelledPassFailAbsent { get; set; }
        public bool? InActive { get; set; }
        public bool? InCancel { get; set; }
        public string TSPName { get; set; }
        public string SchemeName { get; set; }
        public string PenaltyAndUniBagRecvInputRemarks { get; set; }
        public string CertAuthName { get; set; }
        public int? ExpelledRegularVerifiedForTheMonth { get; set; }

        public string StatusApproved { get; set; }
        public DateTime FinalApprovalDate { get; set; }
        public DateTime CreationDate { get; set; }
        public string FundingCategory { get; set; }
        public string kam { get; set; }
    }
}