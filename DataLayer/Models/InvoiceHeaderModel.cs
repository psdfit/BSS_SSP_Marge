using System;
using System.Collections.Generic;
using System.Text;

namespace DataLayer.Models
{
    public class InvoiceHeaderModel
    {
        public int InvoiceHeaderID { get; set; }
        public int TSPID { get; set; }
        public string U_SCHEME { get; set; }
        public string U_SCH_Code { get; set; }
        public DateTime? U_Month { get; set; }
        public string ProcessKey { get; set; }
        public int POHeaderID { get; set; }
        public bool? IsApproved { get; set; }
    }
}
