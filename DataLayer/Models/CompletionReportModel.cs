using System;
using System.Collections.Generic;
using System.Text;

namespace DataLayer.Models
{
    public class CompletionReportModel
    {
        public string Stream { get; set; }
        public string SchemeName { get; set; }
        public string TSPName { get; set; }
        public string TradeName { get; set; }
        public double? TraineesPerClass { get; set; }
        public double? CompletedTrainees { get; set; }
        public double? CompletedPercentage { get; set; }
        public double? Score { get; set; }
    }
}
