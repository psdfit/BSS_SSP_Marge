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
    public class SRVClassEventMap : SRVBase, ISRVClassEventMap
    {
        public SRVClassEventMap() { }
        public ClassEventMapModel GetByClassEventMapID(int ClassEventMapID)
        {
            try
            {
                SqlParameter param = new SqlParameter("@ClassEventMapID", ClassEventMapID);
                DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_ClassEventMap", param).Tables[0];
                if (dt.Rows.Count > 0)
                {
                    return RowOfClassEventMap(dt.Rows[0]);
                }
                else
                    return null;
            }
            catch (Exception ex) { throw new Exception(ex.Message); }

        }
        public List<ClassEventMapModel> SaveClassEventMap(ClassEventMapModel ClassEventMap)
        {
            try
            {

                SqlParameter[] param = new SqlParameter[4];
                param[0] = new SqlParameter("@ClassEventMapID", ClassEventMap.ClassEventMapID);
                param[1] = new SqlParameter("@VisitPlanID", ClassEventMap.VisitPlanID);
                param[2] = new SqlParameter("@ClassID", ClassEventMap.ClassID);

                param[3] = new SqlParameter("@CurUserID", ClassEventMap.CurUserID);
                SqlHelper.ExecuteNonQuery(SqlHelper.GetCon(), CommandType.StoredProcedure, "[AU_ClassEventMap]", param);
                return FetchClassEventMap();
            }
            catch (Exception ex)
            { throw new Exception(ex.Message); }
        }
        private List<ClassEventMapModel> LoopinData(DataTable dt)
        {
            List<ClassEventMapModel> ClassEventMapL = new List<ClassEventMapModel>();

            foreach (DataRow r in dt.Rows)
            {
                ClassEventMapL.Add(RowOfClassEventMap(r));

            }
            return ClassEventMapL;
        }
        public List<ClassEventMapModel> FetchClassEventMap(ClassEventMapModel mod)
        {
            try
            {
                DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_ClassEventMap", Common.GetParams(mod)).Tables[0];
                return LoopinData(dt);
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }
        public List<ClassEventMapModel> FetchClassEventMap()
        {
            try
            {
                DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_ClassEventMap").Tables[0];
                return LoopinData(dt);
            }
            catch (Exception ex) { throw new Exception(ex.Message); }

        }
        public List<ClassEventMapModel> FetchClassEventMap(bool InActive)
        {
            try
            {
                DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_ClassEventMap", new SqlParameter("@InActive", InActive)).Tables[0];
                return LoopinData(dt);
            }
            catch (Exception ex) { throw new Exception(ex.Message); }

        }
        public List<ClassEventMapModel> GetByClassID(int ClassID)
        {
            try
            {
                DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_ClassEventMap", new SqlParameter("@ClassID", ClassID)).Tables[0];
                return LoopinData(dt);
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }
        public List<ClassEventMapModel> FetchClassEventMapAll(int VisitPlanID)
        {
            try
            {
                DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_ClassEventMapAll", new SqlParameter("@VisitPlanID", VisitPlanID)).Tables[0];
                return LoopinData(dt);
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }
        public int BatchInsert(List<ClassEventMapModel> ls, int @BatchFkey, int CurUserID)
        {
            SqlParameter[] param = new SqlParameter[3];

            param[0] = new SqlParameter("@Json", JsonConvert.SerializeObject(ls));
            param[1] = new SqlParameter("@VisitPlanID", @BatchFkey);
            param[2] = new SqlParameter("@CurUserID", CurUserID);
            return SqlHelper.ExecuteNonQuery(SqlHelper.GetCon(), CommandType.StoredProcedure, "[BAU_ClassEventMap]", param);
        }
        public void ActiveInActive(int ClassEventMapID, bool? InActive, int CurUserID)
        {

            SqlParameter[] PLead = new SqlParameter[3];
            PLead[0] = new SqlParameter("@ClassEventMapID", ClassEventMapID);
            PLead[1] = new SqlParameter("@InActive", InActive);
            PLead[2] = new SqlParameter("@CurUserID", CurUserID);
            SqlHelper.ExecuteNonQuery(SqlHelper.GetCon(), CommandType.StoredProcedure, "[AI_ClassEventMap]", PLead);
        }
        private ClassEventMapModel RowOfClassEventMap(DataRow r)
        {
            ClassEventMapModel ClassEventMap = new ClassEventMapModel();
            ClassEventMap.ClassEventMapID = Convert.ToInt32(r["ClassEventMapID"]);
            ClassEventMap.VisitPlanID = Convert.ToInt32(r["VisitPlanID"]);
            ClassEventMap.ClassID = Convert.ToInt32(r["ClassID"]);
            //ClassEventMap.TradeName = r["TradeName"].ToString();
            ClassEventMap.InActive = Convert.ToBoolean(r["InActive"]);
            ClassEventMap.CreatedUserID = Convert.ToInt32(r["CreatedUserID"]);
            ClassEventMap.ModifiedUserID = Convert.ToInt32(r["ModifiedUserID"]);
            ClassEventMap.CreatedDate = r["CreatedDate"].ToString().GetDate();
            ClassEventMap.ModifiedDate = r["ModifiedDate"].ToString().GetDate();

            return ClassEventMap;
        }
    }
}
