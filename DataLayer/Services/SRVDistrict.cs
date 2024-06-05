using DataLayer.Classes;
using DataLayer.Models;
using Microsoft.Data.SqlClient;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;

namespace DataLayer.Services
{
    public class SRVDistrict : SRVBase, DataLayer.Interfaces.ISRVDistrict
    {
        public SRVDistrict()
        {
        }

        public DistrictModel GetByDistrictID(int DistrictID)
        {
            try
            {
                SqlParameter param = new SqlParameter("@DistrictID", DistrictID);
                DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_District", param).Tables[0];
                if (dt.Rows.Count > 0)
                {
                    return RowOfDistrict(dt.Rows[0]);
                }
                else
                    return null;
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }

        public List<DistrictModel> SaveDistrict(DistrictModel District)
        {
            try
            {
                SqlParameter[] param = new SqlParameter[6];
                param[0] = new SqlParameter("@DistrictID", District.DistrictID);
                param[1] = new SqlParameter("@DistrictName", District.DistrictName);
                param[2] = new SqlParameter("@DistrictNameUrdu", District.DistrictNameUrdu);
                param[3] = new SqlParameter("@ClusterID", District.ClusterID);
                param[4] = new SqlParameter("@RegionID", District.RegionID);

                param[5] = new SqlParameter("@CurUserID", District.CurUserID);
                SqlHelper.ExecuteNonQuery(SqlHelper.GetCon(), CommandType.StoredProcedure, "[AU_District]", param);
                return FetchDistrict();
            }
            catch (Exception ex)
            { throw new Exception(ex.Message); }
        }

        public List<DistrictModel> GetByDistrictName(string DistrictName)
        {
            try
            {
                DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_District", new SqlParameter("@DistrictName", DistrictName)).Tables[0];
                if (dt.Rows.Count > 0)
                {
                    List<DistrictModel> district = LoopinData(dt);

                    return district;
                }
                else
                    return null;
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }

        private List<DistrictModel> LoopinData(DataTable dt)
        {
            List<DistrictModel> DistrictL = new List<DistrictModel>();

            foreach (DataRow r in dt.Rows)
            {
                DistrictL.Add(RowOfDistrict(r));
            }
            return DistrictL;
        }
        private List<DistrictModel> LoopinAllPakistanDistrictData(DataTable dt)
        {
            List<DistrictModel> DistrictL = new List<DistrictModel>();

            foreach (DataRow r in dt.Rows)
            {
                DistrictL.Add(RowOfAllPakistanDistrict(r));
            }
            return DistrictL;
        }

        public List<DistrictModel> FetchDistrict(DistrictModel mod)
        {
            try
            {
                DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_District", Common.GetParams(mod)).Tables[0];
                return LoopinData(dt);
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }

        public List<DistrictModel> FetchDistrict()
        {
            try
            {
                DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_District").Tables[0];
                return LoopinData(dt);
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }

        public List<DistrictModel> FetchDistrict(bool InActive)
        {
            try
            {
               DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_District", new SqlParameter("@InActive", InActive)).Tables[0];
                return LoopinData(dt);
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }
        public List<DistrictModel> FetchDistrictForROSIFilter(ROSIFiltersModel rosiFilters)
        {
            try
            {
                SqlParameter[] param = new SqlParameter[10];
                param[0] = new SqlParameter("@SchemeIDs", rosiFilters.SchemeIDs);
                param[1] = new SqlParameter("@TSPIDs", rosiFilters.TSPIDs);
                param[2] = new SqlParameter("@PTypeIDs", rosiFilters.PTypeIDs);
                param[3] = new SqlParameter("@SectorIDs", rosiFilters.SectorIDs);
                param[4] = new SqlParameter("@ClusterIDs", rosiFilters.ClusterIDs);
                param[5] = new SqlParameter("@OIDs", rosiFilters.OrganizationIDs);
                param[6] = new SqlParameter("@TradeIDs", rosiFilters.TradeIDs);
                param[7] = new SqlParameter("@FundingSourceIDs", rosiFilters.FundingSourceIDs);
                param[8] = new SqlParameter("@GenderIDs", rosiFilters.GenderIDs);
                param[9] = new SqlParameter("@DurationIDs", rosiFilters.DurationIDs);
                //param[1] = new SqlParameter("@CurUserID", CurUserID);

                DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_DistrictForROSIFilter", param).Tables[0];
                return LoopinData(dt);
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }
        public List<DistrictModel> AllDistrictsAndTehsils()
        {
            try
            {
                DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "[GetAllDistrictsAndTehsils]").Tables[0];
                return  Helper.ConvertDataTableToModel<DistrictModel>(dt);
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }
        public List<DistrictModel> FetchAllPakistanDistrict(bool InActive)
        {
            try
            {
                DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_AllPakistan_District", new SqlParameter("@InActive", InActive)).Tables[0];
                return LoopinAllPakistanDistrictData(dt);
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }

        //public List<DistrictModel> FetchDistrictForROSIFilter(ROSIFiltersModel rosiFilters)
        //{
        //    try
        //    {
        //        SqlParameter[] param = new SqlParameter[10];
        //        param[0] = new SqlParameter("@SchemeIDs", rosiFilters.SchemeIDs);
        //        param[1] = new SqlParameter("@TSPIDs", rosiFilters.TSPIDs);
        //        param[2] = new SqlParameter("@PTypeIDs", rosiFilters.PTypeIDs);
        //        param[3] = new SqlParameter("@SectorIDs", rosiFilters.SectorIDs);
        //        param[4] = new SqlParameter("@ClusterIDs", rosiFilters.ClusterIDs);
        //        param[5] = new SqlParameter("@OIDs", rosiFilters.OrganizationIDs);
        //        param[6] = new SqlParameter("@TradeIDs", rosiFilters.TradeIDs);
        //        param[7] = new SqlParameter("@FundingSourceIDs", rosiFilters.FundingSourceIDs);
        //        param[8] = new SqlParameter("@GenderIDs", rosiFilters.GenderIDs);
        //        param[9] = new SqlParameter("@DurationIDs", rosiFilters.DurationIDs);
        //        //param[1] = new SqlParameter("@CurUserID", CurUserID);

        //        DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_DistrictForROSIFilter", param).Tables[0];
        //        return LoopinData(dt);
        //    }
        //    catch (Exception ex) { throw new Exception(ex.Message); }
        //}


        public int BatchInsert(List<DistrictModel> ls, int @BatchFkey, int CurUserID)
        {
            SqlParameter[] param = new SqlParameter[3];

            param[0] = new SqlParameter("@Json", JsonConvert.SerializeObject(ls));
            param[1] = new SqlParameter("@", @BatchFkey);
            param[2] = new SqlParameter("@CurUserID", CurUserID);
            return SqlHelper.ExecuteNonQuery(SqlHelper.GetCon(), CommandType.StoredProcedure, "[BAU_District]", param);
        }

        public List<DistrictModel> GetByClusterID(int ClusterID)
        {
            try
            {
                DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_District", new SqlParameter("@ClusterID", ClusterID)).Tables[0];
                return LoopinData(dt);
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }

        public List<DistrictModel> GetByRegionID(int RegionID)
        {
            try
            {
                DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_District", new SqlParameter("@RegionID", RegionID)).Tables[0];
                return LoopinData(dt);
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }

        public void ActiveInActive(int DistrictID, bool? InActive, int CurUserID)
        {
            SqlParameter[] PLead = new SqlParameter[3];
            PLead[0] = new SqlParameter("@DistrictID", DistrictID);
            PLead[1] = new SqlParameter("@InActive", InActive);
            PLead[2] = new SqlParameter("@CurUserID", CurUserID);
            SqlHelper.ExecuteNonQuery(SqlHelper.GetCon(), CommandType.StoredProcedure, "[AI_District]", PLead);
        }

        private DistrictModel RowOfDistrict(DataRow r)
        {
            DistrictModel District = new DistrictModel();
            District.DistrictID = Convert.ToInt32(r["DistrictID"]);
            District.DistrictName = r["DistrictName"].ToString();
            District.DistrictNameUrdu = r["DistrictNameUrdu"].ToString();
            District.ClusterID = Convert.ToInt32(r["ClusterID"]);
            District.RegionID = r.Field<int?>("RegionID") ?? 0;
            District.ClusterName = r["ClusterName"].ToString();
            District.RegionName = r["RegionName"].ToString();
            District.InActive = Convert.ToBoolean(r["InActive"]);
            District.CreatedUserID = Convert.ToInt32(r["CreatedUserID"]);
            District.ModifiedUserID = Convert.ToInt32(r["ModifiedUserID"]);
            District.CreatedDate = r["CreatedDate"].ToString().GetDate();
            District.ModifiedDate = r["ModifiedDate"].ToString().GetDate();
            if (r.Table.Columns.Contains("ProvinceID"))
            { 
                District.ProvinceID = r.Field<int?>("ProvinceID") ?? 0; 
            }
                return District;
        }

        private DistrictModel RowOfAllPakistanDistrict(DataRow r)
        {
            DistrictModel District = new DistrictModel();
            District.DistrictID = Convert.ToInt32(r["DistrictID"]);
            District.DistrictName = r["DistrictName"].ToString();
            if (r.Table.Columns.Contains("ClusterID"))
            {
                District.ClusterID = Convert.ToInt32(r["ClusterID"]);
            }
            if (r.Table.Columns.Contains("ClusterName"))
            {
                District.ClusterName = r["ClusterName"].ToString();
            }
            if (r.Table.Columns.Contains("RegionID"))
            {
                District.RegionID = Convert.ToInt32(r["RegionID"]);
            }
            if (r.Table.Columns.Contains("RegionName"))
            {
                District.RegionName = r["RegionName"].ToString();
            }
            District.InActive = Convert.ToBoolean(r["InActive"]);
            District.CreatedUserID = Convert.ToInt32(r["CreatedUserID"]);
            District.ModifiedUserID = Convert.ToInt32(r["ModifiedUserID"]);
            District.CreatedDate = r["CreatedDate"].ToString().GetDate();
            District.ModifiedDate = r["ModifiedDate"].ToString().GetDate();

            return District;
        }
        public List<DistrictModel> SSPFetchDistrictTSP(int programid, int UserID)
        {
            try
            {
                SqlParameter[] PLead = new SqlParameter[3];
                PLead[0] = new SqlParameter("@ProgramID", programid);
                PLead[1] = new SqlParameter("@UserID", UserID);
                DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_SSPProgramDistrict", PLead).Tables[0];
                return SSPLoopinData(dt);
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }
        private List<DistrictModel> SSPLoopinData(DataTable dt)
        {
            List<DistrictModel> DistrictL = new List<DistrictModel>();

            foreach (DataRow r in dt.Rows)
            {
                DistrictL.Add(SSPRowOfDistrict(r));
            }
            return DistrictL;
        }
        private DistrictModel SSPRowOfDistrict(DataRow r)
        {
            DistrictModel District = new DistrictModel();
            District.DistrictID = Convert.ToInt32(r["DistrictID"]);
            District.DistrictName = r["DistrictName"].ToString();
            return District;
        }
    }
}