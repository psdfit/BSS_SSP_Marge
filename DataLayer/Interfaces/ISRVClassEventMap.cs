using DataLayer.Models;
using System.Collections.Generic;

namespace DataLayer.Interfaces
{

    public interface ISRVClassEventMap
    {
        ClassEventMapModel GetByClassEventMapID(int ClassEventMapID);
        List<ClassEventMapModel> SaveClassEventMap(ClassEventMapModel ClassEventMap);
        List<ClassEventMapModel> FetchClassEventMap(ClassEventMapModel mod);
        List<ClassEventMapModel> FetchClassEventMap();
        List<ClassEventMapModel> FetchClassEventMap(bool InActive);
        void ActiveInActive(int ClassEventMapID, bool? InActive, int CurUserID);

        List<ClassEventMapModel> FetchClassEventMapAll(int VisitPlanID);

    }
}
