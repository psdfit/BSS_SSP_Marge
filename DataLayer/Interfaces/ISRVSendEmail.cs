using DataLayer.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataLayer.Interfaces
{
    public interface ISRVSendEmail
    {
        void GenerateEmailToApprovers(ApprovalModel currentApproval, ApprovalHistoryModel historyModel);
        void GenerateEmailAndSendNotification(ApprovalModel currentApproval, ApprovalHistoryModel historyModel);
    }
}
