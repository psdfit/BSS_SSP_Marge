using System;
using System.Collections.Generic;
using System.Data;
using Microsoft.Data.SqlClient;
using DataLayer.Classes;
using DataLayer.Models;
using Newtonsoft.Json;
using DataLayer.Interfaces;
using System.Linq;

namespace DataLayer.Services
{
    public class SRVTSPMaster : SRVBase, ISRVTSPMaster
    {
        public SRVTSPMaster()
        { }

        public TSPMasterModel GetByTSPMasterID(int TSPMasterID, SqlTransaction transaction = null)
        {
            try
            {
                SqlParameter param = new SqlParameter("@TSPMasterID", TSPMasterID);
                DataTable dt = new DataTable();
                if (transaction != null)
                {
                    SqlDataReader sReader = SqlHelper.ExecuteReader(transaction, CommandType.StoredProcedure, "RD_TSPMaster", param);
                    //Create a new DataTable.
                    dt.Load(sReader);
                }
                else
                {
                    SqlDataReader sReader = SqlHelper.ExecuteReader(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_TSPMaster", param);
                    //Create a new DataTable.
                    dt.Load(sReader);
                }
                //DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_TSPMaster", param).Tables[0];
                if (dt.Rows.Count > 0)
                {
                    return RowOfTSPMaster(dt.Rows[0]);
                }
                else
                    return null;
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }

        public List<TSPMasterModel> SaveTSPMaster(TSPMasterModel TSPMaster, SqlTransaction transaction = null)
        {
            try
            {
                SqlParameter[] param = new SqlParameter[9];
                param[0] = new SqlParameter("@TSPMasterID", TSPMaster.TSPMasterID);
                param[1] = new SqlParameter("@TSPName", TSPMaster.TSPName);
                param[2] = new SqlParameter("@Address", TSPMaster.Address);
                param[3] = new SqlParameter("@NTN", TSPMaster.NTN);
                param[4] = new SqlParameter("@PNTN", TSPMaster.PNTN);
                param[5] = new SqlParameter("@GST", TSPMaster.GST);
                param[6] = new SqlParameter("@FTN", TSPMaster.FTN);
                param[7] = new SqlParameter("@UserID", TSPMaster.UserID);

                param[8] = new SqlParameter("@CurUserID", TSPMaster.CurUserID);
                if (transaction != null)
                {
                    SqlHelper.ExecuteNonQuery(transaction, CommandType.StoredProcedure, "[AU_TSPMaster]", param);
                }
                else
                {
                    SqlHelper.ExecuteNonQuery(SqlHelper.GetCon(), CommandType.StoredProcedure, "[AU_TSPMaster]", param);
                }
                return FetchTSPMaster(transaction);
            }
            catch (Exception ex)
            { throw new Exception(ex.Message); }
        }

        private List<TSPMasterModel> LoopinData(DataTable dt)
        {
            List<TSPMasterModel> TSPMasterL = new List<TSPMasterModel>();

            foreach (DataRow r in dt.Rows)
            {
                TSPMasterL.Add(RowOfTSPMaster(r));
            }
            return TSPMasterL;
        }

        public List<TSPMasterModel> FetchTSPMaster(TSPMasterModel mod)
        {
            try
            {
                DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_TSPMaster", Common.GetParams(mod)).Tables[0];
                return LoopinData(dt);
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }

        public TSPMasterModel CheckDupplicateTspByNTN(string ntn, string TSPName)
        {
            try
            {
                SqlParameter[] PLead = new SqlParameter[3];
                PLead[0] = new SqlParameter("@NTN", ntn);
                PLead[1] = new SqlParameter("@TSPName", TSPName);
                DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_TSPMaster", PLead).Tables[0];
                if (dt.Rows.Count > 0)
                {
                    return RowOfTSPMaster(dt.Rows[0]);
                }
                else
                    return null;
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }

        public long CheckDupplicateTspByNTNAlert(string ntn, string TSPName)
        {
            try
            {
                SqlParameter[] PLead = new SqlParameter[4];
                PLead[0] = new SqlParameter("@NTN", ntn);
                PLead[1] = new SqlParameter("@TSPName", TSPName);
                PLead[2] = new SqlParameter("@ReturnValueForDuplication", SqlDbType.Int);
                PLead[2].Direction = ParameterDirection.Output;
                SqlHelper.ExecuteNonQuery(SqlHelper.GetCon(), CommandType.StoredProcedure, "[RD_CheckDuplicateTSPAndNTN]", PLead);

                long Value = Convert.ToInt32(PLead[2].Value);
                return Value;//RowOfTSPMaster(dt.Rows[0]);
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }

        public void SetTSPPermissions(int UserID, SqlTransaction transaction = null)
        {
            try
            {
                SqlHelper.ExecuteNonQuery(transaction, CommandType.StoredProcedure, "SetTSPPermissions", new SqlParameter("@UserID", UserID));
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }

        public List<TSPMasterModel> FetchTSPMaster(SqlTransaction transaction = null)
        {
            try
            {
                DataTable dt = new DataTable();

                if (transaction != null)
                {
                    SqlDataReader sReader = SqlHelper.ExecuteReader(transaction, CommandType.StoredProcedure, "RD_TSPMaster");
                    //Create a new DataTable.
                    dt.Load(sReader);
                    //SqlHelper.ExecuteNonQuery(transaction, CommandType.StoredProcedure, "POLinesAndHeader", param.ToArray());
                }
                else
                {
                    dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_TSPMaster").Tables[0];
                }
                return LoopinData(dt);
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }

        public List<TSPMasterModel> FetchTSPMaster(bool InActive)
        {
            try
            {
                DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_TSPMaster", new SqlParameter("@InActive", InActive)).Tables[0];
                return LoopinData(dt);
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }

        public void ActiveInActive(int TSPMasterID, bool? InActive, int CurUserID)
        {
            SqlParameter[] PLead = new SqlParameter[3];
            PLead[0] = new SqlParameter("@TSPMasterID", TSPMasterID);
            PLead[1] = new SqlParameter("@InActive", InActive);
            PLead[2] = new SqlParameter("@CurUserID", CurUserID);
            SqlHelper.ExecuteNonQuery(SqlHelper.GetCon(), CommandType.StoredProcedure, "[AI_TSPMaster]", PLead);
        }

        public bool CheckIfExists(int TSPMasterID)
        {
            try
            {
                int UserID = Convert.ToInt32(SqlHelper.ExecuteScalar(SqlHelper.GetCon(), CommandType.StoredProcedure, "CheckTSPExists", new SqlParameter("@TSPMasterID", TSPMasterID)));
                //DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "CheckTSPExists", new SqlParameter("@TSPMasterID", TSPMasterID)).Tables[0];

                if (UserID == 0)
                    return false;
                else
                    return true;
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }

        private TSPMasterModel RowOfTSPMaster(DataRow r)
        {
            TSPMasterModel TSPMaster = new TSPMasterModel();
            TSPMaster.TSPMasterID = Convert.ToInt32(r["TSPMasterID"]);
            TSPMaster.TSPName = r["TSPName"].ToString();
            TSPMaster.Address = r["Address"].ToString();
            TSPMaster.NTN = r["NTN"].ToString();
            TSPMaster.PNTN = r["PNTN"].ToString();
            TSPMaster.GST = r["GST"].ToString();
            TSPMaster.FTN = r["FTN"].ToString();
            TSPMaster.UID = r["UID"].ToString();
            TSPMaster.KAMID = r.Field<int?>("KAMID");
            TSPMaster.UserID = r.Field<int?>("UserID"); //String.IsNullOrEmpty(r["UserID"].ToString())?0: Convert.ToInt32(r["UserID"]);
            TSPMaster.InActive = Convert.ToBoolean(r["InActive"]);
            TSPMaster.CreatedUserID = Convert.ToInt32(r["CreatedUserID"]);
            TSPMaster.ModifiedUserID = Convert.ToInt32(r["ModifiedUserID"]);
            TSPMaster.CreatedDate = r["CreatedDate"].ToString().GetDate();
            TSPMaster.ModifiedDate = r["ModifiedDate"].ToString().GetDate();
            TSPMaster.SAPID = r["SAPID"].ToString();

            return TSPMaster;
        }

        public void AddUpdateTSPMasterFromDetail(TSPDetailModel tspDetailModel)
        {
            SqlParameter[] param = new SqlParameter[8];
            param[0] = new SqlParameter("@TSPMasterID", tspDetailModel.TSPMasterID);
            param[1] = new SqlParameter("@TSPName", tspDetailModel.TSPName);
            param[2] = new SqlParameter("@Address", tspDetailModel.Address);
            param[3] = new SqlParameter("@NTN", tspDetailModel.NTN);
            param[4] = new SqlParameter("@PNTN", tspDetailModel.PNTN);
            param[5] = new SqlParameter("@GST", tspDetailModel.GST);
            param[6] = new SqlParameter("@FTN", tspDetailModel.FTN);
            param[7] = new SqlParameter("@CurUserID", tspDetailModel.CurUserID);
            SqlHelper.ExecuteNonQuery(SqlHelper.GetCon(), CommandType.StoredProcedure, "[AU_TSPMaster]", param);
            //return FetchTSPMaster();
        }

        public bool UpdateTSPSAPId(int tspMasterId, string sapObjId, SqlTransaction transaction = null)
        {
            try
            {
                if (!string.IsNullOrEmpty(sapObjId))
                {
                    SqlParameter[] param = new SqlParameter[2];
                    param[0] = new SqlParameter("@TSPMasterID", tspMasterId);
                    param[1] = new SqlParameter("@SAPID", sapObjId);
                    if (transaction != null)
                    {
                        SqlHelper.ExecuteNonQuery(transaction, CommandType.StoredProcedure, "[Update_TSPMasterSAPID]", param);
                    }
                    else
                    {
                        SqlHelper.ExecuteNonQuery(SqlHelper.GetCon(), CommandType.StoredProcedure, "[Update_TSPMasterSAPID]", param);
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                return false;
                //throw new Exception(ex.Message);
            }
        }

        public TSPMasterModel GetTSPUserByClassID(int ClassID)
        {
            List<TSPMasterModel> model = new List<TSPMasterModel>();
            SqlParameter[] obj = new SqlParameter[3];
            obj[0] = new SqlParameter("@ClassID", ClassID);
            DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "[RD_GetTSPUserByClassID]", obj).Tables[0];
            model = Helper.ConvertDataTableToModel<TSPMasterModel>(dt);
            return model[0];
        }

        public TSPMasterModel GetKAMUserByClassID(int ClassID)
        {
            List<TSPMasterModel> Model = new List<TSPMasterModel>();
            SqlParameter[] param = new SqlParameter[3];
            param[0] = new SqlParameter("@ClassID", ClassID);
            DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_GetKAMUserByClassID", param).Tables[0];
            Model = Helper.ConvertDataTableToModel<TSPMasterModel>(dt);
            return Model[0];
        }

        public UsersModel KAMUserByTSPUserID(int UserID)
        {
            DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "Get_KAMUserByTSPUserID", new SqlParameter("@UserID", UserID)).Tables[0];
            List<UsersModel> KAMUser = Helper.ConvertDataTableToModel<UsersModel>(dt);
            return KAMUser[0];
        }

        public string GET_KAMAndTspUserBySRNIDs_Notification(string SRNIDs, SqlTransaction transaction = null)
        {
            try
            {
                DataTable dt = new DataTable();
                if (transaction != null)
                {
                    dt = SqlHelper.ExecuteDataset(transaction, CommandType.StoredProcedure, "RD_TSPAndKAMUSerbySRNID_Notification", new SqlParameter("@SRNID", SRNIDs)).Tables[0];
                }
                else
                {
                    dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_TSPAndKAMUSerbySRNID_Notification", new SqlParameter("@SRNID", SRNIDs)).Tables[0];
                }

                List<TSPMasterModel> KAMTSPUser = Helper.ConvertDataTableToModel<TSPMasterModel>(dt);
                string TSPIds = (string.Join(",", KAMTSPUser.Select(x => x.UserID.ToString())));
                List<string> Distinct_uniqueValues = TSPIds.ToLower().Split(',').Distinct().ToList();
                string UniqueString = string.Join(",", Distinct_uniqueValues);

                return UniqueString;
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }

        public ApprovalHistoryModel GET_ConcateClassescodebySRNID_Notification(string SRNIDs, SqlTransaction transaction = null)
        {
            try
            {
                DataTable dt = new DataTable();
                if (transaction != null)
                {
                    dt = SqlHelper.ExecuteDataset(transaction, CommandType.StoredProcedure, "RD_ConcateClasscodebySRNID_Notification", new SqlParameter("@SRNID", SRNIDs)).Tables[0];
                }
                else
                {
                    dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_ConcateClasscodebySRNID_Notification", new SqlParameter("@SRNID", SRNIDs)).Tables[0];
                }

                List<ApprovalHistoryModel> KAMTSPUser = Helper.ConvertDataTableToModel<ApprovalHistoryModel>(dt);
                KAMTSPUser[0].ForMonth = String.Format("{0:y}", KAMTSPUser[0].Month);  // "March, 2008"
                return KAMTSPUser[0];
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }

        public ApprovalHistoryModel GET_ConcateClassescodebyTPRNID_Notification(string TPRNIDs, SqlTransaction transaction = null)
        {
            try
            {
                DataTable dt = new DataTable();
                if (transaction != null)
                {
                    dt = SqlHelper.ExecuteDataset(transaction, CommandType.StoredProcedure, "RD_ConcateClasscodebyTPRNID_Notification", new SqlParameter("@TPRNID", TPRNIDs)).Tables[0];
                }
                else
                {
                    dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_ConcateClasscodebyTPRNID_Notification", new SqlParameter("@TPRNID", TPRNIDs)).Tables[0];
                }

                List<ApprovalHistoryModel> KAMTSPUser = Helper.ConvertDataTableToModel<ApprovalHistoryModel>(dt);
                KAMTSPUser[0].ForMonth = String.Format("{0:y}", KAMTSPUser[0].Month);  // "March, 2008"
                return KAMTSPUser[0];
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }
        public string GET_KAMAndTspUserByTPRNIDs_Notification(string TPRNIDs, SqlTransaction transaction = null)
        {
            try
            {
                DataTable dt = new DataTable();
                if (transaction != null)
                {
                    dt = SqlHelper.ExecuteDataset(transaction, CommandType.StoredProcedure, "RD_TSPAndKAMUSerbyTPRNID_Notification", new SqlParameter("@TPRNID", TPRNIDs)).Tables[0];
                }
                else
                {
                    dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_TSPAndKAMUSerbyTPRNID_Notification", new SqlParameter("@TPRNID", TPRNIDs)).Tables[0];
                }

                List<TSPMasterModel> KAMTSPUser = Helper.ConvertDataTableToModel<TSPMasterModel>(dt);
                string TSPIds = (string.Join(",", KAMTSPUser.Select(x => x.UserID.ToString())));
                List<string> Distinct_uniqueValues = TSPIds.ToLower().Split(',').Distinct().ToList();
                string UniqueString = string.Join(",", Distinct_uniqueValues);

                return UniqueString;
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }
    }
}