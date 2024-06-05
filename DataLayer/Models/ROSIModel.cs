
using System;

namespace DataLayer.Models
{
    [Serializable]

    public class ROSIModel : ModelBase
    {
        public ROSIModel() : base() { }
        public ROSIModel(bool InActive) : base(InActive) { }

        //public int ROSIID	{ get; set; }
        public string SchemeName { get; set; }
        public string SchemeCode { get; set; }
        public string SchemeType { get; set; }
        public int Contractual { get; set; }
        public int CancelledClasses { get; set; }
        public int NetContractual { get; set; }
        public string ReportedEmployed { get; set; }
        public int Verified { get; set; }
        public string CostPerAppendix { get; set; }
        public string Appendix { get; set; }
        public string Testing { get; set; }
        public string Total { get; set; }
        public int AverageWageRate { get; set; }
        public int AverageTrainingCost { get; set; }
        public int NetIncrease { get; set; }
        public string ClassCode { get; set; }
        public string TSPName { get; set; }
        public int SchemeID { get; set; }
        public int TSPID { get; set; }
        public int ClassID { get; set; }
        public int CompletedTrainees { get; set; }
        public int OpportunityCost { get; set; }
        public int VerifiedTrainees { get; set; }
        public double VerifiedOverCommitmentRatio { get; set; }
        public double MarginofEmployment { get; set; }
        public double ROSI { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

    }    public class ROSIFiltersModel
    {
        public string SchemeIDs { get; set; }
        public string TSPIDs { get; set; }
        ////public int ClassID { get; set; }
        public string PTypeIDs { get; set; }
        public string SectorIDs { get; set; }
        public string ClusterIDs { get; set; }
        public string DistrictIDs { get; set; }
        public string TradeIDs { get; set; }
        public string OrganizationIDs { get; set; }
        public string FundingSourceIDs { get; set; }
        public bool EmploymentFlag { get; set; }
        public bool ActualContractualFlag { get; set; }

        //public int SchemeID { get; set; }
        //public int TSPID { get; set; }
        ////public int ClassID { get; set; }
        //public int PTypeID { get; set; }
        //public int SectorID { get; set; }
        //public int ClusterID { get; set; }
        //public int DistrictID { get; set; }
        //public int TradeID { get; set; }
        //public int OrganizationID { get; set; }
        //public int FundingSourceID { get; set; }
        //public int GenderID { get; set; }
        public string GenderIDs { get; set; }
        public string DurationIDs { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

    }}
