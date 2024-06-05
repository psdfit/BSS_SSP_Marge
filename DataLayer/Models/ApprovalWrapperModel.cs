using System;
using System.Collections.Generic;
using System.Text;

namespace DataLayer.Models
{
    public class ApprovalWrapperModel
    {
        public ApprovalHistoryModel approvalHistoryModel { get; set; }
        public List<ApprovalModel> approvals { get; set; }
        public ApprovalModel currentApproval { get; set; }
        public bool isFinalApprover { get; set; }
    }
}
