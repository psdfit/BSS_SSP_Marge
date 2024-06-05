
using System;

namespace DataLayer.Models
{
[Serializable]public class ClassStatusModel :ModelBase {
	public ClassStatusModel():base() { }
    public ClassStatusModel(bool InActive) : base(InActive) { }	public int ClassStatusID	{ get; set; }	public string ClassStatusName	{ get; set; }	public int ClassStatusReasonID { get; set; }
	public string ClassReason { get; set; }


	}}
