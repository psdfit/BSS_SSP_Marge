using System;
using System.Collections.Generic;
using System.Text;

namespace DataLayer.Models
{
    public class PaymentScheduleModel
    {
        public int SAP_SchemeID { get; set; }
        public string SchemeCode { get; set; }
        public string Description { get; set; }
        public string PaymentStructure { get; set; }
    }
}
