using DataLayer.Models;
using System.Collections.Generic;

namespace DataLayer.Interfaces
{
    public interface ISRVTehsil
    {
        TehsilModel GetByTehsilID(int TehsilID);

        List<TehsilModel> SaveTehsil(TehsilModel Tehsil);

        List<TehsilModel> FetchTehsil(TehsilModel mod);

        List<TehsilModel> FetchTehsil();

        List<TehsilModel> FetchTehsil(bool InActive);
        List<TehsilModel> GetByTehsilName(string TehsilName);


        void ActiveInActive(int TehsilID, bool? InActive, int CurUserID);
        List<TehsilModel> GetByDistrictID(int DistrictID);

        public List<TehsilModel> FetchAllPakistanTehsil(bool InActive);
        List<TehsilModel> GetByDistrictIDApi(int DistrictID);

    }
}