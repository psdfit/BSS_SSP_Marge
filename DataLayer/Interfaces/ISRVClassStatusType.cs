using DataLayer.Models;
using System.Collections.Generic;

namespace DataLayer.Interfaces
{
    public interface ISRVClassStatusType
    {
        ClassStatusTypeModel GetByStatusTypeID(int StatusTypeID);

        List<ClassStatusTypeModel> SaveClassStatusType(ClassStatusTypeModel ClassStatusType);

        List<ClassStatusTypeModel> FetchClassStatusType(ClassStatusTypeModel mod);

        List<ClassStatusTypeModel> FetchClassStatusType();

        List<ClassStatusTypeModel> FetchClassStatusType(bool InActive);

        void ActiveInActive(int StatusTypeID, bool? InActive, int CurUserID);
    }
}