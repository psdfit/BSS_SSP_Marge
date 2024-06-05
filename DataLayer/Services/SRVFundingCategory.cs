using DataLayer.Classes;
using DataLayer.Models;
using Microsoft.Data.SqlClient;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;

namespace DataLayer.Services
{
    public class SRVFundingCategory : SRVBase, DataLayer.Interfaces.ISRVFundingCategory
    {
        public SRVFundingCategory()
        {
        }

        public FundingCategoryModel GetByFundingCategoryID(int FundingCategoryID)
        {
            try
            {
                SqlParameter param = new SqlParameter("@FundingCategoryID", FundingCategoryID);
                DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_FundingCategory", param).Tables[0];
                if (dt.Rows.Count > 0)
                {
                    return RowOfFundingCategory(dt.Rows[0]);
                }
                else
                    return null;
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }

        public List<FundingCategoryModel> SaveFundingCategory(FundingCategoryModel FundingCategory)
        {
            try
            {
                SqlParameter[] param = new SqlParameter[4];
                param[0] = new SqlParameter("@FundingCategoryID", FundingCategory.FundingCategoryID);
                param[1] = new SqlParameter("@FundingCategoryName", FundingCategory.FundingCategoryName);
                param[2] = new SqlParameter("@FundingSourceID", FundingCategory.FundingSourceID);

                param[3] = new SqlParameter("@CurUserID", FundingCategory.CurUserID);
                SqlHelper.ExecuteNonQuery(SqlHelper.GetCon(), CommandType.StoredProcedure, "[AU_FundingCategory]", param);
                return FetchFundingCategory();
            }
            catch (Exception ex)
            { throw new Exception(ex.Message); }
        }

        private List<FundingCategoryModel> LoopinData(DataTable dt)
        {
            List<FundingCategoryModel> FundingCategoryL = new List<FundingCategoryModel>();

            foreach (DataRow r in dt.Rows)
            {
                FundingCategoryL.Add(RowOfFundingCategory(r));
            }
            return FundingCategoryL;
        }

        public List<FundingCategoryModel> FetchFundingCategory(FundingCategoryModel mod)
        {
            try
            {
                DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_FundingCategory", Common.GetParams(mod)).Tables[0];
                return LoopinData(dt);
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }

        public List<FundingCategoryModel> FetchFundingCategory()
        {
            try
            {
                DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_FundingCategory").Tables[0];
                return LoopinData(dt);
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }

        public List<FundingCategoryModel> FetchFundingCategory(bool InActive)
        {
            try
            {
                DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_FundingCategory", new SqlParameter("@InActive", InActive)).Tables[0];
                return LoopinData(dt);
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }

        public int BatchInsert(List<FundingCategoryModel> ls, int @BatchFkey, int CurUserID)
        {
            SqlParameter[] param = new SqlParameter[3];

            param[0] = new SqlParameter("@Json", JsonConvert.SerializeObject(ls));
            param[1] = new SqlParameter("@", @BatchFkey);
            param[2] = new SqlParameter("@CurUserID", CurUserID);
            return SqlHelper.ExecuteNonQuery(SqlHelper.GetCon(), CommandType.StoredProcedure, "[BAU_FundingCategory]", param);
        }

        public List<FundingCategoryModel> GetByFundingSourceID(int FundingSourceID)
        {
            try
            {
                DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_FundingCategory", new SqlParameter("@FundingSourceID", FundingSourceID)).Tables[0];
                return LoopinData(dt);
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }

        public void ActiveInActive(int FundingCategoryID, bool? InActive, int CurUserID)
        {
            SqlParameter[] PLead = new SqlParameter[3];
            PLead[0] = new SqlParameter("@FundingCategoryID", FundingCategoryID);
            PLead[1] = new SqlParameter("@InActive", InActive);
            PLead[2] = new SqlParameter("@CurUserID", CurUserID);
            SqlHelper.ExecuteNonQuery(SqlHelper.GetCon(), CommandType.StoredProcedure, "[AI_FundingCategory]", PLead);
        }

        private FundingCategoryModel RowOfFundingCategory(DataRow r)
        {
            FundingCategoryModel FundingCategory = new FundingCategoryModel();
            FundingCategory.FundingCategoryID = Convert.ToInt32(r["FundingCategoryID"]);
            FundingCategory.FundingCategoryName = r["FundingCategoryName"].ToString();
            FundingCategory.FundingSourceID = Convert.ToInt32(r["FundingSourceID"]);
            FundingCategory.FundingSourceName = r["FundingSourceName"].ToString();
            FundingCategory.InActive = Convert.ToBoolean(r["InActive"]);
            FundingCategory.CreatedUserID = Convert.ToInt32(r["CreatedUserID"]);
            FundingCategory.ModifiedUserID = Convert.ToInt32(r["ModifiedUserID"]);
            FundingCategory.CreatedDate = r["CreatedDate"].ToString().GetDate();
            FundingCategory.ModifiedDate = r["ModifiedDate"].ToString().GetDate();

            return FundingCategory;
        }
    }
}