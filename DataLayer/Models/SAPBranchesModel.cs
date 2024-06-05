
using System;

namespace DataLayer.Models
{
[Serializable]public class SAPBranchesModel :ModelBase {
	public SAPBranchesModel():base() { }
    public SAPBranchesModel(bool InActive) : base(InActive) { }	public int Id	{ get; set; }	public int BranchId	{ get; set; }	public string BranchName	{ get; set; }}}
