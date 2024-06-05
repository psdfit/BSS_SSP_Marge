using System;
using System.Collections.Generic;
using System.Data;
using Microsoft.Data.SqlClient;
using DataLayer.Classes;
using DataLayer.Models;
using Newtonsoft.Json;
using DataLayer.Interfaces;
namespace DataLayer.Services
{
    public class SRVCertificationAuthority : SRVBase, ISRVCertificationAuthority
    {
        public SRVCertificationAuthority() { }
        public CertificationAuthorityModel GetByCertAuthID(int CertAuthID)
        {
            try
            {
                SqlParameter param = new SqlParameter("@CertAuthID", CertAuthID);
                DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_CertificationAuthority", param).Tables[0];
                if (dt.Rows.Count > 0)
                {
                    return RowOfCertificationAuthority(dt.Rows[0]);
                }
                else
                    return null;
            }
            catch (Exception ex) { throw new Exception(ex.Message); }

        }
        public List<CertificationAuthorityModel> SaveCertificationAuthority(CertificationAuthorityModel CertificationAuthority)
        {
            try
            {

                SqlParameter[] param = new SqlParameter[4];
                param[0] = new SqlParameter("@CertAuthID", CertificationAuthority.CertAuthID);
                param[1] = new SqlParameter("@CertAuthName", CertificationAuthority.CertAuthName);
                param[2] = new SqlParameter("@CertificationCategoryID", CertificationAuthority.CertificationCategoryID);

                param[3] = new SqlParameter("@CurUserID", CertificationAuthority.CurUserID);
                SqlHelper.ExecuteNonQuery(SqlHelper.GetCon(), CommandType.StoredProcedure, "[AU_CertificationAuthority]", param);
                return FetchCertificationAuthority();
            }
            catch (Exception ex)
            { throw new Exception(ex.Message); }
        }
        private List<CertificationAuthorityModel> LoopinData(DataTable dt)
        {
            List<CertificationAuthorityModel> CertificationAuthorityL = new List<CertificationAuthorityModel>();

            foreach (DataRow r in dt.Rows)
            {
                CertificationAuthorityL.Add(RowOfCertificationAuthority(r));

            }
            return CertificationAuthorityL;
        }
        public List<CertificationAuthorityModel> FetchCertificationAuthority(CertificationAuthorityModel mod)
        {
            try
            {
                DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_CertificationAuthority", Common.GetParams(mod)).Tables[0];
                return LoopinData(dt);
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }
        public List<CertificationAuthorityModel> FetchCertificationAuthority()
        {
            try
            {
                DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_CertificationAuthority").Tables[0];
                return LoopinData(dt);
            }
            catch (Exception ex) { throw new Exception(ex.Message); }

        }
        public List<CertificationAuthorityModel> FetchCertificationAuthority(bool InActive)
        {
            try
            {
                DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_CertificationAuthority", new SqlParameter("@InActive", InActive)).Tables[0];
                return LoopinData(dt);
            }
            catch (Exception ex) { throw new Exception(ex.Message); }

        }
        public void ActiveInActive(int CertAuthID, bool? InActive, int CurUserID)
        {

            SqlParameter[] PLead = new SqlParameter[3];
            PLead[0] = new SqlParameter("@CertAuthID", CertAuthID);
            PLead[1] = new SqlParameter("@InActive", InActive);
            PLead[2] = new SqlParameter("@CurUserID", CurUserID);
            SqlHelper.ExecuteNonQuery(SqlHelper.GetCon(), CommandType.StoredProcedure, "[AI_CertificationAuthority]", PLead);
        }
        private CertificationAuthorityModel RowOfCertificationAuthority(DataRow r)
        {
            CertificationAuthorityModel CertificationAuthority = new CertificationAuthorityModel();
            CertificationAuthority.CertAuthID = Convert.ToInt32(r["CertAuthID"]);
            CertificationAuthority.CertAuthName = r["CertAuthName"].ToString();
            CertificationAuthority.CertificationCategoryID = Convert.ToInt32(r["CertificationCategoryID"]);
            CertificationAuthority.CertificationCategoryName = r["CertificationCategoryName"].ToString();
            CertificationAuthority.InActive = Convert.ToBoolean(r["InActive"]);
            CertificationAuthority.CreatedUserID = Convert.ToInt32(r["CreatedUserID"]);
            CertificationAuthority.ModifiedUserID = Convert.ToInt32(r["ModifiedUserID"]);
            CertificationAuthority.CreatedDate = r["CreatedDate"].ToString().GetDate();
            CertificationAuthority.ModifiedDate = r["ModifiedDate"].ToString().GetDate();

            return CertificationAuthority;
        }
    }
}
