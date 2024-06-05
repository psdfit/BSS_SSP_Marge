using DataLayer.Models;
using System.Collections.Generic;

namespace DataLayer.Interfaces
{
    public interface ISRVYearWiseInflationRate
    {
        YearWiseInflationRateModel GetByIRID(int IRID);

        List<YearWiseInflationRateModel> SaveYearWiseInflationRate(YearWiseInflationRateModel YearWiseInflationRate);

        List<YearWiseInflationRateModel> FetchYearWiseInflationRate(YearWiseInflationRateModel mod);

        List<YearWiseInflationRateModel> FetchYearWiseInflationRate();

        List<YearWiseInflationRateModel> FetchYearWiseInflationRate(bool InActive);

        void ActiveInActive(int IRID, bool? InActive, int CurUserID);
    }
}