using DataLayer.Models;
using System.Collections.Generic;

namespace DataLayer.Interfaces
{
    public interface ISRVTestingAgency
    {
        TestingAgencyModel GetByTestingAgencyID(int TestingAgencyID);

        List<TestingAgencyModel> SaveTestingAgency(TestingAgencyModel TestingAgency);

        List<TestingAgencyModel> FetchTestingAgency(TestingAgencyModel mod);

        List<TestingAgencyModel> FetchTestingAgency();

        List<TestingAgencyModel> FetchTestingAgency(bool InActive);

        void ActiveInActive(int TestingAgencyID, bool? InActive, int CurUserID);
    }
}