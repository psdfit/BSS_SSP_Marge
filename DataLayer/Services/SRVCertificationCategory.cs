using DataLayer.Classes;
using DataLayer.Models;
using Microsoft.Data.SqlClient;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;

namespace DataLayer.Services
{
    public class SRVCertificationCategory : SRVBase, DataLayer.Interfaces.ISRVCertificationCategory
    {
        public SRVCertificationCategory()
        {
        }

        public CertificationCategoryModel GetByCertificationCategoryID(int CertificationCategoryID)
        {
            try
            {
                SqlParameter param = new SqlParameter("@CertificationCategoryID", CertificationCategoryID);
                DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_CertificationCategory", param).Tables[0];
                if (dt.Rows.Count > 0)
                {
                    return RowOfCertificationCategory(dt.Rows[0]);
                }
                else
                    return null;
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }

        public List<CertificationCategoryModel> SaveCertificationCategory(CertificationCategoryModel CertificationCategory)
        {
            try
            {
                SqlParameter[] param = new SqlParameter[3];
                param[0] = new SqlParameter("@CertificationCategoryID", CertificationCategory.CertificationCategoryID);
                param[1] = new SqlParameter("@CertificationCategoryName", CertificationCategory.CertificationCategoryName);

                param[2] = new SqlParameter("@CurUserID", CertificationCategory.CurUserID);
                SqlHelper.ExecuteNonQuery(SqlHelper.GetCon(), CommandType.StoredProcedure, "[AU_CertificationCategory]", param);
                return FetchCertificationCategory();
            }
            catch (Exception ex)
            { throw new Exception(ex.Message); }
        }

        private List<CertificationCategoryModel> LoopinData(DataTable dt)
        {
            List<CertificationCategoryModel> CertificationCategoryL = new List<CertificationCategoryModel>();

            foreach (DataRow r in dt.Rows)
            {
                CertificationCategoryL.Add(RowOfCertificationCategory(r));
            }
            return CertificationCategoryL;
        }

        public List<CertificationCategoryModel> FetchCertificationCategory(CertificationCategoryModel mod)
        {
            try
            {
                DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_CertificationCategory", Common.GetParams(mod)).Tables[0];
                return LoopinData(dt);
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }

        public List<CertificationCategoryModel> FetchCertificationCategory()
        {
            try
            {
                DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_CertificationCategory").Tables[0];
                return LoopinData(dt);
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }

        public List<CertificationCategoryModel> FetchCertificationCategory(bool InActive)
        {
            try
            {
                DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_CertificationCategory", new SqlParameter("@InActive", InActive)).Tables[0];
                return LoopinData(dt);
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }

        public int BatchInsert(List<CertificationCategoryModel> ls, int @BatchFkey, int CurUserID)
        {
            SqlParameter[] param = new SqlParameter[3];

            param[0] = new SqlParameter("@Json", JsonConvert.SerializeObject(ls));
            param[1] = new SqlParameter("@", @BatchFkey);
            param[2] = new SqlParameter("@CurUserID", CurUserID);
            return SqlHelper.ExecuteNonQuery(SqlHelper.GetCon(), CommandType.StoredProcedure, "[BAU_CertificationCategory]", param);
        }

        public void ActiveInActive(int CertificationCategoryID, bool? InActive, int CurUserID)
        {
            SqlParameter[] PLead = new SqlParameter[3];
            PLead[0] = new SqlParameter("@CertificationCategoryID", CertificationCategoryID);
            PLead[1] = new SqlParameter("@InActive", InActive);
            PLead[2] = new SqlParameter("@CurUserID", CurUserID);
            SqlHelper.ExecuteNonQuery(SqlHelper.GetCon(), CommandType.StoredProcedure, "[AI_CertificationCategory]", PLead);
        }

        private CertificationCategoryModel RowOfCertificationCategory(DataRow r)
        {
            CertificationCategoryModel CertificationCategory = new CertificationCategoryModel();
            CertificationCategory.CertificationCategoryID = Convert.ToInt32(r["CertificationCategoryID"]);
            CertificationCategory.CertificationCategoryName = r["CertificationCategoryName"].ToString();
            CertificationCategory.InActive = Convert.ToBoolean(r["InActive"]);
            CertificationCategory.CreatedUserID = Convert.ToInt32(r["CreatedUserID"]);
            CertificationCategory.ModifiedUserID = Convert.ToInt32(r["ModifiedUserID"]);
            CertificationCategory.CreatedDate = r["CreatedDate"].ToString().GetDate();
            CertificationCategory.ModifiedDate = r["ModifiedDate"].ToString().GetDate();

            return CertificationCategory;
        }
    }
}