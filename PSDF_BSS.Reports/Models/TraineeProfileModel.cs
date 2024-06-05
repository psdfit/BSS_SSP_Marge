
using System;

namespace DataLayer.Models
{
    [Serializable]

    public class TraineeProfileModel : ModelBase
    {
        public TraineeProfileModel() : base() { }
        public TraineeProfileModel(bool InActive) : base(InActive) { }
        public int TraineeID { get; set; }
        public string TraineeCode { get; set; }
        public string TraineeName { get; set; }
        public string TraineeCNIC { get; set; }
        public string FatherName { get; set; }
        public int GenderID { get; set; }
        public string GenderName { get; set; }
        public int TradeID { get; set; }
        public string TradeName { get; set; }
        public int SectionID { get; set; }
        public string SectionName { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public DateTime? CNICIssueDate { get; set; }
        public bool IsDual { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public int SchemeID { get; set; }
        public string SchemeName { get; set; }
        public int TspID { get; set; }
        public string TSPName { get; set; }
        public int ClassID { get; set; }
        public string ClassCode { get; set; }
        public int EducationID { get; set; }
        public string ContactNumber1 { get; set; }
        public bool CNICVerified { get; set; }
        public string TraineeImg { get; set; }
        public string CNICImg { get; set; }
        public int TraineeAge { get; set; }
        public int ReligionID { get; set; }
        public bool VoucherHolder { get; set; }
        public string VoucherNumber { get; set; }
        public string VoucherOrganization { get; set; }
        public string WheredidYouHearAboutUs { get; set; }
        public int? TraineeIndividualIncomeID { get; set; }
        public int HouseHoldIncomeID { get; set; }
        public int EmploymentStatusBeforeTrainingID { get; set; }
        public bool Undertaking { get; set; }
        public string GuardianNextToKinName { get; set; }
        public string GuardianNextToKinContactNo { get; set; }
        public string TraineeHouseNumber { get; set; }
        public string TraineeStreetMohalla { get; set; }
        public string TraineeMauzaTown { get; set; }
        public int TraineeDistrictID { get; set; }
        public int TraineeTehsilID { get; set; }
        public bool AgeVerified { get; set; }
        public bool DistrictVerified { get; set; }
        public bool TraineeVerified { get; set; }
        public bool IsManual { get; set; }
        public bool IsExtra { get; set; }
        public bool Stipend { get; set; }
        public string ResultStatus { get; set; }
        public bool IsSubmitted { get; set; }

        public string TSP_CPName { get; set; }
        public string TraineeMaxEducation { get; set; }
        public string SchemeCode { get; set; }
        public string ReligionName { get; set; }
        public string ClassCenterLocation { get; set; }
        public string DistrictName { get; set; }
        public string DistrictNameUrdu { get; set; }
        public string TehsilName { get; set; }
        public string EmploymentStatusBeforeTraining { get; set; }
        public string TraineeCurrentStatus { get; set; }
        public string CertAuthName { get; set; }

        public string CNICImgNADRA { get; set; }
        public string CNICUnVerifiedReason { get; set; }
        public string AgeUnVerifiedReason { get; set; }
        public string ResidenceUnVerifiedReason { get; set; }
        public DateTime? CNICVerificationDate { get; set; }
        public int ResultStatusID { get; set; }
        public string UID { get; set; }
        public bool IsMigrated { get; set; }
        //public DateTime? TraineeStatusChangeDate { get; set; }
        //public int TraineeStatusTypeID { get; set; }
        // public string TraineeStatusChangeReason { get; set; }
        public DateTime? ResultStatusChangeDate { get; set; }
        public string ResultStatusChangeReason { get; set; }
        public string TraineeRollNumber { get; set; }
        public string Disability { get; internal set; }
        public string TraineeIndividualIncomeRange { get; internal set; }
        public string HouseHoldIncomeRange { get; internal set; }
        public string PermanentDistrictName { get; set; }
        public string PermanentTehsilName { get; set; }
        public string PermanentResidence { get; set; }
        public string TemporaryDistrictName { get; set; }
        public string TemporaryTehsilName { get; set; }
        public string TemporaryResidence { get; set; }
    }}
