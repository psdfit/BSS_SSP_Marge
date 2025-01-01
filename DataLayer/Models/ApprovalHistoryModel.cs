using System;
using System.Collections.Generic;
using System.Text;

namespace DataLayer.Models
{
    public class ApprovalHistoryModel : ModelBase
    {
        public ApprovalHistoryModel() : base() { }
        public int ApprovalHistoryID { get; set; }
        public int Step { get; set; }
        public int FormID { get; set; }
        public int[] FormIDs { get; set; }
        public int? ApproverID { get; set; }
        public int ApprovalStatusID { get; set; }
        public string Comments { get; set; }
      
        public string ApproverName { get; set; }
        public string ProcessKey { get; set; }
        public string ApproverIDs { get; set; }
        public string StatusDisplayName { get; set; }
        public string ApproverNames { get; set; }
        public bool IsFinalStep { get; set; }
        public bool? EmailSentOrNotBit { get; set; }
        public string ClassCode { get; set; }
        public DateTime? Month { get; set; }
        public string ForMonth { get; set; }
        public bool? IsAutoApproval { get; set; }
        public int filterBy { get; set; }
        
    }
}
