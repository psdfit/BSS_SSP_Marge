
using System;
using System.Collections.Generic;

namespace DataLayer.Models
{
[Serializable]
	public InceptionReportListModel():base() { }
    public InceptionReportListModel(bool InActive) : base(InActive) { }
	public bool? Monday { get; set; }
	public bool? Tuesday { get; set; }
	public bool? Wednesday { get; set; }
	public bool? Thursday { get; set; }
	public bool? Friday { get; set; }
	public bool? Saturday { get; set; }
	public bool? Sunday { get; set; }
	public bool? FinalSubmitted	{ get; set; }
    public string InstrIDs { get; set; }

	public int SectionID { get; set; }
	public string SectionName { get; set; }
	public string SchemeName { get; set; }
	public string TSPName { get; set; }
	public string GenderName { get; set; }
	public string CenterName { get; set; }
	public string AddressOfTrainingCenterTheoratical { get; set; }
	public string InchargeNameTheoratical { get; set; }
	public string InchargeContactTheoratical { get; set; }
	public string AddressOfTrainingCenterPractical { get; set; }
	public string InchargeNamePractical { get; set; }
	public string InchargeContactPractical { get; set; }
	public string TehsilName { get; set; }
	public string DistrictName { get; set; }
	public string TradeName { get; set; }
	public DateTime StartDate { get; set; }
	public DateTime EndDate { get; set; }
	public string Batch { get; set; }
	public string NameOfAuthorizedPerson { get; set; }
	public string MobileContactOfAuthorizedPerson { get; set; }
	public string EmailOfAuthorizedPerson { get; set; }
	public string TrainingDaysNo { get; set; }
	public string TrainingDays { get; set; }
	public string InstructorInfo { get; set; }



	}