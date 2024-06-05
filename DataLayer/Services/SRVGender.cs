using DataLayer.Classes;
using DataLayer.Models;
using Microsoft.Data.SqlClient;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;

namespace DataLayer.Services
{
    public class SRVGender : SRVBase, DataLayer.Interfaces.ISRVGender
    {
        public SRVGender()
        {
        }

        public GenderModel GetByGenderID(int GenderID)
        {
            try
            {
                SqlParameter param = new SqlParameter("@GenderID", GenderID);
                DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_Gender", param).Tables[0];
                if (dt.Rows.Count > 0)
                {
                    return RowOfGender(dt.Rows[0]);
                }
                else
                    return null;
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }

        public List<GenderModel> SaveGender(GenderModel Gender)
        {
            try
            {
                SqlParameter[] param = new SqlParameter[3];
                param[0] = new SqlParameter("@GenderID", Gender.GenderID);
                param[1] = new SqlParameter("@GenderName", Gender.GenderName);

                param[2] = new SqlParameter("@CurUserID", Gender.CurUserID);
                SqlHelper.ExecuteNonQuery(SqlHelper.GetCon(), CommandType.StoredProcedure, "[AU_Gender]", param);
                return FetchGender();
            }
            catch (Exception ex)
            { throw new Exception(ex.Message); }
        }

        private List<GenderModel> LoopinData(DataTable dt)
        {
            List<GenderModel> GenderL = new List<GenderModel>();

            foreach (DataRow r in dt.Rows)
            {
                GenderL.Add(RowOfGender(r));
            }
            return GenderL;
        }

        public List<GenderModel> FetchGender(GenderModel mod)
        {
            try
            {
                DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_Gender", Common.GetParams(mod)).Tables[0];
                return LoopinData(dt);
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }

        public List<GenderModel> FetchGender()
        {
            try
            {
                DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_Gender").Tables[0];
                return LoopinData(dt);
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }

        public List<GenderModel> FetchGender(bool InActive)
        {
            try
            {
                //, new SqlParameter("@InActive", InActive)
                DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_Gender", new SqlParameter("@InActive", InActive)).Tables[0];
                return LoopinData(dt);
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }

        public DataTable FetchGenderSSP(bool InActive)
        {
            try
            {
                DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_Gender", new SqlParameter("@InActive", InActive)).Tables[0];
                return dt;
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }

        public List<GenderModel> FetchGenderForROSIFilter(ROSIFiltersModel rosiFilters)
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
                param[8] = new SqlParameter("@SectorIDs", rosiFilters.SectorIDs);
                param[9] = new SqlParameter("@DurationIDs", rosiFilters.DurationIDs);
                //param[1] = new SqlParameter("@CurUserID", CurUserID);

                DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_GenderForROSIFilter", param).Tables[0];
                return LoopinData(dt);
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }

        public int BatchInsert(List<GenderModel> ls, int @BatchFkey, int CurUserID)
        {
            SqlParameter[] param = new SqlParameter[3];

            param[0] = new SqlParameter("@Json", JsonConvert.SerializeObject(ls));
            param[1] = new SqlParameter("@", @BatchFkey);
            param[2] = new SqlParameter("@CurUserID", CurUserID);
            return SqlHelper.ExecuteNonQuery(SqlHelper.GetCon(), CommandType.StoredProcedure, "[BAU_Gender]", param);
        }

        public void ActiveInActive(int GenderID, bool? InActive, int CurUserID)
        {
            SqlParameter[] PLead = new SqlParameter[3];
            PLead[0] = new SqlParameter("@GenderID", GenderID);
            PLead[1] = new SqlParameter("@InActive", InActive);
            PLead[2] = new SqlParameter("@CurUserID", CurUserID);
            SqlHelper.ExecuteNonQuery(SqlHelper.GetCon(), CommandType.StoredProcedure, "[AI_Gender]", PLead);
        }

        private GenderModel RowOfGender(DataRow r)
        {
            GenderModel Gender = new GenderModel();
            Gender.GenderID = Convert.ToInt32(r["GenderID"]);
            Gender.GenderName = r["GenderName"].ToString();
            Gender.InActive = Convert.ToBoolean(r["InActive"]);
            Gender.CreatedUserID = Convert.ToInt32(r["CreatedUserID"]);
            Gender.ModifiedUserID = Convert.ToInt32(r["ModifiedUserID"]);
            Gender.CreatedDate = r["CreatedDate"].ToString().GetDate();
            Gender.ModifiedDate = r["ModifiedDate"].ToString().GetDate();

            return Gender;
        }
    }
}