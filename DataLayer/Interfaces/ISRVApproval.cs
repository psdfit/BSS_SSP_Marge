using DataLayer.Models;
using Microsoft.Data.SqlClient;
using System.Collections.Generic;

namespace DataLayer.Interfaces
{
    public interface ISRVApproval
    {
        ApprovalModel GetByApprovalD(int ApprovalD);
        System.Data.DataTable GetFactSheet(int ApprovalD);
        List<ApprovalModel> SaveApproval(ApprovalModel Approval);
        List<ApprovalModel> FetchApproval(ApprovalModel mod, SqlTransaction transaction = null);
        List<ApprovalModel> FetchApproval(ApprovalModel mod, out string HasAutoApproval);
        List<ApprovalModel> FetchApproval();
        List<ApprovalModel> FetchApproval(bool InActive);
        int BatchInsert(List<ApprovalModel> ls, string @BatchFkey, int CurUserID);
        void ActiveInActive(int ApprovalD, bool? InActive, int CurUserID);
        bool CheckPendingApprovalStep(string ProcessKey);
        int MaxPendindingStep(string ProcessKey);
    }
}