using System;

namespace PSDF_BSS.Reports.Models
{
    [Serializable]
    public class TradeDataModel
    {
        public string ReportName { get; set; }
        public string SchemeName { get; set; }
        public string Sector { get; set; }
        public long TotalTraineesContracted { get; set; }
        public long ValueofContract { get; set; }
        public double PerofContributionInSchemeTarget { get; set; }
        public double PerofContributionInOverallFYtarget { get; set; }
        public string TSPName { get; set; }
        public string ProgramType { get; set; }
        public string GenderName { get; set; }
    }
}