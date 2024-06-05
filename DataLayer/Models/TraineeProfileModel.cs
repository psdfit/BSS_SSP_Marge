/* **** Aamer Rehman Malik *****/

using System;

namespace DataLayer.Models
{
    [Serializable]
    public class TraineeProfileModel : ModelBase
    {
        public TraineeProfileModel() : base()
        {
        }

        public TraineeProfileModel(bool InActive) : base(InActive)
        {
        }
        public int TraineeID { get; set; }
        public int TemporaryDistrict { get; set; }
        public int TemporaryTehsil { get; set; }
        public string TraineeCode { get; set; }
        public string TraineeName { get; set; }
        public string TraineeCNIC { get; set; }
        public string FatherName { get; set; }
        public int GenderID { get; set; }
        public string GenderName { get; set; }
        public int TradeID { get; set; }
        public string TradeName { get; set; }
        public int? SectionID { get; set; }
        public string SectionName { get; set; }
        public string SchemeCode { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public DateTime? CNICIssueDate { get; set; }
        public bool? IsDual { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public int SchemeID { get; set; }
        public string SchemeName { get; set; }
        public int TSPID { get; set; }
        public string TSPName { get; set; }
        public int ClassID { get; set; }
        public string ClassCode { get; set; }
        public string TemporaryResidence { get; set; }
        public string PermanentResidence { get; set; }
        public string PermanentDistrictName { get; set; }
        public string PermanentTehsilName { get; set; }
        public int? EducationID { get; set; }
        public string ContactNumber1 { get; set; }
        public string ContactNumber2 { get; set; }
        public bool? CNICVerified { get; set; }
        public string TraineeImg { get; set; }
        public string CNICImg { get; set; }
        public string TraineeDoc { get; set; }
        public string TraineeEmail { get; set; }
        public int TraineeAge { get; set; }
        public int? ReligionID { get; set; }
        public bool? VoucherHolder { get; set; }
        public string VoucherNumber { get; set; }
        public string VoucherOrganization { get; set; }
        public string WheredidYouHearAboutUs { get; set; }
        public int? TraineeIndividualIncomeID { get; set; }
        public int? HouseHoldIncomeID { get; set; }
        public int? EmploymentStatusBeforeTrainingID { get; set; }
        public bool? Undertaking { get; set; }
        public string GuardianNextToKinName { get; set; }
        public string GuardianNextToKinContactNo { get; set; }
        public string TraineeHouseNumber { get; set; }
        public string TraineeStreetMohalla { get; set; }
        public string TraineeMauzaTown { get; set; }
        public int? TraineeDistrictID { get; set; }
        public int? TraineeTehsilID { get; set; }
        public bool? AgeVerified { get; set; }
        public bool? DistrictVerified { get; set; }
        public bool? TraineeVerified { get; set; }
        public string VerificationStatus { get; set; }    // To Show Verification Status on Change Request Module
        public string NextToKinName { get; set; }
        public string NextToKinContactNo { get; set; }
        public bool? IsManual { get; set; }
        public bool? IsExtra { get; set; }
        public bool? IsSubmitted { get; set; }
        public string CNICImgNADRA { get; set; }
        public string CNICUnVerifiedReason { get; set; }
        public string AgeUnVerifiedReason { get; set; }
        public string ResidenceUnVerifiedReason { get; set; }
        public DateTime? CNICVerificationDate { get; set; }
        public int ResultStatusID { get; set; }
        public string UID { get; set; }
        public string EnrollmentEndDate { get; set; }
        public bool? IsMigrated { get; set; }
        public DateTime? TraineeStatusChangeDate { get; set; }
        public int? TraineeStatusTypeID { get; set; }
        public string StatusName { get; set; }
        public string TraineeStatusChangeReason { get; set; }
        public DateTime? ResultStatusChangeDate { get; set; }
        public string ResultStatusChangeReason { get; set; }
        public string TraineeRollNumber { get; set; }
        public string ResultDocument { get; set; }
        public int? TraineeDisabilityID { get; set; }
        public int? ReferralSourceID { get; set; }
        public string DistrictName { get; set; }
        public string DistrictNameUrdu { get; set; }
        public int ProvinceID { get; set; }
        public string ProvinceName { get; set; }
        public string ProvinceNameUrdu { get; set; }
        public string TehsilName { get; set; }
        public string TrainingAddressLocation { get; set; }
        public int ClassBatch { get; set; }
        public int TraineeCrID { get; set; }
        public Boolean TraineeCrIsApproved { get; set; }
        public Boolean TraineeCrIsRejected { get; set; }
        public string TraineeDocURL { get; set; }
        public string ReligionName { get; set; }
        public string Disability { get; set; }
        public string TraineeIntrestStatus { get; set; }
    }}