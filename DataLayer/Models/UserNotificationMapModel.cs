using System;

namespace DataLayer.Models
{
    [Serializable]

    public class UserNotificationMapModel : ModelBase
    {
        public UserNotificationMapModel() : base() { }
        public UserNotificationMapModel(bool InActive) : base(InActive) { }

        public int UserNotificationMapID { get; set; }
        public int UserID { get; set; }
        public int? NotificationDetailID { get; set; }
        public string UserName { get; set; }
        public int NotificationID { get; set; }
        public int? NotificationMapID { get; set; }
        public string NotificationName { get; set; }
        public string RouteUrl { get; set; }
        public bool? IsSelected { get; set; }
        public string CustomComments { get; set; }
        public string Subject { get; set; }
        public bool IsSent { get; set; }
        public int NotificationTypeId { get; set; }


    }}
