using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PSDF_BSS.Reports.Models
{
    public class ReportResponse
    {
        public string Response { get; set; }
        public string MimeType { get; set; }
        public string FileName { get; set; }
    }
}