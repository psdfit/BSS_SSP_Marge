
using System;
using System.Security.Cryptography.X509Certificates;

namespace DataLayer.Models
{
    [Serializable]

    public class MasterSheetModel : ModelBase
    {
        public MasterSheetModel() : base() { }
        public MasterSheetModel(bool InActive) : base(InActive) { }

        //public int ID	{ get; set; }
        public int DistrictID { get; set; }
        public string District { get; set; }
        public int SchemeID { get; set; }
        public string Scheme { get; set; }
        public string SchemeCode { get; set; }
        public string InstructorName { get; set; }
        public string InstructorCNIC { get; set; }
        public string FundingSourceName { get; set; }
        public int ClassID { get; set; }
        public string Class { get; set; }
        //public int ClassStatusID { get; set; }
        public string ClassStatusName { get; set; }
        public string Shift { get; set; }
        public string Section { get; set; }

        public decimal Duration { get; set; }
        public int TotalTrainingHours { get; set; }
        public int TraineesPerClass { get; set; }
        public int EmploymentCommitment { get; set; }
        public int TotalTraineeProfilesReceived { get; set; }
        public int EnrolledTrainees { get; set; }
        public int MinHoursPerMonth { get; set; }
        //public string InstructorName { get; set; }
        public string ClassID_U { get; set; }
        public string SchemeID_U { get; set; }
        public string TSPID_U { get; set; }
        public string SchemeType { get; set; }
        public string TraineeProfilesReceived { get; set; }

        public string TraineeProfileReceivedDate { get; set; }
        public string TradeGroup { get; set; }
        public string DateOfDeliveryToTPM { get; set; }
        public string InceptionReportDeliveredToTPM { get; set; }
        public string InceptionReportReceived { get; set; }
  
        public int CertAuthID { get; set; }
        public string Certification_Authority { get; set; }
        public int TSPID { get; set; }
        public string TSP { get; set; }
        public string TSPCode { get; set; }
        public int TradeID { get; set; }
        public string Trade { get; set; }
        public string TradeCode { get; set; }
        public string Batch { get; set; }
        public string TrainingAddressLocation { get; set; }
        public int TehsilID { get; set; }
        public string Tehsil { get; set; }
        //public string DeliveringTrainer	{ get; set; }
        //public string CertifyAuthority	{ get; set; }
        //public int ContractualTrainees	{ get; set; }
        public int GenderID { get; set; }
        public string Gender { get; set; }
        //public int TrainingDuration	{ get; set; }
        //public int TotalTrainingHours	{ get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public DateTime? ClassStartTime { get; set; }
        public DateTime? ClassEndTime { get; set; }
        public DateTime? InceptionReportDueOn { get; set; }
        public DateTime? CompletionReportDue { get; set; }
        public DateTime? StudentProfileOverDueOn { get; set; }
        //public int TotalTraineeProfilesReceived { get; set; }
        public string RTP { get; set; }
        public string Remarks { get; set; }
        public string WhoIsDeliveringTraining { get; set; }
        //public bool CompletionReportStatus	{ get; set; }
        //public string Remarks	{ get; set; }
        //public int SchemeType	{ get; set; }
        //public int ContractualClassHours { get; set; }
        public string EmploymentInvoiceStatus { get; set; }
        public int SectorID { get; set; }
        public string Sector { get; set; }
        public int OverallEmploymentCommitment { get; set; }
        public string MinimumEducation { get; set; }
        public string CompletionReportStatus { get; set; }
        public int OID { get; set; }
        public string Organization { get; set; }
        public int ClusterID { get; set; }
        public string Cluster { get; set; }
        public int? KamID { get; set; }
        public int? UserID { get; set; }
        public int UserLevel { get; set; }
        public int Role { get; set; }
        public string UserName { get; set; }
        public int? ProvinceID { get; set; }
        public string ProvinceName { get; set; }

        public string PaymentSchedule { get; set; }
        public string SourceOfCurriculum { get; set; }
        public string PTypeName { get; set; }
        public string IsDVV { get; set; }
        public string DayNames { get; set; } 
        public string ProgramFocusName { get; set; }  
        public string FundingCategoryName { get; set; }
        public string TSPNTN { get; set; }
        public string TotalClassDays { get; set; }
        public string SAPID { get; set; }
        public string RegistrationAuthorityName { get; set; }
        public string FormalCommitment { get; set; }
        public string SelfCommitment { get; set; }
        public string TraineeVerified { get; set; }
        public string ResultStatus { get; set; }
        public string EmploymentReported { get; set; }
        public string EmploymentVerified { get; set; }
        public string PendingPRNsRegular { get; set; }
        public string PendingPRNsCompleted { get; set; }
        public string PendingPRNsEmployment { get; set; }

        //public string TrainerName	{ get; set; }
        //public string TrainerCNIC	{ get; set; }

    }}
