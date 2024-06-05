using System;
using System.Collections.Generic;
using System.Text;

namespace DataLayer.Models
{
    public class NotificationDetailModel : ModelBase
    {
        public NotificationDetailModel() : base() { }
        public NotificationDetailModel(bool InActive) : base(InActive) { }

        public int? NotificationDetailID { get; set; }
        public string NotificationType { get; set; }
        public string RouteUrl { get; set; }
        public int? UserID { get; set; }
        public int? NotificationMapID { get; set; }
        public int NotificationID { get; set; }
        public string SendToUserName { get; set; }
        public string ProcessKey { get; set; }
        public string CustomComments { get; set; }
        public string NotificationName { get; set; }
        public string Body { get; set; }
        public string Subject { get; set; }
        public string EventAction { get; set; }
        public string FullName { get; set; }
        public string DateDiff { get; set; }
        public bool? IsRead { get; set; }
        public bool? IsDeleted { get; set; }
        public int? PageNo { get; set; }
        public int? RecordParPage { get; set; }
        public int? ReadNotificationcount { get; set; }
        public int? TotleCount { get; set; }
        public List<UserNotificationMapModel> UserNotifications { get; set; }

        public bool IsSent { get; set; }
        public string UserName { get; set; }
        public string UserEmail { get; set; }
        public string Fname { get; set; }
        public string lname { get; set; }
    }
}