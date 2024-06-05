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
    public class SRVMPRTraineeDetail : SRVBase, ISRVMPRTraineeDetail
    {
        public SRVMPRTraineeDetail() { }
        public MPRTraineeDetailModel GetByMPRDetailID(int MPRDetailID)
        {
            try
            {
                SqlParameter param = new SqlParameter("@MPRDetailID", MPRDetailID);
                DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_MPRTraineeDetail", param).Tables[0];
                if (dt.Rows.Count > 0)
                {
                    return RowOfMPRTraineeDetail(dt.Rows[0]);
                }
                else
                    return null;
            }
            catch (Exception ex) { throw new Exception(ex.Message); }

        }
        public List<MPRTraineeDetailModel> SaveMPRTraineeDetail(MPRTraineeDetailModel MPRTraineeDetail)
        {
            try
            {

                SqlParameter[] param = new SqlParameter[9];
                param[0] = new SqlParameter("@MPRDetailID", MPRTraineeDetail.MPRDetailID);
                param[1] = new SqlParameter("@MPRID", MPRTraineeDetail.MPRID);
                param[2] = new SqlParameter("@TraineeID", MPRTraineeDetail.TraineeID);
                param[3] = new SqlParameter("@TraineeCode", MPRTraineeDetail.TraineeCode);
                param[4] = new SqlParameter("@AttendanceMet", MPRTraineeDetail.AttendanceMet);
                param[5] = new SqlParameter("@Reason", MPRTraineeDetail.Reason);
                param[6] = new SqlParameter("@TraineeStatus", MPRTraineeDetail.TraineeStatus);
                param[7] = new SqlParameter("@StipendAmount", MPRTraineeDetail.StipendAmount);

                param[8] = new SqlParameter("@CurUserID", MPRTraineeDetail.CurUserID);
                SqlHelper.ExecuteNonQuery(SqlHelper.GetCon(), CommandType.StoredProcedure, "[AU_MPRTraineeDetail]", param);
                return FetchMPRTraineeDetail();
            }
            catch (Exception ex)
            { throw new Exception(ex.Message); }
        }
        private List<MPRTraineeDetailModel> LoopinData(DataTable dt)
        {
            List<MPRTraineeDetailModel> MPRTraineeDetailL = new List<MPRTraineeDetailModel>();

            foreach (DataRow r in dt.Rows)
            {
                MPRTraineeDetailL.Add(RowOfMPRTraineeDetail(r));

            }
            return MPRTraineeDetailL;
        }
        public List<MPRTraineeDetailModel> FetchMPRTraineeDetail(MPRTraineeDetailModel mod)
        {
            try
            {
                DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_MPRTraineeDetail", Common.GetParams(mod)).Tables[0];
                return LoopinData(dt);
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }
        public List<MPRTraineeDetailModel> FetchMPRTraineeDetail()
        {
            try
            {
                DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_MPRTraineeDetail").Tables[0];
                return LoopinData(dt);
            }
            catch (Exception ex) { throw new Exception(ex.Message); }

        }
        public List<MPRTraineeDetailModel> FetchMPRTraineeDetail(bool InActive)
        {
            try
            {
                DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_MPRTraineeDetail", new SqlParameter("@InActive", InActive)).Tables[0];
                return LoopinData(dt);
            }
            catch (Exception ex) { throw new Exception(ex.Message); }

        }
        public List<MPRTraineeDetailModel> GetByMPRID(int MPRID)
        {
            try
            {
                DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_MPRTraineeDetail", new SqlParameter("@MPRID", MPRID)).Tables[0];
                return LoopinData(dt);
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }

        public List<MPRTraineeDetailModel> GetMPRExcelExportByIDs(string ids)
        {
            List<SqlParameter> param = new List<SqlParameter>();
            DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_MPRTraineeDetail_ByIDs", new SqlParameter("@MPRIDs", ids)).Tables[0];
            return LoopinData(dt);
        }
        public List<MPRTraineeDetailModel> GetByTraineeID(int TraineeID)
        {
            try
            {
                DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_MPRTraineeDetail", new SqlParameter("@TraineeID", TraineeID)).Tables[0];
                return LoopinData(dt);
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }
        public void ActiveInActive(int MPRDetailID, bool? InActive, int CurUserID)
        {

            SqlParameter[] PLead = new SqlParameter[3];
            PLead[0] = new SqlParameter("@MPRDetailID", MPRDetailID);
            PLead[1] = new SqlParameter("@InActive", InActive);
            PLead[2] = new SqlParameter("@CurUserID", CurUserID);
            SqlHelper.ExecuteNonQuery(SqlHelper.GetCon(), CommandType.StoredProcedure, "[AI_MPRTraineeDetail]", PLead);
        }
        private MPRTraineeDetailModel RowOfMPRTraineeDetail(DataRow r)
        {
            MPRTraineeDetailModel MPRTraineeDetail = new MPRTraineeDetailModel();
            MPRTraineeDetail.MPRDetailID = Convert.ToInt32(r["MPRDetailID"]);
            MPRTraineeDetail.MPRID = Convert.ToInt32(r["MPRID"]);
            MPRTraineeDetail.TSPName = r["TSPName"].ToString();
            MPRTraineeDetail.TraineeID = Convert.ToInt32(r["TraineeID"]);

            

            MPRTraineeDetail.TraineeCode = r["TraineeCode"].ToString();
            MPRTraineeDetail.TraineeStatusName = r["TraineeStatusName"].ToString();
            MPRTraineeDetail.ClassCode = r["ClassCode"].ToString();
            MPRTraineeDetail.CNICVerificationStatus = r["CNICVerificationStatus"].ToString();
            MPRTraineeDetail.TraineeCNIC = r["TraineeCNIC"].ToString();
            MPRTraineeDetail.ExtraStatus = r["ExtraStatus"].ToString();
            MPRTraineeDetail.UID = r["UID"].ToString();
            MPRTraineeDetail.AttendanceMet = Convert.ToBoolean(r["AttendanceMet"]);
            MPRTraineeDetail.Reason = r["Reason"].ToString();
            MPRTraineeDetail.TraineeStatus = Convert.ToInt32(r["TraineeStatus"]);
            MPRTraineeDetail.StipendAmount = Convert.ToDouble(r["StipendAmount"]);
            MPRTraineeDetail.InActive = Convert.ToBoolean(r["InActive"]);
            MPRTraineeDetail.CreatedUserID = Convert.ToInt32(r["CreatedUserID"]);
            MPRTraineeDetail.ModifiedUserID = Convert.ToInt32(r["ModifiedUserID"]);
            MPRTraineeDetail.CreatedDate = r["CreatedDate"].ToString().GetDate();
            MPRTraineeDetail.ModifiedDate = r["ModifiedDate"].ToString().GetDate();

            MPRTraineeDetail.IsMarginal = r["IsMarginal"].ToString();
            if (r.Table.Columns.Contains("mprMonth"))
            {
                MPRTraineeDetail.TraineeName = r["TraineeName"].ToString();
                MPRTraineeDetail.mprMonth = r["mprMonth"].ToString();
                MPRTraineeDetail.SchemeName = r["SchemeName"].ToString();
            }
            return MPRTraineeDetail;
        }
    }
}
