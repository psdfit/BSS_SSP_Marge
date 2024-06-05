
using System;

namespace DataLayer.Models
{
[Serializable]public class ClassSectionsModel :ModelBase {
	public ClassSectionsModel():base() { }
    public ClassSectionsModel(bool InActive) : base(InActive) { }	public int SectionID	{ get; set; }	public string SectionName	{ get; set; }}}
