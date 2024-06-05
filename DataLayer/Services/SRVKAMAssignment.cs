using System;
using System.Collections.Generic;
using System.Data;
using Microsoft.Data.SqlClient;
using DataLayer.Classes;
using DataLayer.Models;
using Newtonsoft.Json;
using DataLayer.Interfaces;
namespace DataLayer.Services
{
    public class SRVKAMAssignment : SRVBase, ISRVKAMAssignment
    {
        private readonly ISRVSendEmail srvSendEmail;
        private readonly ISRVTSPMaster srvTSPMaster;
        private readonly ISRVUsers srvUsers;

        public SRVKAMAssignment(ISRVSendEmail srvSendEmail, ISRVTSPMaster srvTSPMaster, ISRVUsers srvUsers) { 
            this.srvSendEmail = srvSendEmail;
            this.srvTSPMaster = srvTSPMaster;
            this.srvUsers = srvUsers;
        }
        public KAMAssignmentModel GetByKamID(int KamID)
        {
            try
            {
                SqlParameter param = new SqlParameter("@KamID", KamID);
                DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_KAMAssignment", param).Tables[0];
                if (dt.Rows.Count > 0)
                {
                    return RowOfKAMAssignment(dt.Rows[0]);
                }
                else
                    return null;
            }
            catch (Exception ex) { throw new Exception(ex.Message); }

        }
        public List<KAMAssignmentModel> SaveKAMAssignment(KAMAssignmentModel KAMAssignment)
        {
            try
            {

                SqlParameter[] param = new SqlParameter[5];
                param[0] = new SqlParameter("@KamID", KAMAssignment.KamID);
                param[1] = new SqlParameter("@UserID", KAMAssignment.UserID);
                param[2] = new SqlParameter("@TspID", KAMAssignment.TspID);
                param[3] = new SqlParameter("@ClassID", KAMAssignment.ClassID);

                param[4] = new SqlParameter("@CurUserID", KAMAssignment.CurUserID);
                SqlHelper.ExecuteNonQuery(SqlHelper.GetCon(), CommandType.StoredProcedure, "[AU_KAMAssignment]", param);
                SendNotificationToKAMAndTSP(KAMAssignment);
                return FetchKAMAssignmentByTSPMaster();
            }
            catch (Exception ex)
            { throw new Exception(ex.Message); }
        }

        public bool SendNotificationToKAMAndTSP(KAMAssignmentModel KAMAssignment)
        {
            try
            {
                //send notification to KAM
                ApprovalModel approvalsModelForNotification = new ApprovalModel();
                ApprovalHistoryModel model = new ApprovalHistoryModel();
                approvalsModelForNotification.ProcessKey = EnumApprovalProcess.KAM_ASSIGNMENT;
                approvalsModelForNotification.CustomComments = KAMAssignment.TSPName + "(TSP) has been assigned to you";
                approvalsModelForNotification.UserIDs = KAMAssignment.UserID.ToString();
                srvSendEmail.GenerateEmailAndSendNotification(approvalsModelForNotification, model);
                //send notification to TSP
                TSPMasterModel tSPMaster = srvTSPMaster.GetByTSPMasterID(KAMAssignment.TspID);
                UsersModel usersModel = srvUsers.GetByUserID(tSPMaster.KAMID??0);
                approvalsModelForNotification.ProcessKey = EnumApprovalProcess.KAM_ASSIGNMENT;
                approvalsModelForNotification.CustomComments = "Mr."+ usersModel.UserName + " has been assigned as your Key Account Manager (KAM)";
                approvalsModelForNotification.UserIDs = tSPMaster.UserID.ToString();
                srvSendEmail.GenerateEmailAndSendNotification(approvalsModelForNotification, model);
                return true;
            }
            catch (Exception ex)
            { throw new Exception(ex.Message); }
        }

