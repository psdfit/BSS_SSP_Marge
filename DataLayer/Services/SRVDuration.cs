/* **** Aamer Rehman Malik *****/

using DataLayer.Classes;
using DataLayer.Interfaces;
using DataLayer.Models;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;

namespace DataLayer.Services
{
    public class SRVDuration : SRVBase, ISRVDuration
    {
        public SRVDuration()
        {
        }

        public DurationModel GetByDurationID(int DurationID)
        {
            try
            {
                SqlParameter param = new SqlParameter("@DurationID", DurationID);
                DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_Duration", param).Tables[0];
                if (dt.Rows.Count > 0)
                {
                    return RowOfDuration(dt.Rows[0]);
                }
                else
                    return null;
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }

        public List<DurationModel> SaveDuration(DurationModel Duration)
        {
            try
            {
                SqlParameter[] param = new SqlParameter[3];
                param[0] = new SqlParameter("@DurationID", Duration.DurationID);
                param[1] = new SqlParameter("@Duration", Duration.Duration);

                param[2] = new SqlParameter("@CurUserID", Duration.CurUserID);
                SqlHelper.ExecuteNonQuery(SqlHelper.GetCon(), CommandType.StoredProcedure, "[AU_Duration]", param);
                return FetchDuration();
            }
            catch (Exception ex)
            { throw new Exception(ex.Message); }
        }

        private List<DurationModel> LoopinData(DataTable dt)
        {
            List<DurationModel> DurationL = new List<DurationModel>();

            foreach (DataRow r in dt.Rows)
            {
                DurationL.Add(RowOfDuration(r));
            }
            return DurationL;
        }
        private List<DurationModel> LoopinDurationROSIFilter(DataTable dt)
        {
            List<DurationModel> DurationL = new List<DurationModel>();

            foreach (DataRow r in dt.Rows)
            {
                DurationL.Add(RowOfDurationROSIFilter(r));
            }
            return DurationL;
        }

        public List<DurationModel> FetchDuration(DurationModel mod)
        {
            try
            {
                DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_Duration", Common.GetParams(mod)).Tables[0];
                return LoopinData(dt);
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }

        public List<DurationModel> FetchDuration()
        {
            try
            {
                DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_Duration").Tables[0];
                return LoopinData(dt);
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }

        public List<DurationModel> FetchDuration(bool InActive)
        {
            try
            {
                DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_Duration", new SqlParameter("@InActive", InActive)).Tables[0];
                return LoopinData(dt);
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }
        public List<DurationModel> FetchDurationForROSIFilter(bool InActive)
        {
            try
            {
                DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_DurationForROSIFilter", new SqlParameter("@InActive", InActive)).Tables[0];
                return LoopinDurationROSIFilter(dt);
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }

        public List<DurationModel> FetchDurationForROSIFilter(ROSIFiltersModel rosiFilters)
        {
            try
            {
                SqlParameter[] param = new SqlParameter[10];
                param[0] = new SqlParameter("@SchemeIDs", rosiFilters.SchemeIDs);
                param[1] = new SqlParameter("@TSPIDs", rosiFilters.TSPIDs);
                param[2] = new SqlParameter("@PTypeIDs", rosiFilters.PTypeIDs);
                param[3] = new SqlParameter("@ClusterIDs", rosiFilters.ClusterIDs);
                param[4] = new SqlParameter("@DistrictIDs", rosiFilters.DistrictIDs);
                param[5] = new SqlParameter("@OIDs", rosiFilters.OrganizationIDs);
                param[6] = new SqlParameter("@TradeIDs", rosiFilters.TradeIDs);
                param[7] = new SqlParameter("@FundingSourceIDs", rosiFilters.FundingSourceIDs);
                param[8] = new SqlParameter("@GenderIDs", rosiFilters.GenderIDs);
                param[9] = new SqlParameter("@SectorIDs", rosiFilters.DurationIDs);
                //param[1] = new SqlParameter("@CurUserID", CurUserID);

                DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_DurationForROSIFilter", param).Tables[0];
                return LoopinDurationROSIFilter(dt);
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }

        public void ActiveInActive(int DurationID, bool? InActive, int CurUserID)
        {
            SqlParameter[] PLead = new SqlParameter[3];
            PLead[0] = new SqlParameter("@DurationID", DurationID);
            PLead[1] = new SqlParameter("@InActive", InActive);
            PLead[2] = new SqlParameter("@CurUserID", CurUserID);
            SqlHelper.ExecuteNonQuery(SqlHelper.GetCon(), CommandType.StoredProcedure, "[AI_Duration]", PLead);
        }

        private DurationModel RowOfDuration(DataRow r)
        {
            DurationModel Duration = new DurationModel();
            Duration.DurationID = Convert.ToInt32(r["DurationID"]);
            Duration.Duration = (float)Convert.ToDecimal(r["Duration"]);
            Duration.InActive = Convert.ToBoolean(r["InActive"]);
            Duration.CreatedUserID = Convert.ToInt32(r["CreatedUserID"]);
            Duration.ModifiedUserID = Convert.ToInt32(r["ModifiedUserID"]);
            Duration.CreatedDate = r["CreatedDate"].ToString().GetDate();
            Duration.ModifiedDate = r["ModifiedDate"].ToString().GetDate();

            return Duration;
        }
        private DurationModel RowOfDurationROSIFilter(DataRow r)
        {
            DurationModel Duration = new DurationModel();
            Duration.Duration = (float)Convert.ToDecimal(r["Duration"]);
            return Duration;
        }
    }
}