
using System;
using System.Collections.Generic;

namespace DataLayer.Models
{
[Serializable]public class InceptionReportListModel :ModelBase {
	public InceptionReportListModel():base() { }
    public InceptionReportListModel(bool InActive) : base(InActive) { }	public int IncepReportID	{ get; set; }	public int ClassID	{ get; set; }	public string ClassCode	{ get; set; }	public DateTime? ClassStartTime	{ get; set; }	public DateTime? ClassEndTime	{ get; set; }	public DateTime? ActualStartDate { get; set; }	public DateTime? ActualEndDate	{ get; set; }	public string ClassTotalHours	{ get; set; }	public int EnrolledTrainees	{ get; set; }	public string Shift	{ get; set; }	//public string CenterLocation	{ get; set; }
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



	}}
