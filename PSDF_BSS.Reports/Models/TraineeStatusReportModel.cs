using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PSDF_BSS.Reports.Models
{
    public class TraineeStatusReportModel
    {
        public string ReportName { get; set; }
        public string SchemeName { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public long TotalContractedTrainees { get; set; }
        public double GenderWiseBifurcationInPer { get; set; }
        public long Enrolled { get; set; }
        public long Completed { get; set; }
        public string SectorWiseContracted { get; set; }
        public string ClusterWiseContracted { get; set; }
        public string FundingSourceName { get; set; }
    }
}