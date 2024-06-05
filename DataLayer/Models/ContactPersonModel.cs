
using System;

namespace DataLayer.Models
{
[Serializable]public class ContactPersonModel :ModelBase {
	public ContactPersonModel():base() { }
    public ContactPersonModel(bool InActive) : base(InActive) { }	public int ContactPersonID	{ get; set; }	public string ContactPersonType	{ get; set; }	public string ContactPersonName	{ get; set; }	public string ContactPersonEmail	{ get; set; }	public string ContactPersonLandline	{ get; set; }	public string ContactPersonMobile	{ get; set; }	public int IncepReportID	{ get; set; }}}
