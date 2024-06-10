using DataLayer.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace DataLayer.Interfaces
{
    public interface ISRVNotificationDetail
    {
        int BatchInsert(List<UserNotificationMapModel> ls, int CurUserID);

        bool UpdateNotificationDetails(NotificationDetailModel model);
        bool UpdateSSPNotificationDetails(int NotificationDetailID);

        List<NotificationDetailModel> FetchNotificationDetailsForEmail();
        DataTable FetchSSPNotificationDetailsForEmail();

        List<TraineeEmailsVerificationModel> GetUnverifiedTraineeEmails();

        int UpdateTraineeEmailsVerification(Guid TraineeEmailVerificationID);
    }
}