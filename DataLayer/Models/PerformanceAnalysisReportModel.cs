using System;
using System.Collections.Generic;
using System.Text;

namespace DataLayer.Models
{
    public class PerformanceAnalysisReportModel
    {
        public string Stream { get; set; }
        public string SchemeName { get; set; }
        public string TSPName { get; set; }
        public string TradeName { get; set; }
        public double? ClassCount { get; set; }
        public double? TotalVisits { get; set; }
        public double? TotalMonthVisits { get; set; }
        public double? Infrastructure { get; set; }
        public double? VoilationSummaryFinalScore { get; set; }
        public double? Attendance { get; set; }
        public double? Deliverables { get; set; }
        public double? CompletionScore { get; set; }
        public double? EmploymentCompletion { get; set; }
        public double? Total { get; set; }
        public double? PerformanceInPercentage { get; set; }
        public double? AvgTraineeCost { get; set; }
    }
}
