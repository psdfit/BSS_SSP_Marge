using DataLayer.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataLayer.Interfaces
{
   public interface ISRVNotificationMap
    {
        bool SaveNotificationMap(NotificationsMapModel Notifications);
        List<NotificationsMapModel> FetchNotificationMap();
        List<NotificationsMapModel> GetProcessInfoByProcessKey(string ProcessKey);
        bool ReadNotification(int? NotificationDetailID);
        List<NotificationDetailModel> GetNotificationDetasilByID(int NotificationDetailID);
      //  bool saveCentreMonitoringClassRecordNotification(List<RTPModel> model, int? CurUserID);
    }

}
