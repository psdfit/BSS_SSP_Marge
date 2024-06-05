
using System;

namespace DataLayer.Models
{
[Serializable]public class InstructorReplaceChangeRequestModel :ModelBase {
	public InstructorReplaceChangeRequestModel():base() { }
    public InstructorReplaceChangeRequestModel(bool InActive) : base(InActive) { }	public int InstructorReplaceChangeRequestID	{ get; set; }	public int IncepReportID	{ get; set; }	public int ClassID	{ get; set; }	public string ClassCode { get; set; }	public bool IsApproved { get; set; }
	public bool IsRejected { get; set; }
	public string SchemeName { get; set; }
	public string TSPName { get; set; }
	public string TradeName { get; set; }
	public string InstructorName { get; set; }
	public string InstrIDs { get; set; }

	}}
