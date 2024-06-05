
using System;

namespace DataLayer.Models
{
[Serializable]public class FundingSourceModel :ModelBase {
	public FundingSourceModel():base() { }
    public FundingSourceModel(bool InActive) : base(InActive) { }	public int FundingSourceID	{ get; set; }	public string FundingSourceName	{ get; set; }}}
