using DataLayer.Models;
using System.Collections.Generic;

namespace DataLayer.Interfaces
{
    public interface ISRVRolesRights
    {
        RolesRightsModel GetByRoleRightID(int RoleRightID);
        List<RolesRightsModel> SaveRolesRights(RolesRightsModel RolesRights);
        List<RolesRightsModel> FetchRolesRights(RolesRightsModel mod);
        List<RolesRightsModel> FetchRolesRights();
        List<RolesRightsModel> FetchRolesRights(bool InActive);
        void ActiveInActive(int RoleRightID, bool? InActive, int CurUserID);
    }
}