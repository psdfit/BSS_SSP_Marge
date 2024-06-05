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
    public class SRVInceptionReportChangeRequest : SRVBase, ISRVInceptionReportChangeRequest
    {

        private readonly ISRVSendEmail srvSendEmail;
        private readonly ISRVTSPMaster srvTSPMaster;
        private readonly ISRVUsers srvUsers;

        public SRVInceptionReportChangeRequest(ISRVSendEmail srvSendEmail, ISRVTSPMaster srvTSPMaster, ISRVUsers srvUsers)
        {
            this.srvSendEmail = srvSendEmail;
            this.srvTSPMaster = srvTSPMaster;
            this.srvUsers = srvUsers;
        }
        public InceptionReportChangeRequestModel GetByInceptionReportChangeRequestID(int InceptionReportChangeRequestID, SqlTransaction transaction = null)
        {
            try
            {
                SqlParameter param = new SqlParameter("@InceptionReportChangeRequestID", InceptionReportChangeRequestID);
                DataTable dt = new DataTable();


                if (transaction != null)
                {
                    dt = SqlHelper.ExecuteDataset(transaction, CommandType.StoredProcedure, "RD_InceptionReportChangeRequest", param).Tables[0];
                }
                else
                {
                    dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_InceptionReportChangeRequest", param).Tables[0];

                }


                if (dt.Rows.Count > 0)
                {
                    return RowOfInceptionReportChangeRequest(dt.Rows[0]);
                }
                else
                    return null;
            }
            catch (Exception ex) { throw new Exception(ex.Message); }

        }
        public List<InceptionReportChangeRequestModel> SaveInceptionReportChangeRequest(InceptionReportChangeRequestModel InceptionReportChangeRequest)
        {
            try
            {

                SqlParameter[] param = new SqlParameter[15];
                param[0] = new SqlParameter("@InceptionReportChangeRequestID", InceptionReportChangeRequest.InceptionReportChangeRequestID);
                param[1] = new SqlParameter("@IncepReportID", InceptionReportChangeRequest.IncepReportID);
                param[2] = new SqlParameter("@ClassID", InceptionReportChangeRequest.ClassID);
                param[3] = new SqlParameter("@ClassStartTime", InceptionReportChangeRequest.ClassStartTime);
                param[4] = new SqlParameter("@ClassEndTime", InceptionReportChangeRequest.ClassEndTime);
                param[5] = new SqlParameter("@ClassTotalHours", InceptionReportChangeRequest.ClassTotalHours);
                param[6] = new SqlParameter("@Shift", InceptionReportChangeRequest.Shift);
                param[7] = new SqlParameter("@Monday", InceptionReportChangeRequest.Monday);
                param[8] = new SqlParameter("@Tuesday", InceptionReportChangeRequest.Tuesday);
                param[9] = new SqlParameter("@Wednesday", InceptionReportChangeRequest.Wednesday);
                param[10] = new SqlParameter("@Thursday", InceptionReportChangeRequest.Thursday);
                param[11] = new SqlParameter("@Friday", InceptionReportChangeRequest.Friday);
                param[12] = new SqlParameter("@Saturday", InceptionReportChangeRequest.Saturday);
                param[13] = new SqlParameter("@Sunday", InceptionReportChangeRequest.Sunday);
                //param[14] = new SqlParameter("@InstrIDs", InceptionReportChangeRequest.InstrIDs);

                param[14] = new SqlParameter("@CurUserID", InceptionReportChangeRequest.CurUserID);
                SqlHelper.ExecuteNonQuery(SqlHelper.GetCon(), CommandType.StoredProcedure, "[AU_InceptionReportChangeRequest]", param);
                //By Nasrullah
               // var firstApproval = new SRVApproval().FetchApproval(new ApprovalModel() { Step = 1, ProcessKey = EnumApprovalProcess.CR_INCEPTION }).FirstOrDefault();
                UsersModel KamUser = srvTSPMaster.KAMUserByTSPUserID(InceptionReportChangeRequest.CurUserID);
                UsersModel usersModel = srvUsers.GetByUserID(InceptionReportChangeRequest.CurUserID);
                ApprovalModel approvalsModelForNotificarion = new ApprovalModel();
                ApprovalHistoryModel model1 = new ApprovalHistoryModel();
                model1.ApprovalStatusID = (int)EnumApprovalStatus.Pending;
                approvalsModelForNotificarion.UserIDs = KamUser.UserID.ToString();
                approvalsModelForNotificarion.ProcessKey = EnumApprovalProcess.CR_INCEPTION;
                approvalsModelForNotificarion.CustomComments = "generated from TSP (" + usersModel.FullName + ")";
                srvSendEmail.GenerateEmailAndSendNotification(approvalsModelForNotificarion, model1);

                return FetchInceptionReportChangeRequest();
            }
            catch (Exception ex)
            { throw new Exception(ex.Message); }
        }
        private List<InceptionReportChangeRequestModel> LoopinData(DataTable dt)
        {
            List<InceptionReportChangeRequestModel> InceptionReportChangeRequestL = new List<InceptionReportChangeRequestModel>();

            foreach (DataRow r in dt.Rows)
            {
                InceptionReportChangeRequestL.Add(RowOfInceptionReportChangeRequest(r));

            }
            return InceptionReportChangeRequestL;
        }
        public List<InceptionReportChangeRequestModel> FetchInceptionReportChangeRequest(InceptionReportChangeRequestModel mod)
        {
            try
            {
                List<SqlParameter> param = new List<SqlParameter>();

                param.Add(new SqlParameter("@Process_Key", mod.Process_Key));
                param.Add(new SqlParameter("@SchemeID", mod.SchemeID));
                param.Add(new SqlParameter("@TSPID", mod.TSPID));
                param.Add(new SqlParameter("@ClassID", mod.ClassID));
                param.Add(new SqlParameter("@KAMID", mod.KamID));
                DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_InceptionReportChangeRequest", param.ToArray()).Tables[0];
                return LoopinData(dt);
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }
        public List<InceptionReportChangeRequestModel> FetchInceptionReportChangeRequest()
        {
            try
            {
                DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_InceptionReportChangeRequest").Tables[0];
                return LoopinData(dt);
            }
            catch (Exception ex) { throw new Exception(ex.Message); }

        }
        public List<InceptionReportChangeRequestModel> FetchInceptionReportChangeRequest(bool InActive)
        {
            try
            {
                DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_InceptionReportChangeRequest", new SqlParameter("@InActive", InActive)).Tables[0];
                return LoopinData(dt);
            }
            catch (Exception ex) { throw new Exception(ex.Message); }

        }
        public List<InceptionReportChangeRequestModel> GetByClassID(int ClassID)
        {
            try
            {
                DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_InceptionReportChangeRequest", new SqlParameter("@ClassID", ClassID)).Tables[0];
                return LoopinData(dt);
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }

        public bool CrInceptionReportApproveReject(InceptionReportChangeRequestModel model, SqlTransaction transaction = null)
        {
            try
            {
                List<SqlParameter> param = new List<SqlParameter>();
                param.Add(new SqlParameter("@InceptionReportChangeRequestID", model.InceptionReportChangeRequestID));
                param.Add(new SqlParameter("@IsApproved", model.IsApproved));
                param.Add(new SqlParameter("@IsRejected", model.IsRejected));
                param.Add(new SqlParameter("@CurUserID", model.CurUserID));

                if (transaction != null)
                {
                    SqlHelper.ExecuteScalar(transaction, CommandType.StoredProcedure, "U_Cr_InceptionReportApproveReject", param.ToArray());
                }
                else
                {
                    SqlHelper.ExecuteScalar(SqlHelper.GetCon(), CommandType.StoredProcedure, "U_Cr_InceptionReportApproveReject", param.ToArray());
                }
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public void ActiveInActive(int InceptionReportChangeRequestID, bool? InActive, int CurUserID)
        {

            SqlParameter[] PLead = new SqlParameter[3];
            PLead[0] = new SqlParameter("@InceptionReportChangeRequestID", InceptionReportChangeRequestID);
            PLead[1] = new SqlParameter("@InActive", InActive);
            PLead[2] = new SqlParameter("@CurUserID", CurUserID);
            SqlHelper.ExecuteNonQuery(SqlHelper.GetCon(), CommandType.StoredProcedure, "[AI_InceptionReportChangeRequest]", PLead);
        }
        private InceptionReportChangeRequestModel RowOfInceptionReportChangeRequest(DataRow r)
        {
            InceptionReportChangeRequestModel InceptionReportChangeRequest = new InceptionReportChangeRequestModel();
            InceptionReportChangeRequest.InceptionReportChangeRequestID = Convert.ToInt32(r["InceptionReportChangeRequestID"]);
            InceptionReportChangeRequest.IncepReportID = Convert.ToInt32(r["IncepReportID"]);
            InceptionReportChangeRequest.ClassID = Convert.ToInt32(r["ClassID"]);
            InceptionReportChangeRequest.ClassCode = r["ClassCode"].ToString();
            InceptionReportChangeRequest.SchemeName = r["SchemeName"].ToString();
            InceptionReportChangeRequest.TSPName = r["TSPName"].ToString();
            InceptionReportChangeRequest.ClassStartTime = r["ClassStartTime"].ToString().GetDate();
            InceptionReportChangeRequest.ClassEndTime = r["ClassEndTime"].ToString().GetDate();
            InceptionReportChangeRequest.ClassTotalHours = r["ClassTotalHours"].ToString();
            InceptionReportChangeRequest.Shift = r["Shift"].ToString();
            InceptionReportChangeRequest.Monday = Convert.ToBoolean(r["Monday"]);
            InceptionReportChangeRequest.Tuesday = Convert.ToBoolean(r["Tuesday"]);
            InceptionReportChangeRequest.Wednesday = Convert.ToBoolean(r["Wednesday"]);
            InceptionReportChangeRequest.Thursday = Convert.ToBoolean(r["Thursday"]);
            InceptionReportChangeRequest.Friday = Convert.ToBoolean(r["Friday"]);
            InceptionReportChangeRequest.Saturday = Convert.ToBoolean(r["Saturday"]);
            InceptionReportChangeRequest.Sunday = Convert.ToBoolean(r["Sunday"]);
            //InceptionReportChangeRequest.InstrIDs = r["InstrIDs"].ToString();
            InceptionReportChangeRequest.InActive = Convert.ToBoolean(r["InActive"]);
            InceptionReportChangeRequest.CreatedUserID = Convert.ToInt32(r["CreatedUserID"]);
            InceptionReportChangeRequest.ModifiedUserID = Convert.ToInt32(r["ModifiedUserID"]);
            InceptionReportChangeRequest.CreatedDate = r["CreatedDate"].ToString().GetDate();
            InceptionReportChangeRequest.ModifiedDate = r["ModifiedDate"].ToString().GetDate();
            InceptionReportChangeRequest.IsApproved = Convert.ToBoolean(r["IsApproved"]);
            InceptionReportChangeRequest.IsRejected = Convert.ToBoolean(r["IsRejected"]);

            return InceptionReportChangeRequest;
        }
    }
}
