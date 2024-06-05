using DataLayer.Classes;
using DataLayer.Models;
using Microsoft.Data.SqlClient;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;

namespace DataLayer.Services
{
    public class SRVOrganization : SRVBase, DataLayer.Interfaces.ISRVOrganization
    {
        public SRVOrganization()
        {
        }

        public OrganizationModel GetByOID(int OID)
        {
            try
            {
                SqlParameter param = new SqlParameter("@OID", OID);
                DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_Organization", param).Tables[0];
                if (dt.Rows.Count > 0)
                {
                    return RowOfOrganization(dt.Rows[0]);
                }
                else
                    return null;
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }

        public List<OrganizationModel> SaveOrganization(OrganizationModel Organization)
        {
            try
            {
                SqlParameter[] param = new SqlParameter[3];
                param[0] = new SqlParameter("@OID", Organization.OID);
                param[1] = new SqlParameter("@OName", Organization.OName);

                param[2] = new SqlParameter("@CurUserID", Organization.CurUserID);
                SqlHelper.ExecuteNonQuery(SqlHelper.GetCon(), CommandType.StoredProcedure, "[AU_Organization]", param);
                return FetchOrganization();
            }
            catch (Exception ex)
            { throw new Exception(ex.Message); }
        }

        private List<OrganizationModel> LoopinData(DataTable dt)
        {
            List<OrganizationModel> OrganizationL = new List<OrganizationModel>();

            foreach (DataRow r in dt.Rows)
            {
                OrganizationL.Add(RowOfOrganization(r));
            }
            return OrganizationL;
        }

        public List<OrganizationModel> FetchOrganization(OrganizationModel mod)
        {
            try
            {
                DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_Organization", Common.GetParams(mod)).Tables[0];
                return LoopinData(dt);
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }

        public List<OrganizationModel> FetchOrganization()
        {
            try
            {
                DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_Organization").Tables[0];
                return LoopinData(dt);
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }

        public List<OrganizationModel> FetchOrganization(bool InActive)
        {
            try
            {
                DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_Organization", new SqlParameter("@InActive", InActive)).Tables[0];
                return LoopinData(dt);
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }

        public List<OrganizationModel> FetchOrganizationByUser(int UserID, int OID)
        {
            try
            {
                List<SqlParameter> param = new List<SqlParameter>();
                param.Add(new SqlParameter("@UserID", UserID));
                param.Add(new SqlParameter("@OID", OID));
                DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_OrganizationByUserID", param.ToArray()).Tables[0];
                return LoopinData(dt);
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }

        public int BatchInsert(List<OrganizationModel> ls, int @BatchFkey, int CurUserID)
        {
            SqlParameter[] param = new SqlParameter[3];

            param[0] = new SqlParameter("@Json", JsonConvert.SerializeObject(ls));
            param[1] = new SqlParameter("@", @BatchFkey);
            param[2] = new SqlParameter("@CurUserID", CurUserID);
            return SqlHelper.ExecuteNonQuery(SqlHelper.GetCon(), CommandType.StoredProcedure, "[BAU_Organization]", param);
        }

        public void ActiveInActive(int OID, bool? InActive, int CurUserID)
        {
            SqlParameter[] PLead = new SqlParameter[3];
            PLead[0] = new SqlParameter("@OID", OID);
            PLead[1] = new SqlParameter("@InActive", InActive);
            PLead[2] = new SqlParameter("@CurUserID", CurUserID);
            SqlHelper.ExecuteNonQuery(SqlHelper.GetCon(), CommandType.StoredProcedure, "[AI_Organization]", PLead);
        }

        private OrganizationModel RowOfOrganization(DataRow r)
        {
            OrganizationModel Organization = new OrganizationModel();
            Organization.OID = Convert.ToInt32(r["OID"]);
            Organization.OName = r["OName"].ToString();
            Organization.InActive = Convert.ToBoolean(r["InActive"]);
            Organization.CreatedUserID = Convert.ToInt32(r["CreatedUserID"]);
            Organization.ModifiedUserID = Convert.ToInt32(r["ModifiedUserID"]);
            Organization.CreatedDate = r["CreatedDate"].ToString().GetDate();
            Organization.ModifiedDate = r["ModifiedDate"].ToString().GetDate();

            return Organization;
        }
    }
}