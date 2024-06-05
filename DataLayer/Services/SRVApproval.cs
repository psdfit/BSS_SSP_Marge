using DataLayer.Classes;
using DataLayer.Interfaces;
using DataLayer.Models;
using Microsoft.Data.SqlClient;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace DataLayer.Services
{
    public class SRVApproval : SRVBase, ISRVApproval
    {
        public SRVApproval()
        {
        }

        public ApprovalModel GetByApprovalD(int ApprovalD)
        {
            try
            {
                SqlParameter param = new SqlParameter("@ApprovalD", ApprovalD);
                DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_Approval", param).Tables[0];
                if (dt.Rows.Count > 0)
                {
                    return RowOfApproval(dt.Rows[0]);
                }
                else
                    return null;
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }

        public List<ApprovalModel> SaveApproval(ApprovalModel Approval)
        {
            try
            {
                SqlParameter[] param = new SqlParameter[5];
                param[0] = new SqlParameter("@ApprovalD", Approval.ApprovalD);
                param[1] = new SqlParameter("@ProcessKey", Approval.ProcessKey);
                param[2] = new SqlParameter("@Step", Approval.Step);
                param[3] = new SqlParameter("@UserID", Approval.UserIDs);

                param[4] = new SqlParameter("@CurUserID", Approval.CurUserID);
                SqlHelper.ExecuteNonQuery(SqlHelper.GetCon(), CommandType.StoredProcedure, "[AU_Approval]", param);
                return FetchApproval();
            }
            catch (Exception ex)
            { throw new Exception(ex.Message); }
        }

        private List<ApprovalModel> LoopinData(DataTable dt)
        {
            List<ApprovalModel> ApprovalL = new List<ApprovalModel>();

            foreach (DataRow r in dt.Rows)
            {
                ApprovalL.Add(RowOfApproval(r));
            }
            return ApprovalL;
        }

        public List<ApprovalModel> FetchApproval(ApprovalModel mod, SqlTransaction transaction = null)
        {
            try
            {
                DataTable dt = new DataTable();
                if (transaction != null)
                {
                    dt = SqlHelper.ExecuteDataset(transaction,  CommandType.StoredProcedure, "RD_Approval", Common.GetParams(mod)).Tables[0];
                }
                else
                {
                    dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_Approval", Common.GetParams(mod)).Tables[0];
                }
                if(dt.Rows.Count>0)
                    return LoopinData(dt);
                return null;
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }

        public List<ApprovalModel> FetchApproval()
        {
            try
            {
                DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_Approval").Tables[0];
                return LoopinData(dt);
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }

        public List<ApprovalModel> FetchApproval(bool InActive)
        {
            try
            {
                DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_Approval", new SqlParameter("@InActive", InActive)).Tables[0];
                return LoopinData(dt);
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }

        public int BatchInsert(List<ApprovalModel> ls, string @BatchFkey, int CurUserID)
        {
            SqlParameter[] param = new SqlParameter[3];

            param[0] = new SqlParameter("@Json", JsonConvert.SerializeObject(ls));
            param[1] = new SqlParameter("@ProcessKey", @BatchFkey);
            param[2] = new SqlParameter("@CurUserID", CurUserID);
            return SqlHelper.ExecuteNonQuery(SqlHelper.GetCon(), CommandType.StoredProcedure, "[BAU_Approval]", param);
        }
        public DataTable GetFactSheet(int SchemeID)
        {
            try
            {
                SqlParameter param = new SqlParameter("@SchemeID", SchemeID);
                return SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "SP_FactSheet", param).Tables[0];

            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }
        public List<ApprovalModel> GetByUserID(int UserID)
        {
            try
            {
                DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_Approval", new SqlParameter("@UserID", UserID)).Tables[0];
                return LoopinData(dt);
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }

        public void ActiveInActive(int ApprovalD, bool? InActive, int CurUserID)
        {
            SqlParameter[] PLead = new SqlParameter[3];
            PLead[0] = new SqlParameter("@ApprovalD", ApprovalD);
            PLead[1] = new SqlParameter("@InActive", InActive);
            PLead[2] = new SqlParameter("@CurUserID", CurUserID);
            SqlHelper.ExecuteNonQuery(SqlHelper.GetCon(), CommandType.StoredProcedure, "[AI_Approval]", PLead);
        }

        private ApprovalModel RowOfApproval(DataRow r)
        {
            ApprovalModel Approval = new ApprovalModel
            {
                ApprovalD = Convert.ToInt32(r["ApprovalD"]),
                ProcessKey = r["ProcessKey"].ToString(),
                ApprovalProcessName = r["ApprovalProcessName"].ToString(),
                Step = Convert.ToInt32(r["Step"]),
                UserIDs = r["UserIDs"].ToString(),
                UserName = r["UserName"].ToString(),
                InActive = Convert.ToBoolean(r["InActive"]),
                CreatedUserID = Convert.ToInt32(r["CreatedUserID"]),
                ModifiedUserID = Convert.ToInt32(r["ModifiedUserID"]),
                CreatedDate = r["CreatedDate"].ToString().GetDate(),
                ModifiedDate = r["ModifiedDate"].ToString().GetDate()
            };
            if (r.Table.Columns.Contains("IsAutoApproval"))
                Approval.IsAutoApproval = r.Field<bool?>("IsAutoApproval");
            return Approval;
        }
        public List<ApprovalModel> FetchApproval(ApprovalModel mod, out string HasAutoApproval)
        {
            try
            {
                HasAutoApproval = null;
                DataSet ds = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_Approval", Common.GetParams(mod));
                HasAutoApproval = ds.Tables[1].Rows[0]["HasAutoApproval"].ToString();
                return LoopinData(ds.Tables[0]);
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }
        public bool CheckPendingApprovalStep(string ProcessKey)
        {
            try
            {
                SqlParameter param = new SqlParameter("@ProcessKey", ProcessKey);
                DataSet ds = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "dbo.RD_CheckPendingApprovalStep", param);
                bool value = Convert.ToBoolean(ds.Tables[0].Rows[0]["CanBeAutoApproved"]);
                return value;
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }
        public int MaxPendindingStep(string ProcessKey)
        {
            try
            {
                SqlParameter param = new SqlParameter("@ProcessKey", ProcessKey);
                DataSet ds = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "dbo.RD_MaxPendindingStep", param);
                int value = (ds.Tables[0].Rows[0]["MaxPendindingStep"]) is DBNull ? 0 : Convert.ToInt32(ds.Tables[0].Rows[0]["MaxPendindingStep"]);
                return value;
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }
    }
}