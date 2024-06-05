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
    public class SRVTSPDetailSchemeMap : SRVBase, ISRVTSPDetailSchemeMap
    {
        public SRVTSPDetailSchemeMap() { }
        public TSPDetailSchemeMapModel GetByID(int ID)
        {
            try
            {
                SqlParameter param = new SqlParameter("@ID", ID);
                DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_TSPDetailSchemeMap", param).Tables[0];
                if (dt.Rows.Count > 0)
                {
                    return RowOfTSPDetailSchemeMap(dt.Rows[0]);
                }
                else
                    return null;
            }
            catch (Exception ex) { throw new Exception(ex.Message); }

        }
        public List<TSPDetailSchemeMapModel> SaveTSPDetailSchemeMap(TSPDetailSchemeMapModel TSPDetailSchemeMap)
        {
            try
            {

                SqlParameter[] param = new SqlParameter[4];
                param[0] = new SqlParameter("@ID", TSPDetailSchemeMap.ID);
                param[1] = new SqlParameter("@SchemeID", TSPDetailSchemeMap.SchemeID);
                param[2] = new SqlParameter("@TSPID", TSPDetailSchemeMap.TSPID);

                param[3] = new SqlParameter("@CurUserID", TSPDetailSchemeMap.CurUserID);
                SqlHelper.ExecuteNonQuery(SqlHelper.GetCon(), CommandType.StoredProcedure, "[AU_TSPDetailSchemeMap]", param);
                return FetchTSPDetailSchemeMap();
            }
            catch (Exception ex)
            { throw new Exception(ex.Message); }
        }
        private List<TSPDetailSchemeMapModel> LoopinData(DataTable dt)
        {
            List<TSPDetailSchemeMapModel> TSPDetailSchemeMapL = new List<TSPDetailSchemeMapModel>();

            foreach (DataRow r in dt.Rows)
            {
                TSPDetailSchemeMapL.Add(RowOfTSPDetailSchemeMap(r));

            }
            return TSPDetailSchemeMapL;
        }
        public List<TSPDetailSchemeMapModel> FetchTSPDetailSchemeMap(TSPDetailSchemeMapModel mod)
        {
            try
            {
                DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_TSPDetailSchemeMap", Common.GetParams(mod)).Tables[0];
                return LoopinData(dt);
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }
        public List<TSPDetailSchemeMapModel> FetchTSPDetailSchemeMap()
        {
            try
            {
                DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_TSPDetailSchemeMap").Tables[0];
                return LoopinData(dt);
            }
            catch (Exception ex) { throw new Exception(ex.Message); }

        }
        public List<TSPDetailSchemeMapModel> FetchTSPDetailSchemeMap(bool InActive)
        {
            try
            {
                DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_TSPDetailSchemeMap", new SqlParameter("@InActive", InActive)).Tables[0];
                return LoopinData(dt);
            }
            catch (Exception ex) { throw new Exception(ex.Message); }

        }
        public List<TSPDetailSchemeMapModel> GetBySchemeID(int SchemeID)
        {
            try
            {
                DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_TSPDetailSchemeMap", new SqlParameter("@SchemeID", SchemeID)).Tables[0];
                return LoopinData(dt);
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }
        public List<TSPDetailSchemeMapModel> GetByTSPID(int TSPID)
        {
            try
            {
                DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_TSPDetailSchemeMap", new SqlParameter("@TSPID", TSPID)).Tables[0];
                return LoopinData(dt);
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }
        public void ActiveInActive(int ID, bool? InActive, int CurUserID)
        {

            SqlParameter[] PLead = new SqlParameter[3];
            PLead[0] = new SqlParameter("@ID", ID);
            PLead[1] = new SqlParameter("@InActive", InActive);
            PLead[2] = new SqlParameter("@CurUserID", CurUserID);
            SqlHelper.ExecuteNonQuery(SqlHelper.GetCon(), CommandType.StoredProcedure, "[AI_TSPDetailSchemeMap]", PLead);
        }
        private TSPDetailSchemeMapModel RowOfTSPDetailSchemeMap(DataRow r)
        {
            TSPDetailSchemeMapModel TSPDetailSchemeMap = new TSPDetailSchemeMapModel();
            TSPDetailSchemeMap.ID = Convert.ToInt32(r["ID"]);
            TSPDetailSchemeMap.SchemeID = Convert.ToInt32(r["SchemeID"]);
            TSPDetailSchemeMap.SchemeName = r["SchemeName"].ToString();
            TSPDetailSchemeMap.TSPID = Convert.ToInt32(r["TSPID"]);
            TSPDetailSchemeMap.TSPName = r["TSPName"].ToString();
            TSPDetailSchemeMap.InActive = Convert.ToBoolean(r["InActive"]);
            TSPDetailSchemeMap.CreatedUserID = Convert.ToInt32(r["CreatedUserID"]);
            TSPDetailSchemeMap.ModifiedUserID = Convert.ToInt32(r["ModifiedUserID"]);
            TSPDetailSchemeMap.CreatedDate = r["CreatedDate"].ToString().GetDate();
            TSPDetailSchemeMap.ModifiedDate = r["ModifiedDate"].ToString().GetDate();

            return TSPDetailSchemeMap;
        }
    }
}
