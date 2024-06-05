using DataLayer.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataLayer.Interfaces
{
    public interface ISRVNotificationDetail
    {
        int BatchInsert(List<UserNotificationMapModel> ls, int CurUserID);

        bool UpdateNotificationDetails(NotificationDetailModel model);

        List<NotificationDetailModel> FetchNotificationDetailsForEmail();

        List<TraineeEmailsVerificationModel> GetUnverifiedTraineeEmails();

        int UpdateTraineeEmailsVerification(Guid TraineeEmailVerificationID);
    }
}