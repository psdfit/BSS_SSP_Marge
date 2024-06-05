
using System;

namespace DataLayer.Models
{
[Serializable]public class ApprovalProcessModel :ModelBase {
	public ApprovalProcessModel():base() { }
    public ApprovalProcessModel(bool InActive) : base(InActive) { }	public string ProcessKey	{ get; set; }	public string ApprovalProcessName	{ get; set; }}}
