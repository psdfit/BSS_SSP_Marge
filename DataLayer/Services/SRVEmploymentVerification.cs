using DataLayer.Classes;
using DataLayer.Interfaces;
using DataLayer.Models;
using Microsoft.Data.SqlClient;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using static DataLayer.Models.CallCenterVerificationModels;

namespace DataLayer.Services
{
    public class SRVEmploymentVerification : ISRVEmploymentVerification
    {
        public SRVEmploymentVerification() { }
        public EmploymentVerificationModel GetByID(int ID)
        {
            try
            {
                SqlParameter param = new SqlParameter("@ID", ID);
                DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_EmploymentVerification", param).Tables[0];
                if (dt.Rows.Count > 0)
                {
                    return RowOfEmploymentVerification(dt.Rows[0]);
                }
                else
                    return null;
            }
            catch (Exception ex) { throw new Exception(ex.Message); }

        }
        public List<EmploymentVerificationModel> SaveEmploymentVerification(EmploymentVerificationModel EmploymentVerification)
        {
            try
            {
                //FilePaths path = new FilePaths();
                string AttachmentPath = Common.AddFile(EmploymentVerification.Attachment, FilePaths.TSP_FILE_DIR);

                SqlParameter[] param = new SqlParameter[28];
                param[0] = new SqlParameter("@ID", EmploymentVerification.ID);
                param[1] = new SqlParameter("@PlacementID", EmploymentVerification.PlacementID);
                param[2] = new SqlParameter("@PhysicalVerificationStatus", EmploymentVerification.IsVerified);
                param[3] = new SqlParameter("@VerificationMethodID", EmploymentVerification.VerificationMethodID);
                param[4] = new SqlParameter("@IsVerified", EmploymentVerification.IsVerified);
                param[5] = new SqlParameter("@Comments", EmploymentVerification.Comments);
                param[6] = new SqlParameter("@Attachment", AttachmentPath);
                param[7] = new SqlParameter("@SupervisorContact", EmploymentVerification.SupervisorContact);
                param[8] = new SqlParameter("@SupervisorName", EmploymentVerification.SupervisorName);
                param[9] = new SqlParameter("@OfficeContactNo", EmploymentVerification.OfficeContactNo);
                param[10] = new SqlParameter("@Designation", EmploymentVerification.Designation);
                param[11] = new SqlParameter("@Department", EmploymentVerification.Department);
                param[12] = new SqlParameter("@EmploymentDuration", EmploymentVerification.EmploymentDuration);
                param[13] = new SqlParameter("@Salary", EmploymentVerification.Salary);
                param[14] = new SqlParameter("@EmploymentStartDate", EmploymentVerification.EmploymentStartDate);
                param[15] = new SqlParameter("@EmploymentStatus", EmploymentVerification.EmploymentStatus);
                param[16] = new SqlParameter("@EmploymentType", EmploymentVerification.EmploymentType);
                param[17] = new SqlParameter("@EmployerName", EmploymentVerification.EmployerName);
                param[18] = new SqlParameter("@EmployerBusinessType", EmploymentVerification.EmployerBusinessType);
                param[19] = new SqlParameter("@EmploymentAddress", EmploymentVerification.EmploymentAddress);
                param[20] = new SqlParameter("@District", EmploymentVerification.District);
                param[21] = new SqlParameter("@EmploymentTehsil", EmploymentVerification.EmploymentTehsil);
                param[22] = new SqlParameter("@EmploymentTiming", EmploymentVerification.EmploymentTiming);
                param[23] = new SqlParameter("@CurUserID", EmploymentVerification.CurUserID);
                param[24] = new SqlParameter("@PlatformName", EmploymentVerification.PlatformName);
                param[25] = new SqlParameter("@NameofTraineeStorePage", EmploymentVerification.NameofTraineeStorePage);
                param[26] = new SqlParameter("@LinkofTraineeStorePage", EmploymentVerification.LinkofTraineeStorePage);
                SqlHelper.ExecuteNonQuery(SqlHelper.GetCon(), CommandType.StoredProcedure, "U_DEOPlacementVerification", param);

                return FetchEmploymentVerification();
            }
            catch (Exception ex)
            { throw new Exception(ex.Message); }
        }
        private List<EmploymentVerificationModel> LoopinData(DataTable dt)
        {
            List<EmploymentVerificationModel> EmploymentVerificationL = new List<EmploymentVerificationModel>();

            foreach (DataRow r in dt.Rows)
            {
                EmploymentVerificationL.Add(RowOfEmploymentVerification(r));

            }
            return EmploymentVerificationL;
        }
        public List<EmploymentVerificationModel> FetchEmploymentVerification(EmploymentVerificationModel mod)
        {
            try
            {
                DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_PlacementVerification", Common.GetParams(mod)).Tables[0];
                return LoopinData(dt);
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }
        public List<EmploymentVerificationModel> FetchEmploymentVerification()
        {
            try
            {
                DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_PlacementVerification").Tables[0];
                return LoopinData(dt);
            }
            catch (Exception ex) { throw new Exception(ex.Message); }

        }
        public List<EmploymentVerificationModel> FetchEmploymentVerification(bool InActive)
        {
            try
            {
                DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_PlacementVerification", new SqlParameter("@InActive", InActive)).Tables[0];
                return LoopinData(dt);
            }
            catch (Exception ex) { throw new Exception(ex.Message); }

        }
        public DataTable GetVerificationMethod(int EmploymentTypeID)
        {
            try
            {
                DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_VerificationMethod", new SqlParameter("@EmploymentTypeID", EmploymentTypeID)).Tables[0];
                return dt;
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }
        public List<EmploymentVerificationModel> GetByPlacementID(int PlacementID)
        {
            try
            {
                DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_PlacementVerification", new SqlParameter("@PlacementID", PlacementID)).Tables[0];
                return LoopinData(dt);
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }
        public void ActiveInActive(int ID, bool? InActive, int CurUserID)
        {

            SqlParameter[] PLead = new SqlParameter[3];
            PLead[0] = new SqlParameter("@ID", ID);
            PLead[1] = new SqlParameter("@InActive", InActive);
            PLead[2] = new SqlParameter("@CurUserID", CurUserID);
            SqlHelper.ExecuteNonQuery(SqlHelper.GetCon(), CommandType.StoredProcedure, "[AI_EmploymentVerification]", PLead);
        }
        public int SubmitVerification(int ClassID, int CurUserID)
        {
            try
            {
                SqlParameter[] param = new SqlParameter[2];
                param[0] = new SqlParameter("@ClassID", ClassID);
                param[1] = new SqlParameter("@CurUserID", CurUserID);


                SqlHelper.ExecuteNonQuery(SqlHelper.GetCon(), CommandType.StoredProcedure, "[SubmitVerificationEmployment]", new SqlParameter("@ClassID", ClassID));
                return SqlHelper.ExecuteNonQuery(SqlHelper.GetCon(), CommandType.StoredProcedure, "CalculateROSIOnEmploymentSubmission", param);
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }
        public void UpdateVerifyStatus(int PlacementID, bool? Status, int CurUserID)
        {

            SqlParameter[] PLead = new SqlParameter[3];
            PLead[0] = new SqlParameter("@PlacementID", PlacementID);
            PLead[1] = new SqlParameter("@Status", Status);
            PLead[2] = new SqlParameter("@CurUserID", CurUserID);
            SqlHelper.ExecuteNonQuery(SqlHelper.GetCon(), CommandType.StoredProcedure, "[U_UpdateVerifyStatus]", PLead);
        }
        private EmploymentVerificationModel RowOfEmploymentVerification(DataRow r)
        {
            EmploymentVerificationModel EmploymentVerification = new EmploymentVerificationModel();
            EmploymentVerification.ID = Convert.ToInt32(r["ID"]);
            EmploymentVerification.PlacementID = Convert.ToInt32(r["PlacementID"]);
            EmploymentVerification.TraineeName = r["TraineeName"].ToString();
            EmploymentVerification.PayrollVerification = r.Field<bool?>("PayrollVerification");
            EmploymentVerification.PayrollVerificationStatus = r.Field<bool?>("PayrollVerificationStatus");
            EmploymentVerification.PhysicalVerification = r.Field<bool?>("PhysicalVerification");
            EmploymentVerification.TelephonicVerification = r.Field<bool?>("TelephonicVerification");
            EmploymentVerification.PhysicalVerificationStatus = r.Field<bool?>("PhysicalVerificationStatus");
            EmploymentVerification.TelephonicVerificationStatus = r.Field<bool?>("TelephonicVerificationStatus");
            EmploymentVerification.IsVerified = r.Field<bool?>("IsVerified");
            EmploymentVerification.VerificationMethodID = Convert.ToInt32(r["VerificationMethodID"]);
            EmploymentVerification.Comments = r["Comments"].ToString();
            EmploymentVerification.Attachment = r["Attachment"].ToString();
            EmploymentVerification.InActive = Convert.ToBoolean(r["InActive"]);
            EmploymentVerification.CreatedUserID = Convert.ToInt32(r["CreatedUserID"]);
            EmploymentVerification.ModifiedUserID = Convert.ToInt32(r["ModifiedUserID"]);
            EmploymentVerification.CreatedDate = r["CreatedDate"].ToString().GetDate();
            EmploymentVerification.ModifiedDate = r["ModifiedDate"].ToString().GetDate();

            return EmploymentVerification;
        }
        public List<EmploymentVerificationModel> UpdateTelephonicEmploymentVerification(EmploymentVerificationModel EmploymentVerification)
        {
            try
            {
                bool VerifiedMatrixBit = CreateMatrixForVerificarion(EmploymentVerification);


                //FilePaths path = new FilePaths();
                string AttachmentPath = Common.AddFile(EmploymentVerification.Attachment, FilePaths.TSP_FILE_DIR);

                SqlParameter[] param = new SqlParameter[25];
                param[0] = new SqlParameter("@ID", EmploymentVerification.ID);
                param[1] = new SqlParameter("@PlacementID", EmploymentVerification.PlacementID);
                param[2] = new SqlParameter("@EmploymentTiming", EmploymentVerification.EmploymentTiming);
                param[3] = new SqlParameter("@TelephonicVerificationStatus", VerifiedMatrixBit);
                param[4] = new SqlParameter("@VerificationMethodID", EmploymentVerification.VerificationMethodID);
                param[5] = new SqlParameter("@IsVerified", VerifiedMatrixBit);
                param[6] = new SqlParameter("@Comments", EmploymentVerification.Comments);
                param[7] = new SqlParameter("@Attachment", AttachmentPath);
                param[8] = new SqlParameter("@CurUserID", EmploymentVerification.CurUserID);
                param[9] = new SqlParameter("@SupervisorContact", EmploymentVerification.SupervisorContact);
                param[10] = new SqlParameter("@SupervisorName", EmploymentVerification.SupervisorName);
                param[11] = new SqlParameter("@EmploymentAddress", EmploymentVerification.EmploymentAddress);
                param[12] = new SqlParameter("@OfficeContactNo", EmploymentVerification.OfficeContactNo);
                param[13] = new SqlParameter("@Designation", EmploymentVerification.Designation);
                param[14] = new SqlParameter("@Department", EmploymentVerification.Department);
                param[15] = new SqlParameter("@EmploymentDuration", EmploymentVerification.EmploymentDuration);
                param[16] = new SqlParameter("@Salary", EmploymentVerification.Salary);
                param[17] = new SqlParameter("@EmploymentStartDate", EmploymentVerification.EmploymentStartDate);
                param[18] = new SqlParameter("@EmploymentStatus", EmploymentVerification.EmploymentStatus);
                param[19] = new SqlParameter("@EmploymentType", EmploymentVerification.EmploymentType);
                param[20] = new SqlParameter("@EmployerName", EmploymentVerification.EmployerName);
                param[21] = new SqlParameter("@EmployerBusinessType", EmploymentVerification.EmployerBusinessType);
                SqlHelper.ExecuteNonQuery(SqlHelper.GetCon(), CommandType.StoredProcedure, "U_TelephonicPlacementVerification", param);
                A_CallCenterVerificationHistory(EmploymentVerification);

                return FetchEmploymentVerification();
            }
            catch (Exception ex)
            { throw new Exception(ex.Message); }
        }

        public static bool CreateMatrixForVerificarion(EmploymentVerificationModel EmploymentVerification)
        {
            try
            {

                if (EmploymentVerification.CallCenterVerificationTraineeID == (int)EnumCallCenterVerification.Yes
                    && EmploymentVerification.CallCenterVerificationSupervisorID == (int)EnumCallCenterVerification.Yes)
                    return true;

                else if (EmploymentVerification.CallCenterVerificationTraineeID == (int)EnumCallCenterVerification.Yes
                    && EmploymentVerification.CallCenterVerificationSupervisorID == (int)EnumCallCenterVerification.No)
                    return false;

                else if (EmploymentVerification.CallCenterVerificationTraineeID == (int)EnumCallCenterVerification.Yes
                    && EmploymentVerification.CallCenterVerificationSupervisorID == (int)EnumCallCenterVerification.NotConnected)
                    return true;

                else if (EmploymentVerification.CallCenterVerificationTraineeID == (int)EnumCallCenterVerification.No
                    && EmploymentVerification.CallCenterVerificationSupervisorID == (int)EnumCallCenterVerification.Yes)
                    return false;

                else if (EmploymentVerification.CallCenterVerificationTraineeID == (int)EnumCallCenterVerification.No
                    && EmploymentVerification.CallCenterVerificationSupervisorID == (int)EnumCallCenterVerification.No)
                    return false;

                else if (EmploymentVerification.CallCenterVerificationTraineeID == (int)EnumCallCenterVerification.No
                    && EmploymentVerification.CallCenterVerificationSupervisorID == (int)EnumCallCenterVerification.NotConnected)
                    return false;

                else if (EmploymentVerification.CallCenterVerificationTraineeID == (int)EnumCallCenterVerification.NotConnected
                    && EmploymentVerification.CallCenterVerificationSupervisorID == (int)EnumCallCenterVerification.Yes)
                    return false; //Changes as per CR by Ali Hadier on 23-01-23

                else if (EmploymentVerification.CallCenterVerificationTraineeID == (int)EnumCallCenterVerification.NotConnected
                    && EmploymentVerification.CallCenterVerificationSupervisorID == (int)EnumCallCenterVerification.No)
                    return false;

                else if (EmploymentVerification.CallCenterVerificationTraineeID == (int)EnumCallCenterVerification.NotConnected
                    && EmploymentVerification.CallCenterVerificationSupervisorID == (int)EnumCallCenterVerification.NotConnected)
                    return false;

                return false;



            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }

        public static void A_CallCenterVerificationHistory(EmploymentVerificationModel EmploymentVerification)
        {
            try
            {
                SqlParameter[] param = new SqlParameter[8];
                param[0] = new SqlParameter("@PlacementID", EmploymentVerification.PlacementID);
                param[1] = new SqlParameter("@CallCenterVerificationTraineeID", EmploymentVerification.CallCenterVerificationTraineeID);
                param[2] = new SqlParameter("@CallCenterVerificationSupervisorID", EmploymentVerification.CallCenterVerificationSupervisorID);
                param[3] = new SqlParameter("@CallCenterVerificationCommentsID", EmploymentVerification.CallCenterVerificationCommentsID);
                param[4] = new SqlParameter("@CurUserID", EmploymentVerification.CurUserID);
                SqlHelper.ExecuteNonQuery(SqlHelper.GetCon(), CommandType.StoredProcedure, "AU_CallCenterVerificationHistory", param);
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }


        public List<CallCenterVerificationTraineeModel> GetCallCenterVerificationTrainee()
        {
            try
            {
                DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "[RD_CallCenterVerificationTrainee]").Tables[0];
                List<CallCenterVerificationTraineeModel> Model = new List<CallCenterVerificationTraineeModel>();
                Model = Helper.ConvertDataTableToModel<CallCenterVerificationTraineeModel>(dt);

                return (Model);
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }
        public List<CallCenterVerificationSupervisorModel> GetCallCenterVerificationSupervisor()
        {
            try
            {
                DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "[RD_CallCenterVerificationSupervisor]").Tables[0];
                List<CallCenterVerificationSupervisorModel> Model = new List<CallCenterVerificationSupervisorModel>();
                Model = Helper.ConvertDataTableToModel<CallCenterVerificationSupervisorModel>(dt);

                return (Model);
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }
        public List<CallCenterVerificationCommentsModel> GetCallCenterVerificationComments()
        {
            try
            {
                DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "[RD_CallCenterVerificationComments]").Tables[0];
                List<CallCenterVerificationCommentsModel> Model = new List<CallCenterVerificationCommentsModel>();
                Model = Helper.ConvertDataTableToModel<CallCenterVerificationCommentsModel>(dt);

                return (Model);
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }
        public int SubmitVerificationByCallCenter(int ClassID, int CurUserID)
        {
            try
            {
                SqlParameter[] param = new SqlParameter[2];
                param[0] = new SqlParameter("@ClassID", ClassID);
                param[1] = new SqlParameter("@CurUserID", CurUserID);


                SqlHelper.ExecuteNonQuery(SqlHelper.GetCon(), CommandType.StoredProcedure, "[SubmitVerificationEmploymentByCallCenter]", new SqlParameter("@ClassID", ClassID));
                return SqlHelper.ExecuteNonQuery(SqlHelper.GetCon(), CommandType.StoredProcedure, "CalculateROSIOnEmploymentSubmission", param);
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }
    }
}
