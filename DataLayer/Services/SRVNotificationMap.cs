using DataLayer.Interfaces;
using DataLayer.Models;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace DataLayer.Services
{
 public   class SRVNotificationMap: ISRVNotificationMap
    {
     
        public bool SaveNotificationMap(NotificationsMapModel Notifications)
        {
            try
            {
                string UserIDs = string.Empty;
                //List<NotificationsMapModel> ComplaintModel = new List<NotificationsMapModel>();
                //if (Notifications.NotificationID != null && Notifications.ProcessKey != null&& Notifications.NotificationMapID==0)
                //{
                //    SqlParameter[] paramCheck = new SqlParameter[3];
                //    paramCheck[0] = new SqlParameter("@NotificationID", Notifications.NotificationID);
                //    paramCheck[1] = new SqlParameter("@ProcessKey", Notifications.ProcessKey); 
                //    DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_NotificationMap", paramCheck).Tables[0];

                //    ComplaintModel = Helper.ConvertDataTableToModel<NotificationsMapModel>(dt);
                //}
                //if (ComplaintModel.Count == 0)
                //{
                    if (Notifications.UserIDs != null)
                    {
                         UserIDs = string.Join(",", Notifications.UserIDs);
                    }
                    SqlParameter[] param = new SqlParameter[7];
                    param[0] = new SqlParameter("@NotificationMapID", Notifications.NotificationMapID);
                    param[1] = new SqlParameter("@NotificationID", Notifications.NotificationID);
                    param[2] = new SqlParameter("@ProcessKey", Notifications.ProcessKey);
                    param[3] = new SqlParameter("@UserIDs", UserIDs);
                    param[4] = new SqlParameter("@CurUserID", Notifications.CurUserID);
                    param[5] = new SqlParameter("@ApprovalStatusID", Notifications.ApprovalStatusID);
                SqlHelper.ExecuteNonQuery(SqlHelper.GetCon(), CommandType.StoredProcedure, "[AU_NotificationMap]", param);
                    return true;
               // }
               // else { return false; }
               
            }
            catch (Exception ex)
            { throw new Exception(ex.Message); }
        }
        public List<NotificationsMapModel> FetchNotificationMap()
        {
            try
            {
                List<NotificationsMapModel> ComplaintModel = new List<NotificationsMapModel>();
                DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_NotificationMap").Tables[0];
                
                ComplaintModel = Helper.ConvertDataTableToModel<NotificationsMapModel>(dt);
                return ComplaintModel;
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }

         public List<NotificationsMapModel> GetProcessInfoByProcessKey(string ProcessKey)
        {
            try
            {
                SqlParameter[] param = new SqlParameter[3];
                param[0] = new SqlParameter("@ProcessKey", ProcessKey);
                List<NotificationsMapModel> NotificationsModal = new List<NotificationsMapModel>();
                DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "[RD_GetProcessInfoByProcessKey]", param).Tables[0];

                NotificationsModal = Helper.ConvertDataTableToModel<NotificationsMapModel>(dt);
                return NotificationsModal;
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }

        public bool ReadNotification(int? NotificationDetailID)
        {
            try
            {
                SqlParameter[] param = new SqlParameter[2];
                param[0] = new SqlParameter("@NotificationDetailID", NotificationDetailID);
                param[1] = new SqlParameter("@IsRead", 1);
                SqlHelper.ExecuteNonQuery(SqlHelper.GetCon(), CommandType.StoredProcedure, "[AI_ReadNotification]", param);
                return true;
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }

        public List<NotificationDetailModel> GetNotificationDetasilByID(int NotificationDetailID)
        {
            try
            {
                SqlParameter[] param = new SqlParameter[2];
                param[0] = new SqlParameter("@NotificationDetailID", NotificationDetailID);
                List<NotificationDetailModel> NotificationsModal = new List<NotificationDetailModel>();
                DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "[RD_GetNotificationDetasilByID]", param).Tables[0];

                NotificationsModal = Helper.ConvertDataTableToModel<NotificationDetailModel>(dt);
                foreach (var item in NotificationsModal)
                {
                    CalculateDateDifference calculateDateDifference = new CalculateDateDifference();
                    string diff = calculateDateDifference.DatetimeCount(Convert.ToDateTime(item.CreatedDate));
                    item.DateDiff = diff;
                }

                return NotificationsModal;
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }


        public bool MarkAllNotificationsAsRead(int userId)
        {
            try
            {
                // First, get all NotificationDetailIDs for the user
                SqlParameter[] fetchParam = new SqlParameter[1];
                fetchParam[0] = new SqlParameter("@UserID", userId);
                DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_GetAllNotificationsByUserID", fetchParam).Tables[0];

                // Convert NotificationDetailIDs to comma-separated string
                List<int> notificationIds = dt.AsEnumerable()
                    .Select(row => row.Field<int>("NotificationDetailID"))
                    .ToList();
                string notificationDetailIds = string.Join(",", notificationIds);

                if (!string.IsNullOrEmpty(notificationDetailIds))
                {
                    // Call the stored procedure to mark notifications as read
                    SqlParameter[] param = new SqlParameter[2];
                    param[0] = new SqlParameter("@NotificationDetailIDs", notificationDetailIds);
                    param[1] = new SqlParameter("@IsRead", 1);
                    SqlHelper.ExecuteNonQuery(SqlHelper.GetCon(), CommandType.StoredProcedure, "AI_ReadAllNotifications", param);
                }
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }





    }
}
