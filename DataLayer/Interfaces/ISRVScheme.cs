using DataLayer.Models;
using Microsoft.Data.SqlClient;
using System.Collections.Generic;
using System.Data;

namespace DataLayer.Interfaces
{
    public interface ISRVScheme
    {
        SchemeModel GetLastScheme(int UserID);

        public DataTable FetchReport(int UserID, string SpName);
        SchemeModel GetBySchemeID(int SchemeID, SqlTransaction transaction = null);

        List<SchemeModel> SaveScheme(SchemeModel Scheme);

        List<SchemeModel> FetchScheme(SchemeModel mod);

        List<SchemeModel> FetchScheme();

        List<SchemeModel> FetchScheme(bool InActive);

        //================Azhar Iqbal =================
        List<SchemeModel> FetchSchemeBusinessRuleType(string BusinessRuleType);

        //=============================================
        List<SchemeModel> FetchAllScheme(SchemeModel mod);

        SchemeModel GetAllSchemeBySchemeID(int SchemeID, SqlTransaction transaction = null);

        void FinalSubmit(SchemeModel mod);

        void DeleteDraftAppendix(int schemeID);

        void ActiveInActive(int SchemeID, bool? InActive, int CurUserID);

        //List<SubmittedSchemesModel> GetSubmittedSchemes();

        List<SchemeModel> FetchSchemeForFilter(bool InActive);

        int GetSchemeSequence();

        bool SchemeApproveReject(SchemeModel model, SqlTransaction transaction = null);

        List<SchemeModel> FetchSchemesByTSPUser(int userID = 0);
        List<SchemeModel> FetchSchemesByTSPUserOnJob(int userID = 0);
        List<SchemeModel> FetchSchemesBySkillScholarshipType(int userID = 0);

        List<SchemeModel> FetchSchemeByUser(QueryFilters filters);

        int GetUnverifiedTraineeEmail(int? tspId);

        int GetUnverifiedTraineeEmailByKam(int KamId);

        List<SchemeModel> FetchSchemeByUser_DVV(int userID);

        //public List<SchemeModel> LoadNotSubmittedSchemes(int UserID);
        void RemoveFromAppendix(int FormID, string Form);

        bool UpdateSchemeSAPID(int schemeID, string sapObjId, SqlTransaction transaction = null);

        List<SchemeModel> FetchSchemeDataByUser(int UserID);

        List<SchemeModel> FetchSchemeByUserPaged(PagingModel pagingModel, SearchFilter filterModel, out int totalCount);

        SchemeModel GetBySchemeID_Notification(int SchemeID, SqlTransaction transaction = null);

        public List<SchemeModel> FetchSchemesByKAMUser(int userID = 0);

        public List<SchemeModel> FetchSchemeForROSIFilter(ROSIFiltersModel rosiFilters);

        List<SchemeModel> SSPFetchSchemeByUser(int UserID);

        void GenerateAutoAppendix(int schemeID, int CurUserID);
    }
}