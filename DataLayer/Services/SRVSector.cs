using DataLayer.Classes;
using DataLayer.Models;
using Microsoft.Data.SqlClient;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;

namespace DataLayer.Services
{
    public class SRVSector : SRVBase, DataLayer.Interfaces.ISRVSector
    {
        public SRVSector()
        {
        }

        public SectorModel GetBySectorID(int SectorID)
        {
            try
            {
                SqlParameter param = new SqlParameter("@SectorID", SectorID);
                DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_Sector", param).Tables[0];
                if (dt.Rows.Count > 0)
                {
                    return RowOfSector(dt.Rows[0]);
                }
                else
                    return null;
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }

        public List<SectorModel> SaveSector(SectorModel Sector)
        {
            try
            {
                SqlParameter[] param = new SqlParameter[3];
                param[0] = new SqlParameter("@SectorID", Sector.SectorID);
                param[1] = new SqlParameter("@SectorName", Sector.SectorName);

                param[2] = new SqlParameter("@CurUserID", Sector.CurUserID);
                SqlHelper.ExecuteNonQuery(SqlHelper.GetCon(), CommandType.StoredProcedure, "[AU_Sector]", param);
                return FetchSector();
            }
            catch (Exception ex)
            { throw new Exception(ex.Message); }
        }

        private List<SectorModel> LoopinData(DataTable dt)
        {
            List<SectorModel> SectorL = new List<SectorModel>();

            foreach (DataRow r in dt.Rows)
            {
                SectorL.Add(RowOfSector(r));
            }
            return SectorL;
        }

        public List<SectorModel> FetchSector(SectorModel mod)
        {
            try
            {
                DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_Sector", Common.GetParams(mod)).Tables[0];
                return LoopinData(dt);
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }

        public List<SectorModel> FetchSector()
        {
            try
            {
                DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_Sector").Tables[0];
                return LoopinData(dt);
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }

        public List<SectorModel> FetchSector(bool InActive)
        {
            try
            {
                DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_Sector", new SqlParameter("@InActive", InActive)).Tables[0];
                return LoopinData(dt);
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }

        public List<SectorModel> FetchSectorForROSIFilter(ROSIFiltersModel rosiFilters)
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
                param[9] = new SqlParameter("@DurationIDs", rosiFilters.DurationIDs);
                //param[1] = new SqlParameter("@CurUserID", CurUserID);

                DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_SectorForROSIFilter", param).Tables[0];
                return LoopinData(dt);
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }

        public int BatchInsert(List<SectorModel> ls, int @BatchFkey, int CurUserID)
        {
            SqlParameter[] param = new SqlParameter[3];

            param[0] = new SqlParameter("@Json", JsonConvert.SerializeObject(ls));
            param[1] = new SqlParameter("@", @BatchFkey);
            param[2] = new SqlParameter("@CurUserID", CurUserID);
            return SqlHelper.ExecuteNonQuery(SqlHelper.GetCon(), CommandType.StoredProcedure, "[BAU_Sector]", param);
        }

        public void ActiveInActive(int SectorID, bool? InActive, int CurUserID)
        {
            SqlParameter[] PLead = new SqlParameter[3];
            PLead[0] = new SqlParameter("@SectorID", SectorID);
            PLead[1] = new SqlParameter("@InActive", InActive);
            PLead[2] = new SqlParameter("@CurUserID", CurUserID);
            SqlHelper.ExecuteNonQuery(SqlHelper.GetCon(), CommandType.StoredProcedure, "[AI_Sector]", PLead);
        }

        private SectorModel RowOfSector(DataRow r)
        {
            SectorModel Sector = new SectorModel();
            Sector.SectorID = Convert.ToInt32(r["SectorID"]);
            Sector.SectorName = r["SectorName"].ToString();
            Sector.InActive = Convert.ToBoolean(r["InActive"]);
            Sector.CreatedUserID = Convert.ToInt32(r["CreatedUserID"]);
            Sector.ModifiedUserID = Convert.ToInt32(r["ModifiedUserID"]);
            Sector.CreatedDate = r["CreatedDate"].ToString().GetDate();
            Sector.ModifiedDate = r["ModifiedDate"].ToString().GetDate();

            return Sector;
        }
    }
}