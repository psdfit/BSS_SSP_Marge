
using System;

namespace DataLayer.Models
{
    [Serializable]

    public class TraineeChangeRequestModel : ModelBase
    {
        public TraineeChangeRequestModel() : base() { }
        public TraineeChangeRequestModel(bool InActive) : base(InActive) { }

        public int TraineeChangeRequestID { get; set; }
        public int TraineeID { get; set; }
        public string TraineeCode { get; set; }
        public string TraineeName { get; set; }
        public string TraineeCNIC { get; set; }
        public string TraineeEmail { get; set; }
        public string FatherName { get; set; }
        public bool IsApproved { get; set; }
        public bool IsRejected { get; set; }
        public DateTime DateOfBirth { get; set; }
        public DateTime? CNICIssueDate { get; set; }
        public string TraineeHouseNumber { get; set; }
        public string TraineeStreetMohalla { get; set; }
        public string TraineeMauzaTown { get; set; }
        public int TraineeDistrictID { get; set; }
        public int? TraineeTehsilID { get; set; }
        public string TehsilName { get; set; }
        public string DistrictName { get; set; }
        public string ContactNumber1 { get; set; }
        public string VerificationStatus { get; set; }
        public string Process_Key { get; set; }
        public int SchemeID { get; set; }
        public int TSPID { get; set; }
        public int ClassID { get; set; }
        public string SchemeName { get; set; }
        public string TSPName { get; set; }
        public string TradeName { get; set; }
        public string ClassCode { get; set; }
        public string GenderName { get; set; }
        public string CNICVerified	 { get; set; }
        public string TraineeStatus { get; set; }
        public string DistrictVerified { get; set; }
        public string CNICUnVerifiedReason { get; set; }
        public string BSSStatus { get; set; }
        public string MEStatus { get; set; }
        public string Religion { get; set; }
        public string PermanentAddress { get; set; }
        public string PermanentDistrict { get; set; }
        public string PermanentTehsil { get; set; }
        public string TraineePreImage { get; set; }
        public string TraineechangeImage { get; set; }
        public string TraineeClassCode { get; set; }
        // Newly add fields by Umair Nadeem, date: 9 August 2024
        public string ClassStartDate { get; set; }
        public string ClassEndDate { get; set; }

        public string SchemeType { get; set; }
        public string KAMName { get; set; }
        public string ProjectName { get; set; }
        // Newly add fields by Umair Nadeem, date: 9 August 2024
        public string AMSName { get; set; }
        public int KAMID { get; set; }
        public int FundingCategoryID { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string? Schemes { get; set; }
        public string? TSPs { get; set; }
        public string? Classes { get; set; }

    }}
