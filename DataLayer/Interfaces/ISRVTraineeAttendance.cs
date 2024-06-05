using DataLayer.Models;
using System.Collections.Generic;

namespace DataLayer.Interfaces
{

    public interface ISRVTraineeAttendance
    {
        TraineeAttendanceModel GetByTraineeAttendanceID(int TraineeAttendanceID);
        List<TraineeAttendanceModel> SaveTraineeAttendance(TraineeAttendanceModel TraineeAttendance);
        List<TraineeAttendanceModel> FetchTraineeAttendance(TraineeAttendanceModel mod);
        List<TraineeAttendanceModel> FetchTraineeAttendanceByMonth(TraineeAttendanceModel model);
        List<TraineeAttendanceModel> FetchTraineeAttendance();
        List<TraineeAttendanceModel> FetchTraineeAttendance(bool InActive);
        void ActiveInActive(int TraineeAttendanceID, bool? InActive, int CurUserID);
    }
}
