using DataLayer.Models;
using System.Collections.Generic;

namespace DataLayer.Interfaces
{
    public interface ISRVCustomFinancialYear
    {
        CustomFinancialYearModel GetCustomFinancialYearByID(int Id);

        List<CustomFinancialYearModel> SaveCustomFinancialYear(CustomFinancialYearModel mod);

        List<CustomFinancialYearModel> FetchCustomFinancialYear(CustomFinancialYearModel mod);

        List<CustomFinancialYearModel> FetchCustomFinancialYear();

        List<CustomFinancialYearModel> FetchCustomFinancialYear(bool InActive);

        void ActiveInActive(int Id, bool? InActive, int CurUserID);
    }
}