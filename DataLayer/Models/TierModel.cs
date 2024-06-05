
using System;

namespace DataLayer.Models
{
[Serializable]public class TierModel :ModelBase {
	public TierModel():base() { }
    public TierModel(bool InActive) : base(InActive) { }	public int TierID	{ get; set; }	public string TierName	{ get; set; }}}
