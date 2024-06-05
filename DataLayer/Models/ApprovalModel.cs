
using System;

namespace DataLayer.Models
{
[Serializable]

	public class ApprovalModel :ModelBase
	{
		public ApprovalModel():base() { }
		public ApprovalModel(bool InActive) : base(InActive) { }
		public int ApprovalD	{ get; set; }
		public string ProcessKey { get; set; }
		public string ApprovalProcessName	{ get; set; }
		public int Step	{ get; set; }
		public string CustomComments { get; set; }
		public string ClassCodeBYIncClassId { get; set; }
		public string logFilePath { get; set; }
		public string UserIDs	{ get; set; }
		public string UserName { get; set; }
		//public bool? isRejected { get; set; }
		public bool? isUserMapping { get; set; }
		public string editor { get; set; }
		public string VisitType { get; set; }
		public bool? IsAutoApproval { get; set; }
	}
}
