using DataLayer.Models;
using Microsoft.Data.SqlClient;
using System.Collections.Generic;
using System.Data;

namespace DataLayer.Interfaces
{

    public interface ISRVClass
    {

        ClassModel GetByClassID(int ClassID);
        List<CheckRegistrationCriteriaModel> CheckRegistrationCriteria(int ClassID);
        List<ClassModel> GetBySchemeID(int SchemeID);
        List<ClassModel> FetchClassByScheme(int SchemeID, SqlTransaction transaction = null);
        ClassModel SaveClass(ClassModel Class);
        void UpdateClassStatus(int ClassID, int ClassStatusID);
        List<ClassModel> FetchClass(ClassModel mod);
        List<ClassModel> FetchClassByTsp(int tspId);
        public List<ClassModel> FetchClass();
        public List<ClassModel> FetchClass(bool InActive);
        public void ActiveInActive(int ClassID, bool? InActive, int CurUserID);
        public void RTP(int ClassID, bool? RTP, int CurUserID);
        public int GetClassSequence();
        public List<ClassModel> FetchClassesDataByUser(int UserID);


        List<ClassModel> FetchClassByFilters(int[] filters);
        List<ClassModel> FetchClassesByUser(QueryFilters filters);
        List<ClassModel> FetchClassesByUser_DVV(QueryFilters filters);
        List<ClassProceeedingStatusData> FetchClassProceeedingStatusDataByFilters(int[] filters);
        public List<ClassModel> FetchApprovedClass();

        public List<ClassModel> FetchPendingInceptionReportClassesByUser(QueryFilters filters);
        public List<ClassModel> FetchPendingRegisterationClassesByUser(QueryFilters filters);
        public List<ClassModel> FetchPendingRTPClassesByUser(QueryFilters filters);
        public List<ClassModel> FetchPendingInceptionReportClassesByKAMUser(QueryFilters filters);
        public List<ClassModel> FetchPendingRegisterationClassesByKAMUser(QueryFilters filters);
        public List<ClassModel> FetchPendingRTPClassesByKAMUser(QueryFilters filters);
        public List<ClassModel> FetchApprovadClassesByModel(ClassModel mod);

        List<ClassModel> FetchClassesByUserPaged(PagingModel pagingModel, SearchFilter filterModel, out int totalCount);

        public List<ClassModel> FetchClassForPBTEFilter();
        public List<ClassModel> FetchClassesForPRNCompletion(QueryFilters filters, out string TotalCompletedClasses, out string CompletedClassesWithResult, out string IsGenerated);
        public List<ClassModel> FetchClassesForPRNFinal(QueryFilters filters, out string TotalCompletedClasses, out string CompletedClassesWithResult, out string IsGenerated);
        public List<ClassModel> FetchClassesForTRN(QueryFilters filters, out string TotalCompletedClasses, out string CompletedClassesWithResult, out string IsGenerated);
        public DataTable FetchDeveiceStatus(int UserID);
    }
}
