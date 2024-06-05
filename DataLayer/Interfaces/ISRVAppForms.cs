using DataLayer.Models;
using System.Collections.Generic;

namespace DataLayer.Interfaces
{
    public interface ISRVAppForms
    {
        AppFormsModel GetByFormID(int FormID);

        List<AppFormsModel> SaveAppForms(AppFormsModel AppForms);

        List<AppFormsModel> FetchAppForms(AppFormsModel mod);

        List<AppFormsModel> FetchAppForms();

        List<AppFormsModel> FetchAppForms(bool InActive);

        void ActiveInActive(int FormID, bool? InActive, int CurUserID);

        int BatchInsert(List<AppFormsModel> ls, int BatchFkey, int CurUserID);
    }
}