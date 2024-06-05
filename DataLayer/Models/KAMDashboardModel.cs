
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace DataLayer.Models
{
	[Serializable]

	public class KAMDashboardModel
	{
		public float ContractualToEnrolled { get; set; }
		public int PendingClassesForEmployment { get; set; }
		public int TSPID { get; set; }
		public int UserID { get; set; }

		//Complaint Statuses
		public int Resolved { get; set; }
		public int Pending { get; set; }
		public int InProcess { get; set; }
		public int Unresolved { get; set; }
		public int TotalComplaints { get; set; }
		public int TotalDeadlines { get; set; }
		public int PendingInceptionReports { get; set; }
		public int PendingRegisterations { get; set; }
		public int Planned { get; set; }
		public int Active { get; set; }
		public int Completed { get; set; }
		public int Abandoned { get; set; }
		public int Cancelled { get; set; }
		public int Ready { get; set; }
		public int Suspended { get; set; }
		public int PendingRTPs { get; set; }
		public string Subject { get; set; }
		public string EmailToSent { get; set; }
		public string LinkForCRM { get; set; }
		public string EmailAttachmentFile { get; set; }
		public DateTime? Month { get; set; }

		public List<object> UserIDs { get; set; }
		public List<PieChart> SDPie { get; set; }

	}	public class KAMDeadlinesModel
	{
		public string InceptionReportDeadline { get; set; }
		public string TraineeRegisterationDeadline { get; set; }
		public string RTPDeadline { get; set; }
		public string EmploymentDeadline { get; set; }
		public string ProcessName { get; set; }
		public DateTime DeadlineDate { get; set; }
		public string DeadlineType { get; set; }
		public int ClassID { get; set; }


	}}
