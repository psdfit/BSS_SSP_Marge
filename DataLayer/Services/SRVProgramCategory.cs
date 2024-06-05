using DataLayer.Classes;
using DataLayer.Models;
using Microsoft.Data.SqlClient;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;

namespace DataLayer.Services
{
    public class SRVProgramCategory : SRVBase, DataLayer.Interfaces.ISRVProgramCategory
    {
        public SRVProgramCategory()
        {
        }

        public ProgramCategoryModel GetByPCategoryID(int PCategoryID)
        {
            try
            {
                SqlParameter param = new SqlParameter("@PCategoryID", PCategoryID);
                DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_ProgramCategory", param).Tables[0];
                if (dt.Rows.Count > 0)
                {
                    return RowOfProgramCategory(dt.Rows[0]);
                }
                else
                    return null;
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }

        public List<ProgramCategoryModel> SaveProgramCategory(ProgramCategoryModel ProgramCategory)
        {
            try
            {
                SqlParameter[] param = new SqlParameter[5];
                param[0] = new SqlParameter("@PCategoryID", ProgramCategory.PCategoryID);
                param[1] = new SqlParameter("@PCategoryName", ProgramCategory.PCategoryName);
                param[2] = new SqlParameter("@PCategoryCode", ProgramCategory.PCategoryCode);
                param[3] = new SqlParameter("@PTypeID", ProgramCategory.PTypeID);

                param[4] = new SqlParameter("@CurUserID", ProgramCategory.CurUserID);
                SqlHelper.ExecuteNonQuery(SqlHelper.GetCon(), CommandType.StoredProcedure, "[AU_ProgramCategory]", param);
                return FetchProgramCategory();
            }
            catch (Exception ex)
            { throw new Exception(ex.Message); }
        }

        private List<ProgramCategoryModel> LoopinData(DataTable dt)
        {
            List<ProgramCategoryModel> ProgramCategoryL = new List<ProgramCategoryModel>();

            foreach (DataRow r in dt.Rows)
            {
                ProgramCategoryL.Add(RowOfProgramCategory(r));
            }
            return ProgramCategoryL;
        }

        public List<ProgramCategoryModel> FetchProgramCategory(ProgramCategoryModel mod)
        {
            try
            {
                DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_ProgramCategory", Common.GetParams(mod)).Tables[0];
                return LoopinData(dt);
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }

        public List<ProgramCategoryModel> FetchProgramCategory()
        {
            try
            {
                DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_ProgramCategory").Tables[0];
                return LoopinData(dt);
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }

        public List<ProgramCategoryModel> FetchProgramCategory(bool InActive)
        {
            try
            {
                DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_ProgramCategory", new SqlParameter("@InActive", InActive)).Tables[0];
                return LoopinData(dt);
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }

        public int BatchInsert(List<ProgramCategoryModel> ls, int @BatchFkey, int CurUserID)
        {
            SqlParameter[] param = new SqlParameter[3];

            param[0] = new SqlParameter("@Json", JsonConvert.SerializeObject(ls));
            param[1] = new SqlParameter("@", @BatchFkey);
            param[2] = new SqlParameter("@CurUserID", CurUserID);
            return SqlHelper.ExecuteNonQuery(SqlHelper.GetCon(), CommandType.StoredProcedure, "[BAU_ProgramCategory]", param);
        }

        public List<ProgramCategoryModel> GetByPTypeID(int PTypeID)
        {
            try
            {
                DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_ProgramCategory", new SqlParameter("@PTypeID", PTypeID)).Tables[0];
                return LoopinData(dt);
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }

        public void ActiveInActive(int PCategoryID, bool? InActive, int CurUserID)
        {
            SqlParameter[] PLead = new SqlParameter[3];
            PLead[0] = new SqlParameter("@PCategoryID", PCategoryID);
            PLead[1] = new SqlParameter("@InActive", InActive);
            PLead[2] = new SqlParameter("@CurUserID", CurUserID);
            SqlHelper.ExecuteNonQuery(SqlHelper.GetCon(), CommandType.StoredProcedure, "[AI_ProgramCategory]", PLead);
        }

        private ProgramCategoryModel RowOfProgramCategory(DataRow r)
        {
            ProgramCategoryModel ProgramCategory = new ProgramCategoryModel();
            ProgramCategory.PCategoryID = Convert.ToInt32(r["PCategoryID"]);
            ProgramCategory.PCategoryName = r["PCategoryName"].ToString();
            ProgramCategory.PCategoryCode = r["PCategoryCode"].ToString();
            ProgramCategory.PTypeID = Convert.ToInt32(r["PTypeID"]);
            ProgramCategory.PTypeName = r["PTypeName"].ToString();
            ProgramCategory.InActive = Convert.ToBoolean(r["InActive"]);
            ProgramCategory.CreatedUserID = Convert.ToInt32(r["CreatedUserID"]);
            ProgramCategory.ModifiedUserID = Convert.ToInt32(r["ModifiedUserID"]);
            ProgramCategory.CreatedDate = r["CreatedDate"].ToString().GetDate();
            ProgramCategory.ModifiedDate = r["ModifiedDate"].ToString().GetDate();

            return ProgramCategory;
        }
    }
}