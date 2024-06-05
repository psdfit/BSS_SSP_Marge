using System;

namespace PSDF_BSS.Reports.Models
{
    [Serializable]
    public class TSPMasterDataModel
    {
        public string ReportName { get; set; }
        public string SchemeName { get; set; }
        public string TSPName { get; set; }
        public Int64 TotalTraineesContracted { get; set; }
        public Int64 ValueofContract { get; set; }
        public string Status { get; set; }
        public double PerofContributionInSchemeTarget { get; set; }
        public double PerofContributionInOverallFYtarget { get; set; }
        public string ProgramType { get; set; }
        public string GenderName { get; set; }
    }
}