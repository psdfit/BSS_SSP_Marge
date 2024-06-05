using DataLayer.Classes;
using DataLayer.Interfaces;
using DataLayer.Models;
using Microsoft.Data.SqlClient;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;

namespace DataLayer.Services
{
    public class SRVNotificationDetail : ISRVNotificationDetail
    {
        public static int saveSendNotification(UserNotificationMapModel model, int? CurUserID)
        {
            try
            {
                SqlParameter[] PLead = new SqlParameter[10];
                PLead[1] = new SqlParameter("@NotificationMapID", model.NotificationMapID);
                PLead[2] = new SqlParameter("@RouteUrl", model.RouteUrl);
                PLead[3] = new SqlParameter("@IsRead", false);
                PLead[4] = new SqlParameter("@UserID", model.UserID);
                PLead[5] = new SqlParameter("@CurUserID", CurUserID);
                PLead[6] = new SqlParameter("@CustomComments", model.CustomComments);
                PLead[7] = new SqlParameter("@Ident", SqlDbType.Int);
                PLead[7].Direction = ParameterDirection.Output;
                PLead[8] = new SqlParameter("@NotificationTypeId", model.NotificationTypeId);
                SqlHelper.ExecuteNonQuery(SqlHelper.GetCon(), CommandType.StoredProcedure, "[AU_NotificationDetail]", PLead);
                int id = Convert.ToInt32(PLead[7].Value);
                return id;
            }
            catch (Exception ex) { throw new Exception(ex.Message); return 0; }
        }

        public int BatchInsert(List<UserNotificationMapModel> ls, int CurUserID)
        {
            SqlParameter[] param = new SqlParameter[3];

            param[0] = new SqlParameter("@Json", JsonConvert.SerializeObject(ls));
            param[2] = new SqlParameter("@CurUserID", CurUserID);
            return SqlHelper.ExecuteNonQuery(SqlHelper.GetCon(), CommandType.StoredProcedure, "[BA_NotificaitonDetails]", param);
        }

        public bool UpdateNotificationDetails(NotificationDetailModel model)
        {
            string query = $"Update dbo.NotificationDetail SET IsSent = 1 , ModifiedUserID={(int)EnumUsers.System} , ModifiedDate = '{DateTime.Now}'  WHERE NotificationDetailID={model.NotificationDetailID}";

            _ = SqlHelper.ExecuteScalar(SqlHelper.GetCon(), CommandType.Text, query);
            return true;
        }

        public List<NotificationDetailModel> FetchNotificationDetailsForEmail()
        {
            try
            {
                DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_NotificationsForEmail").Tables[0];
                return LoopinDataForEmail(dt);
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }

        private List<NotificationDetailModel> LoopinDataForEmail(DataTable dt)
        {
            List<NotificationDetailModel> NotificationsL = new List<NotificationDetailModel>();

            foreach (DataRow r in dt.Rows)
            {
                NotificationsL.Add(RowOfNotificationDetailsForEmail(r));
            }
            return NotificationsL;
        }

        private NotificationDetailModel RowOfNotificationDetailsForEmail(DataRow r)
        {
            NotificationDetailModel Notifications = new NotificationDetailModel();
            Notifications.NotificationDetailID = r.Field<int>("NotificationDetailID");
            Notifications.NotificationMapID = r.Field<int>("NotificationMapID");
            Notifications.RouteUrl = r.Field<string>("RouteUrl");
            Notifications.CustomComments = r.Field<string>("CustomComments");
            Notifications.UserID = r.Field<int>("UserID");
            Notifications.UserName = r.Field<string>("UserName");
            Notifications.Fname = r.Field<string>("Fname");
            Notifications.lname = r.Field<string>("lname");
            Notifications.FullName = r.Field<string>("FullName");
            Notifications.UserEmail = r.Field<string>("Email");
            Notifications.ProcessKey = r.Field<string>("ProcessKey");
            Notifications.NotificationName = r.Field<string>("NotificationName");
            Notifications.Subject = r.Field<string>("Subject");
            Notifications.Body = r.Field<string>("Body");
            return Notifications;
        }

        public List<TraineeEmailsVerificationModel> GetUnverifiedTraineeEmails()
        {
            try
            {
              DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_UnverifiedTraineeEmail").Tables[0];
                return LoopinUnverifiedTraineeEmail(dt);
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }

        private List<TraineeEmailsVerificationModel> LoopinUnverifiedTraineeEmail(DataTable dt)
        {
            List<TraineeEmailsVerificationModel> NotificationsL = new List<TraineeEmailsVerificationModel>();

            foreach (DataRow r in dt.Rows)
            {
                NotificationsL.Add(RowOfUnverifiedTraineeEmail(r));
            }
            return NotificationsL;
        }

        private TraineeEmailsVerificationModel RowOfUnverifiedTraineeEmail(DataRow r)
        {
            TraineeEmailsVerificationModel TraineeEmailVerification = new TraineeEmailsVerificationModel();
            TraineeEmailVerification.TraineeEmailVerificationID = r.Field<Guid>("TraineeEmailVerificationID");
            TraineeEmailVerification.TraineeProfileID = r.Field<int>("TraineeProfileID");
            TraineeEmailVerification.EmailAddress = r.Field<string>("EmailAddress");
            TraineeEmailVerification.TraineeName = r.Field<string>("TraineeName");
            TraineeEmailVerification.EmailSentCount = r.Field<int>("EmailSentCount");
            TraineeEmailVerification.ProcessKey = r.Field<string>("ProcessKey");
            TraineeEmailVerification.IsVerified = r.Field<bool>("IsVerified");
            TraineeEmailVerification.IsEmailSent = r.Field<bool>("IsEmailSent");
            TraineeEmailVerification.IsInactive = r.Field<bool>("IsInactive");
            //TraineeEmailVerification.CreatedByUserID = r.Field<int>("CreatedByUserID");
            //TraineeEmailVerification.ModifiedByUserID = r.Field<int>("ModifiedByUserID");

            return TraineeEmailVerification;
        }

        //public int UpdateTraineeEmailsVerification(int TraineeEmailVerificationID)
        //{
        //    string query = $"Update dbo.TraineeEmailVerification SET IsEmailSent = 1 , ModifiedDate = '{DateTime.Now}'  WHERE TraineeEmailVerificationID={TraineeEmailVerificationID}";

        //    _ = SqlHelper.ExecuteScalar(SqlHelper.GetCon(), CommandType.Text, query);
        //    return 1;
        //}

        public int UpdateTraineeEmailsVerification(Guid TraineeEmailVerificationID)
        {
            string ModifiedDateTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            string query = $"Update dbo.TraineeEmailVerification SET IsEmailSent = 1 ,EmailSentCount=EmailSentCount+1 , ModifiedDate = '{ModifiedDateTime}'  WHERE TraineeEmailVerificationID='{TraineeEmailVerificationID}'";

            _ = SqlHelper.ExecuteScalar(SqlHelper.GetCon(), CommandType.Text, query);
            return 1;
        }
    }
}