using System;
using System.Collections.Generic;
using System.Text;

namespace DataLayer.Models
{
    public class POHeaderModel: ModelBase
    {
        public int POHeaderID { get; set; }
        public int? DocEntry { get; set; }
        public string DocNum { get; set; }
        public string DocType { get; set; }
        public string Printed { get; set; }
        public DateTime? DocDate { get; set; }
        public DateTime? DocDueDate { get; set; }
        public string CardCode { get; set; }
        public string CardName { get; set; }
        public string Comments { get; set; }
        public string JournalMemo { get; set; }
        public decimal? DocTotal { get; set; }
        public int? BPLId { get; set; }
        public string CtlAccount { get; set; }
        public string U_SCHEME { get; set; }
        public string U_Sch_Code { get; set; }
        public string ProcessKey { get; set; }
        
        /// <summary>
        /// This is not added into the service. Kindly use own your risk
        /// </summary>
        public bool? IsApproved { get; set; }
        public bool? IsRejected { get; set; }
        public string? SAPID { get; set; }
        public DateTime? Month { get; set; }
    }
}
