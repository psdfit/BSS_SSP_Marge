using DataLayer.Models;
using System.Collections.Generic;

namespace DataLayer.Interfaces
{
    public interface ISRVUsersRights
    {
        UsersRightsModel GetByUserRightID(int UserRightID);

        List<UsersRightsModel> SaveUsersRights(UsersRightsModel UsersRights);

        List<UsersRightsModel> FetchUsersRights(UsersRightsModel mod);

        List<UsersRightsModel> FetchUsersRights();

        List<UsersRightsModel> FetchUsersRights(bool InActive);

        void ActiveInActive(int UserRightID, bool? InActive, int CurUserID);

        List<UsersRightsModel> GetByFormID(int FormID);

        List<UsersRightsModel> GetByUserID(int UserID);
        List<UsersRightsModel> GetUsersAndRoleRightsByUser(int UserID,int RoleID);
        List<UsersRightsModel> GetRightsByUserAndForm(int userID, int formID);
        List<UsersRightsModel> SSPGetUsersRightsByUser(int UserID);


    }
}