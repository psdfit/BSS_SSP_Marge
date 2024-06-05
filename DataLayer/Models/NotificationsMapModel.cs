using System;
using System.Collections.Generic;
using System.Text;

namespace DataLayer.Models
{
   public class NotificationsMapModel :ModelBase
    {
        public NotificationsMapModel() : base() { }
    public NotificationsMapModel(bool InActive) : base(InActive) { }
        public int? NotificationID { get; set; }
        public int? NotificationMapID { get; set; }
        public int? NotificationDetailID { get; set; }
        public int? ApprovalStatusID { get; set; }
        public string ProcessKey { get; set; }

        public string[] UserIDs { get; set; }
        public string UserID { get; set; }
        public string NotificationName { get; set; }
        public string FullName { get; set; }
        public string EventAction { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }

    }
}
