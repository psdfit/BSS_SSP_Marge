
using System;

namespace DataLayer.Models
{
[Serializable]
	public CertificationAuthorityModel():base() { }
    public CertificationAuthorityModel(bool InActive) : base(InActive) { }
	public int CertificationCategoryID { get; set; }
	public string CertificationCategoryName { get; set; }


	}