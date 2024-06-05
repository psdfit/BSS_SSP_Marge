using DataLayer.Classes;
using DataLayer.Models;
using Microsoft.Data.SqlClient;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace DataLayer.Services
{
    public class SRVNotifications : SRVBase, DataLayer.Interfaces.ISRVNotifications
    {
        public SRVNotifications()
        {
        }

        public NotificationsModel GetByNotificationID(int NotificationID)
        {
            try
            {
                SqlParameter param = new SqlParameter("@NotificationID", NotificationID);
                DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_Notifications", param).Tables[0];
                if (dt.Rows.Count > 0)
                {
                    return RowOfNotifications(dt.Rows[0]);
                }
                else
                    return null;
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }

        public List<NotificationsModel> SaveNotifications(NotificationsModel Notifications)
        {
            try
            {
                SqlParameter[] param = new SqlParameter[7];
                param[0] = new SqlParameter("@NotificationID", Notifications.NotificationID);
                param[1] = new SqlParameter("@NotificationName", Notifications.NotificationName);
                param[2] = new SqlParameter("@EventAction", Notifications.EventAction);
                param[3] = new SqlParameter("@Subject", Notifications.Subject);
                param[4] = new SqlParameter("@Body", Notifications.Body);
                param[5] = new SqlParameter("@CurUserID", Notifications.CurUserID);
                //param[6] = new SqlParameter("@Ident", SqlDbType.Int);
                //param[7].Direction = ParameterDirection.Output;
                SqlHelper.ExecuteNonQuery(SqlHelper.GetCon(), CommandType.StoredProcedure, "[AU_NotificationsV1]", param);
              //  new SRVUserNotificationMap().BatchInsert(Notifications.UserNotifications, Convert.ToInt32(param[6].Value), Notifications.CurUserID);

                return FetchNotifications();
            }
            catch (Exception ex)
            { throw new Exception(ex.Message); }
        }

        private List<NotificationsModel> LoopinData(DataTable dt)
        {
            List<NotificationsModel> NotificationsL = new List<NotificationsModel>();

            foreach (DataRow r in dt.Rows)
            {
                NotificationsL.Add(RowOfNotifications(r));
            }
            return NotificationsL;
        }

        public List<NotificationsModel> FetchNotifications(NotificationsModel mod)
        {
            try
            {
                DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_Notifications", Common.GetParams(mod)).Tables[0];
                return LoopinData(dt);
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }

        public List<NotificationsModel> FetchNotifications()
        {
            try
            {
                DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_Notifications").Tables[0];
                return LoopinData(dt);
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }

        public List<NotificationsModel> FetchNotifications(bool InActive)
        {
            try
            {
                DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_Notifications", new SqlParameter("@InActive", InActive)).Tables[0];
                return LoopinData(dt);
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }

        public int BatchInsert(List<NotificationsModel> ls, int @BatchFkey, int CurUserID)
        {
            SqlParameter[] param = new SqlParameter[3];

            param[0] = new SqlParameter("@Json", JsonConvert.SerializeObject(ls));
            param[1] = new SqlParameter("@", @BatchFkey);
            param[2] = new SqlParameter("@CurUserID", CurUserID);
            return SqlHelper.ExecuteNonQuery(SqlHelper.GetCon(), CommandType.StoredProcedure, "[BAU_Notifications]", param);
        }

        public void ActiveInActive(int NotificationID, bool? InActive, int CurUserID)
        {
            SqlParameter[] PLead = new SqlParameter[3];
            PLead[0] = new SqlParameter("@NotificationID", NotificationID);
            PLead[1] = new SqlParameter("@InActive", InActive);
            PLead[2] = new SqlParameter("@CurUserID", CurUserID);
            SqlHelper.ExecuteNonQuery(SqlHelper.GetCon(), CommandType.StoredProcedure, "[AI_Notifications]", PLead);
        }

        private NotificationsModel RowOfNotifications(DataRow r)
        {
            NotificationsModel Notifications = new NotificationsModel();
            Notifications.NotificationID = Convert.ToInt32(r["NotificationID"]);
            Notifications.NotificationName = r["NotificationName"].ToString();
            Notifications.EventAction = r["EventAction"].ToString();
            Notifications.Subject = r["Subject"].ToString();
            Notifications.Body = r["Body"].ToString();
            Notifications.InActive = Convert.ToBoolean(r["InActive"]);
            Notifications.CreatedUserID = Convert.ToInt32(r["CreatedUserID"]);
            Notifications.ModifiedUserID = Convert.ToInt32(r["ModifiedUserID"]);
            Notifications.CreatedDate = r["CreatedDate"].ToString().GetDate();
            Notifications.ModifiedDate = r["ModifiedDate"].ToString().GetDate();

            return Notifications;
        }
        public List<NotificationDetailModel> FetchNotificationsDetail(NotificationDetailModel modal)
        {
            try
            {
                List<NotificationDetailModel> NotificationDetailModel = new List<NotificationDetailModel>();
                // new SqlParameter("@UserID", CurUserID)

                SqlParameter[] param = new SqlParameter[3];
                param[0] = new SqlParameter("@RecordParPage", modal.RecordParPage);
                param[1] = new SqlParameter("@PageNo", modal.PageNo);
                param[2] = new SqlParameter("@UserID", modal.CurUserID);
                DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "[RD_NotificationDetail]", param).Tables[0];
                NotificationDetailModel = Helper.ConvertDataTableToModel<NotificationDetailModel>(dt);
                foreach (var item in NotificationDetailModel)
                {
                    CalculateDateDifference calculateDateDifference = new CalculateDateDifference();
                  string diff=  calculateDateDifference.DatetimeCount(Convert.ToDateTime(item.CreatedDate));
                    item.DateDiff = diff;
                }


                return (NotificationDetailModel);
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }


    }
}