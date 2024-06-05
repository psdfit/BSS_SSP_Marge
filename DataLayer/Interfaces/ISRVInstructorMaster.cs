using DataLayer.Models;
using System.Collections.Generic;

namespace DataLayer.Interfaces
{

    public interface ISRVInstructorMaster
    {
        InstructorMasterModel GetByInstrMasterID(int InstrMasterID);
        List<InstructorMasterModel> SaveInstructorMaster(InstructorMasterModel InstructorMaster);
        List<InstructorMasterModel> FetchInstructorMaster(InstructorMasterModel mod);
        List<InstructorMasterModel> FetchInstructorMaster();
        List<InstructorMasterModel> FetchInstructorMaster(bool InActive);
        void ActiveInActive(int InstrMasterID, bool? InActive, int CurUserID);
    }
}
