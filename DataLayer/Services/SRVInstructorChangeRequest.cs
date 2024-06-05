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
    public class SRVInstructorChangeRequest : SRVBase, ISRVInstructorChangeRequest
    {
        private readonly ISRVSendEmail srvSendEmail;
        private readonly ISRVTSPMaster srvTSPMaster;
        private readonly ISRVUsers srvUsers;

        public SRVInstructorChangeRequest(ISRVSendEmail srvSendEmail, ISRVTSPMaster srvTSPMaster, ISRVUsers srvUsers)
        {
            this.srvSendEmail = srvSendEmail;
            this.srvTSPMaster = srvTSPMaster;
            this.srvUsers = srvUsers;
        }
        public InstructorChangeRequestModel GetByInstructorChangeRequestID(int InstructorChangeRequestID, SqlTransaction transaction = null)
        {
            try
            {
                SqlParameter param = new SqlParameter("@InstructorChangeRequestID", InstructorChangeRequestID);
                DataTable dt = new DataTable();


                if (transaction != null)
                {
                 dt = SqlHelper.ExecuteDataset(transaction, CommandType.StoredProcedure, "RD_InstructorChangeRequest", param).Tables[0];
                }
                else
                {
                     dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_InstructorChangeRequest", param).Tables[0];

                }

                
                if (dt.Rows.Count > 0)
                {
                    return RowOfInstructorChangeRequest(dt.Rows[0]);
                }
                else
                    return null;
            }
            catch (Exception ex) { throw new Exception(ex.Message); }

        }

        public InstructorChangeRequestModel GetByInstructorChangeRequestID_Notification(int InstructorChangeRequestID, SqlTransaction transaction = null)
        {
            try
            {
                SqlParameter param = new SqlParameter("@InstructorChangeRequestID", InstructorChangeRequestID);
                DataTable dt = new DataTable();
                List<InstructorChangeRequestModel> InstructorChangeRequestModel = new List<InstructorChangeRequestModel>();
              //  dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_InstructorChangeRequest_Notification", param).Tables[0];

                if (transaction != null)
                {
                    dt = SqlHelper.ExecuteDataset(transaction, CommandType.StoredProcedure, "RD_InstructorChangeRequest_Notification", param).Tables[0];
                }
                else
                {
                    dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_InstructorChangeRequest_Notification", param).Tables[0];

                }

                if (dt.Rows.Count > 0)
                {
                    InstructorChangeRequestModel = Helper.ConvertDataTableToModel<InstructorChangeRequestModel>(dt);

                    return InstructorChangeRequestModel[0];
                }
                else
                    return null;
            }
            catch (Exception ex) { throw new Exception(ex.Message); }

        }

        public List<InstructorChangeRequestModel> SaveInstructorChangeRequest(InstructorChangeRequestModel InstructorChangeRequest)
        {
            try
            {
                SqlParameter[] param = new SqlParameter[9];
                param[0] = new SqlParameter("@InstructorChangeRequestID", InstructorChangeRequest.InstructorChangeRequestID);
                param[1] = new SqlParameter("@InstrID", InstructorChangeRequest.InstrID);
                param[2] = new SqlParameter("@InstructorName", InstructorChangeRequest.InstructorName);
                param[3] = new SqlParameter("@CNICofInstructor", InstructorChangeRequest.CNICofInstructor);
                param[4] = new SqlParameter("@QualificationHighest", InstructorChangeRequest.QualificationHighest);
                param[5] = new SqlParameter("@TotalExperience", InstructorChangeRequest.TotalExperience);
                InstructorChangeRequest.PicturePath = String.IsNullOrWhiteSpace(InstructorChangeRequest.PicturePath) ? "" : Common.AddFile(InstructorChangeRequest.PicturePath, FilePaths.INSTRUCTOR_FILE_DIR);
                param[6] = new SqlParameter("@PicturePath", InstructorChangeRequest.PicturePath);
                param[7] = new SqlParameter("@InstructorCRComments", InstructorChangeRequest.InstructorCRComments);
                //param[6] = new SqlParameter("@GenderID", InstructorChangeRequest.GenderID);
                param[8] = new SqlParameter("@CurUserID", InstructorChangeRequest.CurUserID);
                SqlHelper.ExecuteNonQuery(SqlHelper.GetCon(), CommandType.StoredProcedure, "[AU_InstructorChangeRequest]", param);

                //By Nasrullah
                var firstApproval = new SRVApproval().FetchApproval(new ApprovalModel() { Step = 1, ProcessKey = EnumApprovalProcess.CR_INSTRUCTOR}).FirstOrDefault();
                
                UsersModel KamUser = srvTSPMaster.KAMUserByTSPUserID(InstructorChangeRequest.CurUserID);
                UsersModel usersModel = srvUsers.GetByUserID(InstructorChangeRequest.CurUserID);
                ApprovalModel approvalsModelForNotificarion = new ApprovalModel();
                ApprovalHistoryModel model1 = new ApprovalHistoryModel();
                model1.ApprovalStatusID = (int)EnumApprovalStatus.Pending;
                approvalsModelForNotificarion.UserIDs = firstApproval.UserIDs;
                approvalsModelForNotificarion.UserIDs +=","+ KamUser.UserID.ToString();
                approvalsModelForNotificarion.ProcessKey = EnumApprovalProcess.CR_INSTRUCTOR;
                approvalsModelForNotificarion.CustomComments = "generated from TSP (" + usersModel.FullName + ")";
                srvSendEmail.GenerateEmailAndSendNotification(approvalsModelForNotificarion, model1);
                return FetchInstructorChangeRequest();
            }
            catch (Exception ex)
            { throw new Exception(ex.Message); }
        }

        public void SaveInstructor(InstructorModel Instructor)
        {
            try
            {
                SqlParameter[] param = new SqlParameter[16];
                param[0] = new SqlParameter("@InstrID", Instructor.InstrID);
                param[1] = new SqlParameter("@InstructorName", Instructor.InstructorName);
                param[2] = new SqlParameter("@CNICofInstructor", Instructor.CNICofInstructor);
                param[3] = new SqlParameter("@ClassCode", Instructor.ClassCode);
                param[4] = new SqlParameter("@QualificationHighest", Instructor.QualificationHighest);
                param[5] = new SqlParameter("@TotalExperience", Instructor.TotalExperience);
                param[6] = new SqlParameter("@GenderID", Instructor.GenderID);
               
                Instructor.PicturePath = String.IsNullOrWhiteSpace(Instructor.PicturePath) ? "" : Common.AddFile(Instructor.PicturePath, FilePaths.INSTRUCTOR_FILE_DIR);
                param[7] = new SqlParameter("@PicturePath", Instructor.PicturePath);
                param[8] = new SqlParameter("@TSPID", Instructor.CurUserID);
                param[9] = new SqlParameter("@CurUserID", Instructor.CurUserID);
                //param[10] = new SqlParameter("@NameOfOrganization", Instructor.NameOfOrganization);
                param[10] = new SqlParameter("@TradeID", Instructor.TradeID);
                param[11] = new SqlParameter("@LocationAddress", Instructor.LocationAddress);
                param[12] = new SqlParameter("@InstrMasterID", Instructor.InstrMasterID);
                param[13] = new SqlParameter("@SchemeID", Instructor.SchemeID);

                param[14] = new SqlParameter("@Ident", SqlDbType.Int);
                param[14].Direction = ParameterDirection.Output;

                SqlHelper.ExecuteNonQuery(SqlHelper.GetCon(), CommandType.StoredProcedure, "[AU_New_Instructor_CR]", param);

                //int InstrID = Convert.ToInt32(param[9].Value);
                //return (new SRVInstructor.FetchInstructorDataByUser(Instructor.InstrID));
            }
            catch (Exception ex)
            { throw new Exception(ex.Message); }
        }
        public List<InstructorChangeRequestModel> SaveInstructorByCR(InstructorChangeRequestModel Instructor)
        {
            try
            {
                SqlParameter[] param = new SqlParameter[18];
                param[0] = new SqlParameter("@CRNewInstructorID", Instructor.CRNewInstructorID);
                param[1] = new SqlParameter("@InstructorName", Instructor.InstructorName);
                param[2] = new SqlParameter("@CNICofInstructor", Instructor.CNICofInstructor);
                param[3] = new SqlParameter("@ClassID", Instructor.ClassID);
                param[4] = new SqlParameter("@QualificationHighest", Instructor.QualificationHighest);
                param[5] = new SqlParameter("@TotalExperience", Instructor.TotalExperience);
                param[6] = new SqlParameter("@GenderID", Instructor.GenderID);
               
                Instructor.PicturePath = String.IsNullOrWhiteSpace(Instructor.PicturePath) ? "" : Common.AddFile(Instructor.PicturePath, FilePaths.INSTRUCTOR_FILE_DIR);
                param[7] = new SqlParameter("@PicturePath", Instructor.PicturePath);
                param[8] = new SqlParameter("@TSPID", Instructor.CurUserID);
                param[9] = new SqlParameter("@CurUserID", Instructor.CurUserID);
                //param[10] = new SqlParameter("@NameOfOrganization", Instructor.NameOfOrganization);
                param[10] = new SqlParameter("@TradeID", Instructor.TradeID);
                param[11] = new SqlParameter("@LocationAddress", Instructor.LocationAddress);
                param[12] = new SqlParameter("@SchemeID", Instructor.SchemeID);
                param[13] = new SqlParameter("@FilePath", Instructor.FilePath);
                param[14] = new SqlParameter("@NewInstructorCRComments", Instructor.NewInstructorCRComments);

                param[15] = new SqlParameter("@Ident", SqlDbType.Int);
                param[15].Direction = ParameterDirection.Output;

                SqlHelper.ExecuteNonQuery(SqlHelper.GetCon(), CommandType.StoredProcedure, "[AI_New_Instructor_CR]", param);

                //By Nasrullah
                var firstApproval = new SRVApproval().FetchApproval(new ApprovalModel() { Step = 1, ProcessKey = EnumApprovalProcess.CR_NEW_INSTRUCTOR }).FirstOrDefault();
                UsersModel KamUser = srvTSPMaster.KAMUserByTSPUserID(Instructor.CurUserID);
                UsersModel usersModel = srvUsers.GetByUserID(Instructor.CurUserID);
                ApprovalModel approvalsModelForNotificarion = new ApprovalModel();
                ApprovalHistoryModel model1 = new ApprovalHistoryModel();
                model1.ApprovalStatusID = (int)EnumApprovalStatus.Pending;
                approvalsModelForNotificarion.UserIDs = firstApproval.UserIDs;
                approvalsModelForNotificarion.UserIDs += "," + KamUser.UserID.ToString();
                approvalsModelForNotificarion.ProcessKey = EnumApprovalProcess.CR_NEW_INSTRUCTOR;
                approvalsModelForNotificarion.CustomComments = "generated from TSP (" + usersModel.FullName + ")";
                srvSendEmail.GenerateEmailAndSendNotification(approvalsModelForNotificarion, model1);

                return FetchNewInstructorChangeRequest(Instructor.CurUserID);

            }
            catch (Exception ex)
            { throw new Exception(ex.Message); }
        }
        private List<InstructorChangeRequestModel> LoopinData(DataTable dt)
        {
            List<InstructorChangeRequestModel> InstructorChangeRequestL = new List<InstructorChangeRequestModel>();

            foreach (DataRow r in dt.Rows)
            {
                InstructorChangeRequestL.Add(RowOfInstructorChangeRequest(r));

            }
            return InstructorChangeRequestL;
        }
        
        private List<InstructorChangeRequestModel> LoopinNewInstructorData(DataTable dt)
        {
            List<InstructorChangeRequestModel> InstructorChangeRequestL = new List<InstructorChangeRequestModel>();

            foreach (DataRow r in dt.Rows)
            {
                InstructorChangeRequestL.Add(RowOfNewInstructorChangeRequest(r));

            }
            return InstructorChangeRequestL;
        }
        public List<InstructorChangeRequestModel> FetchInstructorChangeRequest(InstructorChangeRequestModel mod)
        {
            try
            {
                DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_InstructorChangeRequest", Common.GetParams(mod)).Tables[0];
                return LoopinData(dt);
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }
        public List<InstructorChangeRequestModel> FetchInstructorChangeRequest()
        {
            try
            {
                DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_InstructorChangeRequest").Tables[0];
                return LoopinData(dt);
            }
            catch (Exception ex) { throw new Exception(ex.Message); }

        }
        public List<InstructorChangeRequestModel> FetchNewInstructorChangeRequest(int userid)
        {
            try
            {
                DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_New_InstructorChangeRequest", new SqlParameter("@UserID", userid)).Tables[0];
                return LoopinNewInstructorData(dt);
            }
            catch (Exception ex) { throw new Exception(ex.Message); }

        }
        public List<InstructorChangeRequestModel> FetchFilteredInstructorChangeRequest(QueryFilters filters)
        {
            try
            {
                List<SqlParameter> param = new List<SqlParameter>();
                param.Add(new SqlParameter("@SchemeID", filters.SchemeID));
                param.Add(new SqlParameter("@TSPID", filters.TSPID));
                param.Add(new SqlParameter("@ClassID", filters.ClassID));
                //param.Add(new SqlParameter("@KAMID", filters.KAMID));

                DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_InstructorChangeRequest", param.ToArray()).Tables[0];
                return LoopinData(dt);
            }
            catch (Exception ex) { throw new Exception(ex.Message); }

        }
        public List<InstructorChangeRequestModel> FetchFilteredNewInstructorRequest(QueryFilters filters)
        {
            try
            {
                List<SqlParameter> param = new List<SqlParameter>();
                param.Add(new SqlParameter("@SchemeID", filters.SchemeID));
                param.Add(new SqlParameter("@TSPID", filters.TSPID));
                param.Add(new SqlParameter("@ClassID", filters.ClassID));
                //param.Add(new SqlParameter("@KAMID", filters.KAMID));

                DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_New_Instructor_CR", param.ToArray()).Tables[0];
                var List = LoopinNewInstructorData(dt);
                //List.ForEach(itm => itm.FilePath = Common.GetFileBase64(itm.FilePath));
                return List;
            }
            catch (Exception ex) { throw new Exception(ex.Message); }

        }

        public List<InstructorChangeRequestModel> FetchNewInstructorRequest()
        {
            try
            {
                DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_New_Instructor_CR").Tables[0];
                var List = LoopinNewInstructorData(dt);
                //List.ForEach(itm => itm.FilePath = Common.GetFileBase64(itm.FilePath));
                return List;
            }
            catch (Exception ex) { throw new Exception(ex.Message); }

        }
        public List<InstructorChangeRequestModel> FetchNewInstructorRequestAttachments(int id)
        {
            try
            {
                DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_NewInstructor_CR_AttachementsByID", new SqlParameter("@CRNewInstructorID", id)).Tables[0];
                var List = LoopinNewInstructorData(dt);
                List.ForEach(itm => itm.FilePath = Common.GetFileBase64(itm.FilePath));
                return List;
            }
            catch (Exception ex) { throw new Exception(ex.Message); }

        }

        public List<InstructorChangeRequestModel> FetchInstructorChangeRequest(bool InActive)
        {
            try
            {
                DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_InstructorChangeRequest", new SqlParameter("@InActive", InActive)).Tables[0];
                return LoopinData(dt);
            }
            catch (Exception ex) { throw new Exception(ex.Message); }

        }
        public List<InstructorChangeRequestModel> GetByInstrID(int InstrID)
        {
            try
            {
                DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_InstructorChangeRequest", new SqlParameter("@InstrID", InstrID)).Tables[0];
                return LoopinData(dt);
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }

        public bool CrInstructorApproveReject(InstructorChangeRequestModel model, SqlTransaction transaction = null)
        {
            try
            {
                List<SqlParameter> param = new List<SqlParameter>();
                param.Add(new SqlParameter("@InstructorChangeRequestID", model.InstructorChangeRequestID));
                param.Add(new SqlParameter("@IsApproved", model.IsApproved));
                param.Add(new SqlParameter("@IsRejected", model.IsRejected));
                param.Add(new SqlParameter("@CurUserID", model.CurUserID));

                if (transaction != null)
                {
                    SqlHelper.ExecuteScalar(transaction, CommandType.StoredProcedure, "U_Cr_InstructorApproveReject", param.ToArray());
                }
                else
                {
                    SqlHelper.ExecuteScalar(SqlHelper.GetCon(), CommandType.StoredProcedure, "U_Cr_InstructorApproveReject", param.ToArray());
                }
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public bool CrNewInstructorApproveReject(InstructorChangeRequestModel model, SqlTransaction transaction = null)
        {
            try
            {
                List<SqlParameter> param = new List<SqlParameter>();
                param.Add(new SqlParameter("@CRNewInstructorID", model.CRNewInstructorID));
                param.Add(new SqlParameter("@IsApproved", model.IsApproved));
                param.Add(new SqlParameter("@IsRejected", model.IsRejected));
                param.Add(new SqlParameter("@CurUserID", model.CurUserID));

                if (transaction != null)
                {
                    SqlHelper.ExecuteScalar(transaction, CommandType.StoredProcedure, "U_Cr_New_InstructorApproveReject_U", param.ToArray());
                }
                else
                {
                    SqlHelper.ExecuteScalar(SqlHelper.GetCon(), CommandType.StoredProcedure, "U_Cr_New_InstructorApproveReject_U", param.ToArray());
                }
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public void ActiveInActive(int InstructorChangeRequestID, bool? InActive, int CurUserID)
        {

            SqlParameter[] PLead = new SqlParameter[3];
            PLead[0] = new SqlParameter("@InstructorChangeRequestID", InstructorChangeRequestID);
            PLead[1] = new SqlParameter("@InActive", InActive);
            PLead[2] = new SqlParameter("@CurUserID", CurUserID);
            SqlHelper.ExecuteNonQuery(SqlHelper.GetCon(), CommandType.StoredProcedure, "[AI_InstructorChangeRequest]", PLead);
        }
        private InstructorChangeRequestModel RowOfInstructorChangeRequest(DataRow r)
        {
            InstructorChangeRequestModel InstructorChangeRequest = new InstructorChangeRequestModel();
            InstructorChangeRequest.InstructorChangeRequestID = Convert.ToInt32(r["InstructorChangeRequestID"]);
            InstructorChangeRequest.InstrID = Convert.ToInt32(r["InstrID"]);
            //InstructorChangeRequest.GenderID = Convert.ToInt32(r["GenderID"]);
            //InstructorChangeRequest.GenderName = r["GenderName"].ToString();
            if (r["PicturePath"].ToString() == "" || r["PicturePath"] == null)
            {
                InstructorChangeRequest.PicturePath = "";
            }
            else
            {
                InstructorChangeRequest.PicturePath = Common.GetFileBase64(r["PicturePath"].ToString());
            }
            InstructorChangeRequest.NameOfOrganization = r["NameOfOrganization"].ToString();
            InstructorChangeRequest.InstructorName = r["InstructorName"].ToString();
            InstructorChangeRequest.CNICofInstructor = r["CNICofInstructor"].ToString();
            InstructorChangeRequest.QualificationHighest = r["QualificationHighest"].ToString();
            InstructorChangeRequest.TotalExperience = r["TotalExperience"].ToString();
            InstructorChangeRequest.InActive = Convert.ToBoolean(r["InActive"]);
            InstructorChangeRequest.CreatedUserID = Convert.ToInt32(r["CreatedUserID"]);
            InstructorChangeRequest.ModifiedUserID = Convert.ToInt32(r["ModifiedUserID"]);
            InstructorChangeRequest.CreatedDate = r["CreatedDate"].ToString().GetDate();
            InstructorChangeRequest.ModifiedDate = r["ModifiedDate"].ToString().GetDate();
            InstructorChangeRequest.IsApproved = Convert.ToBoolean(r["IsApproved"]);
            InstructorChangeRequest.IsRejected = Convert.ToBoolean(r["IsRejected"]);

            if (r.Table.Columns.Contains("FilePath"))
            {
                InstructorChangeRequest.FilePath = r["FilePath"].ToString();

            }
            if (r.Table.Columns.Contains("InstructorCRComments"))
            {
                InstructorChangeRequest.InstructorCRComments = r["InstructorCRComments"].ToString();

            }
            if (r.Table.Columns.Contains("TradeID"))
            {
                InstructorChangeRequest.TradeID = Convert.ToInt32(r["TradeID"].ToString());

            }
            if (r.Table.Columns.Contains("SchemeID"))
            {
                InstructorChangeRequest.SchemeID = Convert.ToInt32(r["SchemeID"].ToString());
                InstructorChangeRequest.SchemeName = r["SchemeName"].ToString();

            }
            if (r.Table.Columns.Contains("TSPID"))
            {
                InstructorChangeRequest.TSPID = Convert.ToInt32(r["TSPID"].ToString());
                InstructorChangeRequest.TSPName = r["TSPName"].ToString();

            }
            if (r.Table.Columns.Contains("LocationAddress"))
            {
                InstructorChangeRequest.LocationAddress = r["LocationAddress"].ToString();

            }

            return InstructorChangeRequest;
        }
        private InstructorChangeRequestModel RowOfNewInstructorChangeRequest(DataRow r)
        {
            InstructorChangeRequestModel InstructorChangeRequest = new InstructorChangeRequestModel();
            //InstructorChangeRequest.InstructorChangeRequestID = Convert.ToInt32(r["InstructorChangeRequestID"]);
            //InstructorChangeRequest.InstrID = Convert.ToInt32(r["InstrID"]);
            //InstructorChangeRequest.GenderID = Convert.ToInt32(r["GenderID"]);
            //InstructorChangeRequest.GenderName = r["GenderName"].ToString();
            if (r["PicturePath"].ToString() == "" || r["PicturePath"] == null)
            {
                InstructorChangeRequest.PicturePath = "";
            }
            else
            {
                InstructorChangeRequest.PicturePath = Common.GetFileBase64(r["PicturePath"].ToString());
            }
            InstructorChangeRequest.NameOfOrganization = r["NameOfOrganization"].ToString();
            InstructorChangeRequest.InstructorName = r["InstructorName"].ToString();
            InstructorChangeRequest.CNICofInstructor = r["CNICofInstructor"].ToString();
            InstructorChangeRequest.QualificationHighest = r["QualificationHighest"].ToString();
            InstructorChangeRequest.TotalExperience = r["TotalExperience"].ToString();
            InstructorChangeRequest.InActive = Convert.ToBoolean(r["InActive"]);
            InstructorChangeRequest.CreatedUserID = Convert.ToInt32(r["CreatedUserID"]);
            InstructorChangeRequest.ModifiedUserID = Convert.ToInt32(r["ModifiedUserID"]);
            InstructorChangeRequest.CreatedDate = r["CreatedDate"].ToString().GetDate();
            InstructorChangeRequest.ModifiedDate = r["ModifiedDate"].ToString().GetDate();
            InstructorChangeRequest.IsApproved = Convert.ToBoolean(r["IsApproved"]);
            InstructorChangeRequest.IsRejected = Convert.ToBoolean(r["IsRejected"]);

            if (r.Table.Columns.Contains("CrNewInstructorIsApproved"))
            {
                InstructorChangeRequest.CrNewInstructorIsApproved = Convert.ToBoolean(r["CrNewInstructorIsApproved"]);

            }
            if (r.Table.Columns.Contains("CrNewInstructorIsRejected"))
            {
                InstructorChangeRequest.CrNewInstructorIsRejected = Convert.ToBoolean(r["CrNewInstructorIsRejected"]);

            }
            if (r.Table.Columns.Contains("NewInstructorCRComments"))
            {
                InstructorChangeRequest.NewInstructorCRComments = r["NewInstructorCRComments"].ToString();

            }
            if (r.Table.Columns.Contains("FilePath"))
            {
                InstructorChangeRequest.FilePath = r["FilePath"].ToString();

            }
            if (r.Table.Columns.Contains("TradeID"))
            {
                InstructorChangeRequest.TradeID = Convert.ToInt32(r["TradeID"]);
                InstructorChangeRequest.TradeName =r["TradeName"].ToString();

            }
            if (r.Table.Columns.Contains("ClassID"))
            {
                InstructorChangeRequest.ClassID = Convert.ToInt32(r["ClassID"]);
                //InstructorChangeRequest.ClassCode = r["I_ClassCode"].ToString();

            }
            if (r.Table.Columns.Contains("I_ClassCode"))
            {
                InstructorChangeRequest.ClassCode = r["I_ClassCode"].ToString();

            }
            if (r.Table.Columns.Contains("ClassCode"))
            {
                InstructorChangeRequest.ClassCode = r["ClassCode"].ToString();

            }

            if (r.Table.Columns.Contains("SchemeID"))
            {
                InstructorChangeRequest.SchemeID = Convert.ToInt32(r["SchemeID"]);
                InstructorChangeRequest.SchemeName = r["SchemeName"].ToString();

            }
            if (r.Table.Columns.Contains("TSPID"))
            {
                InstructorChangeRequest.TSPID = Convert.ToInt32(r["TSPID"]);
                InstructorChangeRequest.TSPName = r["TSPName"].ToString();

            }
            if (r.Table.Columns.Contains("CRNewInstructorID"))
            {
                InstructorChangeRequest.CRNewInstructorID = Convert.ToInt32(r["CRNewInstructorID"]);

            }
            if (r.Table.Columns.Contains("LocationAddress"))
            {
                InstructorChangeRequest.LocationAddress = r["LocationAddress"].ToString();

            }

            return InstructorChangeRequest;
        }
        public InstructorChangeRequestModel GetByNewInstructorChangeRequestID_Notification(int CRNewInstructorID, SqlTransaction transaction = null)
        {
            try
            {
                SqlParameter param = new SqlParameter("@CRNewInstructorID", CRNewInstructorID);
                DataTable dt = new DataTable();
                List<InstructorChangeRequestModel> InstructorChangeRequestModel = new List<InstructorChangeRequestModel>();
             //   dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_NewInstructorChangeRequest_Notification", param).Tables[0];

                if (transaction != null)
                {
                    dt = SqlHelper.ExecuteDataset(transaction, CommandType.StoredProcedure, "RD_NewInstructorChangeRequest_Notification", param).Tables[0];
                }
                else
                {
                    dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_NewInstructorChangeRequest_Notification", param).Tables[0];

                }

                if (dt.Rows.Count > 0)
                {
                    InstructorChangeRequestModel = Helper.ConvertDataTableToModel<InstructorChangeRequestModel>(dt);
                    return InstructorChangeRequestModel[0];
                }
                else
                    return null;
            }
            catch (Exception ex) { throw new Exception(ex.Message); }

        }
    }
}
