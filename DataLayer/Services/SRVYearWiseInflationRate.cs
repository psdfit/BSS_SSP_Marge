using DataLayer.Classes;
using DataLayer.Models;
using Microsoft.Data.SqlClient;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;

namespace DataLayer.Services
{
    public class SRVYearWiseInflationRate : SRVBase, DataLayer.Interfaces.ISRVYearWiseInflationRate
    {
        public SRVYearWiseInflationRate()
        {
        }

        public YearWiseInflationRateModel GetByIRID(int IRID)
        {
            try
            {
                SqlParameter param = new SqlParameter("@IRID", IRID);
                DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_YearWiseInflationRate", param).Tables[0];
                if (dt.Rows.Count > 0)
                {
                    return RowOfYearWiseInflationRate(dt.Rows[0]);
                }
                else
                    return null;
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }

        public List<YearWiseInflationRateModel> SaveYearWiseInflationRate(YearWiseInflationRateModel YearWiseInflationRate)
        {
            try
            {
                SqlParameter[] param = new SqlParameter[5];
                param[0] = new SqlParameter("@IRID", YearWiseInflationRate.IRID);
                param[1] = new SqlParameter("@FinancialYear", YearWiseInflationRate.FinancialYear);
                param[2] = new SqlParameter("@Month", YearWiseInflationRate.Month);
                param[3] = new SqlParameter("@Inflation", YearWiseInflationRate.Inflation);

                param[4] = new SqlParameter("@CurUserID", YearWiseInflationRate.CurUserID);
                SqlHelper.ExecuteNonQuery(SqlHelper.GetCon(), CommandType.StoredProcedure, "[AU_YearWiseInflationRate]", param);
                return FetchYearWiseInflationRate();
            }
            catch (Exception ex)
            { throw new Exception(ex.Message); }
        }

        private List<YearWiseInflationRateModel> LoopinData(DataTable dt)
        {
            List<YearWiseInflationRateModel> YearWiseInflationRateL = new List<YearWiseInflationRateModel>();

            foreach (DataRow r in dt.Rows)
            {
                YearWiseInflationRateL.Add(RowOfYearWiseInflationRate(r));
            }
            return YearWiseInflationRateL;
        }

        public List<YearWiseInflationRateModel> FetchYearWiseInflationRate(YearWiseInflationRateModel mod)
        {
            try
            {
                DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_YearWiseInflationRate", Common.GetParams(mod)).Tables[0];
                return LoopinData(dt);
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }

        public List<YearWiseInflationRateModel> FetchYearWiseInflationRate()
        {
            try
            {
                DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_YearWiseInflationRate").Tables[0];
                return LoopinData(dt);
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }

        public List<YearWiseInflationRateModel> FetchYearWiseInflationRate(bool InActive)
        {
            try
            {
                DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_YearWiseInflationRate", new SqlParameter("@InActive", InActive)).Tables[0];
                return LoopinData(dt);
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }

        public int BatchInsert(List<YearWiseInflationRateModel> ls, int @BatchFkey, int CurUserID)
        {
            SqlParameter[] param = new SqlParameter[3];

            param[0] = new SqlParameter("@Json", JsonConvert.SerializeObject(ls));
            param[1] = new SqlParameter("@", @BatchFkey);
            param[2] = new SqlParameter("@CurUserID", CurUserID);
            return SqlHelper.ExecuteNonQuery(SqlHelper.GetCon(), CommandType.StoredProcedure, "[BAU_YearWiseInflationRate]", param);
        }

        public void ActiveInActive(int IRID, bool? InActive, int CurUserID)
        {
            SqlParameter[] PLead = new SqlParameter[3];
            PLead[0] = new SqlParameter("@IRID", IRID);
            PLead[1] = new SqlParameter("@InActive", InActive);
            PLead[2] = new SqlParameter("@CurUserID", CurUserID);
            SqlHelper.ExecuteNonQuery(SqlHelper.GetCon(), CommandType.StoredProcedure, "[AI_YearWiseInflationRate]", PLead);
        }

        private YearWiseInflationRateModel RowOfYearWiseInflationRate(DataRow r)
        {
            YearWiseInflationRateModel YearWiseInflationRate = new YearWiseInflationRateModel();
            YearWiseInflationRate.IRID = Convert.ToInt32(r["IRID"]);
            YearWiseInflationRate.FinancialYear = r["FinancialYear"].ToString();
            YearWiseInflationRate.Month = r["Month"].ToString();
            YearWiseInflationRate.Inflation = Convert.ToDouble(r["Inflation"]);
            YearWiseInflationRate.InActive = Convert.ToBoolean(r["InActive"]);
            YearWiseInflationRate.CreatedUserID = Convert.ToInt32(r["CreatedUserID"]);
            YearWiseInflationRate.ModifiedUserID = Convert.ToInt32(r["ModifiedUserID"]);
            YearWiseInflationRate.CreatedDate = r["CreatedDate"].ToString().GetDate();
            YearWiseInflationRate.ModifiedDate = r["ModifiedDate"].ToString().GetDate();

            return YearWiseInflationRate;
        }
    }
}