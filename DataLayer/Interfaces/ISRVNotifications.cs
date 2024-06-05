using DataLayer.Models;using System.Collections.Generic;namespace DataLayer.Interfaces{

    public interface ISRVNotifications
    {
        NotificationsModel GetByNotificationID(int NotificationID);
        List<NotificationsModel> SaveNotifications(NotificationsModel Notifications);
        List<NotificationsModel> FetchNotifications(NotificationsModel mod);
        List<NotificationsModel> FetchNotifications();
        List<NotificationsModel> FetchNotifications(bool InActive);
        void ActiveInActive(int NotificationID, bool? InActive, int CurUserID);
        List<NotificationDetailModel> FetchNotificationsDetail(NotificationDetailModel modal);

    }
}