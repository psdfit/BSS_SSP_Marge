using DataLayer.Classes;
using DataLayer.Models;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace DataLayer.Services
{
    public class SRVUsersRights : SRVBase, DataLayer.Interfaces.ISRVUsersRights
    {

        private static IConfigurationRoot configuration;
        static SRVUsersRights()
        {
            configuration = new ConfigurationBuilder().SetBasePath(AppDomain.CurrentDomain.BaseDirectory).AddJsonFile("appsettings.json").Build();
        }

        public UsersRightsModel GetByUserRightID(int UserRightID)
        {
            try
            {
                SqlParameter param = new SqlParameter("@UserRightID", UserRightID);
                DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_UsersRights", param).Tables[0];
                if (dt.Rows.Count > 0)
                {
                    return RowOfUsersRights(dt.Rows[0]);
                }
                else
                    return null;
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }

        public List<UsersRightsModel> SaveUsersRights(UsersRightsModel UsersRights)
        {
            try
            {
                SqlParameter[] param = new SqlParameter[8];
                param[0] = new SqlParameter("@UserRightID", UsersRights.UserRightID);
                param[1] = new SqlParameter("@FormID", UsersRights.FormID);
                param[2] = new SqlParameter("@UserID", UsersRights.UserID);
                param[3] = new SqlParameter("@CanAdd", UsersRights.CanAdd);
                param[4] = new SqlParameter("@CanEdit", UsersRights.CanEdit);
                param[5] = new SqlParameter("@CanDelete", UsersRights.CanDelete);
                param[6] = new SqlParameter("@CanView", UsersRights.CanView);

                param[7] = new SqlParameter("@CurUserID", UsersRights.CurUserID);
                SqlHelper.ExecuteNonQuery(SqlHelper.GetCon(), CommandType.StoredProcedure, "[AU_UsersRights]", param);
                return FetchUsersRights();
            }
            catch (Exception ex)
            { throw new Exception(ex.Message); }
        }

        private List<UsersRightsModel> LoopinData(DataTable dt)
        {
            List<UsersRightsModel> UsersRightsL = new List<UsersRightsModel>();

            foreach (DataRow r in dt.Rows)
            {
                UsersRightsL.Add(RowOfUsersRights(r));
            }         
            return UsersRightsL;
        }

        public List<UsersRightsModel> FetchUsersRights(UsersRightsModel mod)
        {
            try
            {
                DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_UsersRights", Common.GetParams(mod)).Tables[0];
                return LoopinData(dt);
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }

        public List<UsersRightsModel> FetchUsersRights()
        {
            try
            {
                DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_UsersRights").Tables[0];
                return LoopinData(dt);
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }

        public List<UsersRightsModel> FetchUsersRights(bool InActive)
        {
            try
            {
                DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_UsersRights", new SqlParameter("@InActive", InActive)).Tables[0];
                return LoopinData(dt);
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }

        public int BatchInsert(List<UsersRightsModel> ls, int @BatchFkey, int CurUserID)
        {
            SqlParameter[] param = new SqlParameter[3];

            param[0] = new SqlParameter("@Json", JsonConvert.SerializeObject(ls));
            param[1] = new SqlParameter("@", @BatchFkey);
            param[2] = new SqlParameter("@CurUserID", CurUserID);
            return SqlHelper.ExecuteNonQuery(SqlHelper.GetCon(), CommandType.StoredProcedure, "[BAU_UsersRights]", param);
        }

        public List<UsersRightsModel> GetByFormID(int FormID)
        {
            try
            {
                DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_UsersRights", new SqlParameter("@FormID", FormID)).Tables[0];
                return LoopinData(dt);
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }

        public List<UsersRightsModel> GetByUserID(int UserID)
        {
            try
            {
                DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "[GetUsersRightsByUser]", new SqlParameter("@UserID", UserID)).Tables[0];
                return LoopinData(dt);
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }
        
        public List<UsersRightsModel> GetRightsByUserAndForm(int userID, int formID)
        {
            try
            {
                SqlParameter[] PList = new SqlParameter[3];
                PList[0] = new SqlParameter("@UserID", userID);
                PList[1] = new SqlParameter("@FormID", formID);

                DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "[GetRightsByUserAndForm]", PList).Tables[0];
                return LoopinData(dt);
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }

        public List<UsersRightsModel> GetUsersAndRoleRightsByUser(int UserID, int RoleID)
        {
            try
            {
                List<UsersRightsModel> UsersRights = new List<UsersRightsModel>();
                SqlParameter[] PLead = new SqlParameter[3];
                PLead[0] = new SqlParameter("@UserID", UserID);
                PLead[1] = new SqlParameter("@RoleID", RoleID);
                DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "[GetUsersAndRoleRightsByUser]", PLead).Tables[0];
                UsersRights = Helper.ConvertDataTableToModel<UsersRightsModel>(dt);
                return (UsersRights);
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }


        public void ActiveInActive(int UserRightID, bool? InActive, int CurUserID)
        {
            SqlParameter[] PLead = new SqlParameter[3];
            PLead[0] = new SqlParameter("@UserRightID", UserRightID);
            PLead[1] = new SqlParameter("@InActive", InActive);
            PLead[2] = new SqlParameter("@CurUserID", CurUserID);
            SqlHelper.ExecuteNonQuery(SqlHelper.GetCon(), CommandType.StoredProcedure, "[AI_UsersRights]", PLead);
        }
        public List<UsersRightsModel> SSPGetUsersRightsByUser(int UserID)
        {
            try
            {
                DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "SSPGetUsersRightsByUser", new SqlParameter("@UserID", UserID)).Tables[0];
                return LoopinData(dt);
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }
        private UsersRightsModel RowOfUsersRights(DataRow r)
        {
            UsersRightsModel UsersRights = new UsersRightsModel();
            UsersRights.UserRightID = Convert.ToInt32(r["UserRightID"]);
            UsersRights.FormID = Convert.ToInt32(r["FormID"]);
            UsersRights.FormName = r["FormName"].ToString();
            UsersRights.UserID = Convert.ToInt32(r["UserID"]);
            UsersRights.UserName = r["UserName"].ToString();
            UsersRights.CanAdd = Convert.ToBoolean(r["CanAdd"]);
            UsersRights.CanEdit = Convert.ToBoolean(r["CanEdit"]);
            UsersRights.CanDelete = Convert.ToBoolean(r["CanDelete"]);
            UsersRights.CanView = Convert.ToBoolean(r["CanView"]);
            UsersRights.InActive = Convert.ToBoolean(r["InActive"]);
            UsersRights.CreatedUserID = r["CreatedUserID"].ToString().ParseInt();
            UsersRights.ModifiedUserID = r["ModifiedUserID"].ToString().ParseInt();
            UsersRights.CreatedDate = r["CreatedDate"].ToString().GetDate();
            UsersRights.ModifiedDate = r["ModifiedDate"].ToString().GetDate();
            UsersRights.ModuleTitle = r["ModuleTitle"].ToString();
            UsersRights.Path = r["Path"].ToString();
            UsersRights.Controller = r["Controller"].ToString();
            UsersRights.SortOrder = Convert.ToInt32(r["SortOrder"]);
            UsersRights.ModSortOrder = Convert.ToInt32(r["ModSortOrder"]);
            UsersRights.modpath = r["modpath"].ToString();
            UsersRights.IsAddible = Convert.ToBoolean(r["IsAddible"]);
            UsersRights.IsEditable = Convert.ToBoolean(r["IsEditable"]);
            UsersRights.IsDeletable = Convert.ToBoolean(r["IsDeletable"]);
            UsersRights.IsViewable = Convert.ToBoolean(r["IsViewable"]);
            return UsersRights;
        }
    }
}