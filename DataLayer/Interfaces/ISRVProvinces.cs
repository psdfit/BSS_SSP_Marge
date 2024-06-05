using DataLayer.Models;
using System.Collections.Generic;

namespace DataLayer.Interfaces
{
    public interface ISRVProvinces
    {
        ProvincesModel GetById(int Id);

        List<ProvincesModel> SaveProvinces(ProvincesModel Provinces);

        List<ProvincesModel> FetchProvinces(ProvincesModel mod);

        List<ProvincesModel> FetchProvinces();

        List<ProvincesModel> FetchProvinces(bool InActive);

        List<ProvincesModel> FetchProvince(bool InActive);
        void ActiveInActive(int Id, bool? InActive, int CurUserID);
    }
}