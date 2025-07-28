
using System;

namespace DataLayer.Models
{
    [Serializable]

    public class MPRModel
    {
        public MPRModel() { }


        public int MPRID { get; set; }
        public string MPRName { get; set; }
        public string SchemeName { get; set; }
        public string TSPName { get; set; }
        public string ClassCode { get; set; }
        public string TradeName { get; set; }
        public string Batch { get; set; }
        public string GenderName { get; set; }
        public DateTime? Month { get; set; }
        public int MPRNo { get; set; }
        public DateTime? ReportDate { get; set; }
        public string CenterName { get; set; }
        public string CenterDistrict { get; set; }
        public string CenterTehsil { get; set; }
        public string CenterInchargeName { get; set; }
        public string CenterInchargeMobile { get; set; }
        public int ClassID { get; set; }

        public bool? SRNGenrated { get; set; }
        public bool? PRNGenrated { get; set; }
        public bool? InActive { get; set; }
        public int SchemeID { get; set; }
        public int TSPID { get; set; }
        public int UserID { get; set; }
        public int? KAMID { get; set; }
        public int? FundingCategoryID { get; set; }
    }}
