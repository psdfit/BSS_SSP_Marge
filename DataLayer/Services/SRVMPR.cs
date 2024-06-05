/* **** Aamer Rehman Malik *****/

using DataLayer.Classes;
using DataLayer.Interfaces;
using DataLayer.Models;
using Microsoft.Data.SqlClient;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;

namespace DataLayer.Services
{
    public class SRVMPR : SRVBase, ISRVMPR
    {
        public SRVMPR()
        {
        }

        public MPRModel GetByMPRID(int MPRID)
        {
            try
            {
                SqlParameter param = new SqlParameter("@MPRID", MPRID);
                DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_MPR", param).Tables[0];
                if (dt.Rows.Count > 0)
                {
                    return RowOfMPR(dt.Rows[0]);
                }
                else
                    return null;
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }

        public List<MPRModel> SaveMPR(MPRModel MPR)
        {
            try
            {
                SqlParameter[] param = new SqlParameter[19];
                param[0] = new SqlParameter("@MPRID", MPR.MPRID);
                param[1] = new SqlParameter("@MPRName", MPR.MPRName);
                param[2] = new SqlParameter("@SchemeName", MPR.SchemeName);
                param[3] = new SqlParameter("@TSPName", MPR.TSPName);
                param[4] = new SqlParameter("@ClassCode", MPR.ClassCode);
                param[5] = new SqlParameter("@TradeName", MPR.TradeName);
                param[6] = new SqlParameter("@Batch", MPR.Batch);
                param[7] = new SqlParameter("@GenderName", MPR.GenderName);
                param[8] = new SqlParameter("@Month", MPR.Month);
                param[9] = new SqlParameter("@MPRNo", MPR.MPRNo);
                param[10] = new SqlParameter("@ReportDate", MPR.ReportDate);
                param[11] = new SqlParameter("@CenterName", MPR.CenterName);
                param[12] = new SqlParameter("@CenterDistrict", MPR.CenterDistrict);
                param[13] = new SqlParameter("@CenterTehsil", MPR.CenterTehsil);
                param[14] = new SqlParameter("@CenterInchargeName", MPR.CenterInchargeName);
                param[15] = new SqlParameter("@CenterInchargeMobile", MPR.CenterInchargeMobile);
                param[16] = new SqlParameter("@ClassID", MPR.ClassID);
                param[17] = new SqlParameter("@SRNGenrated", MPR.SRNGenrated);
                param[18] = new SqlParameter("@PRNGenrated", MPR.PRNGenrated);

                SqlHelper.ExecuteNonQuery(SqlHelper.GetCon(), CommandType.StoredProcedure, "[AU_MPR]", param);
                return FetchMPR();
            }
            catch (Exception ex)
            { throw new Exception(ex.Message); }
        }

        private List<MPRModel> LoopinData(DataTable dt)
        {
            List<MPRModel> MPRL = new List<MPRModel>();

            foreach (DataRow r in dt.Rows)
            {
                MPRL.Add(RowOfMPR(r));
            }
            return MPRL;
        }

        public List<MPRModel> FetchMPR(MPRModel model)
        {
            try
            {
                SqlParameter[] prams = Common.GetParams(model);
                DataTable dt;
                if (prams != null)
                    dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_MPR", prams).Tables[0];
                else
                    dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_MPR").Tables[0];
                return LoopinData(dt);
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }
        public List<MPRModel> FetchMPRByKAM(MPRModel model)
        {
            try
            {
                SqlParameter[] prams = Common.GetParams(model);
                DataTable dt;
                if (prams != null)
                    dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_MPR_ByKAM", prams).Tables[0];
                else
                    dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_MPR_ByKAM").Tables[0];
                return LoopinData(dt);
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }

        public List<MPRModel> FetchMPR()
        {
            try
            {
                DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_MPR").Tables[0];
                return LoopinData(dt);
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }

        public List<MPRModel> FetchMPR(bool InActive)
        {
            try
            {
                DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_MPR", new SqlParameter("@InActive", InActive)).Tables[0];
                return LoopinData(dt);
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }

        public List<MPRModel> GetByClassID(int ClassID)
        {
            try
            {
                DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_MPR", new SqlParameter("@ClassID", ClassID)).Tables[0];
                return LoopinData(dt);
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }

        public void ActiveInActive(int MPRID, bool? InActive, int CurUserID)
        {
            SqlParameter[] PLead = new SqlParameter[3];
            PLead[0] = new SqlParameter("@MPRID", MPRID);
            PLead[1] = new SqlParameter("@InActive", InActive);
            PLead[2] = new SqlParameter("@CurUserID", CurUserID);
            SqlHelper.ExecuteNonQuery(SqlHelper.GetCon(), CommandType.StoredProcedure, "[AI_MPR]", PLead);
        }

        public DataSet GetClassMonthview(int ClassID, DateTime? Month,string Type)
        {
            SqlParameter[] PLead = new SqlParameter[3];
            PLead[0] = new SqlParameter("@ClassID", ClassID);
            PLead[1] = new SqlParameter("@Month", Month);
            PLead[2] = new SqlParameter("@Type", Type);

            return SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "[GetClassMonthView]", PLead);
        }

        public DataTable GetMPRView(int MPRID)
        {
            SqlParameter[] PLead = new SqlParameter[2];
            PLead[0] = new SqlParameter("@MPRID", MPRID);

            return SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "[GetClassMonthView]", PLead).Tables[0];
        }

        //public DataTable GetClassMonthview(int ClassID, DateTime? Month)
        //{
        //    SqlParameter[] PLead = new SqlParameter[2];
        //    PLead[0] = new SqlParameter("@ClassID", ClassID);
        //    PLead[1] = new SqlParameter("@Month", Month);

        //    return SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "[GetClassMonthView]", PLead).Tables[0];
        //}
        //public DataTable GetClassMonthview(int ClassID, DateTime? Month)
        //{
        //    SqlParameter[] PLead = new SqlParameter[2];
        //    PLead[0] = new SqlParameter("@ClassID", ClassID);
        //    PLead[1] = new SqlParameter("@Month", Month);

        //    return SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "[GetClassMonthView]", PLead).Tables[0];
        //}

        private MPRModel RowOfMPR(DataRow r)
        {
            MPRModel MPR = new MPRModel();
            MPR.MPRID = Convert.ToInt32(r["MPRID"]);
            MPR.MPRName = r["MPRName"].ToString();
            MPR.SchemeName = r["SchemeName"].ToString();
            MPR.TSPName = r["TSPName"].ToString();
            MPR.ClassCode = r["ClassCode"].ToString();
            MPR.TradeName = r["TradeName"].ToString();
            MPR.Batch = r["Batch"].ToString();
            MPR.GenderName = r["GenderName"].ToString();
            MPR.Month = r["Month"].ToString().GetDate();
            MPR.MPRNo = Convert.ToInt32(r["MPRNo"]);
            MPR.ReportDate = r["ReportDate"].ToString().GetDate();
            MPR.CenterName = r["CenterName"].ToString();
            MPR.CenterDistrict = r["CenterDistrict"].ToString();
            MPR.CenterTehsil = r["CenterTehsil"].ToString();
            MPR.CenterInchargeName = r["CenterInchargeName"].ToString();
            MPR.CenterInchargeMobile = r["CenterInchargeMobile"].ToString();
            MPR.ClassID = Convert.ToInt32(r["ClassID"]);
            MPR.TradeName = r["TradeName"].ToString();
            MPR.SRNGenrated = Convert.ToBoolean(r["SRNGenrated"]);
            MPR.PRNGenrated = Convert.ToBoolean(r["PRNGenrated"]);
            MPR.InActive = Convert.ToBoolean(r["InActive"]);
            MPR.SchemeID = Convert.ToInt32(r["SchemeID"]);
            MPR.TSPID = Convert.ToInt32(r["TSPID"]);

            return MPR;
        }

        public DataTable GetMPRview(int MPRID)
        {
            throw new NotImplementedException();
        }
    }
}