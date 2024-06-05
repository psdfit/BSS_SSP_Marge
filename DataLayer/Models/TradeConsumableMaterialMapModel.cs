
using System;

namespace DataLayer.Models
{
[Serializable]public class TradeConsumableMaterialMapModel :ModelBase {
	public TradeConsumableMaterialMapModel():base() { }
    public TradeConsumableMaterialMapModel(bool InActive) : base(InActive) { }	public int TradeConsumableMaterialMapID	{ get; set; }	public int TradeDurationMapID { get; set; }	//public string TradeName { get; set; }	public int ConsumableMaterialID	{ get; set; }	public string ItemName { get; set; }}}
