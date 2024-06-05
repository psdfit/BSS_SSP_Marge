using DataLayer.Models;
using System.Collections.Generic;

namespace DataLayer.Interfaces
{

    public interface ISRVUserEventMap
    {
        UserEventMapModel GetByUserEventMapID(int UserEventMapID);
        List<UserEventMapModel> SaveUserEventMap(UserEventMapModel UserEventMap);
        List<UserEventMapModel> FetchUserEventMap(UserEventMapModel mod);
        List<UserEventMapModel> FetchUserEventMap();
        List<UserEventMapModel> FetchUserEventMap(bool InActive);
        void ActiveInActive(int UserEventMapID, bool? InActive, int CurUserID);
        List<SchemeEventMapModel> FetchSchemeEventMap(SchemeEventMapModel mod);
        List<UserEventMapModel> FetchUserEventMapAll(int VisitPlanID);
        List<SchemeEventMapModel> FetchSchemeEventMapAll(int VisitPlanID);
    }
}
