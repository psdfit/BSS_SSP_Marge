
using System;

namespace DataLayer.Models
{
[Serializable]public class UserEventMapModel :ModelBase {
	public UserEventMapModel():base() { }
    public UserEventMapModel(bool InActive) : base(InActive) { }	public int UserEventMapID	{ get; set; }	public int VisitPlanID	{ get; set; }	public int UserID	{ get; set; }	public string UserName { get; set; }	public string FullName { get; set; }	public string Email { get; set; }	public string UserStatus { get; set; }	public string UserStatusByCallCenter { get; set; }	public string NominatedPersonName { get; set; }	public string NominatedPersonContactNumber { get; set; }}}
