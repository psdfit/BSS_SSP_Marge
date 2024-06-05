using System;
using System.Collections.Generic;
using System.Text;

namespace DataLayer.Models
{
   public class GetTSPPerformanceModel: ModelBase
    {
        public int? TSPPerformanceByAttendanceID { get; set; }
        public int? SchemeID { get; set; }
        public int? TSPID { get; set; }
        public int? TradeID { get; set; }
        public string Date { get; set; }
        public string Stream { get; set; }
        public string SchemeName { get; set; }
        public string TSPName { get; set; }
        public string TradeName { get; set; }
        public int? Violation { get; set; }
        public int? Attendance { get; set; }
        public int? Deliverables { get; set; }
        public int? CompletionPercentage { get; set; }
        public int? PlacementRatio { get; set; }
        public decimal? Total { get; set; }
        public int? PerformanceInPercentage { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        
    }
}
