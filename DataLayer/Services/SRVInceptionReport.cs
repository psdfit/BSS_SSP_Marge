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
    public class SRVInceptionReport : SRVBase, DataLayer.Interfaces.ISRVInceptionReport
    {
        private readonly ISRVSendEmail srvSendEmail;
        private readonly ISRVTSPMaster srvTSPMaster;
        private readonly ISRVUsers srvUsers;

        public SRVInceptionReport(ISRVSendEmail srvSendEmail, ISRVTSPMaster srvTSPMaster, ISRVUsers srvUsers)
        {
            this.srvSendEmail = srvSendEmail;
            this.srvTSPMaster = srvTSPMaster;
            this.srvUsers = srvUsers;
        }
        public InceptionReportModel GetByIncepReportID(int IncepReportID)
        {
            try
            {
                SqlParameter param = new SqlParameter("@IncepReportID", IncepReportID);
                DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_InceptionReport", param).Tables[0];
                if (dt.Rows.Count > 0)
                {
                    return RowOfInceptionReport(dt.Rows[0]);
                }
                else
                    return null;
            }
            catch (Exception ex) { throw new Exception(ex.Message); }

        }  
        public List<InceptionReportModel> GetByInceptionReportID(int IncepReportID)   // To get current incetion report in the change request approvals
        {
            try
            {
                SqlParameter param = new SqlParameter("@IncepReportID", IncepReportID);
                DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_InceptionReport", param).Tables[0];
               
                return LoopinData(dt);
                
            }
            catch (Exception ex) { throw new Exception(ex.Message); }

        }
        public List<InstructorReplaceChangeRequestModel> GetMappedInstructorsByID(int IncepReportID)   // To get current Instructors of class in the change request approvals
        {
            try
            {
                SqlParameter param = new SqlParameter("@IncepReportID", IncepReportID);
                DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_InstructorsByInceptionID", param).Tables[0];
               
                return LoopinClassInstructorsData(dt);
                
            }
            catch (Exception ex) { throw new Exception(ex.Message); }

        }
        public List<InceptionReportModel> SaveInceptionReport(InceptionReportModel InceptionReport)
        {
            try
            {

                SqlParameter[] param = new SqlParameter[21];
                param[0] = new SqlParameter("@IncepReportID", InceptionReport.IncepReportID);
                param[1] = new SqlParameter("@ClassID", InceptionReport.ClassID);
                param[2] = new SqlParameter("@ClassStartTime", InceptionReport.ClassStartTime);
                param[3] = new SqlParameter("@ClassEndTime", InceptionReport.ClassEndTime);
                param[4] = new SqlParameter("@ActualStartDate", InceptionReport.ActualStartDate);
                param[5] = new SqlParameter("@ActualEndDate", InceptionReport.ActualEndDate);
                param[6] = new SqlParameter("@ClassTotalHours", InceptionReport.ClassTotalHours);
                param[7] = new SqlParameter("@EnrolledTrainees", InceptionReport.EnrolledTrainees);
                param[8] = new SqlParameter("@Shift", InceptionReport.Shift);
                //param[9] = new SqlParameter("@CenterLocation", InceptionReport.CenterLocation);
                param[9] = new SqlParameter("@Monday", InceptionReport.Monday);
                param[10] = new SqlParameter("@Tuesday", InceptionReport.Tuesday);
                param[11] = new SqlParameter("@Wednesday", InceptionReport.Wednesday);
                param[12] = new SqlParameter("@Thursday", InceptionReport.Thursday);
                param[13] = new SqlParameter("@Friday", InceptionReport.Friday);
                param[14] = new SqlParameter("@Saturday", InceptionReport.Saturday);
                param[15] = new SqlParameter("@Sunday", InceptionReport.Sunday);
                param[16] = new SqlParameter("@FinalSubmitted", InceptionReport.FinalSubmitted);
                param[17] = new SqlParameter("@SectionID", InceptionReport.SectionID);
                param[18] = new SqlParameter("@CurUserID", InceptionReport.CurUserID);
                param[19] = new SqlParameter("@InstrIDs", InceptionReport.InstrIDs);
                param[20] = new SqlParameter("@Ident", SqlDbType.Int);
                param[20].Direction = ParameterDirection.Output;
                SqlHelper.ExecuteNonQuery(SqlHelper.GetCon(), CommandType.StoredProcedure, "[AU_InceptionReport]", param);
                
                    new SRVContactPerson().BatchInsert(InceptionReport.ContactPersons, Convert.ToInt32(param[20].Value), InceptionReport.CurUserID);
                if (InceptionReport.FinalSubmitted == true)
                {
                    UpdateClassStatusByReport(InceptionReport.ClassID, InceptionReport.CurUserID);
                    // Send push notofication to KAM and TSP 
                    UsersModel KamUser = srvTSPMaster.KAMUserByTSPUserID(InceptionReport.CurUserID);
                    ApprovalModel approvalsModelForNotificarion = new ApprovalModel();
                    ApprovalHistoryModel model1 = new ApprovalHistoryModel();
                    approvalsModelForNotificarion.UserIDs = InceptionReport.CurUserID.ToString();
                    approvalsModelForNotificarion.UserIDs += "," + KamUser.UserID;
                    approvalsModelForNotificarion.ProcessKey = EnumApprovalProcess.Inception_report_submit;
                    approvalsModelForNotificarion.CustomComments = "Inception report against class code (" + InceptionReport.ClassCode + ") has been submitted and class status is ACTIVE now";
                    srvSendEmail.GenerateEmailAndSendNotification(approvalsModelForNotificarion, model1);
                }


                return FetchInceptionReport(InceptionReport.ClassID);
            }
            catch (Exception ex)
            { throw new Exception(ex.Message); }
        }
        public void UpdateClassStatusByReport(int ClassID, int CurUserID)
        {
            try
            {
                SqlParameter[] PLead = new SqlParameter[2];
                PLead[0] = new SqlParameter("@ClassID", ClassID);
                PLead[1] = new SqlParameter("@CurUserID", CurUserID);

                SqlHelper.ExecuteNonQuery(SqlHelper.GetCon(), CommandType.StoredProcedure, "[AU_ClassStatusByReport]", PLead);
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }
        public List<InceptionReportModel> FetchInceptionReport(InceptionReportModel mod)
        {
            try
            {
                DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_InceptionReport", Common.GetParams(mod)).Tables[0];
                return LoopinData(dt);
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }
        public List<CheckRegistrationCriteriaModel> CheckReportCriteria(int ClassID)

        {
            try
            {
                SqlParameter param = new SqlParameter("@ClassID", ClassID);
                DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "CheckReportSubmissionCriteria", param).Tables[0];
                if (dt.Rows.Count > 0)
                {
                    return LoopinReportCriteriaData(dt);
                }
                else
                {
                    return new List<CheckRegistrationCriteriaModel>();
                }

            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }


        public List<InceptionReportModel> FetchInceptionReport(int ClassId)
        {
            try
            {
                DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_InceptionReport", new SqlParameter("@ClassID", ClassId)).Tables[0];
                return LoopinData(dt);
            }
            catch (Exception ex) { throw new Exception(ex.Message); }

        }

        public List<InceptionReportModel> FetchActiveClassInception(string InstruIds)
        {
            try
            {
                DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_InceptionReportActiveClass", new SqlParameter("@InstruIds", InstruIds)).Tables[0];
                return LoopinData(dt);
            }
            catch (Exception ex) { throw new Exception(ex.Message); }

        }

        
        public List<InceptionReportModel> FetchActiveClassInceptionCR(string ClassID)
        {
            try
            {
                DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_InceptionReportActiveClass", new SqlParameter("@ClassID", ClassID)).Tables[0];
                return LoopinData(dt);
            }
            catch (Exception ex) { throw new Exception(ex.Message); }

        }
        public List<InceptionReportListModel> FetchInceptionReportByFilters(int[] filters)
        {
            List<InceptionReportListModel> list = new List<InceptionReportListModel>();
            if (filters.Length > 0)
            {
                int schemeId = filters[0];
                int tspId = filters[1];
                int classId = filters[2];
                int userId = filters[3];
                int oID = filters[4];
                try
                {
                    SqlParameter[] param = new SqlParameter[10];
                    param[0] = new SqlParameter("@SchemeID", schemeId);
                    param[1] = new SqlParameter("@TSPID", tspId);
                    param[2] = new SqlParameter("@ClassID", classId);
                    param[3] = new SqlParameter("@UserID", userId);
                    param[4] = new SqlParameter("@OID", oID);
                    DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_InceptionReportListByFilters", param).Tables[0];
                    list = LoopinListData(dt);
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            return list;
        }

        public List<InceptionReportModel> FetchInceptionReport(bool InActive)
        {
            try
            {
                DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_InceptionReport", new SqlParameter("@InActive", InActive)).Tables[0];
                return LoopinData(dt);
            }
            catch (Exception ex) { throw new Exception(ex.Message); }

        }

        public List<InceptionReportModel> FetchInceptionReportDataByUser(int UserID)
        {
            try
            {
                List<SqlParameter> param = new List<SqlParameter>();
                param.Add(new SqlParameter("@UserID", UserID));

                DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_InceptionReportByTSPUser", param.ToArray()).Tables[0];
                return LoopinData(dt);
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }
        public void ActiveInActive(int IncepReportID, bool? InActive, int CurUserID)
        {

            SqlParameter[] PLead = new SqlParameter[3];
            PLead[0] = new SqlParameter("@IncepReportID", IncepReportID);
            PLead[1] = new SqlParameter("@InActive", InActive);
            PLead[2] = new SqlParameter("@CurUserID", CurUserID);
            SqlHelper.ExecuteNonQuery(SqlHelper.GetCon(), CommandType.StoredProcedure, "[AI_InceptionReport]", PLead);
        }
        private InceptionReportModel RowOfInceptionReport(DataRow r)
        {
            InceptionReportModel InceptionReport = new InceptionReportModel();
            InceptionReport.IncepReportID = Convert.ToInt32(r["IncepReportID"]);
            InceptionReport.ClassID = Convert.ToInt32(r["ClassID"]);
            if (r.Table.Columns.Contains("StartDateTime"))
            {
                InceptionReport.StartDateTime = r["StartDateTime"].ToString();
                InceptionReport.EndDateTime = r["EndDateTime"].ToString();
            }
            else
            {
                InceptionReport.ClassStartTime = r["ClassStartTime"].ToString().GetDate();
                InceptionReport.ClassEndTime = r["ClassEndTime"].ToString().GetDate();
            }
            
            InceptionReport.ActualStartDate = r["ActualStartDate"].ToString().GetDate();
            InceptionReport.ActualEndDate = r["ActualEndDate"].ToString().GetDate();
            InceptionReport.ClassTotalHours = r["ClassTotalHours"].ToString();
            InceptionReport.EnrolledTrainees = Convert.ToInt32(r["EnrolledTrainees"]);
            InceptionReport.Shift = r["Shift"].ToString();
            //InceptionReport.CenterLocation = r["CenterLocation"].ToString();
            InceptionReport.Monday = Convert.ToBoolean(r["Monday"]);
            InceptionReport.Tuesday = Convert.ToBoolean(r["Tuesday"]);
            InceptionReport.Wednesday = Convert.ToBoolean(r["Wednesday"]);
            InceptionReport.Thursday = Convert.ToBoolean(r["Thursday"]);
            InceptionReport.Friday = Convert.ToBoolean(r["Friday"]);
            InceptionReport.Saturday = Convert.ToBoolean(r["Saturday"]);
            InceptionReport.Sunday = Convert.ToBoolean(r["Sunday"]);
            InceptionReport.FinalSubmitted = Convert.ToBoolean(r["FinalSubmitted"]);
            InceptionReport.SectionID = Convert.ToInt32(r["SectionID"]);
            InceptionReport.SectionName = r["SectionName"].ToString();
            InceptionReport.InstrIDs = r["InstrIDs"].ToString();
            InceptionReport.InActive = Convert.ToBoolean(r["InActive"]);
            InceptionReport.CreatedUserID = Convert.ToInt32(r["CreatedUserID"]);
            InceptionReport.ModifiedUserID = Convert.ToInt32(r["ModifiedUserID"]);
            InceptionReport.CreatedDate = r["CreatedDate"].ToString().GetDate();
            InceptionReport.ModifiedDate = r["ModifiedDate"].ToString().GetDate();
            if (r.Table.Columns.Contains("ClassCode"))
            {
                InceptionReport.ClassCode = r["ClassCode"].ToString();
            }
            if (r.Table.Columns.Contains("TradeName"))
            {
                InceptionReport.TradeName = r["TradeName"].ToString();
            }
            if (r.Table.Columns.Contains("InceptionReportCrID"))
            {
                InceptionReport.InceptionReportCrID = Convert.ToInt32(r["InceptionReportCrID"]);
            }
            if (r.Table.Columns.Contains("IrCrIsApproved"))
            {
                InceptionReport.IrCrIsApproved = Convert.ToBoolean(r["IrCrIsApproved"]);
            }
            if (r.Table.Columns.Contains("IrCrIsRejected"))
            {
                InceptionReport.IrCrIsRejected = Convert.ToBoolean(r["IrCrIsRejected"]);
            }

            return InceptionReport;
        }
        private InceptionReportListModel RowOfInceptionReportList(DataRow r)
        {
            InceptionReportListModel InceptionReport = new InceptionReportListModel();
            InceptionReport.IncepReportID = Convert.ToInt32(r["IncepReportID"]);

            InceptionReport.SchemeName = r["SchemeName"].ToString();
            InceptionReport.TSPName = r["TSPName"].ToString();
            InceptionReport.ClassID = Convert.ToInt32(r["ClassID"]);
            InceptionReport.ClassCode = r["ClassCode"].ToString();
            InceptionReport.CenterName = r["CenterName"].ToString();
            InceptionReport.AddressOfTrainingCenterTheoratical = r["AddressOfTrainingCenterTheoratical"].ToString();
            InceptionReport.InchargeNameTheoratical = r["InchargeNameTheoratical"].ToString();
            InceptionReport.InchargeContactTheoratical = r["InchargeContactTheoratical"].ToString();
            InceptionReport.AddressOfTrainingCenterPractical = r["AddressOfTrainingCenterPractical"].ToString();
            InceptionReport.InchargeNamePractical = r["InchargeNamePractical"].ToString();
            InceptionReport.InchargeContactPractical = r["InchargeContactPractical"].ToString();
            InceptionReport.NameOfAuthorizedPerson = r["NameOfAuthorizedPerson"].ToString();
            InceptionReport.MobileContactOfAuthorizedPerson = r["MobileContactOfAuthorizedPerson"].ToString();
            InceptionReport.EmailOfAuthorizedPerson = r["EmailOfAuthorizedPerson"].ToString();
            InceptionReport.TehsilName = r["TehsilName"].ToString();
            InceptionReport.DistrictName = r["DistrictName"].ToString();
            InceptionReport.TradeName = r["TradeName"].ToString();
            InceptionReport.Batch = r["Batch"].ToString();
            InceptionReport.ClassStartTime = r["ClassStartTime"].ToString().GetDate();
            InceptionReport.ClassEndTime = r["ClassEndTime"].ToString().GetDate();
            InceptionReport.StartDate = r["StartDate"].ToString().GetDate();
            InceptionReport.EndDate = r["EndDate"].ToString().GetDate();
            InceptionReport.ClassTotalHours = r["ClassTotalHours"].ToString();
            InceptionReport.EnrolledTrainees = Convert.ToInt32(r["EnrolledTrainees"]);
            InceptionReport.Shift = r["Shift"].ToString();
            InceptionReport.GenderName = r["GenderName"].ToString();
            //InceptionReport.CenterLocation = r["CenterLocation"].ToString();
            InceptionReport.Monday = Convert.ToBoolean(r["Monday"]);
            InceptionReport.Tuesday = Convert.ToBoolean(r["Tuesday"]);
            InceptionReport.Wednesday = Convert.ToBoolean(r["Wednesday"]);
            InceptionReport.Thursday = Convert.ToBoolean(r["Thursday"]);
            InceptionReport.Friday = Convert.ToBoolean(r["Friday"]);
            InceptionReport.Saturday = Convert.ToBoolean(r["Saturday"]);
            InceptionReport.Sunday = Convert.ToBoolean(r["Sunday"]);
            InceptionReport.FinalSubmitted = Convert.ToBoolean(r["FinalSubmitted"]);
            InceptionReport.SectionID = Convert.ToInt32(r["SectionID"]);
            InceptionReport.SectionName = r["SectionName"].ToString();
            InceptionReport.InstrIDs = r["InstrIDs"].ToString();
            InceptionReport.TrainingDaysNo = r["TrainingDaysNo"].ToString();
            InceptionReport.TrainingDays = r["TrainingDays"].ToString();
            InceptionReport.InstructorInfo = r["InstructorInfo"].ToString();


            return InceptionReport;
        }
        private InstructorReplaceChangeRequestModel RowOfClassInstructors(DataRow r)
        {
            InstructorReplaceChangeRequestModel InstrReplace = new InstructorReplaceChangeRequestModel();
            InstrReplace.IncepReportID = Convert.ToInt32(r["IncepReportID"]);

            InstrReplace.SchemeName = r["SchemeName"].ToString();
            InstrReplace.TSPName = r["TSPName"].ToString();
            InstrReplace.ClassCode = r["ClassCode"].ToString();           
            InstrReplace.TradeName = r["TradeName"].ToString();                     
            InstrReplace.InstrIDs = r["InstrIDs"].ToString();
            InstrReplace.InstructorName = r["InstructorName"].ToString();
           

            return InstrReplace;
        }

        private CheckRegistrationCriteriaModel RowOfheckReportCriteria(DataRow r)
        {
            CheckRegistrationCriteriaModel model = new CheckRegistrationCriteriaModel();
            model.ErrorMessage = r.Field<string>("ErrorMessage");
            model.ErrorTypeName = r.Field<string>("ErrorTypeName");
            return model;
        }

        private List<InceptionReportListModel> LoopinListData(DataTable dt)
        {
            List<InceptionReportListModel> InceptionReportL = new List<InceptionReportListModel>();

            foreach (DataRow r in dt.Rows)
            {
                InceptionReportL.Add(RowOfInceptionReportList(r));

            }
            return InceptionReportL;
        }
        private List<InceptionReportModel> LoopinData(DataTable dt)
        {
            List<InceptionReportModel> InceptionReportL = new List<InceptionReportModel>();

            foreach (DataRow r in dt.Rows)
            {
                InceptionReportL.Add(RowOfInceptionReport(r));

            }
            return InceptionReportL;
        }
        private List<InstructorReplaceChangeRequestModel> LoopinClassInstructorsData(DataTable dt)
        {
            List<InstructorReplaceChangeRequestModel> InstructorReplaceL = new List<InstructorReplaceChangeRequestModel>();

            foreach (DataRow r in dt.Rows)
            {
                InstructorReplaceL.Add(RowOfClassInstructors(r));

            }
            return InstructorReplaceL;
        }
        
        private List<CheckRegistrationCriteriaModel> LoopinReportCriteriaData(DataTable dt)
        {
            List<CheckRegistrationCriteriaModel> list = new List<CheckRegistrationCriteriaModel>();

            foreach (DataRow r in dt.Rows)
            {
                list.Add(RowOfheckReportCriteria(r));

            }
            return list;
        }
        public List<InceptionReportListModel> FetchInceptionReportByPaged(PagingModel pagingModel, SearchFilter filterModel, out int totalCount)
        {
            try
            {
                List<SqlParameter> param = new List<SqlParameter>();
                param.Add(new SqlParameter("@SchemeID", filterModel.SchemeID));
                param.Add(new SqlParameter("@TSPID", filterModel.TSPID));
                param.Add(new SqlParameter("@ClassID", filterModel.ClassID));
                param.Add(new SqlParameter("@TraineeID", filterModel.TraineeID));
                param.Add(new SqlParameter("@UserID", filterModel.UserID));
                param.Add(new SqlParameter("@OID", filterModel.OID));
                param.AddRange(Common.GetPagingParams(pagingModel));

                DataTable dt = new DataTable();
                dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_InceptionReportListByPaged", param.ToArray()).Tables[0];
                if (dt.Rows.Count > 0)
                    totalCount = Convert.ToInt32(dt.Rows[0]["TotalRows"]);
                else
                    totalCount = 0;
                return LoopinListData(dt);
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }
    }
}
