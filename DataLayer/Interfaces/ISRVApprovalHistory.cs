using DataLayer.Models;
using Microsoft.Data.SqlClient;
using System.Collections.Generic;

namespace DataLayer.Interfaces
{
    public interface ISRVApprovalHistory
    {
        List<ApprovalHistoryModel> FetchApprovalHistory(ApprovalHistoryModel model, SqlTransaction transaction = null);
        bool SaveSRNApprovalHistory(ref ApprovalWrapperModel model);
        bool SaveGURNApprovalHistory(ref ApprovalWrapperModel model);
        public void SendSRNApprovalNotification(ApprovalWrapperModel model);
        bool SaveTPRNApprovalHistory(ref ApprovalWrapperModel model);
        bool SavePVRNApprovalHistory(ref ApprovalWrapperModel model);
        bool SaveMRNApprovalHistory(ref ApprovalWrapperModel model);
        bool SavePCRNApprovalHistory(ref ApprovalWrapperModel model);
        bool SaveOTRNApprovalHistory(ref ApprovalWrapperModel model);
        public void SendTPRNApprovalNotification(ApprovalWrapperModel model);
        bool SaveApprovalHistory(ref ApprovalWrapperModel wrapperModel);
        public void SendApprovalNotification(ApprovalWrapperModel wrapperModel);
        public List<ApprovalHistoryModel> NextApproval(ApprovalHistoryModel model);
        List<SchemeTradeMapping> FetchTradeTarget(ApprovalHistoryModel model, SqlTransaction transaction = null);
        bool SaveTradeTarget(List<SchemeTradeMapping> model);
        bool UpdateTradeTarget(List<SchemeTradeMapping> model);
    }
}