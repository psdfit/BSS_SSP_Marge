using DataLayer.Models;

    public interface ISRVUserNotificationMap
    {
        UserNotificationMapModel GetByUserNotificationMapID(int UserNotificationMapID);
        List<UserNotificationMapModel> SaveUserNotificationMap(UserNotificationMapModel UserNotificationMap);
        List<UserNotificationMapModel> FetchUserNotificationMap(UserNotificationMapModel mod);
        List<UserNotificationMapModel> FetchUserNotificationMap();
        List<UserNotificationMapModel> FetchUserNotificationMap(bool InActive);
        void ActiveInActive(int UserNotificationMapID, bool? InActive, int CurUserID);
    }