using System;
using System.Collections.Generic;
using System.Text;
using DataLayer.Models;
using DataLayer.Interfaces;
using Microsoft.Data.SqlClient;
using System.Data;
using DataLayer.Classes;

namespace DataLayer.Services
{
    public class SRVPRNMaster : ISRVPRNMaster
    {
        public List<PRNMasterModel> GetPRNMasterForApproval(int id, SqlTransaction transaction = null)
        {
            DataTable dt = new DataTable();


            if (transaction != null)
            {
                dt = SqlHelper.ExecuteDataset(transaction, CommandType.StoredProcedure, "RD_PRNMaster", new SqlParameter("@PRNMasterID", id)).Tables[0];
            }
            else
            {
                dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_PRNMaster", new SqlParameter("@PRNMasterID", id)).Tables[0];

            }

            if (dt.Rows.Count > 0)
            {
                return LoopinPRN(dt);
            }
            else
                return null;

           
        }

        public List<PRNMasterModel> GetPRNMasterForApproval(PRNMasterModel model)
        {
            List<SqlParameter> param = new List<SqlParameter>();
            param.Add(new SqlParameter("@ProcessKey", model.ProcessKey));
            param.Add(new SqlParameter("@Month", model.Month));
            param.Add(new SqlParameter("@OID", model.OID));
            param.Add(new SqlParameter("@KAMID", model.KAMID));
            param.Add(new SqlParameter("@SchemeID", model.SchemeID));
            //param.Add(new SqlParameter("@TSPID", model.TSPID));
            param.Add(new SqlParameter("@TSPMasterID", model.TSPMasterID));
            param.Add(new SqlParameter("@UserID", model.UserID));
            param.Add(new SqlParameter("@StatusID", model.StatusID));
            DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_PRNMaster", param.ToArray()).Tables[0];

            return LoopinPRN(dt);
        }

        public PRNMasterModel GetPRNMasterByID(PRNMasterModel model, SqlTransaction _transaction = null)
        {
            List<SqlParameter> param = new List<SqlParameter>();
            param.Add(new SqlParameter("@PRNIDMasterID", model.PRNMasterID));
            DataTable dt = new DataTable();

            if (_transaction != null)
                dt = SqlHelper.ExecuteDataset(_transaction, CommandType.StoredProcedure, "RD_PRNMaster", param.ToArray()).Tables[0];
            else
                dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_PRNMaster", param.ToArray()).Tables[0];

            if (dt.Rows.Count > 0)
            {
                return RowOfPRN(dt.Rows[0]);
            }
            else
                return null;
        }

        private List<PRNMasterModel> LoopinPRN(DataTable dt)
        {
            List<PRNMasterModel> prn = new List<PRNMasterModel>();
            foreach (DataRow r in dt.Rows)
            {
                prn.Add(RowOfPRN(r));
            }

            return prn;
        }

        private PRNMasterModel RowOfPRN(DataRow r)
        {
            PRNMasterModel model = new PRNMasterModel();

            model.PRNMasterID = Convert.ToInt32(r["PRNMasterID"]);
            model.InvoiceNumber = Convert.ToInt32(r["InvoiceNumber"]);
            model.TSPID = Convert.ToInt32(r["TSPID"]);
            model.KAMID = Convert.ToInt32(r["KAMID"]);
            model.UserID = Convert.ToInt32(r["UserID"]);
            model.CreatedUserID = Convert.ToInt32(r["CreatedUserID"]);
            model.Month = r["Month"].ToString().GetDate();
            model.CreatedDate = r["CreatedDate"].ToString().GetDate();
            model.IsApproved = Convert.ToBoolean(r["IsApproved"]);
            model.IsRejected = Convert.ToBoolean(r["IsRejected"]);
            model.InActive = Convert.ToBoolean(r["InActive"]);
            model.ProcessKey = r["ProcessKey"].ToString();
            model.ApprovalProcessName = r["ApprovalProcessName"].ToString();
            model.TSPName = r["TSPName"].ToString();
            if (r.Table.Columns.Contains("SchemeName"))
                model.SchemeName = r["SchemeName"].ToString();
            model.TSPColorName = r["TSPColorName"].ToString();
            model.TSPColorCode = r["TSPColorCode"].ToString();
            model.ClassCode = r["ClassCode"].ToString();
            model.ApprovalStepID = Convert.ToInt32(r["ApprovalStepID"]);

            return model;
        }

        public bool PRNMasterApproveReject(PRNMasterModel model, SqlTransaction transaction = null)
        {
            try
            {
                List<SqlParameter> param = new List<SqlParameter>();
                param.Add(new SqlParameter("@PRNMasterID", model.PRNMasterID));
                param.Add(new SqlParameter("@IsApproved", model.IsApproved));
                param.Add(new SqlParameter("@IsRejected", model.IsRejected));

                if (transaction != null)
                {
                    SqlHelper.ExecuteScalar(transaction, CommandType.StoredProcedure, "U_PRNMasterApproveReject", param.ToArray());
                }
                else
                {
                    SqlHelper.ExecuteScalar(SqlHelper.GetCon(), CommandType.StoredProcedure, "U_PRNMasterApproveReject", param.ToArray());
                }
                return true;
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }
    }
}
