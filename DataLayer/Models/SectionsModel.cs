
using System;

namespace DataLayer.Models
{
[Serializable]public class SectionsModel :ModelBase {
	public SectionsModel():base() { }
    public SectionsModel(bool InActive) : base(InActive) { }	public int SectionID	{ get; set; }	public string SectionName	{ get; set; }}}
