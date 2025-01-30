
using System;

namespace DataLayer.Models
{
    [Serializable]

    public class GURNDetailsModel : ModelBase
    {
        public GURNDetailsModel() : base() { }
        public GURNDetailsModel(bool InActive) : base(InActive) { }

        public int GURNID { get; set; }
        public string ReportId { get; set; }
        public int? TraineeId { get; set; }
        public decimal? Amount { get; set; }
        public string TokenNumber { get; set; }
        public string TransactionNumber { get; set; }
        public string Comments { get; set; }
        public bool? IsPaid { get; set; }
        public bool? IsVarified { get; set; }
        public string TraineeCode { get; set; }
        public string TraineeName { get; set; }
        public string FatherName { get; set; }
        public string TraineeCNIC { get; set; }
        public string ContactNumber1 { get; set; }
        public string DistrictName { get; set; }
        public string TSPName { get; set; }
        public string ClassCode { get; set; }
        public DateTime? Month { get; set; }
        //=========Azhar Iqbal (17-Aug-2023)==============
        public string ProjectName { get; set; }
        public string SchemeName { get; set; }
        public string TSPNameGURNDetail { get; set; }
        public string ClassCodeGURNDetail { get; set; }
        public string ClassStartdateGURNDetail { get; set; }
        public string ClassEnddateGURNDetail { get; set; }
        public string MyProperty { get; set; }
        public string FundingCategory { get; set; }
        public string ClassEndDate { get; set; }
        public string ClassStartDate { get; set; }
        //================================================


        // Added missing columns
        // public string Project { get; set; }
        public string GURUName { get; set; }
        public string GURUCNIC { get; set; }
        public string GURUContactNumber { get; set; }
        
       
    }
}
