using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Data.SqlClient;
using System.Threading.Tasks;
using DataLayer.Models;
using System.Data;
using DataLayer.Services;

namespace PSDF_BSS.Logging
{
    public static class Authorize
    {
        public static bool CheckAuthorize(bool IsDelete,int? UserID,string controllerName,string actionName, int? PrimaryKey = null) 
        {
            bool IsAuthorize = new bool();
            AppFormEndPointModel AppFormEndPointList = new AppFormEndPointModel();
            UsersModel user = new UsersModel();
            UsersRightsModel UsersRights = new UsersRightsModel();
            IsAuthorize = true;
            //AppFormEndPointList = GetEndPointPermissin(controllerName, actionName);
            //if (AppFormEndPointList == null)
            //    return true;
            // user = GetRoleIDByUserID(UserID);
            // UsersRights = GetUsersAndRoleRightsByEndPoint(UserID, user.RoleID,AppFormEndPointList.FormID);
            //if (PrimaryKey>0 && IsDelete == false)
            //{
            //    if (UsersRights.CanEdit == true)
            //        IsAuthorize = true;
            //    else
            //        IsAuthorize = false;
            //}
            //else  if ((PrimaryKey ==null || PrimaryKey==0) && IsDelete == false)
            //{
            //    if (UsersRights.CanAdd == true)
            //        IsAuthorize = true;
            //    else
            //        IsAuthorize = false;
            //}
            //else if (PrimaryKey == null && IsDelete == true)
            //{
            //    if (UsersRights.CanDelete == true)
            //        IsAuthorize = true;
            //    else
            //        IsAuthorize = false;
            //}
            return IsAuthorize;
        }
        public static UsersRightsModel  GetUsersAndRoleRightsByEndPoint(int? UserID, int RoleID,int? FormID)
        {
            try
            {
                List<UsersRightsModel> UsersRights = new List<UsersRightsModel>();
                SqlParameter[] PLead = new SqlParameter[3];
                PLead[0] = new SqlParameter("@UserID", UserID);
                PLead[1] = new SqlParameter("@RoleID", RoleID);
                PLead[1] = new SqlParameter("@FormID", FormID);
                DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "[RD_CheckUserAuthorize]", PLead).Tables[0];
                UsersRights = Helper.ConvertDataTableToModel<UsersRightsModel>(dt);
                if (UsersRights.Count > 0)
                {
                    return UsersRights[0];
                }
                else
                { return null; }
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }
        public static AppFormEndPointModel GetEndPointPermissin(string controllerName, string actionName)
        {
            try
            {
                List<AppFormEndPointModel> AppFormEndPointModel = new List<AppFormEndPointModel>();
                SqlParameter[] param = new SqlParameter[3];
                param[0] = new SqlParameter("@Controller", controllerName);
                param[1] = new SqlParameter("@Action", actionName);
                DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "[RD_AppFormEndPoint]", param).Tables[0];
                AppFormEndPointModel = Helper.ConvertDataTableToModel<AppFormEndPointModel>(dt);
                if (AppFormEndPointModel.Count > 0)
                {
                    return AppFormEndPointModel[0];
                }
                else
                { return null; }
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }
        public static UsersModel GetRoleIDByUserID(int? UserID)
        {
            try
            {
                List<UsersModel> user = new List<UsersModel>();
                SqlParameter param = new SqlParameter("@UserID", UserID);
                DataTable dt = new DataTable();
                    dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_Users", param).Tables[0];

                if (dt.Rows.Count > 0)
                {
                    user = Helper.ConvertDataTableToModel<UsersModel>(dt);
                    return user[0];
                }
                else
                    return null;
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }
    }
}
