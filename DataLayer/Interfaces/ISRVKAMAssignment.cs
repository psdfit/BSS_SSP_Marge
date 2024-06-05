using DataLayer.Models;
using System.Collections.Generic;

namespace DataLayer.Interfaces
{

    public interface ISRVKAMAssignment
    {
        KAMAssignmentModel GetByKamID(int KamID);
        List<KAMAssignmentModel> SaveKAMAssignment(KAMAssignmentModel KAMAssignment);
        List<KAMAssignmentModel> FetchKAMAssignment(KAMAssignmentModel mod);
        List<KAMAssignmentModel> FetchKAMAssignment();
        List<KAMAssignmentModel> FetchKAMAssignment(bool InActive);
        int BatchInsert(List<KAMAssignmentModel> ls, int @BatchFkey, int CurUserID);
        void ActiveInActive(int KamID, bool? InActive, int CurUserID);
        List<KAMAssignmentModel> FetchTSPKAMHistory(int tspid);
        public List<KAMAssignmentModel> FetchKAMAssignmentByTSPMaster();
        public List<KAMAssignmentModel> FetchUnAssigenedTSPMastersForKAM();
        public List<KAMAssignmentModel> FetchKAMAssignmentForFilters(bool InActive);
        public bool SendNotificationToKAMAndTSP(KAMAssignmentModel KAMAssignment);
        public List<KAMAssignmentModel> FetchKAMInfoForTSP(KAMAssignmentModel mod);

    }
}
