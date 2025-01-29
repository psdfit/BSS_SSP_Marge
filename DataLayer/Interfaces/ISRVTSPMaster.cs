using DataLayer.Models;
using Microsoft.Data.SqlClient;
using System.Collections.Generic;

namespace DataLayer.Interfaces
{

    public interface ISRVTSPMaster
    {
        TSPMasterModel GetByTSPMasterID(int TSPID, SqlTransaction transaction = null);
        List<TSPMasterModel> SaveTSPMaster(TSPMasterModel TSPMaster, SqlTransaction transaction = null);
        List<TSPMasterModel> FetchTSPMaster(TSPMasterModel mod);
        TSPMasterModel CheckDupplicateTspByNTN(string ntn,string TSPName);
        List<TSPMasterModel> FetchTSPMaster(SqlTransaction transaction = null);
        List<TSPMasterModel> FetchTSPMaster(bool InActive);
        void ActiveInActive(int TSPID, bool? InActive, int CurUserID);
        void AddUpdateTSPMasterFromDetail(TSPDetailModel item);
        bool CheckIfExists(int TSPMasterID);
        bool UpdateTSPSAPId(int tspMasterId, string sapObjId, SqlTransaction transaction = null);
        public TSPMasterModel GetTSPUserByClassID(int ClassID);
        public TSPMasterModel GetKAMUserByClassID(int ClassID);
        public UsersModel KAMUserByTSPUserID(int UserID);
        public ApprovalHistoryModel GET_ConcateClassescodebySRNID_Notification(string SRNIDs, SqlTransaction transaction = null);
        public string GET_KAMAndTspUserBySRNIDs_Notification(string SRNIDs, SqlTransaction transaction = null);
        public string GET_KAMAndTspUserByTPRNIDs_Notification(string SRNIDs, SqlTransaction transaction = null);
        public ApprovalHistoryModel GET_ConcateClassescodebyTPRNID_Notification(string SRNIDs, SqlTransaction transaction = null);
        long CheckDupplicateTspByNTNAlert(string ntn, string TSPName);
    }
}
