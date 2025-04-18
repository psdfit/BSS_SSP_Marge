
using System;

namespace DataLayer.Models
{
    [Serializable]

    public class MRNModel : ModelBase
    {
        public MRNModel() : base() { }
        public MRNModel(bool InActive) : base(InActive) { }

        public int MRNID { get; set; }
        public int ClassID { get; set; }
        public string ClassCode { get; set; }
        public DateTime? Month { get; set; }
        public DateTime ReportDate { get; set; }
        public int NumberOfMonths { get; set; }
        public bool IsApproved { get; set; }
        public bool IsRejected { get; set; }
        public int Batch { get; set; }
        public string TSPName { get; set; }
        public string TrainingDistrict { get; set; }
        public string TradeName { get; set; }
        public int SchemeID { get; set; }
        public string SchemeName { get; set; }
        public string SchemeCode { get; set; }
        public int OID { get; set; }
        public int TSPID { get; set; }
        public int TSPMasterID { get; set; }
        public int KAMID { get; set; }
        public int ApprovalBatchNo { get; set; }
        public int UserID { get; set; }
        public string ProcessKey { get; set; }
    }
}
