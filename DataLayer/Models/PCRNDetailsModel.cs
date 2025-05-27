
using System;

namespace DataLayer.Models
{
    [Serializable]

    public class PCRNDetailsModel : ModelBase
    {
        public PCRNDetailsModel() : base() { }
        public PCRNDetailsModel(bool InActive) : base(InActive) { }

        public int PCRNID { get; set; }
        public string ReportId { get; set; }
        public int TraineeId { get; set; }
        public decimal Amount { get; set; }
        public string TokenNumber { get; set; }
        public string TransactionNumber { get; set; }
        public string Comments { get; set; }
        public bool IsPaid { get; set; }
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
        public string TSPNamePCRNDetail { get; set; }
        public string ClassCodePCRNDetail { get; set; }
        public string ClassStartdatePCRNDetail { get; set; }
        public string ClassEnddatePCRNDetail { get; set; }
        public string MyProperty { get; set; }
        public string FundingCategory { get; set; }
        public string ClassEndDate { get; set; }
        public string ClassStartDate { get; set; }
        //================================================

    }
}
