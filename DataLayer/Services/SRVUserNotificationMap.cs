using DataLayer.Classes;
using DataLayer.Models;
using Microsoft.Data.SqlClient;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;

namespace DataLayer.Services
{
    public class SRVUserNotificationMap : SRVBase, DataLayer.Interfaces.ISRVUserNotificationMap
    {
        public SRVUserNotificationMap()
        {
        }

        public UserNotificationMapModel GetByUserNotificationMapID(int UserNotificationMapID)
        {
            try
            {
                SqlParameter param = new SqlParameter("@UserNotificationMapID", UserNotificationMapID);
                DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_UserNotificationMap", param).Tables[0];
                if (dt.Rows.Count > 0)
                {
                    return RowOfUserNotificationMap(dt.Rows[0]);
                }
                else
                    return null;
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }

        public List<UserNotificationMapModel> SaveUserNotificationMap(UserNotificationMapModel UserNotificationMap)
        {
            try
            {
                SqlParameter[] param = new SqlParameter[4];
                param[0] = new SqlParameter("@UserNotificationMapID", UserNotificationMap.UserNotificationMapID);
                param[1] = new SqlParameter("@UserID", UserNotificationMap.UserID);
                param[2] = new SqlParameter("@NotificationID", UserNotificationMap.NotificationID);

                param[3] = new SqlParameter("@CurUserID", UserNotificationMap.CurUserID);
                SqlHelper.ExecuteNonQuery(SqlHelper.GetCon(), CommandType.StoredProcedure, "[AU_UserNotificationMap]", param);
                return FetchUserNotificationMap();
            }
            catch (Exception ex)
            { throw new Exception(ex.Message); }
        }

        private List<UserNotificationMapModel> LoopinData(DataTable dt)
        {
            List<UserNotificationMapModel> UserNotificationMapL = new List<UserNotificationMapModel>();

            foreach (DataRow r in dt.Rows)
            {
                UserNotificationMapL.Add(RowOfUserNotificationMap(r));
            }
            return UserNotificationMapL;
        }

        public List<UserNotificationMapModel> FetchUserNotificationMap(UserNotificationMapModel mod)
        {
            try
            {
                DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_UserNotificationMap", Common.GetParams(mod)).Tables[0];
                return LoopinData(dt);
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }

        public List<UserNotificationMapModel> FetchUserNotificationMap()
        {
            try
            {
                DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_UserNotificationMap").Tables[0];
                return LoopinData(dt);
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }

        public List<UserNotificationMapModel> FetchUserNotificationMap(bool InActive)
        {
            try
            {
                DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_UserNotificationMap", new SqlParameter("@InActive", InActive)).Tables[0];
                return LoopinData(dt);
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }

        public List<UserNotificationMapModel> FetchUserNotificationMapAll(int NotificationID)
        {
            try
            {
                DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_UserNotificationMapAll", new SqlParameter("@NotificationID", NotificationID)).Tables[0];
                return LoopinData(dt);
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }

        public int BatchInsert(List<UserNotificationMapModel> ls, int @BatchFkey, int CurUserID)
        {
            SqlParameter[] param = new SqlParameter[3];

            param[0] = new SqlParameter("@Json", JsonConvert.SerializeObject(ls));
            param[1] = new SqlParameter("@NotificationID", @BatchFkey);
            param[2] = new SqlParameter("@CurUserID", CurUserID);
            return SqlHelper.ExecuteNonQuery(SqlHelper.GetCon(), CommandType.StoredProcedure, "[BAU_UserNotificationMap]", param);
        }

        public List<UserNotificationMapModel> GetByUserID(int UserID)
        {
            try
            {
                DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_UserNotificationMap", new SqlParameter("@UserID", UserID)).Tables[0];
                return LoopinData(dt);
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }

        public List<UserNotificationMapModel> GetByNotificationID(int NotificationID)
        {
            try
            {
                DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_UserNotificationMap", new SqlParameter("@NotificationID", NotificationID)).Tables[0];
                return LoopinData(dt);
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }

        public void ActiveInActive(int UserNotificationMapID, bool? InActive, int CurUserID)
        {
            SqlParameter[] PLead = new SqlParameter[3];
            PLead[0] = new SqlParameter("@UserNotificationMapID", UserNotificationMapID);
            PLead[1] = new SqlParameter("@InActive", InActive);
            PLead[2] = new SqlParameter("@CurUserID", CurUserID);
            SqlHelper.ExecuteNonQuery(SqlHelper.GetCon(), CommandType.StoredProcedure, "[AI_UserNotificationMap]", PLead);
        }

        private UserNotificationMapModel RowOfUserNotificationMap(DataRow r)
        {
            UserNotificationMapModel UserNotificationMap = new UserNotificationMapModel();
            UserNotificationMap.UserNotificationMapID = Convert.ToInt32(r["UserNotificationMapID"]);
            UserNotificationMap.UserID = Convert.ToInt32(r["UserID"]);
            UserNotificationMap.UserName = r["UserName"].ToString();
            UserNotificationMap.NotificationID = Convert.ToInt32(r["NotificationID"]);
            UserNotificationMap.NotificationName = r["NotificationName"].ToString();
            UserNotificationMap.InActive = Convert.ToBoolean(r["InActive"]);
            UserNotificationMap.CreatedUserID = Convert.ToInt32(r["CreatedUserID"]);
            UserNotificationMap.ModifiedUserID = Convert.ToInt32(r["ModifiedUserID"]);
            UserNotificationMap.CreatedDate = r["CreatedDate"].ToString().GetDate();
            if (r.Table.Columns.Contains("IsSelected"))
                UserNotificationMap.IsSelected = Convert.ToBoolean(r["IsSelected"]);
            UserNotificationMap.ModifiedDate = r["ModifiedDate"].ToString().GetDate();

            return UserNotificationMap;
        }
    }
}