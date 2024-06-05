using DataLayer.Classes;
using DataLayer.Models;
using Microsoft.Data.SqlClient;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;

namespace DataLayer.Services
{
    public class SRVKAMDashboard : SRVBase, DataLayer.Interfaces.ISRVKAMDashboard
    {
        public SRVKAMDashboard()
        {
        }

        private List<TSPDetailModel> LoopinKAMRelevantTSPsData(DataTable dt)
        {
            List<TSPDetailModel> TehsilL = new List<TSPDetailModel>();

            foreach (DataRow r in dt.Rows)
            {
                TehsilL.Add(RowOfKAMRelevantTSPs(r));
            }
            return TehsilL;
        }
        
        private List<KAMDashboardModel> LoopinKAMDashboardStats(DataTable dt)
        {
            List<KAMDashboardModel> kamDashboardL = new List<KAMDashboardModel>();

            foreach (DataRow r in dt.Rows)
            {
                kamDashboardL.Add(RowOfKAMDashboardStats(r));
            }
            return kamDashboardL;
        }
        private List<KAMDeadlinesModel> LoopinKAMDeadlinesData(DataTable dt)
        {
            List<KAMDeadlinesModel> kamDeadlineL = new List<KAMDeadlinesModel>();

            foreach (DataRow r in dt.Rows)
            {
                kamDeadlineL.Add(RowOfKAMDeadline(r));
            }
            return kamDeadlineL;
        }


        public List<TSPDetailModel> FetchKAMRelevantTSPs(int userid)
        {
            try
            {
                DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_KAM_Relevant_TSPs", new SqlParameter("@UserID", userid)).Tables[0];
                return LoopinKAMRelevantTSPsData(dt);
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }
   
        public List<KAMDashboardModel> FetchKAMDashboardStats(int userid, int tspid,DateTime? month)
        {
            try
            {
                SqlParameter[] param = new SqlParameter[3];
                param[0] = new SqlParameter("@UserID", userid);
                param[1] = new SqlParameter("@TSPID", tspid);

                param[2] = new SqlParameter("@Month", month);
                DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_KAM_Dashboard_Stats", param).Tables[0];
                return LoopinKAMDashboardStats(dt);
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }

        public void SendEmailToTspUsers(List<object> UserIDs, string subjectOfEmail,string EmailAttachmentFile, string Email)
        {
            try
            {
                List<object> list = new List<object>();

                foreach (object uv in UserIDs)
                {

                    list.Add(new SRVUsers().GetByUserID(Convert.ToInt32(uv)));
                }


                foreach (UsersModel uv in list)
                {
                    string subject, body;
                    body = Email;
                    subject = subjectOfEmail;

                    Common.SendEmail(uv.Email, subject, body,false, EmailAttachmentFile);

                }
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }


        public List<ClassModel> GetPendingEmploymentsClassesByKAM(int KAMUserID)
        {
            try
            {
                List<ClassModel> ClassesList = new List<ClassModel>();
                SqlParameter[] param = new SqlParameter[2];
                param[0] = new SqlParameter("@UserID", KAMUserID);
                DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "Get_PendingEmploymentsClassesByKAM", param).Tables[0];
                ClassesList = Helper.ConvertDataTableToModel<ClassModel>(dt);
                return ClassesList;
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }

        public List<KAMDeadlinesModel> FetchDeadlinesByKAM(KAMDashboardModel mod)
        {
            try
            {
                SqlParameter[] param = new SqlParameter[2];
                param[0] = new SqlParameter("@UserID", mod.UserID);
                param[1] = new SqlParameter("@TSPID", mod.TSPID);
                DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_DeadlineStatsByKAM",param).Tables[0];
                return LoopinKAMDeadlinesData(dt);
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }


        private TSPDetailModel RowOfKAMRelevantTSPs(DataRow r)
        {
            TSPDetailModel tsp = new TSPDetailModel();
            tsp.TSPID = Convert.ToInt32(r["TSPID"]);
            tsp.UserID = Convert.ToInt32(r["UserID"]);
            tsp.TSPName = r["TSPName"].ToString();
            tsp.TSPCode = r["TSPCode"].ToString();
            tsp.TSPColorName = r["TSPColorName"].ToString();
           
            return tsp;
        }
        private KAMDeadlinesModel RowOfKAMDeadline(DataRow r)
        {
            KAMDeadlinesModel deadline = new KAMDeadlinesModel();
            //deadline.TSPID = Convert.ToInt32(r["TSPID"]);
            //deadline.UserID = Convert.ToInt32(r["UserID"]);
            deadline.DeadlineType = r["DeadlineType"].ToString();
            deadline.DeadlineDate = r["DeadlineDate"].ToString().GetDate();
            //deadline.ClassID = Convert.ToInt32(r["ClassID"]);

            return deadline;
        }
        
        private KAMDashboardModel RowOfKAMDashboardStats(DataRow r)
        {
            KAMDashboardModel kamDashboard = new KAMDashboardModel();
            kamDashboard.ContractualToEnrolled = (float)Convert.ToDouble(r["ContractualToEnrolled"]);
            kamDashboard.PendingClassesForEmployment = Convert.ToInt32(r["PendingClassesForEmployment"]);
            kamDashboard.Resolved = Convert.ToInt32(r["Resolved"]);
            kamDashboard.Pending = Convert.ToInt32(r["Pending"]);
            kamDashboard.InProcess = Convert.ToInt32(r["InProcess"]);
            kamDashboard.Unresolved = Convert.ToInt32(r["Unresolved"]);
            kamDashboard.TotalComplaints = Convert.ToInt32(r["TotalComplaints"]);
            kamDashboard.TotalDeadlines = Convert.ToInt32(r["TotalDeadlines"]);
            kamDashboard.Planned = Convert.ToInt32(r["Planned"]);
            kamDashboard.Active = Convert.ToInt32(r["Active"]);
            kamDashboard.Completed = Convert.ToInt32(r["Completed"]);
            kamDashboard.Abandoned = Convert.ToInt32(r["Abandoned"]);
            kamDashboard.Cancelled = Convert.ToInt32(r["Cancelled"]);
            kamDashboard.Ready = Convert.ToInt32(r["Ready"]);
            kamDashboard.Suspended = Convert.ToInt32(r["Suspended"]);
            kamDashboard.PendingInceptionReports = Convert.ToInt32(r["PendingInceptionReports"]);
            kamDashboard.PendingRegisterations = Convert.ToInt32(r["PendingRegisterations"]);
            kamDashboard.PendingRTPs = Convert.ToInt32(r["PendingRTPs"]);
            kamDashboard.LinkForCRM = r["LinkForCRM"].ToString();

            List<PieChart> ClassStatusPieData = new List<PieChart>();

            ClassStatusPieData.Add(new PieChart("Planned", Convert.ToInt32(r["Planned"])));
            ClassStatusPieData.Add(new PieChart("Active", Convert.ToInt32(r["Active"])));
            ClassStatusPieData.Add(new PieChart("Completed", Convert.ToInt32(r["Completed"])));
            ClassStatusPieData.Add(new PieChart("Abandoned", Convert.ToInt32(r["Abandoned"])));
            ClassStatusPieData.Add(new PieChart("Cancelled", Convert.ToInt32(r["Cancelled"])));
            ClassStatusPieData.Add(new PieChart("Ready", Convert.ToInt32(r["Ready"])));
            ClassStatusPieData.Add(new PieChart("Suspended ", Convert.ToInt32(r["Suspended"])));

            kamDashboard.SDPie = ClassStatusPieData;
            return kamDashboard;
        }
        
      
    }
}