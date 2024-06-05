
using System;

namespace DataLayer.Models
{
[Serializable]public class CertificationAuthorityModel :ModelBase {
	public CertificationAuthorityModel():base() { }
    public CertificationAuthorityModel(bool InActive) : base(InActive) { }	public int CertAuthID	{ get; set; }	public string CertAuthName	{ get; set; }
	public int CertificationCategoryID { get; set; }
	public string CertificationCategoryName { get; set; }


	}}
