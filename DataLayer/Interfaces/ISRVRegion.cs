using DataLayer.Models;
using System.Collections.Generic;

namespace DataLayer.Interfaces
{
    public interface ISRVRegion
    {
        RegionModel GetByRegionID(int RegionID);

        List<RegionModel> SaveRegion(RegionModel Region);

        List<RegionModel> FetchRegion(RegionModel mod);

        List<RegionModel> FetchRegion();

        List<RegionModel> FetchRegion(bool InActive);

        void ActiveInActive(int RegionID, bool? InActive, int CurUserID);
    }
}