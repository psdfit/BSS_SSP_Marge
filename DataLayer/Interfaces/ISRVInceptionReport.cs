using DataLayer.Models;
using System.Collections.Generic;

namespace DataLayer.Interfaces
{

    public interface ISRVInceptionReport
    {
        InceptionReportModel GetByIncepReportID(int IncepReportID);
        List<InceptionReportModel> SaveInceptionReport(InceptionReportModel InceptionReport);
        List<InceptionReportModel> FetchInceptionReport(InceptionReportModel mod);
        List<InceptionReportModel> FetchInceptionReport(int ClassID);
        List<InceptionReportModel> FetchActiveClassInception(string InstruIds);
        List<InceptionReportModel> FetchActiveClassInceptionCR(string ClassID);
        List<InceptionReportModel> FetchInceptionReport(bool InActive);
        void ActiveInActive(int IncepReportID, bool? InActive, int CurUserID);
        void UpdateClassStatusByReport(int ClassID, int CurUserID);
        public List<InceptionReportModel> FetchInceptionReportDataByUser(int UserID);
        public List<InceptionReportModel> GetByInceptionReportID(int IncepReportID);
        public List<InceptionReportListModel> FetchInceptionReportByFilters(int[] filters);

        public List<InstructorReplaceChangeRequestModel> GetMappedInstructorsByID(int IncepReportID);  // To get current Instructors of class in the change request approvals

        public List<InceptionReportListModel> FetchInceptionReportByPaged(PagingModel pagingModel, SearchFilter filterModel, out int totalCount);

        public List<CheckRegistrationCriteriaModel> CheckReportCriteria(int ClassID);

    }
}
