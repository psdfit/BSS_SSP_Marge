using System;
using System.Collections.Generic;
using System.Text;

namespace DataLayer.Models
{

    public class TSPInvoiceStatusModel
    {
        public int ClassID { get; set; }
        public int InvoiceHeaderID { get; set; }
        public int UserID { get; set; }
        public DateTime? Month {get; set;}

    }
    
}
