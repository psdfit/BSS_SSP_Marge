using DataLayer.Models;
using System.Collections.Generic;

namespace DataLayer.Interfaces
{
    public interface ISRVCertificationCategory
    {
        CertificationCategoryModel GetByCertificationCategoryID(int CertificationCategoryID);

        List<CertificationCategoryModel> SaveCertificationCategory(CertificationCategoryModel CertificationCategory);

        List<CertificationCategoryModel> FetchCertificationCategory(CertificationCategoryModel mod);

        List<CertificationCategoryModel> FetchCertificationCategory();

        List<CertificationCategoryModel> FetchCertificationCategory(bool InActive);

        void ActiveInActive(int CertificationCategoryID, bool? InActive, int CurUserID);
    }
}