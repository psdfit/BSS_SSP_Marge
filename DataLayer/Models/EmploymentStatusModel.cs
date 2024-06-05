
using System;

namespace DataLayer.Models
{
[Serializable]public class EmploymentStatusModel :ModelBase {
	public EmploymentStatusModel():base() { }
    public EmploymentStatusModel(bool InActive) : base(InActive) { }	public int EmploymentStatusID	{ get; set; }	public string EmploymentStatusName	{ get; set; }}}
