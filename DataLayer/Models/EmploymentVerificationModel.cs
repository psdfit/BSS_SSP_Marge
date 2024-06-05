using System;
using System.Collections.Generic;
using System.Text;

namespace DataLayer.Models
{
	public class EmploymentVerificationModel : ModelBase
	{
		public EmploymentVerificationModel() : base() { }
		public EmploymentVerificationModel(bool InActive) : base(InActive) { }

		public int ID { get; set; }
		public int PlacementID { get; set; }
		public string TraineeName { get; set; }
		public bool? PayrollVerification { get; set; }
		public bool? PayrollVerificationStatus { get; set; }
		public bool? PhysicalVerification { get; set; }
		public bool? TelephonicVerification { get; set; }
		public bool? PhysicalVerificationStatus { get; set; }
		public bool? TelephonicVerificationStatus { get; set; }
		public int VerificationMethodID { get; set; }
		public bool? IsVerified { get; set; }
		public string Comments { get; set; }
		public string Attachment { get; set; }
		public string Designation { get; set; }
		public string Department { get; set; }
		public int? EmploymentDuration { get; set; }
		public double? Salary { get; set; }
		public string SupervisorName { get; set; }
		public string SupervisorContact { get; set; }
		public DateTime? EmploymentStartDate { get; set; }
		public string? EmploymentStatus { get; set; }
		public int? EmploymentType { get; set; }
		public string EmployerName { get; set; }
		public string EmployerBusinessType { get; set; }
		public string EmploymentAddress { get; set; }
		public int? District { get; set; }
		public int? EmploymentTehsil { get; set; }
		public string EmploymentTiming { get; set; }

		public string OfficeContactNo { get; set; }

		public string PlatformName { get; set; }
		public string NameofTraineeStorePage { get; set; }
		public string LinkofTraineeStorePage { get; set; }

		public int? CallCenterVerificationTraineeID { get; set; }
		public int? CallCenterVerificationSupervisorID { get; set; }
		public int? CallCenterVerificationCommentsID { get; set; }
	}
}
