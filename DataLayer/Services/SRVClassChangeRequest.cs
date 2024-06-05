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
    public class SRVClassChangeRequest : SRVBase, ISRVClassChangeRequest
    {
       
        private readonly ISRVSendEmail srvSendEmail;
        private readonly ISRVTSPMaster srvTSPMaster;
        private readonly ISRVUsers srvUsers;

        public SRVClassChangeRequest(ISRVSendEmail srvSendEmail, ISRVTSPMaster srvTSPMaster, ISRVUsers srvUsers)
        {
            this.srvSendEmail = srvSendEmail;
            this.srvTSPMaster = srvTSPMaster;
            this.srvUsers = srvUsers;
        }
        public ClassChangeRequestModel GetByClassChangeRequestID(int ClassChangeRequestID, SqlTransaction transaction = null)
        {
            try
            {
                SqlParameter param = new SqlParameter("@ClassChangeRequestID", ClassChangeRequestID);
                DataTable dt = new DataTable();

                if (transaction != null)
                {
                    dt = SqlHelper.ExecuteDataset(transaction, CommandType.StoredProcedure, "RD_ClassChangeRequest", param).Tables[0];
                }
                else
                {
                    dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_ClassChangeRequest", param).Tables[0];

                }

                if (dt.Rows.Count > 0)
                {
                    return RowOfClassChangeRequest(dt.Rows[0]);
                }
                else
                    return null;
            }
            catch (Exception ex) { throw new Exception(ex.Message); }

        }
        public List<ClassChangeRequestModel> SaveClassChangeRequest(ClassChangeRequestModel ClassChangeRequest)
        {
            try
            {

                SqlParameter[] param = new SqlParameter[9];
                param[0] = new SqlParameter("@ClassChangeRequestID", ClassChangeRequest.ClassChangeRequestID);
                param[1] = new SqlParameter("@ClassID", ClassChangeRequest.ClassID);
                param[2] = new SqlParameter("@ClassCode", ClassChangeRequest.ClassCode);
                param[3] = new SqlParameter("@TrainingAddressLocation", ClassChangeRequest.TrainingAddressLocation);
                param[4] = new SqlParameter("@DistrictID", ClassChangeRequest.DistrictID);
                param[5] = new SqlParameter("@TehsilID", ClassChangeRequest.TehsilID);
                //param[8] = new SqlParameter("@StartDate", ClassChangeRequest.StartDate);
                //param[9] = new SqlParameter("@EndDate", ClassChangeRequest.EndDate);

                param[6] = new SqlParameter("@CurUserID", ClassChangeRequest.CurUserID);
                SqlHelper.ExecuteNonQuery(SqlHelper.GetCon(), CommandType.StoredProcedure, "[AU_ClassChangeRequest]", param);

                //By Nasrullah // send notification to KAM
              //  var firstApproval = new SRVApproval().FetchApproval(new ApprovalModel() { Step = 1, ProcessKey = EnumApprovalProcess.CR_CLASS_LOCATION }).FirstOrDefault();
                UsersModel KamUser = srvTSPMaster.KAMUserByTSPUserID(ClassChangeRequest.CurUserID);
                UsersModel usersModel = srvUsers.GetByUserID(ClassChangeRequest.CurUserID);
                ApprovalModel approvalsModelForNotificarion = new ApprovalModel();
                ApprovalHistoryModel model1 = new ApprovalHistoryModel();
                model1.ApprovalStatusID = (int)EnumApprovalStatus.Pending;
                approvalsModelForNotificarion.UserIDs = KamUser.UserID.ToString();
                approvalsModelForNotificarion.ProcessKey = EnumApprovalProcess.CR_CLASS_LOCATION;
                approvalsModelForNotificarion.CustomComments = "generated from TSP (" + usersModel.FullName + ")";
                srvSendEmail.GenerateEmailAndSendNotification(approvalsModelForNotificarion, model1);

                return FetchClassChangeRequest();
            }
            catch (Exception ex)
            { throw new Exception(ex.Message); }
        }

        public List<ClassChangeRequestModel> SaveClassDatesChangeRequest(ClassChangeRequestModel ClassChangeRequest)
        {
            try
            {

                SqlParameter[] param = new SqlParameter[7];
                param[0] = new SqlParameter("@ClassDatesChangeRequestID", ClassChangeRequest.ClassChangeRequestID);
                param[1] = new SqlParameter("@ClassID", ClassChangeRequest.ClassID);
                param[2] = new SqlParameter("@ClassCode", ClassChangeRequest.ClassCode);
                param[3] = new SqlParameter("@StartDate", ClassChangeRequest.StartDate);
                param[4] = new SqlParameter("@EndDate", ClassChangeRequest.EndDate);

                param[5] = new SqlParameter("@CurUserID", ClassChangeRequest.CurUserID);
                SqlHelper.ExecuteNonQuery(SqlHelper.GetCon(), CommandType.StoredProcedure, "[AU_ClassDatesChangeRequest]", param);


                //By Nasrullah // send notification to KAM
                var firstApproval = new SRVApproval().FetchApproval(new ApprovalModel() { Step = 1, ProcessKey = EnumApprovalProcess.CR_CLASS_DATES }).FirstOrDefault();
                UsersModel KamUser = srvTSPMaster.KAMUserByTSPUserID(ClassChangeRequest.CurUserID);
                UsersModel usersModel = srvUsers.GetByUserID(ClassChangeRequest.CurUserID);
                ApprovalModel approvalsModelForNotificarion = new ApprovalModel();
                ApprovalHistoryModel model1 = new ApprovalHistoryModel();
                model1.ApprovalStatusID = (int)EnumApprovalStatus.Pending;
                approvalsModelForNotificarion.UserIDs = firstApproval.UserIDs;
                approvalsModelForNotificarion.UserIDs += "," + KamUser.UserID.ToString();
                approvalsModelForNotificarion.ProcessKey = EnumApprovalProcess.CR_CLASS_DATES;
                approvalsModelForNotificarion.CustomComments = "generated from TSP (" + usersModel.FullName + ")";
                srvSendEmail.GenerateEmailAndSendNotification(approvalsModelForNotificarion, model1);
                return FetchClassChangeRequest();
            }
            catch (Exception ex)
            { throw new Exception(ex.Message); }
        }
        private List<ClassChangeRequestModel> LoopinData(DataTable dt)
        {
            List<ClassChangeRequestModel> ClassChangeRequestL = new List<ClassChangeRequestModel>();

            foreach (DataRow r in dt.Rows)
            {
                ClassChangeRequestL.Add(RowOfClassChangeRequest(r));

            }
            return ClassChangeRequestL;
        }
        private List<ClassChangeRequestModel> LoopinClassForLocationChange(DataTable dt)
        {
            List<ClassChangeRequestModel> ClassChangeRequestL = new List<ClassChangeRequestModel>();

            foreach (DataRow r in dt.Rows)
            {
                ClassChangeRequestL.Add(RowOfClassForLocationChange(r));

            }
            return ClassChangeRequestL;
        }
        private List<ClassChangeRequestModel> LoopinClassForDatesChange(DataTable dt)
        {
            List<ClassChangeRequestModel> ClassChangeRequestL = new List<ClassChangeRequestModel>();

            foreach (DataRow r in dt.Rows)
            {
                ClassChangeRequestL.Add(RowOfClassForDatesChange(r));

            }
            return ClassChangeRequestL;
        }
        public List<ClassChangeRequestModel> FetchClassChangeRequest(ClassChangeRequestModel mod)
        {
            try
            {
                DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_ClassChangeRequest", Common.GetParams(mod)).Tables[0];
                return LoopinData(dt);
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }
        public List<ClassChangeRequestModel> FetchClassChangeRequest()
        {
            try
            {
                DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_ClassChangeRequest").Tables[0];
                return LoopinData(dt);
            }
            catch (Exception ex) { throw new Exception(ex.Message); }

        }
        
        public List<ClassChangeRequestModel> FetchClassDatesChangeRequest()
        {
            try
            {
                DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_ClassDatesChangeRequest").Tables[0];
                return LoopinClassForDatesChange(dt);
            }
            catch (Exception ex) { throw new Exception(ex.Message); }

        }
        public List<ClassChangeRequestModel> FetchClassChangeRequest(bool InActive)
        {
            try
            {
                DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_ClassChangeRequest", new SqlParameter("@InActive", InActive)).Tables[0];
                return LoopinData(dt);
            }
            catch (Exception ex) { throw new Exception(ex.Message); }

        }
        public List<ClassChangeRequestModel> GetByClassID(int ClassID)
        {
            try
            {
                DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_ClassChangeRequest", new SqlParameter("@ClassID", ClassID)).Tables[0];
                return LoopinData(dt);
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }
        public List<ClassChangeRequestModel> GetByTradeID(int TradeID)
        {
            try
            {
                DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_ClassChangeRequest", new SqlParameter("@TradeID", TradeID)).Tables[0];
                return LoopinData(dt);
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }
        public List<ClassChangeRequestModel> GetBySourceOfCurriculumID(int SourceOfCurriculumID)
        {
            try
            {
                DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_ClassChangeRequest", new SqlParameter("@SourceOfCurriculumID", SourceOfCurriculumID)).Tables[0];
                return LoopinData(dt);
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }
        public List<ClassChangeRequestModel> GetByCertAuthID(int CertAuthID)
        {
            try
            {
                DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_ClassChangeRequest", new SqlParameter("@CertAuthID", CertAuthID)).Tables[0];
                return LoopinData(dt);
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }
        public List<ClassChangeRequestModel> GetByClusterID(int ClusterID)
        {
            try
            {
                DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_ClassChangeRequest", new SqlParameter("@ClusterID", ClusterID)).Tables[0];
                return LoopinData(dt);
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }
        public List<ClassChangeRequestModel> GetByTehsilID(int TehsilID)
        {
            try
            {
                DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_ClassChangeRequest", new SqlParameter("@TehsilID", TehsilID)).Tables[0];
                return LoopinData(dt);
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }
        public List<ClassChangeRequestModel> GetByDistrictID(int DistrictID)
        {
            try
            {
                DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_ClassChangeRequest", new SqlParameter("@DistrictID", DistrictID)).Tables[0];
                return LoopinData(dt);
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }
        public List<ClassChangeRequestModel> FetchClassesForLocationChangeByUser(int UserID)
        {
            try
            {
                List<SqlParameter> param = new List<SqlParameter>();
                param.Add(new SqlParameter("@UserID", UserID));

                DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_ClassesForLocationChangeByUser", param.ToArray()).Tables[0];
                return LoopinClassForLocationChange(dt);
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }
        public List<ClassChangeRequestModel> FetchClassesForDatesChangeByUser(int UserID)
        {
            try
            {
                List<SqlParameter> param = new List<SqlParameter>();
                param.Add(new SqlParameter("@UserID", UserID));

                DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_ClassesForDatesChangeByUser", param.ToArray()).Tables[0];
                return LoopinClassForLocationChange(dt);
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }

        public bool CrClassApproveReject(ClassChangeRequestModel model, SqlTransaction transaction = null)
        {
            try
            {
                List<SqlParameter> param = new List<SqlParameter>();
                param.Add(new SqlParameter("@ClassChangeRequestID", model.ClassChangeRequestID));
                param.Add(new SqlParameter("@IsApproved", model.IsApproved));
                param.Add(new SqlParameter("@IsRejected", model.IsRejected));
                param.Add(new SqlParameter("@CurUserID", model.CurUserID));

                if (transaction != null)
                {
                    SqlHelper.ExecuteScalar(transaction, CommandType.StoredProcedure, "U_Cr_ClassApproveReject", param.ToArray());
                }
                else
                {
                    SqlHelper.ExecuteScalar(SqlHelper.GetCon(), CommandType.StoredProcedure, "U_Cr_TSPApproveReject", param.ToArray());
                }
                return true;
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }
        
        public bool CrClassDatesApproveReject(ClassChangeRequestModel model, SqlTransaction transaction = null)
        {
            try
            {
                List<SqlParameter> param = new List<SqlParameter>();
                param.Add(new SqlParameter("@ClassDatesChangeRequestID", model.ClassDatesChangeRequestID));
                param.Add(new SqlParameter("@IsApproved", model.IsApproved));
                param.Add(new SqlParameter("@IsRejected", model.IsRejected));
                param.Add(new SqlParameter("@CurUserID", model.CurUserID));

                if (transaction != null)
                {
                    SqlHelper.ExecuteScalar(transaction, CommandType.StoredProcedure, "U_Cr_ClassDatesApproveReject", param.ToArray());
                }
                else
                {
                    SqlHelper.ExecuteScalar(SqlHelper.GetCon(), CommandType.StoredProcedure, "U_Cr_ClassDatesApproveReject", param.ToArray());
                }
                return true;
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }

        public ClassChangeRequestModel GetByClassChangeRequestID_Notification(int ClassChangeRequestID, SqlTransaction transaction = null)
        {
            try
            {
                SqlParameter param = new SqlParameter("@ClassChangeRequestID", ClassChangeRequestID);
                DataTable dt = new DataTable();
                List<ClassChangeRequestModel> ClassChangeRequestModel = new List<ClassChangeRequestModel>();
                //dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "[RD_ClassChangeRequest_Notification]", param).Tables[0];

                if (transaction != null)
                {
                    dt = SqlHelper.ExecuteDataset(transaction, CommandType.StoredProcedure, "RD_ClassChangeRequest_Notification", param).Tables[0];
                }
                else
                {
                    dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_ClassChangeRequest_Notification", param).Tables[0];

                }

                if (dt.Rows.Count > 0)
                {
                    ClassChangeRequestModel = Helper.ConvertDataTableToModel<ClassChangeRequestModel>(dt);
                    return ClassChangeRequestModel[0];
                }
                else
                    return null;
            }
            catch (Exception ex) { throw new Exception(ex.Message); }

        }

        public void ActiveInActive(int ClassChangeRequestID, bool? InActive, int CurUserID)
        {

            SqlParameter[] PLead = new SqlParameter[3];
            PLead[0] = new SqlParameter("@ClassChangeRequestID", ClassChangeRequestID);
            PLead[1] = new SqlParameter("@InActive", InActive);
            PLead[2] = new SqlParameter("@CurUserID", CurUserID);
            SqlHelper.ExecuteNonQuery(SqlHelper.GetCon(), CommandType.StoredProcedure, "[AI_ClassChangeRequest]", PLead);
        }
        private ClassChangeRequestModel RowOfClassForLocationChange(DataRow r)
        {
            ClassChangeRequestModel ClassChangeRequest = new ClassChangeRequestModel();
            //ClassChangeRequest.ClassChangeRequestID = Convert.ToInt32(r["ClassChangeRequestID"]);
            ClassChangeRequest.ClassID = Convert.ToInt32(r["ClassID"]);
            ClassChangeRequest.TehsilID = Convert.ToInt32(r["TehsilID"]);
            ClassChangeRequest.DistrictID = Convert.ToInt32(r["DistrictID"]);
            ClassChangeRequest.ClassCode = r["ClassCode"].ToString();
            ClassChangeRequest.TrainingAddressLocation = r["TrainingAddressLocation"].ToString();

            ClassChangeRequest.StartDate = r["StartDate"].ToString().GetDate();
            ClassChangeRequest.EndDate = r["EndDate"].ToString().GetDate();
            ClassChangeRequest.Duration = Convert.ToDecimal(r["Duration"]);

            if (r.Table.Columns.Contains("CrClassLocationID"))
            {
                ClassChangeRequest.CrClassLocationID = Convert.ToInt32(r["CrClassLocationID"]);
            }
            if (r.Table.Columns.Contains("CrClassLocationIsApproved"))
            {
                ClassChangeRequest.CrClassLocationIsApproved = Convert.ToBoolean(r["CrClassLocationIsApproved"]);
            }
            if (r.Table.Columns.Contains("CrClassLocationIsRejected"))
            {
                ClassChangeRequest.CrClassLocationIsRejected = Convert.ToBoolean(r["CrClassLocationIsRejected"]);
            }

            if (r.Table.Columns.Contains("CrClassDatesID"))
            {
                ClassChangeRequest.CrClassDatesID = Convert.ToInt32(r["CrClassDatesID"]);
            }
            if (r.Table.Columns.Contains("CrClassDatesIsApproved"))
            {
                ClassChangeRequest.CrClassDatesIsApproved = Convert.ToBoolean(r["CrClassDatesIsApproved"]);
            }
            if (r.Table.Columns.Contains("CrClassDatesIsRejected"))
            {
                ClassChangeRequest.CrClassDatesIsRejected = Convert.ToBoolean(r["CrClassDatesIsRejected"]);
            }


            ClassChangeRequest.InActive = Convert.ToBoolean(r["InActive"]);
            ClassChangeRequest.CreatedUserID = Convert.ToInt32(r["CreatedUserID"]);
            ClassChangeRequest.ModifiedUserID = Convert.ToInt32(r["ModifiedUserID"]);
            ClassChangeRequest.CreatedDate = r["CreatedDate"].ToString().GetDate();
            ClassChangeRequest.ModifiedDate = r["ModifiedDate"].ToString().GetDate();
            return ClassChangeRequest;
        }
        private ClassChangeRequestModel RowOfClassForDatesChange(DataRow r)
        {
            ClassChangeRequestModel ClassChangeRequest = new ClassChangeRequestModel();
            ClassChangeRequest.ClassDatesChangeRequestID = Convert.ToInt32(r["ClassDatesChangeRequestID"]);
            ClassChangeRequest.ClassID = Convert.ToInt32(r["ClassID"]);
            ClassChangeRequest.ClassCode = r["ClassCode"].ToString();
            //ClassChangeRequest.TrainingAddressLocation = r["TrainingAddressLocation"].ToString();

            ClassChangeRequest.StartDate = r["StartDate"].ToString().GetDate();
            ClassChangeRequest.EndDate = r["EndDate"].ToString().GetDate();
            ClassChangeRequest.Duration = Convert.ToDecimal(r["Duration"]);

            ClassChangeRequest.IsApproved = Convert.ToBoolean(r["IsApproved"]);
            ClassChangeRequest.IsRejected = Convert.ToBoolean(r["IsRejected"]);

            ClassChangeRequest.InActive = Convert.ToBoolean(r["InActive"]);
            ClassChangeRequest.CreatedUserID = Convert.ToInt32(r["CreatedUserID"]);
            ClassChangeRequest.ModifiedUserID = Convert.ToInt32(r["ModifiedUserID"]);
            ClassChangeRequest.CreatedDate = r["CreatedDate"].ToString().GetDate();
            ClassChangeRequest.ModifiedDate = r["ModifiedDate"].ToString().GetDate();
            return ClassChangeRequest;
        }
        
        private ClassChangeRequestModel RowOfClassChangeRequest(DataRow r)
        {
            ClassChangeRequestModel ClassChangeRequest = new ClassChangeRequestModel();
            ClassChangeRequest.ClassChangeRequestID = Convert.ToInt32(r["ClassChangeRequestID"]);
            ClassChangeRequest.ClassID = Convert.ToInt32(r["ClassID"]);
            ClassChangeRequest.ClassCode = r["ClassCode"].ToString();
            ClassChangeRequest.TrainingAddressLocation = r["TrainingAddressLocation"].ToString();
            if (r.Table.Columns.Contains("DistrictID"))
            {
                ClassChangeRequest.DistrictID = Convert.ToInt32(r["DistrictID"]);
                ClassChangeRequest.DistrictName = r["DistrictName"].ToString();
            }
            if (r.Table.Columns.Contains("TehsilID"))
            {
                ClassChangeRequest.TehsilID = Convert.ToInt32(r["TehsilID"]);
                ClassChangeRequest.TehsilName = r["TehsilName"].ToString();
            }
            ClassChangeRequest.TrainingAddressLocation = r["TrainingAddressLocation"].ToString();
          
            ClassChangeRequest.InActive = Convert.ToBoolean(r["InActive"]);
            ClassChangeRequest.CreatedUserID = Convert.ToInt32(r["CreatedUserID"]);
            ClassChangeRequest.ModifiedUserID = Convert.ToInt32(r["ModifiedUserID"]);
            ClassChangeRequest.CreatedDate = r["CreatedDate"].ToString().GetDate();
            ClassChangeRequest.ModifiedDate = r["ModifiedDate"].ToString().GetDate();
            ClassChangeRequest.IsApproved = Convert.ToBoolean(r["IsApproved"]);
            ClassChangeRequest.IsRejected = Convert.ToBoolean(r["IsRejected"]);
            return ClassChangeRequest;
        }
        public List<ClassChangeRequestModel> FetchClassChangeRequestByFilter(QueryFilters filters)
        {
            try
            {
                List<SqlParameter> param = new List<SqlParameter>();
                param.Add(new SqlParameter("@SchemeID", filters.SchemeID));
                param.Add(new SqlParameter("@TSPID", filters.TSPID));
                param.Add(new SqlParameter("@ClassID", filters.ClassID));
                param.Add(new SqlParameter("@KAMID", filters.KAMID));
                DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_ClassChangeRequestByFilter", param.ToArray()).Tables[0];
                return LoopinData(dt);
            }
            catch (Exception ex) { throw new Exception(ex.Message); }

        }
    }
}
