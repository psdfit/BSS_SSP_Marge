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
    public class SRVVisitPlan : SRVBase, ISRVVisitPlan
    {
        private readonly ISRVSendEmail srvSendEmail;
        public SRVVisitPlan(ISRVSendEmail srvSendEmail) {
            this.srvSendEmail = srvSendEmail;
        }
        public VisitPlanModel GetByVisitPlanID(int VisitPlanID)
        {
            try
            {
                SqlParameter param = new SqlParameter("@VisitPlanID", VisitPlanID);
                DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_VisitPlan", param).Tables[0];
                if (dt.Rows.Count > 0)
                {
                    return RowOfVisitPlan(dt.Rows[0]);
                }
                else
                    return null;
            }
            catch (Exception ex) { throw new Exception(ex.Message); }

        }
        public void SaveNewCallCenterAgent(UserEventMapModel callCenterAgent)
        {
            try
            {

                SqlParameter[] param = new SqlParameter[5];
                //param[0] = new SqlParameter("@CallCenterAgentID", callCenterAgent.CallCenterAgentID);
                param[0] = new SqlParameter("@NominatedPersonName", callCenterAgent.NominatedPersonName);
                param[1] = new SqlParameter("@NominatedPersonContactNumber", callCenterAgent.NominatedPersonContactNumber);
                param[2] = new SqlParameter("@VisitPlanID", callCenterAgent.VisitPlanID);
                param[3] = new SqlParameter("@CurUserID", callCenterAgent.CurUserID);
                param[4] = new SqlParameter("@UserID", callCenterAgent.UserID);

                SqlHelper.ExecuteNonQuery(SqlHelper.GetCon(), CommandType.StoredProcedure, "[AU_CallCenterAgent]", param);
            }

            catch (Exception ex)
            { throw new Exception(ex.Message); }
        }
        
        public List<VisitPlanModel> SaveVisitPlan(VisitPlanModel VisitPlan)
        {
            try
            {

                //FilePaths path = new FilePaths();
                ApprovalModel approvalModel = new ApprovalModel();
                ApprovalHistoryModel approvalHistoryModel = new ApprovalHistoryModel();
                string visitplanAttachmentPath = Common.AddFile(VisitPlan.Attachment, FilePaths.VisitPlan_FILE_DIR);
                SqlParameter[] param = new SqlParameter[17];
                param[0] = new SqlParameter("@VisitPlanID", VisitPlan.VisitPlanID);
                param[1] = new SqlParameter("@VisitType", VisitPlan.VisitType);
                //param[2] = new SqlParameter("@UserID", VisitPlan.UserID);
                param[2] = new SqlParameter("@VisitStartDate", VisitPlan.VisitStartDate);
                param[3] = new SqlParameter("@VisitEndDate", VisitPlan.VisitEndDate);
                param[4] = new SqlParameter("@VisitStartTime", VisitPlan.VisitStartTime);
                param[5] = new SqlParameter("@VisitEndTime", VisitPlan.VisitEndTime);
                //param[5] = new SqlParameter("@Attachment", Common.AddFile(VisitPlan.Attachment, new FilePaths().INSTRUCTOR_FILE_DIR)); ; //Common.AddFile(Instructor.PicturePath, new FilePaths().INSTRUCTOR_FILE_DIR));
                param[6] = new SqlParameter("@Attachment", visitplanAttachmentPath); //Common.AddFile(Instructor.PicturePath, new FilePaths().INSTRUCTOR_FILE_DIR));
                param[7] = new SqlParameter("@Comments", VisitPlan.Comments);
                param[8] = new SqlParameter("@RegionID", VisitPlan.RegionID);
                param[9] = new SqlParameter("@ClusterID", VisitPlan.ClusterID);
                param[10] = new SqlParameter("@DistrictID", VisitPlan.DistrictID);
                param[11] = new SqlParameter("@Venue", VisitPlan.Venue);
                param[12] = new SqlParameter("@LinkWithCRM", VisitPlan.LinkWithCRM);
                //param[9] = new SqlParameter("@ClassID", VisitPlan.ClassID);
                param[13] = new SqlParameter("@IsVisited", VisitPlan.IsVisited);
                //param[14] = new SqlParameter("@SchemeID", VisitPlan.SchemeID);

                param[14] = new SqlParameter("@CurUserID", VisitPlan.CreatedUserID);
                param[15] = new SqlParameter("@Ident", SqlDbType.Int);
                param[15].Direction = ParameterDirection.Output;
                SqlHelper.ExecuteNonQuery(SqlHelper.GetCon(), CommandType.StoredProcedure, "[AU_VisitPlan]", param);
                new SRVUserEventMap().BatchInsert(VisitPlan.EventUsers, Convert.ToInt32(param[15].Value), VisitPlan.CurUserID);
                new SRVClassEventMap().BatchInsert(VisitPlan.EventClasses, Convert.ToInt32(param[15].Value), VisitPlan.CurUserID);
                new SRVUserEventMap().BatchInsertSchemes(VisitPlan.EventSchemes, Convert.ToInt32(param[15].Value), VisitPlan.CurUserID);

                if (VisitPlan.VisitType == "5")
                {
                    if (VisitPlan.VisitPlanID == 0)
                    {
                        List<object> list = new List<object>();

                        foreach (UserEventMapModel uv in VisitPlan.EventUsers)
                        {

                            list.Add(new SRVUsers().GetByUserID(uv.UserID));
                        }

                        foreach (UsersModel uv in list)
                        {
                            string subject, body;

                            subject = "PSDF Visit Plan Notification";
                            if (VisitPlan.VisitType == "5" && VisitPlan.editor != null)
                            {
                                body = VisitPlan.editor;
                            }
                            else
                            {
                                 body = "Dear User, \nYour have been assigned a new Event/Visit Click on this link to View: http://mis.psdf.org.pk/test/";
                                
                            }
                            if (uv.Email != null && uv.Email != "")
                            {
                                Common.SendEmail(uv.Email, subject, body);
                                approvalModel.ProcessKey = EnumApprovalProcess.CALENDER_EV;
                                approvalModel.isUserMapping = true;
                                approvalModel.CurUserID = VisitPlan.CreatedUserID ?? 0;
                                approvalHistoryModel.EmailSentOrNotBit = false;
                                approvalModel.UserIDs = uv.UserID.ToString();
                                int VisitTypeID =Convert.ToInt32( VisitPlan.VisitType);
                                var VisitTypeName = (EnumEventType)VisitTypeID;
                                approvalModel.CustomComments = ",(Event Type : " + VisitTypeName.ToString()+")";
                                srvSendEmail.GenerateEmailAndSendNotification(approvalModel, approvalHistoryModel);
                            }
                            else
                            {
                            }


                        }
                    }
                }
                else {
                   
                    foreach (var item in VisitPlan.EventUsers)
                    {
                        approvalModel.UserIDs += item.UserID.ToString() + ",";
                    }
                    approvalModel.ProcessKey = EnumApprovalProcess.CALENDER_EV;
                    approvalModel.isUserMapping = true;
                    approvalModel.CurUserID = VisitPlan.CreatedUserID ?? 0;
                    int VisitTypeID = Convert.ToInt32(VisitPlan.VisitType);
                    var VisitTypeName = (EnumEventType)VisitTypeID;
                    approvalModel.CustomComments = ",(Event Type : " + VisitTypeName.ToString()+")";
                    srvSendEmail.GenerateEmailAndSendNotification(approvalModel, approvalHistoryModel);
                }
                return FetchByStartDate(VisitPlan.VisitStartDate);
            }

            catch (Exception ex)
            { throw new Exception(ex.Message); }
        }
        private List<VisitPlanModel> LoopinData(DataTable dt)
        {
            List<VisitPlanModel> VisitPlanL = new List<VisitPlanModel>();

            foreach (DataRow r in dt.Rows)
            {
                VisitPlanL.Add(RowOfVisitPlan(r));

            }
            return VisitPlanL;
        } 
        private List<UsersModel> LoopinUserData(DataTable dt)
        {
            List<UsersModel> VisitPlanL = new List<UsersModel>();

            foreach (DataRow r in dt.Rows)
            {
                VisitPlanL.Add(RowOfTSPUsers(r));

            }
            return VisitPlanL;
        } 
        private List<VisitPlanModel> LoopinCallCenterData(DataTable dt)
        {
            List<VisitPlanModel> VisitPlanL = new List<VisitPlanModel>();

            foreach (DataRow r in dt.Rows)
            {
                VisitPlanL.Add(RowOfCallCenterVisitPlan(r));

            }
            return VisitPlanL;
        }
        private List<UserEventMapModel> LoopinEventUserData(DataTable dt)
        {
            List<UserEventMapModel> VisitPlanL = new List<UserEventMapModel>();

            foreach (DataRow r in dt.Rows)
            {
                VisitPlanL.Add(RowOfEventUsers(r));

            }
            return VisitPlanL;
        }
        private List<VisitPlanModel> LoopinUserEventReportData(DataTable dt)
        {
            List<VisitPlanModel> VisitPlanL = new List<VisitPlanModel>();

            foreach (DataRow r in dt.Rows)
            {
                VisitPlanL.Add(RowOfUserEventReport(r));

            }
            return VisitPlanL;
        }
        public List<VisitPlanModel> FetchVisitPlan(VisitPlanModel mod)
        {
            try
            {
                DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_VisitPlan", Common.GetParams(mod)).Tables[0];
                return LoopinData(dt);
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        } 
        
        public List<VisitPlanModel> FetchCallCenterVisitPlan()
        {
            try
            {
                DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_CallCenterVisitPlan").Tables[0];
                return LoopinCallCenterData(dt);
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }

        public List<VisitPlanModel> FetchCalendarVisitPlan(VisitPlanModel mod)
        {
            try
            {
                DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_Calendar_VisitPlan", Common.GetParams(mod)).Tables[0];
                return LoopinData(dt);
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }
        
        public List<VisitPlanModel> GetUserEventReport(int visitplanid)
        {
            try
            {
                DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "Get_UserEvent_Report", new SqlParameter("@VisitPlanID", visitplanid)).Tables[0];
                return LoopinUserEventReportData(dt);
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }
        
        public List<UserEventMapModel> GetEventUsers(int visitplanid)
        {
            try
            {
                DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "Get_Event_Users", new SqlParameter("@VisitPlanID", visitplanid)).Tables[0];
                return LoopinEventUserData(dt);
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }

        public int BatchInsert(List<VisitPlanModel> ls, int @BatchFkey, int CurUserID)
        {
            SqlParameter[] param = new SqlParameter[3];

            param[0] = new SqlParameter("@Json", JsonConvert.SerializeObject(ls));
            param[1] = new SqlParameter("@", @BatchFkey);
            param[2] = new SqlParameter("@CurUserID", CurUserID);
            return SqlHelper.ExecuteNonQuery(SqlHelper.GetCon(), CommandType.StoredProcedure, "[BAU_UserEventMap]", param);
        }
        public List<VisitPlanModel> FetchVisitPlanByDate(DateTime? date)
        {
            try
            {
                DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_VisitPlan", new SqlParameter("@VisitDate", date)).Tables[0];
                return LoopinData(dt);
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }

        public List<VisitPlanModel> FetchVisitPlan()
        {
            try
            {
                DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_VisitPlan").Tables[0];
                return LoopinData(dt);
            }
            catch (Exception ex) { throw new Exception(ex.Message); }

        }
        public List<VisitPlanModel> FetchVisitPlan(bool InActive)
        {
            try
            {
                DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_VisitPlan", new SqlParameter("@InActive", InActive)).Tables[0];
                return LoopinData(dt);
            }
            catch (Exception ex) { throw new Exception(ex.Message); }

        }
        public List<UsersModel> FetchTSPUsers(String ids)
        {
            try
            {
                DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_TSPEventUsersByScheme", new SqlParameter("@SchemeID", ids)).Tables[0];
                return LoopinUserData(dt);
            }
            catch (Exception ex) { throw new Exception(ex.Message); }

        }
        public List<VisitPlanModel> GetByUserID(int UserID)
        {
            try
            {
                DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_VisitPlan", new SqlParameter("@UserID", UserID)).Tables[0];
                return LoopinData(dt);
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }
        public List<VisitPlanModel> FetchByClassID(int? ClassID)
        {
            try
            {
                DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_VisitPlan", new SqlParameter("@ClassID", ClassID)).Tables[0];
                return LoopinData(dt);
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }

        public List<VisitPlanModel> FetchByStartDate(DateTime? VisitStartDate)
        {
            try
            {
                DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_Calendar_VisitPlan", new SqlParameter("@VisitStartDate", VisitStartDate)).Tables[0];
                return LoopinData(dt);
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }

        public List<VisitPlanModel> GetByVisitType(int userLevel)
        {
            try
            {
                DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_VisitPlan", new SqlParameter("@VisitType", userLevel)).Tables[0];
                return LoopinData(dt);
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }

        public void UpdateUserEventStatus(UserEventMapModel uvm)
        {

            SqlHelper.ExecuteNonQuery(SqlHelper.GetCon(), CommandType.StoredProcedure, "[Update_UserEvent_Status]", Common.GetParams(uvm));
        }
        public void UpdateCallCenterAgentEventStatus(UserEventMapModel vpm)
        {
            SqlParameter[] PLead = new SqlParameter[3];
            PLead[0] = new SqlParameter("@VisitPlanID", vpm.VisitPlanID);
            PLead[1] = new SqlParameter("@UserStatusByCallCenter", vpm.UserStatusByCallCenter);
            PLead[2] = new SqlParameter("@UserID", vpm.UserID);

            SqlHelper.ExecuteNonQuery(SqlHelper.GetCon(), CommandType.StoredProcedure, "[Update_CallCeter_Agent_Event_Status]", PLead);
        }


        public void ActiveInActive(int VisitPlanID, bool? InActive, int CurUserID)
        {

            SqlParameter[] PLead = new SqlParameter[3];
            PLead[0] = new SqlParameter("@VisitPlanID", VisitPlanID);
            PLead[1] = new SqlParameter("@InActive", InActive);
            PLead[2] = new SqlParameter("@CurUserID", CurUserID);
            SqlHelper.ExecuteNonQuery(SqlHelper.GetCon(), CommandType.StoredProcedure, "[AI_VisitPlan]", PLead);
        }
        private VisitPlanModel RowOfUserEventReport(DataRow r)
        {
            VisitPlanModel VisitPlan = new VisitPlanModel();
            VisitPlan.TSPName = r["TSPName"].ToString();
            VisitPlan.ClusterName = r["ClusterName"].ToString();
            VisitPlan.RegionName = r["RegionName"].ToString();
            VisitPlan.DistrictName = r["DistrictName"].ToString();
            VisitPlan.UserStatus = r["UserStatus"].ToString();
            VisitPlan.UserStatusByCallCenter = r["UserStatusByCallCenter"].ToString();
            VisitPlan.CPLandline = r["CPLandline"].ToString();
            VisitPlan.HeadName = r["HeadName"].ToString();
            VisitPlan.CPAdmissionsName = r["CPAdmissionsName"].ToString();
            VisitPlan.NominatedPersonName = r["NominatedPersonName"].ToString();
            VisitPlan.NominatedPersonContactNumber = r["NominatedPersonContactNumber"].ToString();
           

            return VisitPlan;
        }
        private UsersModel RowOfTSPUsers(DataRow r)
        {
            UsersModel user = new UsersModel();
            user.UserID = Convert.ToInt32(r["UserID"]);
            user.FullName = r["FullName"].ToString();
            user.Email = r["Email"].ToString();

            return user;
        }
        private UserEventMapModel RowOfEventUsers(DataRow r)
        {
            UserEventMapModel user = new UserEventMapModel();
            user.VisitPlanID = Convert.ToInt32(r["VisitPlanID"]);
            user.UserID = Convert.ToInt32(r["UserID"]);
            user.FullName = r["FullName"].ToString();
            user.Email = r["Email"].ToString();
            user.UserStatusByCallCenter = r["UserStatusByCallCenter"].ToString();

            return user;
        }
        private VisitPlanModel RowOfVisitPlan(DataRow r)
        {
            VisitPlanModel VisitPlan = new VisitPlanModel();
            VisitPlan.VisitPlanID = Convert.ToInt32(r["VisitPlanID"]);
            VisitPlan.VisitType = r["VisitType"].ToString();
            VisitPlan.VisitTypeName = r["VisitTypeName"].ToString();
            //VisitPlan.UserID = Convert.ToInt32(r["UserID"]);
            VisitPlan.RegionID = Convert.ToInt32(r["RegionID"]);
            VisitPlan.ClusterID = Convert.ToInt32(r["ClusterID"]);
            VisitPlan.DistrictID = Convert.ToInt32(r["DistrictID"]);
            //VisitPlan.SchemeID = Convert.ToInt32(r["SchemeID"]);
            //VisitPlan.UserName = r["UserName"].ToString();
            VisitPlan.VisitStartDate = r["VisitStartDate"].ToString().GetDate();
            VisitPlan.VisitEndDate = r["VisitEndDate"].ToString().GetDate();
            VisitPlan.VisitStartTime = r["VisitStartTime"].ToString().GetDate();
            VisitPlan.VisitEndTime = r["VisitEndTime"].ToString().GetDate();
            VisitPlan.Attachment = string.IsNullOrEmpty(r["Attachment"].ToString()) ? string.Empty : Common.GetFileBase64(r["Attachment"].ToString());
            //if (r["Attachment"].ToString() == "" || r["Attachment"] == null)
            //    VisitPlan.Attachment = "";
            //else
            //    VisitPlan.Attachment = Common.GetFileBase64(r["Attachment"].ToString());

            //VisitPlan.Attachment = r["Attachment"].ToString();
            VisitPlan.Comments = r["Comments"].ToString();
            VisitPlan.Venue = r["Venue"].ToString();
            if (r.Table.Columns.Contains("UserStatus"))
            {
                VisitPlan.UserStatus = r["UserStatus"].ToString();
            }
            //VisitPlan.UserStatus = r["UserStatus"].ToString();
            //VisitPlan.ClassID = Convert.ToInt32(r["ClassID"]);
            VisitPlan.LinkWithCRM = Convert.ToBoolean(r["LinkWithCRM"]);
            VisitPlan.IsVisited = Convert.ToBoolean(r["IsVisited"]);
            VisitPlan.InActive = Convert.ToBoolean(r["InActive"]);
            VisitPlan.CreatedUserID = Convert.ToInt32(r["CreatedUserID"]);
            VisitPlan.ModifiedUserID = Convert.ToInt32(r["ModifiedUserID"]);
            VisitPlan.CreatedDate = r["CreatedDate"].ToString().GetDate();
            VisitPlan.ModifiedDate = r["ModifiedDate"].ToString().GetDate();

            return VisitPlan;
        }
        private VisitPlanModel RowOfCallCenterVisitPlan(DataRow r)
        {
            VisitPlanModel VisitPlan = new VisitPlanModel();
            VisitPlan.VisitPlanID = Convert.ToInt32(r["VisitPlanID"]);
            VisitPlan.VisitType = r["VisitType"].ToString();
            VisitPlan.VisitTypeName = r["VisitTypeName"].ToString();
            //VisitPlan.UserID = Convert.ToInt32(r["UserID"]);
            VisitPlan.RegionID = Convert.ToInt32(r["RegionID"]);
            VisitPlan.ClusterID = Convert.ToInt32(r["ClusterID"]);
            VisitPlan.DistrictID = Convert.ToInt32(r["DistrictID"]);
            //VisitPlan.UserName = r["UserName"].ToString();
            VisitPlan.VisitStartDate = r["VisitStartDate"].ToString().GetDate();
            VisitPlan.VisitEndDate = r["VisitEndDate"].ToString().GetDate();
            VisitPlan.VisitStartTime = r["VisitStartTime"].ToString().GetDate();
            VisitPlan.VisitEndTime = r["VisitEndTime"].ToString().GetDate();
            VisitPlan.Attachment = string.IsNullOrEmpty(r["Attachment"].ToString()) ? string.Empty : Common.GetFileBase64(r["Attachment"].ToString());
            //if (r["Attachment"].ToString() == "" || r["Attachment"] == null)
            //    VisitPlan.Attachment = "";
            //else
            //    VisitPlan.Attachment = Common.GetFileBase64(r["Attachment"].ToString());

            //VisitPlan.Attachment = r["Attachment"].ToString();
            VisitPlan.Comments = r["Comments"].ToString();
            VisitPlan.Venue = r["Venue"].ToString();
            //VisitPlan.CallCenterAgentStatus = r["CallCenterAgentStatus"].ToString();
            //VisitPlan.ClassID = Convert.ToInt32(r["ClassID"]);
            VisitPlan.LinkWithCRM = Convert.ToBoolean(r["LinkWithCRM"]);
            VisitPlan.IsVisited = Convert.ToBoolean(r["IsVisited"]);
            VisitPlan.InActive = Convert.ToBoolean(r["InActive"]);
            VisitPlan.CreatedUserID = Convert.ToInt32(r["CreatedUserID"]);
            VisitPlan.ModifiedUserID = Convert.ToInt32(r["ModifiedUserID"]);
            VisitPlan.CreatedDate = r["CreatedDate"].ToString().GetDate();
            VisitPlan.ModifiedDate = r["ModifiedDate"].ToString().GetDate();

            return VisitPlan;
        }
    }
}
