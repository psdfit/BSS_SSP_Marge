using System;
using System.Collections.Generic;
using System.Data;
using Microsoft.Data.SqlClient;
using DataLayer.Classes;
using DataLayer.Models;
using Newtonsoft.Json;
using DataLayer.Interfaces;
using System.Linq;

namespace DataLayer.Services
{
    public class SRVTSPChangeRequest : SRVBase, ISRVTSPChangeRequest
    {
        private readonly ISRVSendEmail srvSendEmail;
        private readonly ISRVTSPMaster srvTSPMaster;
        public SRVTSPChangeRequest(ISRVSendEmail srvSendEmail, ISRVTSPMaster srvTSPMaster)
        {
            this.srvSendEmail = srvSendEmail;
            this.srvTSPMaster = srvTSPMaster;
        }

        public TSPChangeRequestModel GetByTSPChangeRequestID(int TSPChangeRequestID, SqlTransaction transaction = null)
        {
            try
            {
                SqlParameter param = new SqlParameter("@TSPChangeRequestID", TSPChangeRequestID);
                DataTable dt = new DataTable();


                //DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_TSPChangeRequest", param).Tables[0];
                //dt = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "RD_TSPChangeRequest", new SqlParameter("@TSPID", TSPChangeRequestID));
                if (transaction != null)
                {
                    dt = SqlHelper.ExecuteDataset(transaction, CommandType.StoredProcedure, "RD_TSPChangeRequest", param).Tables[0];
                }
                else
                {
                    dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_TSPChangeRequest", param).Tables[0];

                }

                //return LoopinData(dt);

                if (dt.Rows.Count > 0)
                {
                    return RowOfTSPChangeRequest(dt.Rows[0]);
                }
                else
                    return null;
            }
            catch (Exception ex) { throw new Exception(ex.Message); }

        }
        public List<TSPChangeRequestModel> SaveTSPChangeRequest(TSPChangeRequestModel TSPChangeRequest)
        {
            try
            {

                SqlParameter[] param = new SqlParameter[17];
                param[0] = new SqlParameter("@TSPChangeRequestID", TSPChangeRequest.TSPChangeRequestID);
                param[1] = new SqlParameter("@TSPID", TSPChangeRequest.TSPID);
                param[2] = new SqlParameter("@TSPName", TSPChangeRequest.TSPName);
                param[3] = new SqlParameter("@Address", TSPChangeRequest.Address);
                param[4] = new SqlParameter("@HeadName", TSPChangeRequest.HeadName);
                param[5] = new SqlParameter("@HeadDesignation", TSPChangeRequest.HeadDesignation);
                param[6] = new SqlParameter("@HeadEmail", TSPChangeRequest.HeadEmail);
                param[7] = new SqlParameter("@HeadLandline", TSPChangeRequest.HeadLandline);
                param[8] = new SqlParameter("@CPName", TSPChangeRequest.CPName);
                param[9] = new SqlParameter("@CPDesignation", TSPChangeRequest.CPDesignation);
                param[10] = new SqlParameter("@CPEmail", TSPChangeRequest.CPEmail);
                param[11] = new SqlParameter("@CPLandline", TSPChangeRequest.CPLandline);
                //param[12] = new SqlParameter("@BankName", TSPChangeRequest.BankName);
                param[12] = new SqlParameter("@BankAccountNumber", TSPChangeRequest.BankAccountNumber);
                param[13] = new SqlParameter("@AccountTitle", TSPChangeRequest.AccountTitle);
                //param[15] = new SqlParameter("@BankBranch", TSPChangeRequest.BankBranch);
                param[14] = new SqlParameter("@IsApproved", TSPChangeRequest.IsApproved);
                param[15] = new SqlParameter("@IsRejected", TSPChangeRequest.IsRejected);

                param[16] = new SqlParameter("@CurUserID", TSPChangeRequest.CurUserID);
                SqlHelper.ExecuteNonQuery(SqlHelper.GetCon(), CommandType.StoredProcedure, "[AU_TSPChangeRequest]", param);
                //By Nasrullah
                var firstApproval = new SRVApproval().FetchApproval(new ApprovalModel() { Step = 1, ProcessKey = EnumApprovalProcess.CR_TSP }).FirstOrDefault();
                UsersModel KamUser = srvTSPMaster.KAMUserByTSPUserID(TSPChangeRequest.CurUserID);
                 ApprovalModel approvalsModelForNotificarion = new ApprovalModel();
                ApprovalHistoryModel model1 = new ApprovalHistoryModel();
                model1.ApprovalStatusID = (int)EnumApprovalStatus.Pending;
                approvalsModelForNotificarion.UserIDs = firstApproval.UserIDs;
                approvalsModelForNotificarion.UserIDs += "," + KamUser.UserID.ToString();
                approvalsModelForNotificarion.ProcessKey = EnumApprovalProcess.CR_TSP;
                approvalsModelForNotificarion.CustomComments = "generated from TSP (" + TSPChangeRequest.TSPName + ")";
                srvSendEmail.GenerateEmailAndSendNotification(approvalsModelForNotificarion, model1);
                return FetchTSPChangeRequest();
            }
            catch (Exception ex)
            { throw new Exception(ex.Message); }
        }
        private List<TSPChangeRequestModel> LoopinData(DataTable dt)
        {
            List<TSPChangeRequestModel> TSPChangeRequestL = new List<TSPChangeRequestModel>();

            foreach (DataRow r in dt.Rows)
            {
                TSPChangeRequestL.Add(RowOfTSPChangeRequest(r));

            }
            return TSPChangeRequestL;
        }
        public List<TSPChangeRequestModel> FetchTSPChangeRequest(TSPChangeRequestModel mod)
        {
            try
            {
                DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_TSPChangeRequest", Common.GetParams(mod)).Tables[0];
                return LoopinData(dt);
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }
        public List<TSPChangeRequestModel> FetchTSPChangeRequest()
        {
            try
            {
                DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_TSPChangeRequest").Tables[0];
                return LoopinData(dt);
            }
            catch (Exception ex) { throw new Exception(ex.Message); }

        }
        public List<TSPChangeRequestModel> FetchTSPChangeRequest(bool InActive)
        {
            try
            {
                DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_TSPChangeRequest", new SqlParameter("@InActive", InActive)).Tables[0];
                return LoopinData(dt);
            }
            catch (Exception ex) { throw new Exception(ex.Message); }

        }
        public List<TSPChangeRequestModel> GetByTSPID(int TSPID)
        {
            try
            {
                DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_TSPChangeRequest", new SqlParameter("@TSPID", TSPID)).Tables[0];
                return LoopinData(dt);
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }
        public bool CrTSPApproveReject(TSPChangeRequestModel model, SqlTransaction transaction = null)
        {
            try
            {
                List<SqlParameter> param = new List<SqlParameter>();
                param.Add(new SqlParameter("@TSPChangeRequestID", model.TSPChangeRequestID));
                param.Add(new SqlParameter("@IsApproved", model.IsApproved));
                param.Add(new SqlParameter("@IsRejected", model.IsRejected));
                param.Add(new SqlParameter("@CurUserID", model.CurUserID));

                if (transaction != null)
                {
                    SqlHelper.ExecuteScalar(transaction, CommandType.StoredProcedure, "U_Cr_TSPApproveReject", param.ToArray());
                }
                else
                {
                    SqlHelper.ExecuteScalar(SqlHelper.GetCon(), CommandType.StoredProcedure, "U_Cr_TSPApproveReject", param.ToArray());
                }
                return true;
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }
        public TSPChangeRequestModel GetByTSPChangeRequestID_Notification(int TSPChangeRequestID, SqlTransaction transaction = null)
        {
            try
            {
                SqlParameter param = new SqlParameter("@TSPChangeRequestID", TSPChangeRequestID);
                DataTable dt = new DataTable();
                List<TSPChangeRequestModel> TSPChangeRequestModel = new List<TSPChangeRequestModel>();
               // dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_TSPChangeRequest", param).Tables[0];
                
                if (transaction != null)
                {
                    dt = SqlHelper.ExecuteDataset(transaction, CommandType.StoredProcedure, "RD_TSPChangeRequest", param).Tables[0];
                }
                else
                {
                    dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_TSPChangeRequest", param).Tables[0];

                }
                if (dt.Rows.Count > 0)
                {
                    TSPChangeRequestModel = Helper.ConvertDataTableToModel<TSPChangeRequestModel>(dt);
                    return TSPChangeRequestModel[0];
                }
                else
                    return null;
            }
            catch (Exception ex) { throw new Exception(ex.Message); }

        }
        public void ActiveInActive(int TSPChangeRequestID, bool? InActive, int CurUserID)
        {

            SqlParameter[] PLead = new SqlParameter[3];
            PLead[0] = new SqlParameter("@TSPChangeRequestID", TSPChangeRequestID);
            PLead[1] = new SqlParameter("@InActive", InActive);
            PLead[2] = new SqlParameter("@CurUserID", CurUserID);
            SqlHelper.ExecuteNonQuery(SqlHelper.GetCon(), CommandType.StoredProcedure, "[AI_TSPChangeRequest]", PLead);
        }
        private TSPChangeRequestModel RowOfTSPChangeRequest(DataRow r)
        {
            TSPChangeRequestModel TSPChangeRequest = new TSPChangeRequestModel();
            TSPChangeRequest.TSPChangeRequestID = Convert.ToInt32(r["TSPChangeRequestID"]);
            TSPChangeRequest.TSPID = Convert.ToInt32(r["TSPID"]);
            TSPChangeRequest.TSPName = r["TSPName"].ToString();
            TSPChangeRequest.Address = r["Address"].ToString();
            TSPChangeRequest.HeadName = r["HeadName"].ToString();
            TSPChangeRequest.HeadDesignation = r["HeadDesignation"].ToString();
            TSPChangeRequest.HeadEmail = r["HeadEmail"].ToString();
            TSPChangeRequest.HeadLandline = r["HeadLandline"].ToString();
            TSPChangeRequest.CPName = r["CPName"].ToString();
            TSPChangeRequest.CPDesignation = r["CPDesignation"].ToString();
            TSPChangeRequest.CPEmail = r["CPEmail"].ToString();
            TSPChangeRequest.CPLandline = r["CPLandline"].ToString();
            //TSPChangeRequest.BankName = r["BankName"].ToString();
            TSPChangeRequest.BankAccountNumber = r["BankAccountNumber"].ToString();
            TSPChangeRequest.AccountTitle = r["AccountTitle"].ToString();
            //TSPChangeRequest.BankBranch = r["BankBranch"].ToString();
            TSPChangeRequest.IsApproved = Convert.ToBoolean(r["IsApproved"]);
            TSPChangeRequest.IsRejected = Convert.ToBoolean(r["IsRejected"]);
            TSPChangeRequest.InActive = Convert.ToBoolean(r["InActive"]);
            TSPChangeRequest.CreatedUserID = Convert.ToInt32(r["CreatedUserID"]);
            TSPChangeRequest.ModifiedUserID = Convert.ToInt32(r["ModifiedUserID"]);
            TSPChangeRequest.CreatedDate = r["CreatedDate"].ToString().GetDate();
            TSPChangeRequest.ModifiedDate = r["ModifiedDate"].ToString().GetDate();
            TSPChangeRequest.SAPID = r["SAPID"].ToString();

            return TSPChangeRequest;
        }
    }
}
