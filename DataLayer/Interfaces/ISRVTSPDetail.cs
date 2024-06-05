using DataLayer.Models;
using Microsoft.Data.SqlClient;
using System.Collections.Generic;
using System.Data;

namespace DataLayer.Interfaces
{

    public interface ISRVTSPDetail
    {
        TSPDetailModel GetByTSPID(int TSPID);
        List<TSPDetailModel> GetByScheme(int SchemeID);
        TSPDetailModel SaveTSPDetail(TSPDetailModel TSPDetail);
        TSPDetailModel SaveTSPMaster(TSPDetailModel TSPMaster);

        List<TSPDetailModel> FetchTSPDetail(TSPDetailModel mod);
        List<TSPDetailModel> FetchTSPDetailByScheme(int schemeId, SqlTransaction transaction = null);
        List<TSPDetailModel> FetchTSPDetail();
        List<TSPDetailModel> FetchTSPDetail(bool InActive);

        void ActiveInActive(int TSPID, bool? InActive, int CurUserID);

        List<KAMAssignedTSPsModel> FetchTSPListByKamUser(int UserID);
        List<TSPDetailModel> FetchTSPsByFilters(int[] filters);

        int GetTSPSequence();
        List<TSPDetailModel> FetchTSPByUser(QueryFilters filters);
        List<TSPDetailModel> FetchTSPDataByUser(int UserID);
        void UpdateTSPDetail(TSPDetailModel mod);

        public List<TSPDetailModel> FetchApprovedTSPs();

        public List<TSPDetailModel> FetchTSPByUserPaged(PagingModel pagingModel, SearchFilter filterModel, out int totalCount);
        TSPDetailModel GetUserByTSPID(int TSPID);
        TSPDetailModel FetchTSPByTSPCode(string TSPCode);
        public List<TSPDetailModel> FetchKamRelevantTSPByScheme(QueryFilters filters);
        public List<TSPDetailModel> FetchTSPDetailForROSIFilter(ROSIFiltersModel rosiFilters);
        public List<TSPDetailModel> FetchTSPMasterForFilters(bool InActive);
        DataTable FetchTSPCRDataByUser(int UserID);

        TSPDetailModel FetchTSPByUserID(QueryFilters queryFilters);

    }
}
