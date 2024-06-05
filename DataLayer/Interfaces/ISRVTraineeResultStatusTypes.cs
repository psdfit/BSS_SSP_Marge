using DataLayer.Models;
using System.Collections.Generic;

namespace DataLayer.Interfaces
{
    public interface ISRVTraineeResultStatusTypes
    {
        TraineeResultStatusTypesModel GetByResultStatusID(int ResultStatusID);

        List<TraineeResultStatusTypesModel> SaveTraineeResultStatusTypes(TraineeResultStatusTypesModel TraineeResultStatusTypes);

        List<TraineeResultStatusTypesModel> FetchTraineeResultStatusTypes(TraineeResultStatusTypesModel mod);

        List<TraineeResultStatusTypesModel> FetchTraineeResultStatusTypes();

        List<TraineeResultStatusTypesModel> FetchTraineeResultStatusTypes(bool InActive);

        void ActiveInActive(int ResultStatusID, bool? InActive, int CurUserID);
    }
}