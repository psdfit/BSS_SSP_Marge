using DataLayer.Models;
using System.Collections.Generic;

namespace DataLayer.Interfaces
{
    public interface ISRVTraineeStatusTypes
    {
        TraineeStatusTypesModel GetByStatusID(int StatusID);

        List<TraineeStatusTypesModel> SaveTraineeStatusTypes(TraineeStatusTypesModel TraineeStatusTypes);

        List<TraineeStatusTypesModel> FetchTraineeStatusTypes(TraineeStatusTypesModel mod);

        List<TraineeStatusTypesModel> FetchTraineeStatusTypes();

        List<TraineeStatusTypesModel> FetchTraineeStatusTypes(bool InActive);
        List<TraineeStatusTypesModel> FetchTraineeStatusReason(bool InActive);
        void ActiveInActive(int StatusID, bool? InActive, int CurUserID);
    }
}