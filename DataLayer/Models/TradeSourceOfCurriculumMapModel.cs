
using System;

namespace DataLayer.Models
{
[Serializable]public class TradeSourceOfCurriculumMapModel :ModelBase {
	public TradeSourceOfCurriculumMapModel():base() { }
    public TradeSourceOfCurriculumMapModel(bool InActive) : base(InActive) { }	public int TradeSourceOfCurriculumMapID	{ get; set; }	public int TradeID	{ get; set; }	public string TradeName { get; set; }	public int SourceOfCurriculumID	{ get; set; }	public string Name { get; set; }}}
