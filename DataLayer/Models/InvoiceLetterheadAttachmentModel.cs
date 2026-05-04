using System;
using System.Collections.Generic;
using System.Text;

namespace DataLayer.Models
{
    public class InvoiceLetterheadAttachmentModel
    {
        public int? Id { get; set; }  // 🔹 NULL = Insert, Value = Update
        public int TSPID { get; set; }
        public int InvoiceHeaderID { get; set; }
        public int SalesTaxRate { get; set; }
        public bool IsPRARegistered { get; set; }
        public string InvoiceAttachment { get; set; }

        public int? UserID { get; set; }
    }
}
