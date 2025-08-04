using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataLayer.Models
{
    public class InvoiceMasterModel
    {
        public int InvoiceHeaderID { get; set; }
        public int? DocNum { get; set; }
        public int BPL_IDAssignedToInvoice { get; set; }
        public decimal DocTotal { get; set; }
        public bool IsApproved { get; set; }
        public bool IsRejected { get; set; }
        public string TSPName { get; set; }
        public string DocType { get; set; }
        public string Printed { get; set; }
        public string CardCode { get; set; }
        public string CardName { get; set; }
        public string Comments { get; set; }
        public string SchemeName { get; set; }
        public string JournalMemo { get; set; }
        public string CtlAccount { get; set; }
        public string U_SCHEME { get; set; }
        public string U_SCH_Code { get; set; }
        public string SAPCODE { get; set; }
        public string POSAPCODE { get; set; }
        public string ProcessKey { get; set; }
        public string OcrCode { get; internal set; }
        public string OcrCode2 { get; internal set; }
        public string OcrCode3 { get; internal set; }
        public string AcctCode { get; internal set; }
        public DateTime? U_Month { get; set; }
        public DateTime? DocDate { get; set; }
        public DateTime? DocDueDate { get; set; }
        public DateTime? CreatedDate { get; set; }
        public int OID { get; set; }
        public int KAMID { get; set; }
        public int SchemeID { get; set; }
        public int ClassID { get; set; }
        public int TSPID { get; set; }
        public int TSPMasterID { get; set; }
        public int? CreatedUserID { get; set; }
        public string TSPColorCode { get; set; }
        public string TSPColorName { get; set; }
        public string SAPID { get; set; }
        public int? U_IPS { get; set; }
        public string ClassCode { get; set; }

        public string SchemeCode { get; set; }
        public string TSPSAPCode { get; set; }
        public string InvoiceType { get; set; }
        public DateTime? InvoiceMonth { get; set; }
        public string InvoiceStatus { get; set; }
        public decimal NetPayableAmount { get; set; }
        public string? Status { get; set; }


    }
}
