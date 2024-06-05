using System;
using System.Collections.Generic;
using System.Text;

namespace DataLayer.Models
{
    public class TPMReportsModel
    {
        public string ReportType { get; set; }
        public string SchemeUID { get; set; }
        public string TSPUID { get; set; }
        public DateTime Month { get; set; }
    }
}
