using DataLayer.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataLayer.Interfaces
{
    public interface ISRVTSRLiveData
    {
        List<TSRLiveDataModel> FetchTSRLiveData();

        List<TSRLiveDataModel> FetchTSRLiveDataByFilters(int[] filters);

        List<TSRLiveDataModel> GetFilteredTSRData(SearchFilter filters);

        List<GSRLiveDataModel> GetFilteredGSRData(SearchFilter filters);

        List<TSRLiveDataModel> FetchPaged(PagingModel pagingModel, SearchFilter filterModel, out int totalCount);
        List<TERLiveDataModel> FetchTERPaged(PagingModel pagingModel, SearchFilter filterModel, out int totalCount);
        List<TARLiveDataModel> FetchTARPaged(PagingModel pagingModel, SearchFilter filterModel, out int totalCount);

        List<GSRLiveDataModel> FetchGSRPaged(PagingModel pagingModel, SearchFilter filterModel);

        List<TSRLiveDataModel> FetchTSULiveData(PagingModel pagingModel, SearchFilter filterModel, out int totalCount);

        List<TSRLiveDataModel> FetchClassLiveData(PagingModel pagingModel, SearchFilter filterModel, int loggedInUserLevel, out int totalCount);

        bool UpdateClassStatus(int traineeID, int traineeStatusTypeID, int CurUserID, string ClassReason);

        public List<TSRLiveDataModel> FetchTCRLiveDataByFilters(int[] filters);
        
        List<TSRLiveDataModel> FetchCourseraPaged(PagingModel pagingModel, SearchFilter filterModel, out int totalCount);
        List<TSRLiveDataModel> UpdateTraineeStatusExpell(TSRLiveDataModel traineestatus);
        List<TSRLiveDataModel> UpdateTraineeStatusDropout(TSRLiveDataModel traineestatus);
    }
}