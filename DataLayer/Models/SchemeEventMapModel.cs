
using System;

namespace DataLayer.Models
{
[Serializable]public class SchemeEventMapModel :ModelBase {
	public SchemeEventMapModel():base() { }
    public SchemeEventMapModel(bool InActive) : base(InActive) { }	public int SchemeEventMapID	{ get; set; }	public int VisitPlanID	{ get; set; }	public int SchemeID	{ get; set; }}}
