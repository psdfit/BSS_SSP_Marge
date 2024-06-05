
using System;

namespace DataLayer.Models
{
[Serializable]public class DurationModel :ModelBase {
	public DurationModel():base() { }
    public DurationModel(bool InActive) : base(InActive) { }	public int DurationID	{ get; set; }	public float Duration	{ get; set; }}}
