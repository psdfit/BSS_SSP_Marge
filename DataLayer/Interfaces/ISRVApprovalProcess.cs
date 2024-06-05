using DataLayer.Models;
using System.Collections.Generic;

namespace DataLayer.Interfaces
{

    public interface ISRVApprovalProcess
    {
        ApprovalProcessModel GetByProcessKey(string ProcessKey);
        List<ApprovalProcessModel> SaveApprovalProcess(ApprovalProcessModel ApprovalProcess);
        List<ApprovalProcessModel> FetchApprovalProcess(ApprovalProcessModel mod);
        List<ApprovalProcessModel> FetchApprovalProcess();
        List<ApprovalProcessModel> FetchApprovalProcess(bool InActive);
        void ActiveInActive(string ProcessKey, bool? InActive, int CurUserID);
       // public List<ApprovalStatusModel> FetchApprovalStatus();
    }
}
