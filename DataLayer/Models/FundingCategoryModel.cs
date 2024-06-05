
using System;

namespace DataLayer.Models
{
[Serializable]public class FundingCategoryModel :ModelBase {
	public FundingCategoryModel():base() { }
    public FundingCategoryModel(bool InActive) : base(InActive) { }	public int FundingCategoryID	{ get; set; }	public string FundingCategoryName	{ get; set; }	public int FundingSourceID	{ get; set; }	public string FundingSourceName { get; set; }}}
