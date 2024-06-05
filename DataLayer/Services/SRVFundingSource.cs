using DataLayer.Classes;
using DataLayer.Models;
using Microsoft.Data.SqlClient;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;

namespace DataLayer.Services
{
    public class SRVFundingSource : SRVBase, DataLayer.Interfaces.ISRVFundingSource
    {
        public SRVFundingSource()
        {
        }

        public FundingSourceModel GetByFundingSourceID(int FundingSourceID)
        {
            try
            {
                SqlParameter param = new SqlParameter("@FundingSourceID", FundingSourceID);
                DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_FundingSource", param).Tables[0];
                if (dt.Rows.Count > 0)
                {
                    return RowOfFundingSource(dt.Rows[0]);
                }
                else
                    return null;
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }

        public List<FundingSourceModel> SaveFundingSource(FundingSourceModel FundingSource)
        {
            try
            {
                SqlParameter[] param = new SqlParameter[3];
                param[0] = new SqlParameter("@FundingSourceID", FundingSource.FundingSourceID);
                param[1] = new SqlParameter("@FundingSourceName", FundingSource.FundingSourceName);

                param[2] = new SqlParameter("@CurUserID", FundingSource.CurUserID);
                SqlHelper.ExecuteNonQuery(SqlHelper.GetCon(), CommandType.StoredProcedure, "[AU_FundingSource]", param);
                return FetchFundingSource();
            }
            catch (Exception ex)
            { throw new Exception(ex.Message); }
        }

        private List<FundingSourceModel> LoopinData(DataTable dt)
        {
            List<FundingSourceModel> FundingSourceL = new List<FundingSourceModel>();

            foreach (DataRow r in dt.Rows)
            {
                FundingSourceL.Add(RowOfFundingSource(r));
            }
            return FundingSourceL;
        }

        public List<FundingSourceModel> FetchFundingSource(FundingSourceModel mod)
        {
            try
            {
                DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_FundingSource", Common.GetParams(mod)).Tables[0];
                return LoopinData(dt);
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }

        public List<FundingSourceModel> FetchFundingSource()
        {
            try
            {
                DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_FundingSource").Tables[0];
                return LoopinData(dt);
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }

        public List<FundingSourceModel> FetchFundingSource(bool InActive)
        {
            try
            {
                DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_FundingSource", new SqlParameter("@InActive", InActive)).Tables[0];
                return LoopinData(dt);
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }

        public DataTable FetchFundingSourceSSP(bool InActive)
        {
            try
            {
                DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_FundingSource", new SqlParameter("@InActive", InActive)).Tables[0];
                return dt;
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }

        public List<FundingSourceModel> FetchFundingSourceForROSIFilter(ROSIFiltersModel rosiFilters)
        {
            try
            {
                SqlParameter[] param = new SqlParameter[10];
                param[0] = new SqlParameter("@SchemeIDs", rosiFilters.SchemeIDs);
                param[1] = new SqlParameter("@TSPIDs", rosiFilters.TSPIDs);
                param[2] = new SqlParameter("@SectorIDs", rosiFilters.SectorIDs);
                param[3] = new SqlParameter("@ClusterIDs", rosiFilters.ClusterIDs);
                param[4] = new SqlParameter("@DistrictIDs", rosiFilters.DistrictIDs);
                param[5] = new SqlParameter("@OIDs", rosiFilters.OrganizationIDs);
                param[6] = new SqlParameter("@TradeIDs", rosiFilters.TradeIDs);
                param[7] = new SqlParameter("@PTypeIDs", rosiFilters.PTypeIDs);
                param[8] = new SqlParameter("@GenderIDs", rosiFilters.GenderIDs);
                param[9] = new SqlParameter("@DurationIDs", rosiFilters.DurationIDs);
                //param[1] = new SqlParameter("@CurUserID", CurUserID);

                DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_FundingSourceForROSIFilter", param).Tables[0];
                return LoopinData(dt);
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }


        public int BatchInsert(List<FundingSourceModel> ls, int @BatchFkey, int CurUserID)
        {
            SqlParameter[] param = new SqlParameter[3];

            param[0] = new SqlParameter("@Json", JsonConvert.SerializeObject(ls));
            param[1] = new SqlParameter("@", @BatchFkey);
            param[2] = new SqlParameter("@CurUserID", CurUserID);
            return SqlHelper.ExecuteNonQuery(SqlHelper.GetCon(), CommandType.StoredProcedure, "[BAU_FundingSource]", param);
        }

        public void ActiveInActive(int FundingSourceID, bool? InActive, int CurUserID)
        {
            SqlParameter[] PLead = new SqlParameter[3];
            PLead[0] = new SqlParameter("@FundingSourceID", FundingSourceID);
            PLead[1] = new SqlParameter("@InActive", InActive);
            PLead[2] = new SqlParameter("@CurUserID", CurUserID);
            SqlHelper.ExecuteNonQuery(SqlHelper.GetCon(), CommandType.StoredProcedure, "[AI_FundingSource]", PLead);
        }

        private FundingSourceModel RowOfFundingSource(DataRow r)
        {
            FundingSourceModel FundingSource = new FundingSourceModel();
            FundingSource.FundingSourceID = Convert.ToInt32(r["FundingSourceID"]);
            FundingSource.FundingSourceName = r["FundingSourceName"].ToString();
            FundingSource.InActive = Convert.ToBoolean(r["InActive"]);
            FundingSource.CreatedUserID = Convert.ToInt32(r["CreatedUserID"]);
            FundingSource.ModifiedUserID = Convert.ToInt32(r["ModifiedUserID"]);
            FundingSource.CreatedDate = r["CreatedDate"].ToString().GetDate();
            FundingSource.ModifiedDate = r["ModifiedDate"].ToString().GetDate();

            return FundingSource;
        }
    }
}