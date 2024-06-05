
using System;

namespace DataLayer.Models
{
[Serializable]public class TradeEquipmentToolsMapModel :ModelBase {
	public TradeEquipmentToolsMapModel():base() { }
    public TradeEquipmentToolsMapModel(bool InActive) : base(InActive) { }	public int TradeEquipmentToolsMapID	{ get; set; }	public int TradeDurationMapID { get; set; }	//public string TradeName { get; set; }	public int EquipmentToolID	{ get; set; }	public string EquipmentName { get; set; }}}
