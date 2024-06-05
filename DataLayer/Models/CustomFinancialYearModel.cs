using System;

namespace DataLayer.Models
{
    [Serializable]
    public class CustomFinancialYearModel : ModelBase
    {
        public CustomFinancialYearModel() : base()
        {
        }

        public CustomFinancialYearModel(bool InActive) : base(InActive)
        {
        }

        public int Id { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public int OrgID { get; set; }
        public string OrgName { get; set; }
    }
}