
using System;

namespace DataLayer.Models
{
[Serializable]
	public InstructorReplaceChangeRequestModel():base() { }
    public InstructorReplaceChangeRequestModel(bool InActive) : base(InActive) { }
	public bool IsRejected { get; set; }
	public string SchemeName { get; set; }
	public string TSPName { get; set; }
	public string TradeName { get; set; }
	public string InstructorName { get; set; }
	public string InstrIDs { get; set; }

	}