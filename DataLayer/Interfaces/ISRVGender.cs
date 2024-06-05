using DataLayer.Models;
using System.Collections.Generic;

namespace DataLayer.Interfaces
{
    public interface ISRVGender
    {
        GenderModel GetByGenderID(int GenderID);

        List<GenderModel> SaveGender(GenderModel Gender);

        List<GenderModel> FetchGender(GenderModel mod);

        List<GenderModel> FetchGender();

        List<GenderModel> FetchGender(bool InActive);

        void ActiveInActive(int GenderID, bool? InActive, int CurUserID);
        public List<GenderModel> FetchGenderForROSIFilter(ROSIFiltersModel rosiFilters);

    }
}