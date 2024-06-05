using DataLayer.Models;
using System.Collections.Generic;

namespace DataLayer.Interfaces
{

    public interface ISRVClassStatus
    {
        ClassStatusModel GetByClassStatusID(int ClassStatusID);
        List<ClassStatusModel> SaveClassStatus(ClassStatusModel ClassStatus);
        List<ClassStatusModel> FetchClassStatus(ClassStatusModel mod);
        List<ClassStatusModel> FetchClassStatus();
        List<ClassStatusModel> FetchClassReason();
        List<ClassStatusModel> FetchClassStatus(bool InActive);
        void ActiveInActive(int ClassStatusID, bool? InActive, int CurUserID);
    }
}
