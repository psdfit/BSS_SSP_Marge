
using System;

namespace DataLayer.Models
{
[Serializable]public class TSPDetailSchemeMapModel :ModelBase {
	public TSPDetailSchemeMapModel():base() { }
    public TSPDetailSchemeMapModel(bool InActive) : base(InActive) { }	public int ID	{ get; set; }	public int SchemeID	{ get; set; }	public string SchemeName { get; set; }	public int TSPID	{ get; set; }	public string TSPName { get; set; }}}
