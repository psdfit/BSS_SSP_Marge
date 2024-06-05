using DataLayer.Classes;
using DataLayer.Models;
using Microsoft.Data.SqlClient;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;

namespace DataLayer.Services
{
    public class SRVCluster : SRVBase, DataLayer.Interfaces.ISRVCluster
    {
        public SRVCluster()
        {
        }

        public ClusterModel GetByClusterID(int ClusterID)
        {
            try
            {
                SqlParameter param = new SqlParameter("@ClusterID", ClusterID);
                DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_Cluster", param).Tables[0];
                if (dt.Rows.Count > 0)
                {
                    return RowOfCluster(dt.Rows[0]);
                }
                else
                    return null;
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }

        public List<ClusterModel> SaveCluster(ClusterModel Cluster)
        {
            try
            {
                SqlParameter[] param = new SqlParameter[3];
                param[0] = new SqlParameter("@ClusterID", Cluster.ClusterID);
                param[1] = new SqlParameter("@ClusterName", Cluster.ClusterName);

                param[2] = new SqlParameter("@CurUserID", Cluster.CurUserID);
                SqlHelper.ExecuteNonQuery(SqlHelper.GetCon(), CommandType.StoredProcedure, "[AU_Cluster]", param);
                return FetchCluster();
            }
            catch (Exception ex)
            { throw new Exception(ex.Message); }
        }

        private List<ClusterModel> LoopinData(DataTable dt)
        {
            List<ClusterModel> ClusterL = new List<ClusterModel>();

            foreach (DataRow r in dt.Rows)
            {
                ClusterL.Add(RowOfCluster(r));
            }
            return ClusterL;
        }

        public List<ClusterModel> FetchCluster(ClusterModel mod)
        {
            try
            {
                DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_Cluster", Common.GetParams(mod)).Tables[0];
                return LoopinData(dt);
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }

        public List<ClusterModel> FetchCluster()
        {
            try
            {
                DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_Cluster").Tables[0];
                return LoopinData(dt);
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }

        public List<ClusterModel> FetchCluster(bool InActive)
        {
            try
            {
                DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_Cluster", new SqlParameter("@InActive", InActive)).Tables[0];
                return LoopinData(dt);
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }
        public List<ClusterModel> FetchClusterForROSIFilter(ROSIFiltersModel rosiFilters)
        {
            try
            {
                SqlParameter[] param = new SqlParameter[10];
                param[0] = new SqlParameter("@SchemeIDs", rosiFilters.SchemeIDs);
                param[1] = new SqlParameter("@TSPIDs", rosiFilters.TSPIDs);
                param[2] = new SqlParameter("@PTypeIDs", rosiFilters.PTypeIDs);
                param[3] = new SqlParameter("@SectorIDs", rosiFilters.SectorIDs);
                param[4] = new SqlParameter("@DistrictIDs", rosiFilters.DistrictIDs);
                param[5] = new SqlParameter("@OIDs", rosiFilters.OrganizationIDs);
                param[6] = new SqlParameter("@TradeIDs", rosiFilters.TradeIDs);
                param[7] = new SqlParameter("@FundingSourceIDs", rosiFilters.FundingSourceIDs);
                param[8] = new SqlParameter("@GenderIDs", rosiFilters.GenderIDs);
                param[9] = new SqlParameter("@DurationIDs", rosiFilters.DurationIDs);
                //param[1] = new SqlParameter("@CurUserID", CurUserID);

                DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_ClusterForROSIFilter", param).Tables[0];
                return LoopinData(dt);
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }

        //public List<ClusterModel> FetchClusterForROSIFilter(ROSIFiltersModel rosiFilters)
        //{
        //    try
        //    {
        //        SqlParameter[] param = new SqlParameter[10];
        //        param[0] = new SqlParameter("@SchemeIDs", rosiFilters.SchemeIDs);
        //        param[1] = new SqlParameter("@TSPIDs", rosiFilters.TSPIDs);
        //        param[2] = new SqlParameter("@PTypeIDs", rosiFilters.PTypeIDs);
        //        param[3] = new SqlParameter("@SectorIDs", rosiFilters.SectorIDs);
        //        param[4] = new SqlParameter("@DistrictIDs", rosiFilters.DistrictIDs);
        //        param[5] = new SqlParameter("@OIDs", rosiFilters.OrganizationIDs);
        //        param[6] = new SqlParameter("@TradeIDs", rosiFilters.TradeIDs);
        //        param[7] = new SqlParameter("@FundingSourceIDs", rosiFilters.FundingSourceIDs);
        //        param[8] = new SqlParameter("@GenderIDs", rosiFilters.GenderIDs);
        //        param[9] = new SqlParameter("@DurationIDs", rosiFilters.DurationIDs);
        //        //param[1] = new SqlParameter("@CurUserID", CurUserID);

        //        DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_ClusterForROSIFilter", param).Tables[0];
        //        return LoopinData(dt);
        //    }
        //    catch (Exception ex) { throw new Exception(ex.Message); }
        //}

        public int BatchInsert(List<ClusterModel> ls, int @BatchFkey, int CurUserID)
        {
            SqlParameter[] param = new SqlParameter[3];

            param[0] = new SqlParameter("@Json", JsonConvert.SerializeObject(ls));
            param[1] = new SqlParameter("@", @BatchFkey);
            param[2] = new SqlParameter("@CurUserID", CurUserID);
            return SqlHelper.ExecuteNonQuery(SqlHelper.GetCon(), CommandType.StoredProcedure, "[BAU_Cluster]", param);
        }

        public void ActiveInActive(int ClusterID, bool? InActive, int CurUserID)
        {
            SqlParameter[] PLead = new SqlParameter[3];
            PLead[0] = new SqlParameter("@ClusterID", ClusterID);
            PLead[1] = new SqlParameter("@InActive", InActive);
            PLead[2] = new SqlParameter("@CurUserID", CurUserID);
            SqlHelper.ExecuteNonQuery(SqlHelper.GetCon(), CommandType.StoredProcedure, "[AI_Cluster]", PLead);
        }

        private ClusterModel RowOfCluster(DataRow r)
        {
            ClusterModel Cluster = new ClusterModel();
            Cluster.ClusterID = Convert.ToInt32(r["ClusterID"]);
            Cluster.ClusterName = r["ClusterName"].ToString();
            Cluster.ProvinceID = Convert.ToInt32(r["ProvinceID"]);
            Cluster.InActive = Convert.ToBoolean(r["InActive"]);
            Cluster.CreatedUserID = Convert.ToInt32(r["CreatedUserID"]);
            Cluster.ModifiedUserID = Convert.ToInt32(r["ModifiedUserID"]);
            Cluster.CreatedDate = r["CreatedDate"].ToString().GetDate();
            Cluster.ModifiedDate = r["ModifiedDate"].ToString().GetDate();

            return Cluster;
        }
    }
}