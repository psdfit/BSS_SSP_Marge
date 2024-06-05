using DataLayer.Models;
using System.Collections.Generic;

namespace DataLayer.Interfaces
{

    public interface ISRVTSPDetailSchemeMap
    {
        TSPDetailSchemeMapModel GetByID(int ID);
        List<TSPDetailSchemeMapModel> SaveTSPDetailSchemeMap(TSPDetailSchemeMapModel TSPDetailSchemeMap);
        List<TSPDetailSchemeMapModel> FetchTSPDetailSchemeMap(TSPDetailSchemeMapModel mod);
        List<TSPDetailSchemeMapModel> FetchTSPDetailSchemeMap();
        List<TSPDetailSchemeMapModel> FetchTSPDetailSchemeMap(bool InActive);
        void ActiveInActive(int ID, bool? InActive, int CurUserID);
    }
}
