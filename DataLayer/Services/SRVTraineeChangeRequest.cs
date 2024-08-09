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
    public class SRVTraineeChangeRequest : SRVBase, ISRVTraineeChangeRequest
    {
        private readonly ISRVSendEmail srvSendEmail;
        private readonly ISRVTSPMaster srvTSPMaster;
        private readonly ISRVUsers srvUsers;

        public SRVTraineeChangeRequest(ISRVSendEmail srvSendEmail, ISRVTSPMaster srvTSPMaster, ISRVUsers srvUsers)
        {
            this.srvSendEmail = srvSendEmail;
            this.srvTSPMaster = srvTSPMaster;
            this.srvUsers = srvUsers;
        }
        public TraineeChangeRequestModel GetByTraineeChangeRequestID(int TraineeChangeRequestID, SqlTransaction transaction = null)
        {
            try
            {
                SqlParameter param = new SqlParameter("@TraineeChangeRequestID", TraineeChangeRequestID);
                DataTable dt = new DataTable();

                if (transaction != null)
                {
                    dt = SqlHelper.ExecuteDataset(transaction, CommandType.StoredProcedure, "RD_TraineeChangeRequest", param).Tables[0];
                }
                else
                {
                    dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_TraineeChangeRequest", param).Tables[0];

                }


                if (dt.Rows.Count > 0)
                {
                    return RowOfTraineeChangeRequest(dt.Rows[0]);
                }
                else
                    return null;
            }
            catch (Exception ex) { throw new Exception(ex.Message); }

        }
        public List<TraineeChangeRequestModel> SaveTraineeChangeRequest(TraineeChangeRequestModel TraineeChangeRequest)
        {
            try
            {

                SqlParameter[] param = new SqlParameter[15];
                param[0] = new SqlParameter("@TraineeChangeRequestID", TraineeChangeRequest.TraineeChangeRequestID);
                param[1] = new SqlParameter("@TraineeID", TraineeChangeRequest.TraineeID);
                param[2] = new SqlParameter("@TraineeName", TraineeChangeRequest.TraineeName);
                param[3] = new SqlParameter("@TraineeCNIC", TraineeChangeRequest.TraineeCNIC);
                param[4] = new SqlParameter("@FatherName", TraineeChangeRequest.FatherName);
                param[5]= new SqlParameter("@CNICIssueDate", TraineeChangeRequest.CNICIssueDate);
                param[6]= new SqlParameter("@ContactNumber1", TraineeChangeRequest.ContactNumber1);
                param[7]= new SqlParameter("@TraineeHouseNumber", TraineeChangeRequest.TraineeHouseNumber);
                param[8]= new SqlParameter("@TraineeStreetMohalla", TraineeChangeRequest.TraineeStreetMohalla);
                param[9]= new SqlParameter("@TraineeMauzaTown", TraineeChangeRequest.TraineeMauzaTown);
                param[10]= new SqlParameter("@TraineeDistrictID", TraineeChangeRequest.TraineeDistrictID);
                param[11]= new SqlParameter("@TraineeTehsilID", TraineeChangeRequest.TraineeTehsilID);
                //param[5] = new SqlParameter("@DateOfBirth", TraineeChangeRequest.DateOfBirth);
                param[12] = new SqlParameter("@TraineeEmail", TraineeChangeRequest.TraineeEmail);
                param[13] = new SqlParameter("@CurUserID", TraineeChangeRequest.CurUserID);
                param[14] = new SqlParameter("@TraineechangeImage", TraineeChangeRequest.TraineechangeImage);

                SqlHelper.ExecuteNonQuery(SqlHelper.GetCon(), CommandType.StoredProcedure, "[AU_TraineeChangeRequest]", param);

                //By Nasrullah
               // var firstApproval = new SRVApproval().FetchApproval(new ApprovalModel() { Step = 1, ProcessKey = EnumApprovalProcess.CR_TRAINEE_UNVERIFIED }).FirstOrDefault();
                UsersModel KamUser = srvTSPMaster.KAMUserByTSPUserID(TraineeChangeRequest.CurUserID);
                UsersModel usersModel = srvUsers.GetByUserID(TraineeChangeRequest.CurUserID);
                ApprovalModel approvalsModelForNotificarion = new ApprovalModel();
                ApprovalHistoryModel model1 = new ApprovalHistoryModel();
                model1.ApprovalStatusID = (int)EnumApprovalStatus.Pending;
                approvalsModelForNotificarion.UserIDs = KamUser.UserID.ToString();
                approvalsModelForNotificarion.ProcessKey = EnumApprovalProcess.CR_TRAINEE_UNVERIFIED;
                approvalsModelForNotificarion.CustomComments = "generated from TSP (" + usersModel.FullName + ")";
                srvSendEmail.GenerateEmailAndSendNotification(approvalsModelForNotificarion, model1);

                return FetchTraineeChangeRequest();
            }
            catch (Exception ex)
            { throw new Exception(ex.Message); }
        }

        public TraineeChangeRequestModel GetByTraineeChangeRequestID_Notification(int TraineeChangeRequestID, SqlTransaction transaction = null)
        {
            try
            {
                SqlParameter param = new SqlParameter("@TraineeChangeRequestID", TraineeChangeRequestID);
                DataTable dt = new DataTable();
                List<TraineeChangeRequestModel> TraineeChangeRequestModel = new List<TraineeChangeRequestModel>();
               // dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "[RD_Trainee_Unverified_Notification]", param).Tables[0];


                if (transaction != null)
                {
                    dt = SqlHelper.ExecuteDataset(transaction, CommandType.StoredProcedure, "RD_Trainee_Unverified_Notification", param).Tables[0];
                }
                else
                {
                    dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_Trainee_Unverified_Notification", param).Tables[0];

                }

                if (dt.Rows.Count > 0)
                {
                    TraineeChangeRequestModel = Helper.ConvertDataTableToModel<TraineeChangeRequestModel>(dt);

                    return TraineeChangeRequestModel[0];
                }
                else
                    return null;
            }
            catch (Exception ex) { throw new Exception(ex.Message); }

        }


        public List<TraineeChangeRequestModel> SaveVerifiedTraineeChangeRequest(TraineeChangeRequestModel TraineeChangeRequest)
        {
            try
            {

                SqlParameter[] param = new SqlParameter[15];
                param[0] = new SqlParameter("@TraineeChangeRequestID", TraineeChangeRequest.TraineeChangeRequestID);
                param[1] = new SqlParameter("@TraineeID", TraineeChangeRequest.TraineeID);
                param[2] = new SqlParameter("@TraineeName", TraineeChangeRequest.TraineeName);
                param[3] = new SqlParameter("@TraineeCNIC", TraineeChangeRequest.TraineeCNIC);
                param[4] = new SqlParameter("@FatherName", TraineeChangeRequest.FatherName);
                param[5]= new SqlParameter("@CNICIssueDate", TraineeChangeRequest.CNICIssueDate);
                param[6]= new SqlParameter("@ContactNumber1", TraineeChangeRequest.ContactNumber1);
                param[7]= new SqlParameter("@TraineeHouseNumber", TraineeChangeRequest.TraineeHouseNumber);
                param[8]= new SqlParameter("@TraineeStreetMohalla", TraineeChangeRequest.TraineeStreetMohalla);
                param[9]= new SqlParameter("@TraineeMauzaTown", TraineeChangeRequest.TraineeMauzaTown);
                param[10]= new SqlParameter("@TraineeDistrictID", TraineeChangeRequest.TraineeDistrictID);
                param[11]= new SqlParameter("@TraineeTehsilID", TraineeChangeRequest.TraineeTehsilID);
                param[12] = new SqlParameter("@TraineeEmail", TraineeChangeRequest.TraineeEmail);

                param[13] = new SqlParameter("@CurUserID", TraineeChangeRequest.CurUserID);
                param[14] = new SqlParameter("@TraineechangeImage", TraineeChangeRequest.TraineechangeImage);
                SqlHelper.ExecuteNonQuery(SqlHelper.GetCon(), CommandType.StoredProcedure, "[AU_Verified_TraineeChangeRequest]", param);

                //By Nasrullah
               // var firstApproval = new SRVApproval().FetchApproval(new ApprovalModel() { Step = 1, ProcessKey = EnumApprovalProcess.CR_TRAINEE_VERIFIED }).FirstOrDefault();
                DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "Get_KAMUserByTSPUserID", new SqlParameter("@UserID", TraineeChangeRequest.CurUserID)).Tables[0];
                List<UsersModel> KAMUser = Helper.ConvertDataTableToModel<UsersModel>(dt);
                UsersModel usersModel = srvUsers.GetByUserID(TraineeChangeRequest.CurUserID);
                ApprovalModel approvalsModelForNotificarion = new ApprovalModel();
                ApprovalHistoryModel model1 = new ApprovalHistoryModel();
                model1.ApprovalStatusID = (int)EnumApprovalStatus.Pending;
                approvalsModelForNotificarion.UserIDs = KAMUser[0].UserID.ToString();
                approvalsModelForNotificarion.ProcessKey = EnumApprovalProcess.CR_TRAINEE_VERIFIED;
                approvalsModelForNotificarion.CustomComments = "generated from TSP (" + usersModel.FullName + ")";
                srvSendEmail.GenerateEmailAndSendNotification(approvalsModelForNotificarion, model1);

                return FetchTraineeChangeRequest();
            }
            catch (Exception ex)
            { throw new Exception(ex.Message); }
        }
        private List<TraineeChangeRequestModel> LoopinData(DataTable dt)
        {
            List<TraineeChangeRequestModel> TraineeChangeRequestL = new List<TraineeChangeRequestModel>();

            foreach (DataRow r in dt.Rows)
            {
                TraineeChangeRequestL.Add(RowOfTraineeChangeRequest(r));

            }
            return TraineeChangeRequestL;
        }
        public List<TraineeChangeRequestModel> FetchTraineeChangeRequest(TraineeChangeRequestModel mod)
        {
            try
            {
                List<SqlParameter> param = new List<SqlParameter>();
                param.Add(new SqlParameter("@Process_Key", mod.Process_Key));
                param.Add(new SqlParameter("@SchemeID", mod.SchemeID));
                param.Add(new SqlParameter("@TSPID", mod.TSPID));
                param.Add(new SqlParameter("@ClassID", mod.ClassID));
                DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_TraineeChangeRequest",param.ToArray()).Tables[0];
                return LoopinData(dt);
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }
        public List<TraineeChangeRequestModel> FetchTraineeChangeRequest()
        {
            try
            {
                DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_TraineeChangeRequest").Tables[0];
                return LoopinData(dt);
            }
            catch (Exception ex) { throw new Exception(ex.Message); }

        }
        public List<TraineeChangeRequestModel> FetchTraineeChangeRequest(bool InActive)
        {
            try
            {
                DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_TraineeChangeRequest", new SqlParameter("@InActive", InActive)).Tables[0];
                return LoopinData(dt);
            }
            catch (Exception ex) { throw new Exception(ex.Message); }

        }
        public List<TraineeChangeRequestModel> GetByTraineeID(int TraineeID)
        {
            try
            {
                DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_TraineeChangeRequest", new SqlParameter("@TraineeID", TraineeID)).Tables[0];
                return LoopinData(dt);
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }

        public bool CrTraineeApproveReject(TraineeChangeRequestModel model, SqlTransaction transaction = null)
        {
            try
            {
                List<SqlParameter> param = new List<SqlParameter>();
                param.Add(new SqlParameter("@TraineeChangeRequestID", model.TraineeChangeRequestID));
                param.Add(new SqlParameter("@IsApproved", model.IsApproved));
                param.Add(new SqlParameter("@IsRejected", model.IsRejected));
                param.Add(new SqlParameter("@CurUserID", model.CurUserID));

                if (transaction != null)
                {
                    SqlHelper.ExecuteScalar(transaction, CommandType.StoredProcedure, "U_Cr_TraineeApproveReject", param.ToArray());
                }
                else
                {
                    SqlHelper.ExecuteScalar(SqlHelper.GetCon(), CommandType.StoredProcedure, "U_Cr_TraineeApproveReject", param.ToArray());
                }
                return true;
            }
            catch (Exception ex) { throw new Exception(ex.Message);
            }
        }

        public void ActiveInActive(int TraineeChangeRequestID, bool? InActive, int CurUserID)
        {

            SqlParameter[] PLead = new SqlParameter[3];
            PLead[0] = new SqlParameter("@TraineeChangeRequestID", TraineeChangeRequestID);
            PLead[1] = new SqlParameter("@InActive", InActive);
            PLead[2] = new SqlParameter("@CurUserID", CurUserID);
            SqlHelper.ExecuteNonQuery(SqlHelper.GetCon(), CommandType.StoredProcedure, "[AI_TraineeChangeRequest]", PLead);
        }
        private TraineeChangeRequestModel RowOfTraineeChangeRequest(DataRow r)
        {
            TraineeChangeRequestModel TraineeChangeRequest = new TraineeChangeRequestModel();
            TraineeChangeRequest.TraineeChangeRequestID = Convert.ToInt32(r["TraineeChangeRequestID"]);
            TraineeChangeRequest.TraineeID = Convert.ToInt32(r["TraineeID"]);
            TraineeChangeRequest.TraineeName = r["TraineeName"].ToString();
            if (r.Table.Columns.Contains("TraineeCode"))
                TraineeChangeRequest.TraineeCode = r["TraineeCode"].ToString();
            TraineeChangeRequest.TraineeCNIC = r["TraineeCNIC"].ToString();
            TraineeChangeRequest.TraineeEmail = r["TraineeEmail"].ToString();            
            TraineeChangeRequest.FatherName = r["FatherName"].ToString();
            TraineeChangeRequest.CNICIssueDate = r["CNICIssueDate"].ToString().GetDate();
            TraineeChangeRequest.ContactNumber1 = r["ContactNumber1"].ToString();
            TraineeChangeRequest.TraineeHouseNumber = r["TraineeHouseNumber"].ToString();
            TraineeChangeRequest.TraineeStreetMohalla = r["TraineeStreetMohalla"].ToString();
            TraineeChangeRequest.TraineeMauzaTown = r["TraineeMauzaTown"].ToString();
            TraineeChangeRequest.TraineeDistrictID = Convert.ToInt32(r["TraineeDistrictID"]);
            if (r.Table.Columns.Contains("DistrictName"))
            {
                TraineeChangeRequest.DistrictName = r["DistrictName"].ToString();
            }
            TraineeChangeRequest.TraineeTehsilID = Convert.ToInt32(r["TraineeTehsilID"]);
            if (r.Table.Columns.Contains("TehsilName"))
            {
                TraineeChangeRequest.TehsilName = r["TehsilName"].ToString();
            }

            if (r.Table.Columns.Contains("TraineePicturePath"))
            {
                TraineeChangeRequest.TraineechangeImage = string.IsNullOrEmpty(r["TraineePicturePath"].ToString()) ? string.Empty : Common.GetFileBase64(r["TraineePicturePath"].ToString());

                //TraineeChangeRequest.TraineechangeImage = r["TraineePicturePath"].ToString();
            }

            TraineeChangeRequest.InActive = Convert.ToBoolean(r["InActive"]);
            TraineeChangeRequest.CreatedUserID = Convert.ToInt32(r["CreatedUserID"]);
            TraineeChangeRequest.ModifiedUserID = Convert.ToInt32(r["ModifiedUserID"]);
            TraineeChangeRequest.CreatedDate = r["CreatedDate"].ToString().GetDate();
            TraineeChangeRequest.ModifiedDate = r["ModifiedDate"].ToString().GetDate();
            TraineeChangeRequest.IsApproved = Convert.ToBoolean(r["IsApproved"]);
            TraineeChangeRequest.IsRejected = Convert.ToBoolean(r["IsRejected"]);
            if (r.Table.Columns.Contains("SchemeName"))
                TraineeChangeRequest.SchemeName = r["SchemeName"].ToString();
            if (r.Table.Columns.Contains("TSPName"))
                TraineeChangeRequest.TSPName = r["TSPName"].ToString();
            if (r.Table.Columns.Contains("TradeName"))
                TraineeChangeRequest.TradeName = r["TradeName"].ToString();
            if (r.Table.Columns.Contains("ClassCode"))
                TraineeChangeRequest.ClassCode = r["ClassCode"].ToString();
            if (r.Table.Columns.Contains("DateOfBirth"))
                TraineeChangeRequest.DateOfBirth = r["DateOfBirth"].ToString().GetDate();
            if (r.Table.Columns.Contains("GenderName"))
                TraineeChangeRequest.GenderName = r["GenderName"].ToString();
            if (r.Table.Columns.Contains("CNICVerified"))
                TraineeChangeRequest.CNICVerified = r["CNICVerified"].ToString();
            if (r.Table.Columns.Contains("TraineeStatus"))
                TraineeChangeRequest.TraineeStatus = r["TraineeStatus"].ToString();
            if (r.Table.Columns.Contains("DistrictVerified"))
                TraineeChangeRequest.DistrictVerified = r["DistrictVerified"].ToString();
            if (r.Table.Columns.Contains("CNICUnVerifiedReason"))
                TraineeChangeRequest.CNICUnVerifiedReason = r["CNICUnVerifiedReason"].ToString();
            if (r.Table.Columns.Contains("BSSStatus"))
                TraineeChangeRequest.BSSStatus = r["BSSStatus"].ToString();
            if (r.Table.Columns.Contains("MEStatus"))
                TraineeChangeRequest.MEStatus = r["MEStatus"].ToString();
            if (r.Table.Columns.Contains("Religion"))
                TraineeChangeRequest.Religion = r["Religion"].ToString();
            if (r.Table.Columns.Contains("PermanentAddress"))
                TraineeChangeRequest.PermanentAddress = r["PermanentAddress"].ToString();
            if (r.Table.Columns.Contains("PermanentDistrict"))
                TraineeChangeRequest.PermanentDistrict = r["PermanentDistrict"].ToString();
            if (r.Table.Columns.Contains("PermanentTehsil"))
                TraineeChangeRequest.PermanentTehsil = r["PermanentTehsil"].ToString();
            //newly added columns by Umair Nadeem, date: 9 August 2024
            if (r.Table.Columns.Contains("ClassStartDate"))
                TraineeChangeRequest.ClassStartDate = r["ClassStartDate"].ToString();
            if (r.Table.Columns.Contains("ClassEndDate"))
                TraineeChangeRequest.ClassEndDate = r["ClassEndDate"].ToString();
            if (r.Table.Columns.Contains("SchemeType"))
                TraineeChangeRequest.SchemeType = r["SchemeType"].ToString();
            if (r.Table.Columns.Contains("SchemeType"))
                TraineeChangeRequest.KAMName = r["KAMName"].ToString();
            if (r.Table.Columns.Contains("FundingSourceName"))
                TraineeChangeRequest.ProjectName = r["FundingSourceName"].ToString();

            return TraineeChangeRequest;
        }
    }
}
