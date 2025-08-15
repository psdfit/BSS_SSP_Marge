/* **** Aamer Rehman Malik *****/

using DataLayer.Classes;
using DataLayer.Interfaces;
using DataLayer.Models;
using Microsoft.Data.SqlClient;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;

namespace DataLayer.Services
{
    public class SRVTSPDetail : SRVBase, DataLayer.Interfaces.ISRVTSPDetail
    {
        public SRVTSPDetail()
        {
        }

        public TSPDetailModel GetByTSPID(int TSPID)
        {
            try
            {
                SqlParameter param = new SqlParameter("@TSPID", TSPID);
                DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_TSPDetail", param).Tables[0];
                if (dt.Rows.Count > 0)
                {
                    return RowOfTSPDetail(dt.Rows[0]);
                }
                else
                    return null;
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }

        public List<TSPDetailModel> GetByScheme(int SchemeID)
        {
            try
            {
                SqlParameter param = new SqlParameter("@SchemeID", SchemeID);
                DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "[RD_TSPDetail]", param).Tables[0];
                if (dt.Rows.Count > 0)
                {
                    return LoopinData(dt);
                }
                else
                    return null;
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }

        public List<KAMAssignedTSPsModel> FetchTSPListByKamUser(int UserID)
        {
            try
            {
                SqlParameter param = new SqlParameter("@UserID", UserID);
                DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "[RD_TSPDetailByKAMUser]", param).Tables[0];
                if (dt.Rows.Count > 0)
                {
                    return LoopinDataKAMAssignments(dt);
                }
                else
                    return null;
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }

        private List<KAMAssignedTSPsModel> LoopinDataKAMAssignments(DataTable dt)
        {
            List<KAMAssignedTSPsModel> TSPsAssigned = new List<KAMAssignedTSPsModel>();

            foreach (DataRow r in dt.Rows)
            {
                TSPsAssigned.Add(RowOfTSPsAssignedToKAM(r));
            }
            return TSPsAssigned;
        }

        private KAMAssignedTSPsModel RowOfTSPsAssignedToKAM(DataRow r)
        {
            KAMAssignedTSPsModel TSPsAssigned = new KAMAssignedTSPsModel();
            TSPsAssigned.TSPID = Convert.ToInt32(r["TSPID"]);
            TSPsAssigned.IsSelected = Convert.ToBoolean(r["IsSelected"]);
            TSPsAssigned.TSPName = r["TSPName"].ToString();
            TSPsAssigned.SchemeID = Convert.ToInt32(r["SchemeID"]);
            TSPsAssigned.SchemeName = r["SchemeName"].ToString();

            return TSPsAssigned;
        }

        public TSPDetailModel SaveTSPDetail(TSPDetailModel TSPDetail)
        {
            try
            {
                int i = 0;
                SqlParameter[] param = new SqlParameter[41];
                param[i++] = new SqlParameter("@TSPID", TSPDetail.TSPID);
                param[i++] = new SqlParameter("@TSPMasterID", TSPDetail.TSPMasterID);
                param[i++] = new SqlParameter("@TSPName", TSPDetail.TSPName);
                param[i++] = new SqlParameter("@Address", TSPDetail.Address);
                param[i++] = new SqlParameter("@TSPCode", TSPDetail.TSPCode);
                param[i++] = new SqlParameter("@OrganizationID", TSPDetail.OrganizationID);
                param[i++] = new SqlParameter("@TSPColor", TSPDetail.TSPColor);
                param[i++] = new SqlParameter("@TierID", TSPDetail.TierID);

                param[i++] = new SqlParameter("@NTN", TSPDetail.NTN);
                param[i++] = new SqlParameter("@PNTN", TSPDetail.PNTN);
                param[i++] = new SqlParameter("@GST", TSPDetail.GST);
                param[i++] = new SqlParameter("@FTN", TSPDetail.FTN);
                //param[i++] = new SqlParameter("@TspStatusID_OLD", TSPDetail.TspStatusID_OLD);
                param[i++] = new SqlParameter("@DistrictID", TSPDetail.DistrictID);
                //param[i++] = new SqlParameter("@TehsilID", TSPDetail.TehsilID);
                param[i++] = new SqlParameter("@HeadName", TSPDetail.HeadName);
                param[i++] = new SqlParameter("@HeadDesignation", TSPDetail.HeadDesignation);
                param[i++] = new SqlParameter("@HeadEmail", TSPDetail.HeadEmail);
                param[i++] = new SqlParameter("@OrgLandline", TSPDetail.OrgLandline);
                param[i++] = new SqlParameter("@HeadLandline", TSPDetail.HeadLandline);
                param[i++] = new SqlParameter("@CPName", TSPDetail.CPName);
                param[i++] = new SqlParameter("@CPDesignation", TSPDetail.CPDesignation);
                //param[i++] = new SqlParameter("@CPMobile", TSPDetail.CPMobile);
                param[i++] = new SqlParameter("@CPLandline", TSPDetail.CPLandline);
                param[i++] = new SqlParameter("@CPEmail", TSPDetail.CPEmail);
                param[i++] = new SqlParameter("@Website", TSPDetail.Website);
                param[i++] = new SqlParameter("@CPAdmissionsName", TSPDetail.CPAdmissionsName);
                param[i++] = new SqlParameter("@CPAdmissionsDesignation", TSPDetail.CPAdmissionsDesignation);
                //parai++27] = new SqlParameter("@CPAdmissionsMobile", TSPDetail.CPAdmissionsMobile);
                param[i++] = new SqlParameter("@CPAdmissionsLandline", TSPDetail.CPAdmissionsLandline);
                param[i++] = new SqlParameter("@CPAdmissionsEmail", TSPDetail.CPAdmissionsEmail);
                param[i++] = new SqlParameter("@CPAccountsName", TSPDetail.CPAccountsName);
                param[i++] = new SqlParameter("@CPAccountsDesignation", TSPDetail.CPAccountsDesignation);
                //parai++32] = new SqlParameter("@CPAccountsMobile", TSPDetail.CPAccountsMobile);
                param[i++] = new SqlParameter("@CPAccountsLandline", TSPDetail.CPAccountsLandline);
                param[i++] = new SqlParameter("@CPAccountsEmail", TSPDetail.CPAccountsEmail);
                param[i++] = new SqlParameter("@BankName", TSPDetail.BankName);
                param[i++] = new SqlParameter("@BankAccountNumber", TSPDetail.BankAccountNumber);
                param[i++] = new SqlParameter("@AccountTitle", TSPDetail.AccountTitle);
                param[i++] = new SqlParameter("@BankBranch", TSPDetail.BankBranch);
                param[i++] = new SqlParameter("@SchemeID", Convert.ToInt32(TSPDetail.SchemeID));
                param[i++] = new SqlParameter("@InActive", Convert.ToBoolean(TSPDetail.InActive));

                param[i++] = new SqlParameter("@CurUserID", TSPDetail.CurUserID);

                TSPDetail.UID = Guid.NewGuid().ToString();
                TSPDetail.IsMigrated = false;

                param[i++] = new SqlParameter("@UID", TSPDetail.UID);
                param[i++] = new SqlParameter("@IsMigrated", TSPDetail.IsMigrated);

                param[i] = new SqlParameter("@Ident", SqlDbType.Int);
                param[i].Direction = ParameterDirection.Output;

                SqlHelper.ExecuteNonQuery(SqlHelper.GetCon(), CommandType.StoredProcedure, "[AU_TSPDetail]", param);
                return GetByTSPID(Convert.ToInt32(param[i].Value));
            }
            catch (Exception ex)
            { throw new Exception(ex.Message); }
        }

        public TSPDetailModel SaveTSPMaster(TSPDetailModel TSPMaster)
        {
            try
            {
                int i = 0;
                SqlParameter[] param = new SqlParameter[15];
                param[i++] = new SqlParameter("@TSPID", TSPMaster.TSPID);
                param[i++] = new SqlParameter("@TSPName", TSPMaster.TSPName);
                param[i++] = new SqlParameter("@Address", TSPMaster.Address);

                param[i++] = new SqlParameter("@NTN", Convert.ToInt32(TSPMaster.NTN));
                param[i++] = new SqlParameter("@PNTN", Convert.ToInt32(TSPMaster.PNTN));
                param[i++] = new SqlParameter("@GST", TSPMaster.GST);
                param[i++] = new SqlParameter("@FTN", Convert.ToInt32(TSPMaster.FTN));

                param[i++] = new SqlParameter("@CurUserID", TSPMaster.CurUserID);

                param[i] = new SqlParameter("@Ident", SqlDbType.Int);
                param[i].Direction = ParameterDirection.Output;

                SqlHelper.ExecuteNonQuery(SqlHelper.GetCon(), CommandType.StoredProcedure, "[AU_TSPMaster]", param);
                return GetByTSPID(Convert.ToInt32(param[i].Value));
            }
            catch (Exception ex)
            { throw new Exception(ex.Message); }
        }

        private List<TSPDetailModel> LoopinData(DataTable dt)
        {
            List<TSPDetailModel> TSPDetailL = new List<TSPDetailModel>();

            foreach (DataRow r in dt.Rows)
            {
                TSPDetailL.Add(RowOfTSPDetail(r));
            }
            return TSPDetailL;
        }

        private List<TSPDetailModel> LoopinTSPMasterFilterData(DataTable dt)
        {
            List<TSPDetailModel> TSPDetailL = new List<TSPDetailModel>();

            foreach (DataRow r in dt.Rows)
            {
                TSPDetailL.Add(RowOfTSPMasterFilter(r));
            }
            return TSPDetailL;
        }

        public List<TSPDetailModel> FetchTSPDetail(TSPDetailModel mod)
        {
            try
            {
                DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_TSPDetail", Common.GetParams(mod)).Tables[0];
                return LoopinData(dt);
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }

        public List<TSPDetailModel> FetchTSPDetailByScheme(int schemeId, SqlTransaction transaction = null)
        {
            try
            {
                DataTable dt = new DataTable();

                if (transaction != null)
                {
                    //SqlDataReader sReader = SqlHelper.ExecuteReader(transaction, CommandType.StoredProcedure, "RD_TSPDetailByScheme", new SqlParameter("@SchemeID", schemeId));
                    //Create a new DataTable.
                    //dt.Load(sReader);
                    dt = SqlHelper.ExecuteDataset(transaction, CommandType.StoredProcedure, "RD_TSPDetailByScheme", new SqlParameter("@SchemeID", schemeId)).Tables[0];
                }
                else
                {
                    dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_TSPDetailByScheme", new SqlParameter("@SchemeID", schemeId)).Tables[0];
                }

                return LoopinData(dt);
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }

        public List<TSPDetailModel> FetchTSPDetailBySchemes(List<int> schemeIds, SqlTransaction transaction = null)
        {
            try
            {
                DataTable dt = new DataTable();

                // Join IDs into comma-separated string for SQL parameter
                string idsCsv = string.Join(",", schemeIds);

                SqlParameter param = new SqlParameter("@SchemeIDs", idsCsv);

                if (transaction != null)
                {
                    dt = SqlHelper.ExecuteDataset(transaction, CommandType.StoredProcedure, "RD_TSPDetailByScheme", param).Tables[0];
                }
                else
                {
                    dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_TSPDetailByScheme", param).Tables[0];
                }

                return LoopinData(dt);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }


        public int GetTSPSequence()
        {
            try
            {
                return Convert.ToInt32(SqlHelper.ExecuteScalar(SqlHelper.GetCon(), CommandType.StoredProcedure, "GetTSPSequence"));
            }
            catch (Exception e)
            { throw new Exception(e.Message); }
        }

        public List<TSPDetailModel> FetchTSPDetail()
        {
            try
            {
                DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_TSPDetail").Tables[0];
                return LoopinData(dt);
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }

        public List<TSPDetailModel> FetchTSPs()
        {
            try
            {
                DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_TSPDetailFOrKam").Tables[0];
                return LoopinData(dt);
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }

        public List<TSPDetailModel> FetchApprovedTSPs()
        {
            try
            {
                DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_Approved_TSPDetail").Tables[0];
                return LoopinData(dt);
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }

        public List<TSPDetailModel> FetchTSPDetail(bool InActive)
        {
            try
            {
                DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_TSPDetail", new SqlParameter("@InActive", InActive)).Tables[0];
                return LoopinData(dt);
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }

        public List<TSPDetailModel> FetchTSPMasterForFilters(bool InActive)
        {
            try
            {
                DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_TSPMasterForFilter").Tables[0];
                return LoopinTSPMasterFilterData(dt);
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }

        public List<TSPDetailModel> FetchTSPDetailForROSIFilter(ROSIFiltersModel rosiFilters)
        {
            try
            {
                SqlParameter[] param = new SqlParameter[10];
                param[0] = new SqlParameter("@SchemeIDs", rosiFilters.SchemeIDs);
                param[1] = new SqlParameter("@PTypeIDs", rosiFilters.PTypeIDs);
                param[2] = new SqlParameter("@SectorIDs", rosiFilters.SectorIDs);
                param[3] = new SqlParameter("@ClusterIDs", rosiFilters.ClusterIDs);
                param[4] = new SqlParameter("@DistrictIDs", rosiFilters.DistrictIDs);
                param[5] = new SqlParameter("@OIDs", rosiFilters.OrganizationIDs);
                param[6] = new SqlParameter("@TradeIDs", rosiFilters.TradeIDs);
                param[7] = new SqlParameter("@FundingSourceIDs", rosiFilters.FundingSourceIDs);
                param[8] = new SqlParameter("@GenderIDs", rosiFilters.GenderIDs);
                param[9] = new SqlParameter("@DurationIDs", rosiFilters.DurationIDs);

                DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_TSPDetailForROSIFilter", param).Tables[0];
                return LoopinData(dt);
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }

        public List<TSPDetailModel> FetchTSPDetailForKam()
        {
            try
            {
                DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_TSPDetailFOrKam").Tables[0];
                return LoopinData(dt);
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }

        public int BatchInsert(List<TSPDetailModel> ls, int @BatchFkey, int CurUserID)
        {
            SqlParameter[] param = new SqlParameter[3];

            param[0] = new SqlParameter("@Json", JsonConvert.SerializeObject(ls));
            param[1] = new SqlParameter("@", @BatchFkey);
            param[2] = new SqlParameter("@CurUserID", CurUserID);
            return SqlHelper.ExecuteNonQuery(SqlHelper.GetCon(), CommandType.StoredProcedure, "[BAU_TSPDetail]", param);
        }

        public List<TSPDetailModel> GetByDistrictID(int DistrictID)
        {
            try
            {
                DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_TSPDetail", new SqlParameter("@DistrictID", DistrictID)).Tables[0];
                return LoopinData(dt);
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }

        public List<TSPDetailModel> GetByTehsilID(int TehsilID)
        {
            try
            {
                DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_TSPDetail", new SqlParameter("@TehsilID", TehsilID)).Tables[0];
                return LoopinData(dt);
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }

        public List<TSPDetailModel> FetchTSPsByFilters(int[] filters)
        {
            List<TSPDetailModel> list = new List<TSPDetailModel>();
            if (filters.Length > 0)
            {
                int schemeId = filters[0];
                int tspId = filters[1];
                int classId = filters[2];
                int traineeId = filters[3];
                try
                {
                    SqlParameter[] param = new SqlParameter[10];
                    param[0] = new SqlParameter("@SchemeID", schemeId);
                    param[1] = new SqlParameter("@TSPID", tspId);
                    param[2] = new SqlParameter("@ClassID", classId);
                    param[3] = new SqlParameter("@TraineeID", traineeId);
                    DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_FilteredTSPs", param).Tables[0];
                    list = LoopinData(dt);
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            return list;
        }

        public void ActiveInActive(int TSPID, bool? InActive, int CurUserID)
        {
            SqlParameter[] PLead = new SqlParameter[3];
            PLead[0] = new SqlParameter("@TSPID", TSPID);
            PLead[1] = new SqlParameter("@InActive", InActive);
            PLead[2] = new SqlParameter("@CurUserID", CurUserID);
            SqlHelper.ExecuteNonQuery(SqlHelper.GetCon(), CommandType.StoredProcedure, "[AI_TSPDetail]", PLead);
        }

        public void UpdateTSPDetail(TSPDetailModel TSPDetail)
        {
            SqlParameter[] param = new SqlParameter[6];
            param[0] = new SqlParameter("@TSPID", TSPDetail.TSPID);
            param[1] = new SqlParameter("@TSPName", TSPDetail.TSPName);
            param[2] = new SqlParameter("@HeadName", TSPDetail.HeadName);
            param[3] = new SqlParameter("@CPName", TSPDetail.CPName);
            param[4] = new SqlParameter("@Address", TSPDetail.Address);

            SqlHelper.ExecuteNonQuery(SqlHelper.GetCon(), CommandType.StoredProcedure, "[Update_TSPDetail]", param);
        }

        private TSPDetailModel RowOfTSPDetail(DataRow r)
        {
            TSPDetailModel TSPDetail = new TSPDetailModel();
            TSPDetail.TSPID = Convert.ToInt32(r["TSPID"]);
            TSPDetail.TSPMasterID = Convert.ToInt32(r["TSPMasterID"]);
            TSPDetail.TSPName = r["TSPName"].ToString();
            TSPDetail.Address = r["Address"].ToString();
            TSPDetail.TSPCode = r["TSPCode"].ToString();
            TSPDetail.OrganizationID = Convert.ToInt32(r["OrganizationID"]);
            TSPDetail.TSPColor = r["TSPColor"].ToString();
            TSPDetail.TierID = Convert.ToInt32(r["TierID"]);
            TSPDetail.NTN = r["NTN"].ToString();
            TSPDetail.PNTN = r["PNTN"].ToString();
            TSPDetail.GST = r["GST"].ToString();
            TSPDetail.FTN = r["FTN"].ToString();
            //TSPDetail.TspStatusID_OLD = Convert.ToInt32(r["TspStatusID_OLD"]);
            TSPDetail.DistrictID = Convert.ToInt32(r["DistrictID"]);
            TSPDetail.DistrictName = r["DistrictName"].ToString();
            //TSPDetail.TehsilID = Convert.ToInt32(r["TehsilID"]);
            //TSPDetail.TehsilName = r["TehsilName"].ToString();
            TSPDetail.HeadName = r["HeadName"].ToString();
            TSPDetail.HeadDesignation = r["HeadDesignation"].ToString();
            TSPDetail.HeadEmail = r["HeadEmail"].ToString();
            TSPDetail.OrgLandline = r["OrgLandline"].ToString();
            TSPDetail.HeadLandline = r["HeadLandline"].ToString();
            TSPDetail.CPName = r["CPName"].ToString();
            TSPDetail.CPDesignation = r["CPDesignation"].ToString();
            //TSPDetail.CPMobile = Convert.ToInt32(r["CPMobile"]);
            TSPDetail.CPLandline = r["CPLandline"].ToString();
            TSPDetail.CPEmail = r["CPEmail"].ToString();
            TSPDetail.Website = r["Website"].ToString();
            if (r.Table.Columns.Contains("AssignedUser"))
                TSPDetail.AssignedUser = Convert.ToInt32(r["AssignedUser"]);
            if (r.Table.Columns.Contains("RegionName"))
            {
                TSPDetail.RegionID = Convert.ToInt32(r["RegionID"]);
                TSPDetail.RegionName = r["RegionName"].ToString();
            }

            TSPDetail.CPAdmissionsName = r["CPAdmissionsName"].ToString();
            TSPDetail.CPAdmissionsDesignation = r["CPAdmissionsDesignation"].ToString();
            //TSPDetail.CPAdmissionsMobile = Convert.ToInt32(r["CPAdmissionsMobile"]);
            TSPDetail.CPAdmissionsLandline = r["CPAdmissionsLandline"].ToString();
            TSPDetail.CPAdmissionsEmail = r["CPAdmissionsEmail"].ToString();
            TSPDetail.CPAccountsName = r["CPAccountsName"].ToString();
            TSPDetail.CPAccountsDesignation = r["CPAccountsDesignation"].ToString();
            //TSPDetail.CPAccountsMobile = Convert.ToInt32(r["CPAccountsMobile"]);
            TSPDetail.CPAccountsLandline = r["CPAccountsLandline"].ToString();
            TSPDetail.CPAccountsEmail = r["CPAccountsEmail"].ToString();
            TSPDetail.BankName = r["BankName"].ToString();
            TSPDetail.BankAccountNumber = r["BankAccountNumber"].ToString();
            TSPDetail.AccountTitle = r["AccountTitle"].ToString();
            TSPDetail.BankBranch = r["BankBranch"].ToString();
            TSPDetail.SchemeID = Convert.ToInt32(r["SchemeID"]);
            TSPDetail.SchemeName = r["SchemeName"].ToString();
            TSPDetail.InActive = Convert.ToBoolean(r["InActive"]);
            TSPDetail.UID = r["UID"].ToString();
            TSPDetail.IsMigrated = Convert.ToBoolean(r["IsMigrated"]);
            TSPDetail.CreatedUserID = Convert.ToInt32(r["CreatedUserID"]);
            TSPDetail.ModifiedUserID = Convert.ToInt32(r["ModifiedUserID"]);
            TSPDetail.CreatedDate = r["CreatedDate"].ToString().GetDate();
            TSPDetail.OrganizationName = r["OrganizationName"].ToString();

            if (string.IsNullOrEmpty(r["ModifiedDate"].ToString()))
            {
                TSPDetail.ModifiedDate = null;
            }
            else
            {
                TSPDetail.ModifiedDate = r["ModifiedDate"].ToString().GetDate();
            }

            return TSPDetail;
        }

        private TSPDetailModel RowOfTSPMasterFilter(DataRow r)
        {
            TSPDetailModel TSPDetail = new TSPDetailModel();
            TSPDetail.TSPID = Convert.ToInt32(r["TSPID"]);
            TSPDetail.TSPMasterID = Convert.ToInt32(r["TSPMasterID"]);
            TSPDetail.TSPName = r["TSPName"].ToString();

            return TSPDetail;
        }

        public List<TSPDetailModel> FetchTSPByUser(QueryFilters filters)
        {
            try
            {
                List<SqlParameter> param = new List<SqlParameter>();
                param.Add(new SqlParameter("@SchemeID", filters.SchemeID));
                param.Add(new SqlParameter("@UserID", filters.UserID));
                param.Add(new SqlParameter("@TSPID", filters.TSPID));
                param.Add(new SqlParameter("@OID", filters.OID));

                DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_TSPByUser", param.ToArray()).Tables[0];
                return LoopinData(dt);
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }

        public List<TSPDetailModel> FetchKamRelevantTSPByScheme(QueryFilters filters)
        {
            try
            {
                List<SqlParameter> param = new List<SqlParameter>();
                param.Add(new SqlParameter("@SchemeID", filters.SchemeID));
                param.Add(new SqlParameter("@UserID", filters.UserID));
                //param.Add(new SqlParameter("@TSPID", filters.TSPID));
                param.Add(new SqlParameter("@OID", filters.OID));

                DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_RelevantSchemeTSPByKAMUser", param.ToArray()).Tables[0];
                return LoopinData(dt);
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }

        public List<TSPDetailModel> FetchTSPDataByUser(int UserID)
        {
            try
            {
                List<SqlParameter> param = new List<SqlParameter>();
                param.Add(new SqlParameter("@UserID", UserID));

                DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_TSPByUser", param.ToArray()).Tables[0];
                return LoopinData(dt);
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }

        public DataTable FetchTSPCRDataByUser(int UserID)
        {
            try
            {
                List<SqlParameter> param = new List<SqlParameter>();
                param.Add(new SqlParameter("@UserID", UserID));

                DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_TSPChangeRequestDataByUser", param.ToArray()).Tables[0];
                return (dt);
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }

        public List<TSPDetailModel> FetchTSPByUserPaged(PagingModel pagingModel, SearchFilter filterModel, out int totalCount)
        {
            try
            {
                List<SqlParameter> param = new List<SqlParameter>();
                param.Add(new SqlParameter("@SchemeID", filterModel.SchemeID));
                param.Add(new SqlParameter("@TSPID", filterModel.TSPID));
                param.Add(new SqlParameter("@ClassID", filterModel.ClassID));
                param.Add(new SqlParameter("@TraineeID", filterModel.TraineeID));
                param.Add(new SqlParameter("@UserID", filterModel.UserID));
                param.Add(new SqlParameter("@OID", filterModel.OID));
                param.AddRange(Common.GetPagingParams(pagingModel));

                DataTable dt = new DataTable();
                dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_TSPByUserPaged", param.ToArray()).Tables[0];

                if (dt.Rows.Count > 0)
                    totalCount = Convert.ToInt32(dt.Rows[0]["TotalRows"]);
                else
                    totalCount = 0;
                return LoopinData(dt);
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }

        public TSPDetailModel GetUserByTSPID(int TSPID)
        {
            try
            {
                SqlParameter param = new SqlParameter("@TSPID", TSPID);
                DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_GetUserByTSPID", param).Tables[0];
                List<TSPDetailModel> ComplaintModel = Helper.ConvertDataTableToModel<TSPDetailModel>(dt);
                if (ComplaintModel.Count > 0)
                    return ComplaintModel[0];
                return null;
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }

        public TSPDetailModel FetchTSPByTSPCode(string TSPCode)
        {
            try
            {
                DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_TSPMaster", new SqlParameter("@TSPCode", TSPCode)).Tables[0];
                List<TSPDetailModel> TSPDetailModel = Helper.ConvertDataTableToModel<TSPDetailModel>(dt);
                if (TSPDetailModel.Count > 0)
                {
                    return (TSPDetailModel[0]);
                }
                else { return null; }
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }

        public TSPDetailModel FetchTSPByUserID(QueryFilters filters)
        {
            try
            {
                List<SqlParameter> param = new List<SqlParameter>();
                param.Add(new SqlParameter("@SchemeID", filters.SchemeID));
                param.Add(new SqlParameter("@UserID", filters.UserID));
                param.Add(new SqlParameter("@TSPID", filters.TSPID));
                param.Add(new SqlParameter("@OID", filters.OID));

                DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_TSPByUser", param.ToArray()).Tables[0];

                if (dt.Rows.Count > 0)
                {
                    return RowOfTSPDetail(dt.Rows[0]);
                }
                else
                    return null;
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }
    }
}