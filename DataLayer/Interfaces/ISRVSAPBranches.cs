using DataLayer.Models;
using System.Collections.Generic;

namespace DataLayer.Interfaces
{

    public interface ISRVSAPBranches
    {
        SAPBranchesModel GetById(int Id);
        List<SAPBranchesModel> SaveSAPBranches(SAPBranchesModel SAPBranches);
        List<SAPBranchesModel> FetchSAPBranches(SAPBranchesModel mod);
        List<SAPBranchesModel> FetchSAPBranches();
        List<SAPBranchesModel> FetchSAPBranches(bool InActive);
        void ActiveInActive(int Id, bool? InActive, int CurUserID);
    }
}
