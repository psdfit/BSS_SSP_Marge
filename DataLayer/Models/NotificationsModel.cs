
using System;
using System.Collections.Generic;

namespace DataLayer.Models
{
[Serializable]public class NotificationsModel :ModelBase {
	public NotificationsModel():base() { }
    public NotificationsModel(bool InActive) : base(InActive) { }	public int NotificationID	{ get; set; }	public string NotificationName	{ get; set; }	public string EventAction	{ get; set; }	public string Subject	{ get; set; }	public string Body	{ get; set; }	public List<UserNotificationMapModel> UserNotifications { get; set; }

	}}
