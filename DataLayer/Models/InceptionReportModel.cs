
using System;
using System.Collections.Generic;

namespace DataLayer.Models
{
[Serializable]
	public InceptionReportModel():base() { }
    public InceptionReportModel(bool InActive) : base(InActive) { }
	public bool? Monday { get; set; }
	public bool? Tuesday { get; set; }
	public bool? Wednesday { get; set; }
	public bool? Thursday { get; set; }
	public bool? Friday { get; set; }
	public bool? Saturday { get; set; }
	public bool? Sunday { get; set; }
	public bool? FinalSubmitted	{ get; set; }
    public string InstrIDs { get; set; }


        public List<ContactPersonModel> ContactPersons { get; set; }

	public int SectionID { get; set; }
	public string SectionName { get; set; }
	public string TradeName { get; set; }
	public int InceptionReportCrID { get; set; }
	public Boolean IrCrIsApproved { get; set; }
	public Boolean IrCrIsRejected { get; set; }
	public string StartDateTime { get; set; }
	public string EndDateTime { get; set; }

	}