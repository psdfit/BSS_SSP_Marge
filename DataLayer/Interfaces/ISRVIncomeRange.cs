using DataLayer.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataLayer.Interfaces
{
    public interface ISRVIncomeRange
    {
        List<IncomeRangeModel> FetchIncomeRanges(IncomeRangeModel mod);
    }
}
