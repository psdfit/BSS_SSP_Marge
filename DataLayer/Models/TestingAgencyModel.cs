using System;

namespace DataLayer.Models
{
    [Serializable]
    public class TestingAgencyModel : ModelBase
    {
        public TestingAgencyModel() : base()
        {
        }

        public TestingAgencyModel(bool InActive) : base(InActive)
        {
        }

        public int TestingAgencyID { get; set; }
        public string TestingAgencyName { get; set; }
        public int CertificationCategoryID { get; set; }
        public string CertificationCategoryName { get; set; }
    }}