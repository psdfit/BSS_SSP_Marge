using DataLayer.Models;
using System.Collections.Generic;

namespace DataLayer.Interfaces
{

    public interface ISRVContactPerson
    {
        ContactPersonModel GetByContactPersonID(int ContactPersonID);
        List<ContactPersonModel> SaveContactPerson(ContactPersonModel ContactPerson);
        List<ContactPersonModel> FetchContactPerson(ContactPersonModel mod);
        List<ContactPersonModel> FetchContactPerson();
        List<ContactPersonModel> FetchContactPerson(bool InActive);
        List<ContactPersonModel> FetchContactPerson(int IncepReportId);
        int BatchInsert(List<ContactPersonModel> ls, int @BatchFkey, int CurUserID);
        List<ContactPersonModel> GetByContactPersonMobile(string ContactPersonMobile);

        void ActiveInActive(int ContactPersonID, bool? InActive, int CurUserID);
    }
}
