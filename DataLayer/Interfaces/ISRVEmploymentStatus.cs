using DataLayer.Models;

    public interface ISRVEmploymentStatus
    {
        EmploymentStatusModel GetByEmploymentStatusID(int EmploymentStatusID);
        List<EmploymentStatusModel> SaveEmploymentStatus(EmploymentStatusModel EmploymentStatus);
        List<EmploymentStatusModel> FetchEmploymentStatus(EmploymentStatusModel mod);
        List<EmploymentStatusModel> FetchEmploymentStatus();
        List<EmploymentStatusModel> FetchEmploymentStatus(bool InActive);
        void ActiveInActive(int EmploymentStatusID, bool? InActive, int CurUserID);
    }