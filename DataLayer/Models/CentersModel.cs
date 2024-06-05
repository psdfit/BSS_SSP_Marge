
using System;

namespace DataLayer.Models
{
[Serializable]

public class CentersModel :ModelBase
 {
	public CentersModel():base() { }
    public CentersModel(bool InActive) : base(InActive) { }

	public int CenterID	{ get; set; }
	public string CenterName	{ get; set; }
	public string CenterAddress	{ get; set; }
	public string CenterGeoLocation	{ get; set; }
	public int CenterDistrict	{ get; set; }
	public string DistrictName	{ get; set; }
	public int CenterTehsil	{ get; set; }
	public String TehsilName	{ get; set; }
	public string CenterInchargeName	{ get; set; }
	public string CenterInchargeMobile	{ get; set; }
	public string UID	{ get; set; }
	public bool IsMigrated	{ get; set; }

}
}
