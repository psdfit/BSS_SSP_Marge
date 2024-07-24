
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace DataLayer.Models
{
    [Serializable]

    public class ClassModel : ModelBase
    {
        public ClassModel() : base() { }
        public ClassModel(bool InActive) : base(InActive) { }

        public int ClassID { get; set; }
        public string ClassCode { get; set; }
        public int ClassStatusID { get; set; }
        public string ClassStatusName { get; set; }
        public int TSPID { get; set; }
        public int SectorID { get; set; }
        public string SectorName { get; set; }
        public int TradeID { get; set; }
        public string TradeName { get; set; }
        public decimal Duration { get; set; }
        public int TraineesPerClass { get; set; }
        public int Batch { get; set; }
        public int GenderID { get; set; }
        public string GenderName { get; set; }
        public string TrainingAddressLocation { get; set; }
        public string AttendanceStandardPercentage { get; set; }
        public string Latitude { get; set; }
        public string Longitude { get; set; }
        public int DistrictID { get; set; }
        public string DistrictNameUrdu { get; set; }
        public string DistrictName { get; set; }
        public int TehsilID { get; set; }
        public int? PTypeID { get; set; }
        public string TehsilName { get; set; }
        public int ClusterID { get; set; }
        public string ClusterName { get; set; }
        public int MinHoursPerMonth { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string SourceOfCurriculum { get; set; }
        public string EntryQualification { get; set; }
        public int CertAuthID { get; set; }
        public string CertAuthName { get; set; }
        public string BusinessRuleType { get; set; }
        public int EmploymentCommitmentSelf { get; set; }
        public decimal TrainingCostPerTraineePerMonthExTax { get; set; }
        public decimal SalesTax { get; set; }
        public decimal TrainingCostPerTraineePerMonthInTax { get; set; }
        public decimal UniformBagCost { get; set; }
        public decimal TotalPerTraineeCostInTax { get; set; }
        public decimal TotalCostPerClassInTax { get; set; }
        public decimal PerTraineeTestCertCost { get; set; }
        public decimal TotalCostPerClass { get; set; }
        public int EmploymentCommitmentFormal { get; set; }
        public int OverallEmploymentCommitment { get; set; }
        public int Stipend { get; set; }
        public string StipendMode { get; set; }
        public int BoardingAllowancePerTrainee { get; set; }

       // [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public decimal BidPrice { get; set; }

        //[JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public decimal BMPrice { get; set; }
        public decimal OfferedPrice { get; set; }
        public decimal BidOfferPriceSavings { get; set; }
        public decimal BMOfferPriceSaving { get; set; }
        public decimal TotalTestingCertificationOfClass { get; set; }
        public decimal SalesTaxRate { get; set; }
        public int SchemeID { get; set; }
        public string SchemeName { get; set; }
        public string PTypeName { get; set; }
        public bool IsDual { get; set; }
        public string TSPCode { get; set; }
        public int OrganizationID { get; set; }
        public int? MinAge { get; set; }
        public int? MaxAge { get; set; }
        public int? KamID { get; set; }
        public bool? EmploymentSubmited { get; set; }
        public bool? RTP { get; set; }
        public bool? NTP { get; set; }
        public string UID { get; set; }
        public bool IsMigrated { get; set; }
        public int EnrolledTrainees { get; set; }
        public string InstructorName { get; set; }
        public string TSPName { get; set; }
        public string EntryQualificationName { get; set; }
        public string NTPStatus { get; set; }
        public string ShowInceptionToInternal { get; set; }
        public DateTime? TraineeRegistrationDueOn { get; set; }
        public DateTime? InceptionReportDueOn { get; set; }
        public List<InstructorModel> Instructors { get; set; }
        public int? MaleCountPerClass { get; set; }
        public int? FemaleCountPerClass { get; set; }
        public int? TransgenderCountPerClass { get; set; }
        public int? TradeDetailMapID { get; set; }
        public string ProvinceName { get; set; }
        public int ProvinceID { get; set; }
        public int RegistrationAuthorityID { get; set; }
        public int ProgramFocusID { get; set; }
        public string ProgramFocusName { get; set; }
        public string RegistrationAuthorityName { get; set; }

        /// Added by Rao Ali Haider on 22-July-2024 for VRN Payment
        public int balloonpayment { get; set; }

    }
}
