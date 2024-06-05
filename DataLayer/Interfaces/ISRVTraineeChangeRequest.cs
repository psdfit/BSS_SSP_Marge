using DataLayer.Models;
using System.Collections.Generic;
using Microsoft.Data.SqlClient;


namespace DataLayer.Interfaces
{

    public interface ISRVTraineeChangeRequest
    {
        public TraineeChangeRequestModel GetByTraineeChangeRequestID(int TraineeChangeRequestID, SqlTransaction transaction = null);
        public List<TraineeChangeRequestModel> SaveTraineeChangeRequest(TraineeChangeRequestModel TraineeChangeRequest);
        public List<TraineeChangeRequestModel> FetchTraineeChangeRequest(TraineeChangeRequestModel mod);
        public List<TraineeChangeRequestModel> FetchTraineeChangeRequest();
        public List<TraineeChangeRequestModel> FetchTraineeChangeRequest(bool InActive);
        public void ActiveInActive(int TraineeChangeRequestID, bool? InActive, int CurUserID);

        public bool CrTraineeApproveReject(TraineeChangeRequestModel model, SqlTransaction transaction = null);

        public List<TraineeChangeRequestModel> SaveVerifiedTraineeChangeRequest(TraineeChangeRequestModel TraineeChangeRequest);
        public TraineeChangeRequestModel GetByTraineeChangeRequestID_Notification(int TraineeChangeRequestID, SqlTransaction transaction = null);
    }
}
