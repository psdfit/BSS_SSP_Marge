
using System;

namespace DataLayer.Models
{
[Serializable]public class ClassEventMapModel :ModelBase {
	public ClassEventMapModel():base() { }
    public ClassEventMapModel(bool InActive) : base(InActive) { }	public int ClassEventMapID	{ get; set; }	public int VisitPlanID	{ get; set; }	public int ClassID	{ get; set; }	public string TradeName { get; set; }}}
