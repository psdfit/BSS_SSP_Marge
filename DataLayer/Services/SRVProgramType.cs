using DataLayer.Classes;
using DataLayer.Models;
using Microsoft.Data.SqlClient;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;

namespace DataLayer.Services
{
    public class SRVProgramType : SRVBase, DataLayer.Interfaces.ISRVProgramType
    {
        public SRVProgramType()
        {
        }

        public ProgramTypeModel GetByPTypeID(int PTypeID)
        {
            try
            {
                SqlParameter param = new SqlParameter("@PTypeID", PTypeID);
                DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_ProgramType", param).Tables[0];
                if (dt.Rows.Count > 0)
                {
                    return RowOfProgramType(dt.Rows[0]);
                }
                else
                    return null;
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }

        public List<ProgramTypeModel> SaveProgramType(ProgramTypeModel ProgramType)
        {
            try
            {
                SqlParameter[] param = new SqlParameter[3];
                param[0] = new SqlParameter("@PTypeID", ProgramType.PTypeID);
                param[1] = new SqlParameter("@PTypeName", ProgramType.PTypeName);

                param[2] = new SqlParameter("@CurUserID", ProgramType.CurUserID);
                SqlHelper.ExecuteNonQuery(SqlHelper.GetCon(), CommandType.StoredProcedure, "[AU_ProgramType]", param);
                return FetchProgramType();
            }
            catch (Exception ex)
            { throw new Exception(ex.Message); }
        }

        private List<ProgramTypeModel> LoopinData(DataTable dt)
        {
            List<ProgramTypeModel> ProgramTypeL = new List<ProgramTypeModel>();

            foreach (DataRow r in dt.Rows)
            {
                ProgramTypeL.Add(RowOfProgramType(r));
            }
            return ProgramTypeL;
        }

        public List<ProgramTypeModel> FetchProgramType(ProgramTypeModel mod)
        {
            try
            {
                DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_ProgramType", Common.GetParams(mod)).Tables[0];
                return LoopinData(dt);
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }

        public List<ProgramTypeModel> FetchProgramType()
        {
            try
            {
                DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_ProgramType").Tables[0];
                return LoopinData(dt);
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }

        public List<ProgramTypeModel> FetchProgramType(bool InActive)
        {
            try
            {
                DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_ProgramType", new SqlParameter("@InActive", InActive)).Tables[0];
                return LoopinData(dt);
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }

        public List<ProgramTypeModel> FetchPTypeForROSIFilter(ROSIFiltersModel rosiFilters)
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
                param[7] = new SqlParameter("@FundingSourceIDs", rosiFilters.FundingSourceIDs);
                param[8] = new SqlParameter("@GenderIDs", rosiFilters.GenderIDs);
                param[9] = new SqlParameter("@DurationIDs", rosiFilters.DurationIDs);
                //param[1] = new SqlParameter("@CurUserID", CurUserID);

                DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_ProgramTypeForROSIFilter", param).Tables[0];
                return LoopinData(dt);
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }

        public int BatchInsert(List<ProgramTypeModel> ls, int @BatchFkey, int CurUserID)
        {
            SqlParameter[] param = new SqlParameter[3];

            param[0] = new SqlParameter("@Json", JsonConvert.SerializeObject(ls));
            param[1] = new SqlParameter("@", @BatchFkey);
            param[2] = new SqlParameter("@CurUserID", CurUserID);
            return SqlHelper.ExecuteNonQuery(SqlHelper.GetCon(), CommandType.StoredProcedure, "[BAU_ProgramType]", param);
        }

        public void ActiveInActive(int PTypeID, bool? InActive, int CurUserID)
        {
            SqlParameter[] PLead = new SqlParameter[3];
            PLead[0] = new SqlParameter("@PTypeID", PTypeID);
            PLead[1] = new SqlParameter("@InActive", InActive);
            PLead[2] = new SqlParameter("@CurUserID", CurUserID);
            SqlHelper.ExecuteNonQuery(SqlHelper.GetCon(), CommandType.StoredProcedure, "[AI_ProgramType]", PLead);
        }

        private ProgramTypeModel RowOfProgramType(DataRow r)
        {
            ProgramTypeModel ProgramType = new ProgramTypeModel();
            ProgramType.PTypeID = Convert.ToInt32(r["PTypeID"]);
            ProgramType.PTypeName = r["PTypeName"].ToString();
            ProgramType.InActive = Convert.ToBoolean(r["InActive"]);
            ProgramType.CreatedUserID = Convert.ToInt32(r["CreatedUserID"]);
            ProgramType.ModifiedUserID = Convert.ToInt32(r["ModifiedUserID"]);
            ProgramType.CreatedDate = r["CreatedDate"].ToString().GetDate();
            ProgramType.ModifiedDate = r["ModifiedDate"].ToString().GetDate();

            return ProgramType;
        }
    }
}