        public int BatchInsert(List<KAMAssignmentModel> ls, int @BatchFkey, int CurUserID)
        {
           
            SqlParameter[] param = new SqlParameter[3];
            param[0] = new SqlParameter("@Json", JsonConvert.SerializeObject(ls));
            param[1] = new SqlParameter("@UserID", @BatchFkey);
            param[2] = new SqlParameter("@CurUserID", CurUserID);
            //return SqlHelper.ExecuteNonQuery(SqlHelper.GetCon(), CommandType.StoredProcedure, "[BAU_KAMAssignment]", param);
            return SqlHelper.ExecuteNonQuery(SqlHelper.GetCon(), CommandType.StoredProcedure, "[BAU_KAMAssignment_ByTSPMaster]", param);
            
        }
        private List<KAMAssignmentModel> LoopinData(DataTable dt)
        {
            List<KAMAssignmentModel> KAMAssignmentL = new List<KAMAssignmentModel>();

            foreach (DataRow r in dt.Rows)
            {
                KAMAssignmentL.Add(RowOfKAMAssignment(r));

            }
            return KAMAssignmentL;
        }
        private List<KAMAssignmentModel> LoopinKAMInfoData(DataTable dt)
        {
            List<KAMAssignmentModel> KAMAssignmentL = new List<KAMAssignmentModel>();

            foreach (DataRow r in dt.Rows)
            {
                KAMAssignmentL.Add(RowOfKAMInfo(r));

            }
            return KAMAssignmentL;
        }
        private List<KAMAssignmentModel> LoopinUnAssignedTSPsData(DataTable dt)
        {
            List<KAMAssignmentModel> KAMAssignmentL = new List<KAMAssignmentModel>();

            foreach (DataRow r in dt.Rows)
            {
                KAMAssignmentL.Add(RowOfUnAssignedKamTSPMaster(r));

            }
            return KAMAssignmentL;
        }
        private List<KAMAssignmentModel> LoopinTSPMastersAssigenedKamsData(DataTable dt)
        {
            List<KAMAssignmentModel> KAMAssignmentL = new List<KAMAssignmentModel>();

            foreach (DataRow r in dt.Rows)
            {
                KAMAssignmentL.Add(RowOfKAMAssignmentByTSPMaster(r));

            }
            return KAMAssignmentL;
        }
        private List<KAMAssignmentModel> LoopinKAMHistoryData(DataTable dt)
        {
            List<KAMAssignmentModel> KAMAssignmentL = new List<KAMAssignmentModel>();

            foreach (DataRow r in dt.Rows)
            {
                KAMAssignmentL.Add(RowOfTSPKAMHistory(r));

            }
            return KAMAssignmentL;
        }
        public List<KAMAssignmentModel> FetchKAMAssignment(KAMAssignmentModel mod)
        {
            try
            {
                DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_KAMAssignment", Common.GetParams(mod)).Tables[0];
                return LoopinData(dt);
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }
        public List<KAMAssignmentModel> FetchKAMAssignment()
        {
            try
            {
                DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_KAMAssignment").Tables[0];
                return LoopinData(dt);
            }
            catch (Exception ex) { throw new Exception(ex.Message); }

        }
        public List<KAMAssignmentModel> FetchKAMInfoForTSP(KAMAssignmentModel mod)
        {
            try
            {
                DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_KAMAssignment_byTSPUser", Common.GetParams(mod)).Tables[0];
                return LoopinKAMInfoData(dt);
            }
            catch (Exception ex) { throw new Exception(ex.Message); }

        }
        public List<KAMAssignmentModel> FetchKAMAssignmentByTSPMaster()
        {
            try
            {
                DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_KAMAssignment_By_TSPMaster").Tables[0];
                return LoopinTSPMastersAssigenedKamsData(dt);
            }
            catch (Exception ex) { throw new Exception(ex.Message); }

        }
        public List<KAMAssignmentModel> FetchUnAssigenedTSPMastersForKAM()
        {
            try
            {
                DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_TSPMastersForKAM").Tables[0];
                return LoopinUnAssignedTSPsData(dt);
            }
            catch (Exception ex) { throw new Exception(ex.Message); }

        }
        public List<KAMAssignmentModel> FetchKAMAssignment(bool InActive)
        {
            try
            {
                DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_KAMAssignment", new SqlParameter("@InActive", InActive)).Tables[0];
                return LoopinData(dt);
            }
            catch (Exception ex) { throw new Exception(ex.Message); }

        }
        public List<KAMAssignmentModel> FetchTSPKAMHistory(int tspid)
        {
            try
            {
                DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_TSPMasterKAMHistory", new SqlParameter("@TSPMasterID", tspid)).Tables[0];
                return LoopinKAMHistoryData(dt);
            }
            catch (Exception ex) { throw new Exception(ex.Message); }

        }
        public List<KAMAssignmentModel> FetchTSPs(int UserID)
        {
            try
            {
                DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_UserNotificationMapAll", new SqlParameter("@UserID", UserID)).Tables[0];
                return LoopinData(dt);
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }
        public List<KAMAssignmentModel> GetByUserID(int UserID)
        {
            try
            {
                DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_KAMAssignment", new SqlParameter("@UserID", UserID)).Tables[0];
                return LoopinData(dt);
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }
        public List<KAMAssignmentModel> GetByTspID(int TspID)
        {
            try
            {
                DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_KAMAssignment", new SqlParameter("@TspID", TspID)).Tables[0];
                return LoopinData(dt);
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }
        public List<KAMAssignmentModel> FetchKAMAssignmentForFilters(bool InActive)
        {
            try
            {
                DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_KAMAssignmentForFilters", new SqlParameter("@InActive", InActive)).Tables[0];
                List<KAMAssignmentModel> KAMAssignmentModel = new List<KAMAssignmentModel>();
                KAMAssignmentModel = Helper.ConvertDataTableToModel<KAMAssignmentModel>(dt);
                return (KAMAssignmentModel);
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }
        public void ActiveInActive(int KamID, bool? InActive, int CurUserID)
        {

            SqlParameter[] PLead = new SqlParameter[3];
            PLead[0] = new SqlParameter("@KamID", KamID);
            PLead[1] = new SqlParameter("@InActive", InActive);
            PLead[2] = new SqlParameter("@CurUserID", CurUserID);
            SqlHelper.ExecuteNonQuery(SqlHelper.GetCon(), CommandType.StoredProcedure, "[AI_KAMAssignment]", PLead);
        }
        private KAMAssignmentModel RowOfKAMAssignment(DataRow r)
        {
            KAMAssignmentModel KAMAssignment = new KAMAssignmentModel();
            KAMAssignment.KamID = Convert.ToInt32(r["KamID"]);
            KAMAssignment.UserID = Convert.ToInt32(r["UserID"]);
            KAMAssignment.UserName = r["UserName"].ToString();
            KAMAssignment.TspID = Convert.ToInt32(r["TspID"]);
            KAMAssignment.TSPName = r["TSPName"].ToString();
            KAMAssignment.DistrictName = r["DistrictName"].ToString();

            ///NOT FOUND IN BOTH DBs (BSS & PSDF_DB)
            KAMAssignment.RegionName = r["RegionName"].ToString();
            KAMAssignment.ClassID = Convert.ToInt32(r["ClassID"]);
            KAMAssignment.InActive = Convert.ToBoolean(r["InActive"]);
            KAMAssignment.CreatedUserID = Convert.ToInt32(r["CreatedUserID"]);
            KAMAssignment.ModifiedUserID = Convert.ToInt32(r["ModifiedUserID"]);
            KAMAssignment.CreatedDate = r["CreatedDate"].ToString().GetDate();
            KAMAssignment.ModifiedDate = r["ModifiedDate"].ToString().GetDate();

            if (r.Table.Columns.Contains("FullName"))
            {
                KAMAssignment.FullName = r["FullName"].ToString();
            }
            if (r.Table.Columns.Contains("Email"))
            {
                KAMAssignment.Email = r["Email"].ToString();
            }

            return KAMAssignment;
        }
        
        private KAMAssignmentModel RowOfKAMInfo(DataRow r)
        {
            KAMAssignmentModel KAMAssignment = new KAMAssignmentModel();

            KAMAssignment.UserName = r["UserName"].ToString();

            if (r.Table.Columns.Contains("FullName"))
            {
                KAMAssignment.FullName = r["FullName"].ToString();
            }
            if (r.Table.Columns.Contains("Email"))
            {
                KAMAssignment.Email = r["Email"].ToString();
            }
            if (r.Table.Columns.Contains("ContactNo"))
            {
                KAMAssignment.ContactNo = r["ContactNo"].ToString();
            }

            return KAMAssignment;
        }
        
        private KAMAssignmentModel RowOfKAMAssignmentByTSPMaster(DataRow r)
        {
            KAMAssignmentModel KAMAssignment = new KAMAssignmentModel();
            //KAMAssignment.KamID = Convert.ToInt32(r["KamID"]);
            KAMAssignment.UserID = Convert.ToInt32(r["UserID"]);
            KAMAssignment.UserName = r["UserName"].ToString();
            KAMAssignment.TspID = Convert.ToInt32(r["TspID"]);
            KAMAssignment.TSPName = r["TSPName"].ToString();
            KAMAssignment.DistrictID = Convert.ToInt32(r["DistrictID"]);
            KAMAssignment.DistrictName = r["DistrictName"].ToString();

            KAMAssignment.RegionID = Convert.ToInt32(r["RegionID"]);
            KAMAssignment.RegionName = r["RegionName"].ToString();

            return KAMAssignment;
        }
        private KAMAssignmentModel RowOfUnAssignedKamTSPMaster(DataRow r)
        {
            KAMAssignmentModel KAMAssignment = new KAMAssignmentModel();
            //KAMAssignment.KamID = Convert.ToInt32(r["KamID"]);
            KAMAssignment.AssignedUser = Convert.ToInt32(r["AssignedUser"]);
            //KAMAssignment.UserID = Convert.ToInt32(r["UserID"]);
            //KAMAssignment.UserName = r["UserName"].ToString();
            KAMAssignment.TspID = Convert.ToInt32(r["TspID"]);
            KAMAssignment.TSPName = r["TSPName"].ToString();
            KAMAssignment.DistrictID = Convert.ToInt32(r["DistrictID"]);
            KAMAssignment.DistrictName = r["DistrictName"].ToString();

            KAMAssignment.RegionID = Convert.ToInt32(r["RegionID"]);
            KAMAssignment.RegionName = r["RegionName"].ToString();

            return KAMAssignment;
        }
        private KAMAssignmentModel RowOfTSPKAMHistory(DataRow r)
        {
            KAMAssignmentModel KAMHistory = new KAMAssignmentModel();
            KAMHistory.KamID = Convert.ToInt32(r["KamID"]);
            KAMHistory.UserID = Convert.ToInt32(r["UserID"]);
            KAMHistory.UserName = r["UserName"].ToString();
            KAMHistory.TspID = Convert.ToInt32(r["TspID"]);
            KAMHistory.TSPName = r["TSPName"].ToString();
            KAMHistory.CreatedUserID = Convert.ToInt32(r["CreatedUserID"]);
            KAMHistory.CreatedDate = r["CreatedDate"].ToString().GetDate();

            return KAMHistory;
        }
    }
}
