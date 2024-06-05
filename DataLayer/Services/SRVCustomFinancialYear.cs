using DataLayer.Classes;
using DataLayer.Models;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;

namespace DataLayer.Services
{
    public class SRVCustomFinancialYear : SRVBase, DataLayer.Interfaces.ISRVCustomFinancialYear
    {
        public SRVCustomFinancialYear()
        {
        }

        public CustomFinancialYearModel GetCustomFinancialYearByID(int Id)
        {
            try
            {
                SqlParameter param = new SqlParameter("@Id", Id);
                DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_CustomFinancialYear", param).Tables[0];

                if (dt.Rows.Count > 0)
                {
                    return RowOfCustomFinancialYears(dt.Rows[0]);
                }
                else
                    return null;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public CustomFinancialYearModel RowOfCustomFinancialYears(DataRow r)
        {
            CustomFinancialYearModel data = new CustomFinancialYearModel();

            data.Id = Convert.ToInt32(r["Id"]);
            data.OrgID = Convert.ToInt32(r["OrgID"]);
            data.OrgName = (r["OName"]).ToString();
            data.InActive = Convert.ToBoolean(r["InActive"]);
            data.CreatedUserID = Convert.ToInt32(r["CreatedUserID"]);
            data.ModifiedUserID = Convert.ToInt32(r["ModifiedUserID"]);
            data.FromDate = r["FromDate"].ToString().GetDate();
            data.ToDate = r["ToDate"].ToString().GetDate();
            data.CreatedDate = r["CreatedDate"].ToString().GetDate();
            data.ModifiedDate = r["ModifiedDate"].ToString().GetDate();

            return data;
        }

        public List<CustomFinancialYearModel> SaveCustomFinancialYear(CustomFinancialYearModel mod)
        {
            try
            {
                SqlParameter[] param = new SqlParameter[5];
                param[0] = new SqlParameter("@Id", mod.Id);
                param[1] = new SqlParameter("@FromDate", mod.FromDate);
                param[2] = new SqlParameter("@ToDate", mod.ToDate);
                param[3] = new SqlParameter("@OrgID", mod.OrgID);
                //param[4] = new SqlParameter("@OrgName", mod.OrgName);
                param[4] = new SqlParameter("@CurUserID", mod.CurUserID);
                SqlHelper.ExecuteNonQuery(SqlHelper.GetCon(), CommandType.StoredProcedure, "[AU_CustomFinancialYears]", param);
                return FetchCustomFinancialYear();
            }
            catch (Exception ex)
            { throw new Exception(ex.Message); }
        }

        public List<CustomFinancialYearModel> FetchCustomFinancialYear()
        {
            try
            {
                DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_CustomFinancialYears").Tables[0];
                return LoopinData(dt);
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }

        public List<CustomFinancialYearModel> FetchCustomFinancialYear(CustomFinancialYearModel mod)
        {
            try
            {
                DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_CustomFinancialYears", Common.GetParams(mod)).Tables[0];
                return LoopinData(dt);
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }

        public DataTable FetchFinancialYear(bool InActive)
        {
            try
            {
                DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_CustomFinancialYears", new SqlParameter("@InActive", InActive)).Tables[0];
                return dt;
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }

        public List<CustomFinancialYearModel> FetchCustomFinancialYear(bool InActive)
        {
            try
            {
                DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_CustomFinancialYears", new SqlParameter("@InActive", InActive)).Tables[0];
                return LoopinData(dt);
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }

        public List<CustomFinancialYearModel> GetByOrgID(int OrgID)
        {
            try
            {
                DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_CustomFinancialYears", new SqlParameter("@OrgID", OrgID)).Tables[0];
                return LoopinData(dt);
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }

        public void ActiveInActive(int Id, bool? InActive, int CurUserID)
        {
            SqlParameter[] PLead = new SqlParameter[3];
            PLead[0] = new SqlParameter("@Id", Id);
            PLead[1] = new SqlParameter("@InActive", InActive);
            PLead[2] = new SqlParameter("@CurUserID", CurUserID);
            SqlHelper.ExecuteNonQuery(SqlHelper.GetCon(), CommandType.StoredProcedure, "[AI_CustomFinancialYears]", PLead);
        }

        private List<CustomFinancialYearModel> LoopinData(DataTable dt)
        {
            List<CustomFinancialYearModel> CustFY = new List<CustomFinancialYearModel>();

            foreach (DataRow r in dt.Rows)
            {
                CustFY.Add(RowOfCustomFinancialYears(r));
            }
            return CustFY;
        }
    }
}