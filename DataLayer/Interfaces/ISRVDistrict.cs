using DataLayer.Models;
using System.Collections.Generic;

namespace DataLayer.Interfaces
{
    public interface ISRVDistrict
    {
         DistrictModel GetByDistrictID(int DistrictID);

         List<DistrictModel> SaveDistrict(DistrictModel District);

         List<DistrictModel> FetchDistrict(DistrictModel mod);

         List<DistrictModel> FetchDistrict();

         List<DistrictModel> FetchDistrict(bool InActive);
         List<DistrictModel> GetByDistrictName(string DistrictName);

         void ActiveInActive(int DistrictID, bool? InActive, int CurUserID);
        public List<DistrictModel> FetchAllPakistanDistrict(bool InActive);
        public List<DistrictModel> FetchDistrictForROSIFilter(ROSIFiltersModel rosiFilters);
        List<DistrictModel> AllDistrictsAndTehsils();
        List<DistrictModel> SSPFetchDistrictTSP(int programid, int UserID);

    }
}