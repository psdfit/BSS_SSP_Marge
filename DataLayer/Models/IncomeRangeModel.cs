using System;
using System.Collections.Generic;
using System.Text;

namespace DataLayer.Models
{
    public class IncomeRangeModel : ModelBase
    {
        public IncomeRangeModel() : base()
        {
        }
        public int IncomeRangeID { get; set; }
        public string RangeName { get; set; }
        public decimal RangeFrom { get; set; }
        public decimal RangeTo { get; set; }
    }
}
