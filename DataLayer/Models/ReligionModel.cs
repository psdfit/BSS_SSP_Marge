
using System;

namespace DataLayer.Models
{
[Serializable]public class ReligionModel :ModelBase {
	public ReligionModel():base() { }
    public ReligionModel(bool InActive) : base(InActive) { }	public int ReligionID	{ get; set; }	public string ReligionName	{ get; set; }}}
