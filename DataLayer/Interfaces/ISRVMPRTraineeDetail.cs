using DataLayer.Models;
using System.Collections.Generic;

namespace DataLayer.Interfaces
{

    public interface ISRVMPRTraineeDetail
    {
        MPRTraineeDetailModel GetByMPRDetailID(int MPRDetailID);
        List<MPRTraineeDetailModel> SaveMPRTraineeDetail(MPRTraineeDetailModel MPRTraineeDetail);
        List<MPRTraineeDetailModel> FetchMPRTraineeDetail(MPRTraineeDetailModel mod);
        List<MPRTraineeDetailModel> FetchMPRTraineeDetail();
        List<MPRTraineeDetailModel> GetByMPRID(int MPRID);
        List<MPRTraineeDetailModel> FetchMPRTraineeDetail(bool InActive);
        void ActiveInActive(int MPRDetailID, bool? InActive, int CurUserID);
        public List<MPRTraineeDetailModel> GetMPRExcelExportByIDs(string ids);

    }
}
