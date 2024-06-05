using DataLayer.Models;
using System.Collections.Generic;

namespace DataLayer.Interfaces
{
    public interface ISRVUserOrganizations
    {
        UserOrganizationsModel GetBySrno(int Srno);

        List<UserOrganizationsModel> SaveUserOrganizations(UserOrganizationsModel UserOrganizations);

        List<UserOrganizationsModel> FetchUserOrganizations(UserOrganizationsModel mod);

        List<UserOrganizationsModel> FetchUserOrganizations();

        List<UserOrganizationsModel> FetchUserOrganizations(bool InActive);

        List<UserOrganizationsModel> GetByUserID(int id);
        List<UserOrganizationsModel> GetByUserIDForSSP(int id);

        int BatchInsert(List<UserOrganizationsModel> ls, int @BatchFkey, int CurUserID);

        void ActiveInActive(int Srno, bool? InActive, int CurUserID);
    }
}