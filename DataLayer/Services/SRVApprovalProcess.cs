using DataLayer.Classes;
using DataLayer.Models;
using Microsoft.Data.SqlClient;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;

namespace DataLayer.Services
{
    public class SRVApprovalProcess : SRVBase, DataLayer.Interfaces.ISRVApprovalProcess
    {
        public SRVApprovalProcess()
        {
        }

        public ApprovalProcessModel GetByProcessKey(string ProcessKey)
        {
            try
            {
                SqlParameter param = new SqlParameter("@ProcessKey", ProcessKey);
                DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_ApprovalProcess", param).Tables[0];
                if (dt.Rows.Count > 0)
                {
                    return RowOfApprovalProcess(dt.Rows[0]);
                }
                else
                    return null;
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }

        public List<ApprovalProcessModel> SaveApprovalProcess(ApprovalProcessModel ApprovalProcess)
        {
            try
            {
                SqlParameter[] param = new SqlParameter[3];
                param[0] = new SqlParameter("@ProcessKey", ApprovalProcess.ProcessKey);
                param[1] = new SqlParameter("@ApprovalProcessName", ApprovalProcess.ApprovalProcessName);

                param[2] = new SqlParameter("@CurUserID", ApprovalProcess.CurUserID);
                SqlHelper.ExecuteNonQuery(SqlHelper.GetCon(), CommandType.StoredProcedure, "[AU_ApprovalProcess]", param);
                return FetchApprovalProcess();
            }
            catch (Exception ex)
            { throw new Exception(ex.Message); }
        }

        private List<ApprovalProcessModel> LoopinData(DataTable dt)
        {
            List<ApprovalProcessModel> ApprovalProcessL = new List<ApprovalProcessModel>();

            foreach (DataRow r in dt.Rows)
            {
                ApprovalProcessL.Add(RowOfApprovalProcess(r));
            }
            return ApprovalProcessL;
        }

        public List<ApprovalProcessModel> FetchApprovalProcess(ApprovalProcessModel mod)
        {
            try
            {
                DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_ApprovalProcess", Common.GetParams(mod)).Tables[0];
                return LoopinData(dt);
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }

        public List<ApprovalProcessModel> FetchApprovalProcess()
        {
            try
            {
                DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_ApprovalProcess").Tables[0];
                return LoopinData(dt);
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }

        //public List<ApprovalStatusModel> FetchApprovalStatus()
        //{
        //    try
        //    {
        //        DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_ApprovalStatus").Tables[0];
        //        List<ApprovalStatusModel> KAMUser = Helper.ConvertDataTableToModel<ApprovalStatusModel>(dt);
        //        return KAMUser;
        //    }
        //    catch (Exception ex) { throw new Exception(ex.Message); }
        //}

        public List<ApprovalProcessModel> FetchApprovalProcess(bool InActive)
        {
            try
            {
                DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_ApprovalProcess", new SqlParameter("@InActive", InActive)).Tables[0];
                return LoopinData(dt);
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }

        public int BatchInsert(List<ApprovalProcessModel> ls, int @BatchFkey, int CurUserID)
        {
            SqlParameter[] param = new SqlParameter[3];

            param[0] = new SqlParameter("@Json", JsonConvert.SerializeObject(ls));
            param[1] = new SqlParameter("@", @BatchFkey);
            param[2] = new SqlParameter("@CurUserID", CurUserID);
            return SqlHelper.ExecuteNonQuery(SqlHelper.GetCon(), CommandType.StoredProcedure, "[BAU_ApprovalProcess]", param);
        }

        public void ActiveInActive(string ProcessKey, bool? InActive, int CurUserID)
        {
            SqlParameter[] PLead = new SqlParameter[3];
            PLead[0] = new SqlParameter("@ProcessKey", ProcessKey);
            PLead[1] = new SqlParameter("@InActive", InActive);
            PLead[2] = new SqlParameter("@CurUserID", CurUserID);
            SqlHelper.ExecuteNonQuery(SqlHelper.GetCon(), CommandType.StoredProcedure, "[AI_ApprovalProcess]", PLead);
        }

        private ApprovalProcessModel RowOfApprovalProcess(DataRow r)
        {
            ApprovalProcessModel ApprovalProcess = new ApprovalProcessModel();
            ApprovalProcess.ProcessKey = r["ProcessKey"].ToString();
            ApprovalProcess.ApprovalProcessName = r["ApprovalProcessName"].ToString();
            ApprovalProcess.InActive = Convert.ToBoolean(r["InActive"]);
            ApprovalProcess.CreatedUserID = Convert.ToInt32(r["CreatedUserID"]);
            ApprovalProcess.ModifiedUserID = Convert.ToInt32(r["ModifiedUserID"]);
            ApprovalProcess.CreatedDate = r["CreatedDate"].ToString().GetDate();
            ApprovalProcess.ModifiedDate = r["ModifiedDate"].ToString().GetDate();

            return ApprovalProcess;
        }
    }
}