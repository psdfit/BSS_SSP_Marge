using System;

namespace DataLayer.Models
{
    [Serializable]
    public class CertificationCategoryModel : ModelBase
    {
        public CertificationCategoryModel() : base()
        {
        }

        public CertificationCategoryModel(bool InActive) : base(InActive)
        {
        }

        public int CertificationCategoryID { get; set; }
        public string CertificationCategoryName { get; set; }
    }
}