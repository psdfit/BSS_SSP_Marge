using DataLayer.Models;
using System.Collections.Generic;

namespace DataLayer.Interfaces
{

    public interface ISRVCenters
    {
        CentersModel GetByCenterID(int CenterID);
        List<CentersModel> SaveCenters(CentersModel Centers);
        List<CentersModel> FetchCenters(CentersModel mod);
        List<CentersModel> FetchCenters();
        List<CentersModel> FetchCenters(bool InActive);
        void ActiveInActive(int CenterID, bool? InActive, int CurUserID);
    }
}
