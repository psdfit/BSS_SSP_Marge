
using System;

namespace DataLayer.Models
{
[Serializable]public class PlacementTypeModel :ModelBase {
	public PlacementTypeModel():base() { }
    public PlacementTypeModel(bool InActive) : base(InActive) { }	public int PlacementTypeID	{ get; set; }	public string PlacementType	{ get; set; }}}
