using System;
using System.Collections.Generic;
using System.Text;

namespace DataLayer.Models
{
    [Serializable]
    public class ViolationSummaryModel : ModelBase
    {

        public ViolationSummaryModel() : base()
        {
        }

        public string SchemeName { get; set; }
        public string TSPName { get; set; }
        public string ClassCode { get; set; }
        public int Minor { get; set; }
        public int Major { get; set; }
        public int Serious { get; set; }
        public int Total { get; set; }
        public int Observation { get; set; }
        public int TotalClasses { get; set; }
        public string Remarks { get; set; }
        public DateTime MonthForReport { get; set; }
  

    }
}
