
using Newtonsoft.Json;
using System;

namespace DataLayer.Models
{
	[Serializable]

	public class SchemeModel : ModelBase
	{
		public SchemeModel() : base() { }
		public SchemeModel(bool InActive) : base(InActive) { }
		public int SchemeID { get; set; }
		public int? SchemeUserID_OLD { get; set; }
		public string SchemeName { get; set; }
		public string SchemeCode { get; set; }
		public int? SchemeTypeID_OLD { get; set; }
		public int ProgramTypeID { get; set; }
		public string PTypeName { get; set; }
		public int PCategoryID { get; set; }
		public string PCategoryName { get; set; }
		public int? ProjectID_OLD { get; set; }
		public int FundingSourceID { get; set; }
		public string FundingSourceName { get; set; }
		public int FundingCategoryID { get; set; }
		public string FundingCategoryName { get; set; }
		public string PaymentSchedule { get; set; }
		public string Description { get; set; }
		public double Stipend { get; set; }
		public string StipendMode { get; set; }
		public double UniformAndBag { get; set; }
        public int MinimumEducation { get; set; }
		public int MaximumEducation { get; set; }
		public int MinAge { get; set; }
		public int MaxAge { get; set; }
		public int GenderID { get; set; }
		public string GenderName { get; set; }
		public Nullable<bool> DualEnrollment { get; set; }
		public DateTime? ContractAwardDate { get; set; }
		public string BusinessRuleType { get; set; }
		public int OrganizationID { get; set; }
		public string OName { get; set; }
		public bool? FinalSubmitted { get; set; }
		public string UID { get; set; }
		public bool? isMigrated { get; set; }
		public string UserName { get; set; }
		public bool? IsApproved { get; set; }
		public bool? IsRejected { get; set; }
        public string ProcessKey { get; set; }
        public string MinimumEducationName { get; set; }
        public string MaximumEducationName { get; set; }
        public string SAPID { get; set; }
		/// <summary>
		/// Added for SSP 
		/// Ali Haider
		/// </summary>
		public int ProgramID { get; set; }
		public string ProgramName { get; set; }
		public DateTime? ProcessStartDate { get; set; }
		public DateTime? ProcessEndDate { get; set; }
		public string IsLocked { get; set; }
	}}
