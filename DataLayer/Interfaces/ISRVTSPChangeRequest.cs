using DataLayer.Models;
using System.Collections.Generic;
using Microsoft.Data.SqlClient;


namespace DataLayer.Interfaces
{

    public interface ISRVTSPChangeRequest
    {
        public TSPChangeRequestModel GetByTSPChangeRequestID(int TSPChangeRequestID, SqlTransaction transaction = null);
        public List<TSPChangeRequestModel> SaveTSPChangeRequest(TSPChangeRequestModel TSPChangeRequest);
        public List<TSPChangeRequestModel> FetchTSPChangeRequest(TSPChangeRequestModel mod);
        public List<TSPChangeRequestModel> FetchTSPChangeRequest();
        public List<TSPChangeRequestModel> FetchTSPChangeRequest(bool InActive);
        public void ActiveInActive(int TSPChangeRequestID, bool? InActive, int CurUserID);
        public bool CrTSPApproveReject(TSPChangeRequestModel model, SqlTransaction transaction = null);
        public TSPChangeRequestModel GetByTSPChangeRequestID_Notification(int TSPChangeRequestID, SqlTransaction transaction = null);
    }
}
