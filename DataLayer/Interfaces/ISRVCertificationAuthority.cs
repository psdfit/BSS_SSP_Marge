using DataLayer.Models;
using System.Collections.Generic;

namespace DataLayer.Interfaces
{

    public interface ISRVCertificationAuthority
    {
        CertificationAuthorityModel GetByCertAuthID(int CertAuthID);
        List<CertificationAuthorityModel> SaveCertificationAuthority(CertificationAuthorityModel CertificationAuthority);
        List<CertificationAuthorityModel> FetchCertificationAuthority(CertificationAuthorityModel mod);
        List<CertificationAuthorityModel> FetchCertificationAuthority();
        List<CertificationAuthorityModel> FetchCertificationAuthority(bool InActive);
        void ActiveInActive(int CertAuthID, bool? InActive, int CurUserID);
    }
}
