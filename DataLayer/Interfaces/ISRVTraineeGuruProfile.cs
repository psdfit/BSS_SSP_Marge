using DataLayer.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Interfaces
{
    public interface ISRVTraineeGuruProfile
    {
        DataTable GetByTraineeProfileID(int UserID);
        DataTable GetTraineeGuruDetail(int traineeID);
        //bool DeleteByTraineeProfileID(int traineeProfileID);
        //bool UpdateTraineeGuruProfile(TraineeGuruModel model);
        bool SaveTraineeGuruProfile(TraineeGuruModel model);
    }
}
