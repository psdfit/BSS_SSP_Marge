using System;

namespace DataLayer.Models
{
    [Serializable]
    public class ClassInvoiceMapModel
    {
        public int InvoiceID { get; set; }
        public int ClassID { get; set; }
        public int InvoiceNo { get; set; }
        public decimal Amount { get; set; }
        public string InvoiceType { get; set; }
        public string ClassCode { get; set; }
        public DateTime Month { get; set; }
        public DateTime InvoiceStartDate { get; set; }        
        public DateTime InvoiceEndDate { get; set; }
        public int InvoiceDays { get; set; }
        public DateTime POStartDate { get; set; }
        public DateTime POEndDate { get; set; }
    }
}
