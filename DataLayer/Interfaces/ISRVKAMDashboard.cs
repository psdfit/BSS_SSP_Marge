using DataLayer.Models;
using System.Collections.Generic;
using Microsoft.Data.SqlClient;
using System.Data;
using System;


namespace DataLayer.Interfaces
{
    public interface ISRVKAMDashboard
    {
        public List<TSPDetailModel> FetchKAMRelevantTSPs(int userid);
        public List<KAMDashboardModel> FetchKAMDashboardStats(int userid, int tspid, DateTime? month);
        public void SendEmailToTspUsers(List<object> UserIDs, string subjectOfEmail, string EmailAttachmentFile, string Email);
        public List<ClassModel> GetPendingEmploymentsClassesByKAM(int KAMUserID);
        public List<KAMDeadlinesModel> FetchDeadlinesByKAM(KAMDashboardModel mod);


    }
}