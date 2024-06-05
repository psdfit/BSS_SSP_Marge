/* **** Aamer Rehman Malik *****/

using System;

namespace DataLayer.Models
{
    [Serializable]
    public class ClassInvoiceMapExtModel : ClassInvoiceMapModel
    {
        public int MPRID { get; set; }
        public int PRNID { get; set; }
        public int Invoices { get; set; }
        public int InvoicesHeader { get; set; }
        public bool MPRGenrated { get; set; }
        public bool RegenrateMPR { get; set; }
        public bool RegenratePRN { get; set; }
        
        public bool IsGenerated { get; set; }
        public bool InCancelation { get; set; }
        public int SRNID { get; set; }
        public int POLineID { get; set; }
        public string InvSapID { get; set; }
        public string POSapID { get; set; }
        public string SRNInvSapID { get; set; }
        public int SRNInvoice { get; set; }
        public int SRNInvHeader { get; set; }
        public bool InvIsCanceled { get; set; }
        public bool InvSRNIsCanceled { get; set; }

        public string Type { get; set; }
        public string Index { get; set; }
    }
}