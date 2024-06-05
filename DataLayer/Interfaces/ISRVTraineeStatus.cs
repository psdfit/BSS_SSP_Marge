using DataLayer.Models;
using System.Collections.Generic;

namespace DataLayer.Interfaces
{
    public interface ISRVTraineeStatus
    {
        bool SaveTraineeStatus(TraineeStatusModel TraineeStatus);
        List<TraineeStatusModel> FetchTraineeStatus(TraineeStatusModel mod);
        List<TraineeStatusModel> FetchTraineeStatusByTraineeID(int TraineeID);
        List<TraineeStatusModel> FetchTraineeStatus();
        TraineeStatusModel GetTraineeStatusByMonth(TraineeStatusModel model);
        string GetTSPUserByClassID_NADRA_Notification(List<int> ClassID);
    }
}