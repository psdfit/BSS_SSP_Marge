using DataLayer.Classes;
using DataLayer.Models;
using Microsoft.Data.SqlClient;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;

namespace DataLayer.Services
{
    public class SRVReligion : SRVBase, DataLayer.Interfaces.ISRVReligion
    {
        public SRVReligion()
        {
        }

        public ReligionModel GetByReligionID(int ReligionID)
        {
            try
            {
                SqlParameter param = new SqlParameter("@ReligionID", ReligionID);
                DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_Religion", param).Tables[0];
                if (dt.Rows.Count > 0)
                {
                    return RowOfReligion(dt.Rows[0]);
                }
                else
                    return null;
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }

        public List<ReligionModel> SaveReligion(ReligionModel Religion)
        {
            try
            {
                SqlParameter[] param = new SqlParameter[3];
                param[0] = new SqlParameter("@ReligionID", Religion.ReligionID);
                param[1] = new SqlParameter("@ReligionName", Religion.ReligionName);

                param[2] = new SqlParameter("@CurUserID", Religion.CurUserID);
                SqlHelper.ExecuteNonQuery(SqlHelper.GetCon(), CommandType.StoredProcedure, "[AU_Religion]", param);
                return FetchReligion();
            }
            catch (Exception ex)
            { throw new Exception(ex.Message); }
        }

        private List<ReligionModel> LoopinData(DataTable dt)
        {
            List<ReligionModel> ReligionL = new List<ReligionModel>();

            foreach (DataRow r in dt.Rows)
            {
                ReligionL.Add(RowOfReligion(r));
            }
            return ReligionL;
        }

        public List<ReligionModel> FetchReligion(ReligionModel mod)
        {
            try
            {
                DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_Religion", Common.GetParams(mod)).Tables[0];
                return LoopinData(dt);
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }

        public List<ReligionModel> FetchReligion()
        {
            try
            {
                DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_Religion").Tables[0];
                return LoopinData(dt);
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }

        public List<ReligionModel> FetchReligion(bool InActive)
        {
            try
            {
                DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_Religion", new SqlParameter("@InActive", InActive)).Tables[0];
                return LoopinData(dt);
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }

        public int BatchInsert(List<ReligionModel> ls, int @BatchFkey, int CurUserID)
        {
            SqlParameter[] param = new SqlParameter[3];

            param[0] = new SqlParameter("@Json", JsonConvert.SerializeObject(ls));
            param[1] = new SqlParameter("@", @BatchFkey);
            param[2] = new SqlParameter("@CurUserID", CurUserID);
            return SqlHelper.ExecuteNonQuery(SqlHelper.GetCon(), CommandType.StoredProcedure, "[BAU_Religion]", param);
        }

        public void ActiveInActive(int ReligionID, bool? InActive, int CurUserID)
        {
            SqlParameter[] PLead = new SqlParameter[3];
            PLead[0] = new SqlParameter("@ReligionID", ReligionID);
            PLead[1] = new SqlParameter("@InActive", InActive);
            PLead[2] = new SqlParameter("@CurUserID", CurUserID);
            SqlHelper.ExecuteNonQuery(SqlHelper.GetCon(), CommandType.StoredProcedure, "[AI_Religion]", PLead);
        }

        private ReligionModel RowOfReligion(DataRow r)
        {
            ReligionModel Religion = new ReligionModel();
            Religion.ReligionID = Convert.ToInt32(r["ReligionID"]);
            Religion.ReligionName = r["ReligionName"].ToString();
            Religion.InActive = Convert.ToBoolean(r["InActive"]);
            Religion.CreatedUserID = Convert.ToInt32(r["CreatedUserID"]);
            Religion.ModifiedUserID = Convert.ToInt32(r["ModifiedUserID"]);
            Religion.CreatedDate = r["CreatedDate"].ToString().GetDate();
            Religion.ModifiedDate = r["ModifiedDate"].ToString().GetDate();

            return Religion;
        }
    }
}