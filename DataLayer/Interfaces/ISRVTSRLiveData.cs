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

        List<TSRLiveDataModel> FetchPaged(PagingModel pagingModel, SearchFilter filterModel, out int totalCount);

        List<TSRLiveDataModel> FetchTSULiveData(PagingModel pagingModel, SearchFilter filterModel, out int totalCount);

        List<TSRLiveDataModel> FetchClassLiveData(PagingModel pagingModel, SearchFilter filterModel, int loggedInUserLevel, out int totalCount);

        bool UpdateClassStatus(int traineeID, int traineeStatusTypeID, int CurUserID, string ClassReason);

        public List<TSRLiveDataModel> FetchTCRLiveDataByFilters(int[] filters);
        
        List<TSRLiveDataModel> FetchCourseraPaged(PagingModel pagingModel, SearchFilter filterModel, out int totalCount);
        List<TSRLiveDataModel> UpdateTraineeStatusExpell(TSRLiveDataModel traineestatus);
        List<TSRLiveDataModel> UpdateTraineeStatusDropout(TSRLiveDataModel traineestatus);
    }
}