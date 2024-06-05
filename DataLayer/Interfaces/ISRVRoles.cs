using DataLayer.Models;
using System.Collections.Generic;

namespace DataLayer.Interfaces
{
    public interface ISRVRoles
    {
        RolesModel GetByRoleID(int RoleID);

        List<RolesModel> SaveRoles(RolesModel Roles);

        List<RolesModel> FetchRoles(RolesModel mod);

        List<RolesModel> FetchRoles();

        List<RolesModel> FetchRoles(bool InActive);

        void ActiveInActive(int RoleID, bool? InActive, int CurUserID);
    }
}