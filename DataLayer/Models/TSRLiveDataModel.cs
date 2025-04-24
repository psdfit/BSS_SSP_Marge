using System;
using System.Collections.Generic;
using System.Text;

namespace DataLayer.Models
{
    public class TSRLiveDataModel
    {
        public TSRLiveDataModel() : base()
        {
        }

        public int TraineeID { get; set; }
        public string TraineeCode { get; set; }
        public string TraineeName { get; set; }
        public string TraineeCNIC { get; set; }
        public string FatherName { get; set; }
        public int GenderID { get; set; }
        public string GenderName { get; set; }
        public int Batch { get; set; }
        public int TradeID { get; set; }
        public string TradeName { get; set; }
        public int SectionID { get; set; }
        public string SectionName { get; set; }

        //public DateTime? Shift { get; set; }
        public DateTime? DateOfBirth { get; set; }

        public DateTime? CNICIssueDate { get; set; }
        public bool IsDual { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public int SchemeID { get; set; }
        public string SchemeName { get; set; }
        public int TSPID { get; set; }
        public string TSPName { get; set; }
        public string SectorName { get; set; }
        public int ClassID { get; set; }
        public string ClassCode { get; set; }
        public string TemporaryResidence { get; set; }
        public string PermanentResidence { get; set; }
        public int EducationID { get; set; }
        public string ContactNumber1 { get; set; }
        public string ContactNumber2 { get; set; }
        public bool CNICVerified { get; set; }
        public string TraineeImg { get; set; }
        public string CNICImg { get; set; }
        public int TraineeAge { get; set; }
        public DateTime? ClassStartDate { get; set; }
        public DateTime? ClassEndDate { get; set; }
        public string ClassStatusName { get; set; }
        public int ClassStatusID { get; set; }
        public string CertAuthName { get; set; }
        public string ClusterName { get; set; }
        public string KAM { get; set; }
        public string TraineeEmploymentStatus { get; set; }
        public string TraineeEmploymentVerificationStatus { get; set; }

        //public int ReligionID { get; set; }
        public bool VoucherHolder { get; set; }

        public string VoucherNumber { get; set; }
        public string VoucherOrganization { get; set; }
        public string WheredidYouHearAboutUs { get; set; }
        public int TraineeIndividualIncomeID { get; set; }
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
        public string NextToKinName { get; set; }
        public string NextToKinContactNo { get; set; }
        public bool IsManual { get; set; }
        public bool IsExtra { get; set; }
        public bool Stipend { get; set; }
        public string ResultStatus { get; set; }
        public bool IsSubmitted { get; set; }
        public string CNICImgNADRA { get; set; }
        public string CNICUnVerifiedReason { get; set; }
        public string AgeUnVerifiedReason { get; set; }
        public string ResidenceUnVerifiedReason { get; set; }
        public string CNICVerificationDate { get; set; }
        public int ResultStatusID { get; set; }
        public string TraineeUID { get; set; }
        public bool IsMigrated { get; set; }
        public DateTime? TraineeStatusChangeDate { get; set; }

        //public int TraineeStatusTypeID { get; set; }
        public string TraineeStatusChangeReason { get; set; }

        public DateTime? ResultStatusChangeDate { get; set; }
        public string ResultStatusChangeReason { get; set; }
        public string TraineeRollNumber { get; set; }
        public string TraineeStatusName { get; internal set; }
        public int CertAuthID { get; internal set; }
        public bool? IsCreatedPRNCompletion { get; internal set; }
        public string ResultDocument { get; internal set; }
        public string ResultStatusName { get; internal set; }
        public string Education { get; internal set; }
        public int TraineeStatusTypeID { get; internal set; }
        public string SchemeCode { get; internal set; }
        public int TSROpeningDays { get; internal set; }
        public string Shift { get; internal set; }
        public string TraineeDistrictName { get; internal set; }
        public string TraineeTehsilName { get; internal set; }
        public string ClassDistrictName { get; internal set; }
        public string ClassTehsilName { get; internal set; }
        public string TrainingAddressLocation { get; internal set; }
        public string ClassUID { get; set; }
        public bool IsVarifiedCNIC { get; set; }
        public int IsDocumentGenerated { get; set; }
        public string TraineeEmail { get; set; }

        public int ProvinceID { get; set; }
        public string ProvinceName { get; set; }
        public decimal Duration { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string ReligionName { get; set; }
        public string Disability { get; set; }
        public string Dvv { get; set; }

        public DateTime InvitationDate { get; set; }
        public int CurUserID { get; set; }
        public string Comment { get; set; }
        public DateTime LastActivityDate { get; set; }
        public double PercentageCompleted { get; set; }
        public int DaysSinceLastActivity { get; set; }
        public string CoursraTraineeIDs { get; set; }
        //Added by Rao Ali Haider for International Placement
        public string IBANNumber { get; set; }
        public string BankName { get; set; }
        public string Accounttitle { get; set; }
    }
}