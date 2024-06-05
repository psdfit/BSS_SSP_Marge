
using System;

namespace DataLayer.Models
{
[Serializable]public class ConsumableMaterialModel :ModelBase {
	public ConsumableMaterialModel():base() { }
    public ConsumableMaterialModel(bool InActive) : base(InActive) { }	public int ConsumableMaterialID	{ get; set; }	public string ItemName	{ get; set; }}}
