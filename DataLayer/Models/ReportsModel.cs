using System;
using System.Collections.Generic;
using System.Text;

namespace DataLayer.Models
{
    public class ReportsModel
    {
        public int ReportID { get; set; }
        public string ReportName { get; set; }
        public int SubReportID { get; set; }
        public string SubReportName { get; set; }
        public string FiltersName { get; set; }
    }
}
