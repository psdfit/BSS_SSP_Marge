using System;

namespace DataLayer.Models
{
    [Serializable]
    public class YearWiseInflationRateModel : ModelBase
    {
        public YearWiseInflationRateModel() : base()
        {
        }

        public YearWiseInflationRateModel(bool InActive) : base(InActive)
        {
        }

        public int IRID { get; set; }
        public string FinancialYear { get; set; }
        public string Month { get; set; }
        public double Inflation { get; set; }
    }}