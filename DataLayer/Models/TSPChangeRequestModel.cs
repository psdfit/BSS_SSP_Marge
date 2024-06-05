
using System;

namespace DataLayer.Models
{
    [Serializable]

    public class TSPChangeRequestModel : ModelBase
    {
        public TSPChangeRequestModel() : base() { }
        public TSPChangeRequestModel(bool InActive) : base(InActive) { }

        public int TSPChangeRequestID { get; set; }
        public int TSPID { get; set; }
        public string TSPName { get; set; }
        public string NTN { get; set; }
        public string PNTN { get; set; }
        public string GST { get; set; }
        public string Address { get; set; }
        public string HeadName { get; set; }
        public string HeadDesignation { get; set; }
        public string HeadEmail { get; set; }
        public string HeadLandline { get; set; }
        public string CPName { get; set; }
        public string CPDesignation { get; set; }
        public string CPEmail { get; set; }
        public string CPLandline { get; set; }
        //public string BankName { get; set; }
        public string BankAccountNumber { get; set; }
        public string AccountTitle { get; set; }
        //public string BankBranch { get; set; }
        public bool IsApproved { get; set; }
        public bool IsRejected { get; set; }
        public string SAPID { get; set; }

        public string AddUpdate { get; set; }
    }}
