using DataLayer.Models;
using System.Collections.Generic;

namespace DataLayer.Interfaces
{
    public interface ISRVTraineeDisability
    {
        TraineeDisabilityModel GetById(int Id);

        List<TraineeDisabilityModel> SaveTraineeDisability(TraineeDisabilityModel TraineeDisability);

        List<TraineeDisabilityModel> FetchTraineeDisability(TraineeDisabilityModel mod);

        List<TraineeDisabilityModel> FetchTraineeDisability();

        List<TraineeDisabilityModel> FetchTraineeDisability(bool InActive);

        void ActiveInActive(int Id, bool? InActive, int CurUserID);
    }
}