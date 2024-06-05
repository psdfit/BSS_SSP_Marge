using DataLayer.Classes;
using DataLayer.Models;
using Microsoft.Data.SqlClient;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;

namespace DataLayer.Services
{
    public class SRVRegion : SRVBase, DataLayer.Interfaces.ISRVRegion
    {
        public SRVRegion()
        {
        }

        public RegionModel GetByRegionID(int RegionID)
        {
            try
            {
                SqlParameter param = new SqlParameter("@RegionID", RegionID);
                DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_Region", param).Tables[0];
                if (dt.Rows.Count > 0)
                {
                    return RowOfRegion(dt.Rows[0]);
                }
                else
                    return null;
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }

        public List<RegionModel> SaveRegion(RegionModel Region)
        {
            try
            {
                SqlParameter[] param = new SqlParameter[3];
                param[0] = new SqlParameter("@RegionID", Region.RegionID);
                param[1] = new SqlParameter("@RegionName", Region.RegionName);

                param[2] = new SqlParameter("@CurUserID", Region.CurUserID);
                SqlHelper.ExecuteNonQuery(SqlHelper.GetCon(), CommandType.StoredProcedure, "[AU_Region]", param);
                return FetchRegion();
            }
            catch (Exception ex)
            { throw new Exception(ex.Message); }
        }

        private List<RegionModel> LoopinData(DataTable dt)
        {
            List<RegionModel> RegionL = new List<RegionModel>();

            foreach (DataRow r in dt.Rows)
            {
                RegionL.Add(RowOfRegion(r));
            }
            return RegionL;
        }

        public List<RegionModel> FetchRegion(RegionModel mod)
        {
            try
            {
                DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_Region", Common.GetParams(mod)).Tables[0];
                return LoopinData(dt);
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }

        public List<RegionModel> FetchRegion()
        {
            try
            {
                DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_Region").Tables[0];
                return LoopinData(dt);
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }

        public List<RegionModel> FetchRegion(bool InActive)
        {
            try
            {
                DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_Region", new SqlParameter("@InActive", InActive)).Tables[0];
                return LoopinData(dt);
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }

        public int BatchInsert(List<RegionModel> ls, int @BatchFkey, int CurUserID)
        {
            SqlParameter[] param = new SqlParameter[3];

            param[0] = new SqlParameter("@Json", JsonConvert.SerializeObject(ls));
            param[1] = new SqlParameter("@", @BatchFkey);
            param[2] = new SqlParameter("@CurUserID", CurUserID);
            return SqlHelper.ExecuteNonQuery(SqlHelper.GetCon(), CommandType.StoredProcedure, "[BAU_Region]", param);
        }

        public void ActiveInActive(int RegionID, bool? InActive, int CurUserID)
        {
            SqlParameter[] PLead = new SqlParameter[3];
            PLead[0] = new SqlParameter("@RegionID", RegionID);
            PLead[1] = new SqlParameter("@InActive", InActive);
            PLead[2] = new SqlParameter("@CurUserID", CurUserID);
            SqlHelper.ExecuteNonQuery(SqlHelper.GetCon(), CommandType.StoredProcedure, "[AI_Region]", PLead);
        }

        private RegionModel RowOfRegion(DataRow r)
        {
            RegionModel Region = new RegionModel();
            Region.RegionID = Convert.ToInt32(r["RegionID"]);
            Region.RegionName = r["RegionName"].ToString();
            Region.InActive = Convert.ToBoolean(r["InActive"]);
            Region.CreatedUserID = Convert.ToInt32(r["CreatedUserID"]);
            Region.ModifiedUserID = Convert.ToInt32(r["ModifiedUserID"]);
            Region.CreatedDate = r["CreatedDate"].ToString().GetDate();
            Region.ModifiedDate = r["ModifiedDate"].ToString().GetDate();

            return Region;
        }
    }
}