using System;
using System.Collections.Generic;
using System.Data;
using Microsoft.Data.SqlClient;
using DataLayer.Classes;
using DataLayer.Models;
using Newtonsoft.Json;
using DataLayer.Interfaces;
namespace DataLayer.Services
{
    public class SRVUserEventMap : SRVBase, ISRVUserEventMap
    {
        public SRVUserEventMap() { }
        public UserEventMapModel GetByUserEventMapID(int UserEventMapID)
        {
            try
            {
                SqlParameter param = new SqlParameter("@UserEventMapID", UserEventMapID);
                DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_UserEventMap", param).Tables[0];
                if (dt.Rows.Count > 0)
                {
                    return RowOfUserEventMap(dt.Rows[0]);
                }
                else
                    return null;
            }
            catch (Exception ex) { throw new Exception(ex.Message); }

        }
        public List<UserEventMapModel> SaveUserEventMap(UserEventMapModel UserEventMap)
        {
            try
            {

                SqlParameter[] param = new SqlParameter[4];
                param[0] = new SqlParameter("@UserEventMapID", UserEventMap.UserEventMapID);
                param[1] = new SqlParameter("@VisitPlanID", UserEventMap.VisitPlanID);
                param[2] = new SqlParameter("@UserID", UserEventMap.UserID);

                param[3] = new SqlParameter("@CurUserID", UserEventMap.CurUserID);
                SqlHelper.ExecuteNonQuery(SqlHelper.GetCon(), CommandType.StoredProcedure, "[AU_UserEventMap]", param);
                return FetchUserEventMap();
            }
            catch (Exception ex)
            { throw new Exception(ex.Message); }
        }
        private List<UserEventMapModel> LoopinData(DataTable dt)
        {
            List<UserEventMapModel> UserEventMapL = new List<UserEventMapModel>();

            foreach (DataRow r in dt.Rows)
            {
                UserEventMapL.Add(RowOfUserEventMap(r));

            }
            return UserEventMapL;
        }
        private List<SchemeEventMapModel> LoopinSchemeEventData(DataTable dt)
        {
            List<SchemeEventMapModel> SchemeEventMapL = new List<SchemeEventMapModel>();

            foreach (DataRow r in dt.Rows)
            {
                SchemeEventMapL.Add(RowOfSchemeEventMap(r));

            }
            return SchemeEventMapL;
        }
        public List<UserEventMapModel> FetchUserEventMap(UserEventMapModel mod)
        {
            try
            {
                DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_UserEventMap", Common.GetParams(mod)).Tables[0];
                return LoopinData(dt);
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }
        public List<SchemeEventMapModel> FetchSchemeEventMap(SchemeEventMapModel mod)
        {
            try
            {
                DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_SchemeEventMap", Common.GetParams(mod)).Tables[0];
                return LoopinSchemeEventData(dt);
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }
        public List<UserEventMapModel> FetchUserEventMap()
        {
            try
            {
                DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_UserEventMap").Tables[0];
                return LoopinData(dt);
            }
            catch (Exception ex) { throw new Exception(ex.Message); }

        }
        public List<UserEventMapModel> FetchUserEventMap(bool InActive)
        {
            try
            {
                DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_UserEventMap", new SqlParameter("@InActive", InActive)).Tables[0];
                return LoopinData(dt);
            }
            catch (Exception ex) { throw new Exception(ex.Message); }

        }

        public List<UserEventMapModel> FetchUserEvents(int UserID)
        {
            try
            {
                DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_UserEventMap", new SqlParameter("@UserID", UserID)).Tables[0];
                return LoopinData(dt);
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }

        public List<UserEventMapModel> FetchUserEventMapAll(int VisitPlanID)
        {
            try
            {
                DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_UserEventMapAll", new SqlParameter("@VisitPlanID", VisitPlanID)).Tables[0];
                return LoopinData(dt);
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }
        public List<SchemeEventMapModel> FetchSchemeEventMapAll(int VisitPlanID)
        {
            try
            {
                DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_SchemeEventMapAll", new SqlParameter("@VisitPlanID", VisitPlanID)).Tables[0];
                return LoopinSchemeEventData(dt);
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }


        public List<UserEventMapModel> GetByUserID(int UserID)
        {
            try
            {
                DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_UserEventMap", new SqlParameter("@UserID", UserID)).Tables[0];
                return LoopinData(dt);
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }

        public int BatchInsert(List<UserEventMapModel> ls, int @BatchFkey, int CurUserID)
        {
            SqlParameter[] param = new SqlParameter[3];

            param[0] = new SqlParameter("@Json", JsonConvert.SerializeObject(ls));
            param[1] = new SqlParameter("@VisitPlanID", @BatchFkey);
            param[2] = new SqlParameter("@CurUserID", CurUserID);
            return SqlHelper.ExecuteNonQuery(SqlHelper.GetCon(), CommandType.StoredProcedure, "[BAU_UserEventMap]", param);
        }
        public int BatchInsertSchemes(List<SchemeEventMapModel> ls, int @BatchFkey, int CurUserID)
        {
            SqlParameter[] param = new SqlParameter[3];

            param[0] = new SqlParameter("@Json", JsonConvert.SerializeObject(ls));
            param[1] = new SqlParameter("@VisitPlanID", @BatchFkey);
            param[2] = new SqlParameter("@CurUserID", CurUserID);
            return SqlHelper.ExecuteNonQuery(SqlHelper.GetCon(), CommandType.StoredProcedure, "[BAU_SchemeEventMap]", param);
        }

        public void ActiveInActive(int UserEventMapID, bool? InActive, int CurUserID)
        {

            SqlParameter[] PLead = new SqlParameter[3];
            PLead[0] = new SqlParameter("@UserEventMapID", UserEventMapID);
            PLead[1] = new SqlParameter("@InActive", InActive);
            PLead[2] = new SqlParameter("@CurUserID", CurUserID);
            SqlHelper.ExecuteNonQuery(SqlHelper.GetCon(), CommandType.StoredProcedure, "[AI_UserEventMap]", PLead);
        }
        private UserEventMapModel RowOfUserEventMap(DataRow r)
        {
            UserEventMapModel UserEventMap = new UserEventMapModel();
            UserEventMap.UserEventMapID = Convert.ToInt32(r["UserEventMapID"]);
            UserEventMap.VisitPlanID = Convert.ToInt32(r["VisitPlanID"]);
            UserEventMap.UserID = Convert.ToInt32(r["UserID"]);
            UserEventMap.UserName = r["UserName"].ToString();
            UserEventMap.InActive = Convert.ToBoolean(r["InActive"]);
            UserEventMap.CreatedUserID = Convert.ToInt32(r["CreatedUserID"]);
            UserEventMap.ModifiedUserID = Convert.ToInt32(r["ModifiedUserID"]);
            UserEventMap.CreatedDate = r["CreatedDate"].ToString().GetDate();
            UserEventMap.ModifiedDate = r["ModifiedDate"].ToString().GetDate();

            return UserEventMap;
        }
        private SchemeEventMapModel RowOfSchemeEventMap(DataRow r)
        {
            SchemeEventMapModel SchemeEventMap = new SchemeEventMapModel();
            SchemeEventMap.SchemeEventMapID = Convert.ToInt32(r["SchemeEventMapID"]);
            SchemeEventMap.VisitPlanID = Convert.ToInt32(r["VisitPlanID"]);
            SchemeEventMap.SchemeID = Convert.ToInt32(r["SchemeID"]);
            //SchemeEventMap.UserName = r["UserName"].ToString();
            SchemeEventMap.InActive = Convert.ToBoolean(r["InActive"]);
            SchemeEventMap.CreatedUserID = Convert.ToInt32(r["CreatedUserID"]);
            SchemeEventMap.ModifiedUserID = Convert.ToInt32(r["ModifiedUserID"]);
            SchemeEventMap.CreatedDate = r["CreatedDate"].ToString().GetDate();
            SchemeEventMap.ModifiedDate = r["ModifiedDate"].ToString().GetDate();

            return SchemeEventMap;
        }
    }
}
