using System;
using System.Collections.Generic;
using System.Text;

namespace DataLayer.Models
{
    public class VoilationSummaryReportModel
    {
        public string Stream { get; set; }
        public string SchemeName { get; set; }
        public string TSPName { get; set; }
        public string TradeName { get; set; }
        public double? FakeGhost { get; set; }
        public double? FeeCharge { get; set; }
        public double? CenterRelocation { get; set; }
        public double? Total { get; set; }
        public double? NonFunctional { get; set; }
        public int? Month { get; set; }
        public string MonthName { get; set; }
        public double? MonthlyScore { get; set; }
        public double? OnScaleOfFive { get; set; }
        public double? FinalScore { get; set; }
        public double? CFFTotal { get; set; }
        public double? NonFunctionalTotal { get; set; }
    }
    public class VoilationSummaryModel
    {
        public string Stream { get; set; }
        public string SchemeName { get; set; }
        public string TSPName { get; set; }
        public string TradeName { get; set; }
        public List<VoilationSummaryReportModel> ItemsList { get; set; }
        public List<string> MonthsList { get; set; }
        public double? AvgMonthlyScore { get; set; }
        public double? CFFOnScaleOfFive { get; set; }
        public double? SeventyPercentWeightage { get; set; }
        public double? AvgMonthlyScoreNonFunc { get; set; }
        public double? NonFuncOnScaleOfFive { get; set; }
        public double? ThirtyPercentWeightage { get; set; }
        public double? FinalScore { get; set; }
    }
}
