using DataLayer.Classes;
using DataLayer.Models;
using Microsoft.Data.SqlClient;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;

namespace DataLayer.Services
{
    public class SRVUserOrganizations : SRVBase, DataLayer.Interfaces.ISRVUserOrganizations
    {
        public SRVUserOrganizations()
        {
        }

        public UserOrganizationsModel GetBySrno(int Srno)
        {
            try
            {
                SqlParameter param = new SqlParameter("@Srno", Srno);
                DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_UserOrganizations", param).Tables[0];
                if (dt.Rows.Count > 0)
                {
                    return RowOfUserOrganizations(dt.Rows[0]);
                }
                else
                    return null;
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }

        public List<UserOrganizationsModel> SaveUserOrganizations(UserOrganizationsModel UserOrganizations)
        {
            try
            {
                SqlParameter[] param = new SqlParameter[4];
                param[0] = new SqlParameter("@Srno", UserOrganizations.Srno);
                param[1] = new SqlParameter("@UserID", UserOrganizations.UserID);
                param[2] = new SqlParameter("@OID", UserOrganizations.OID);

                param[3] = new SqlParameter("@CurUserID", UserOrganizations.CurUserID);
                SqlHelper.ExecuteNonQuery(SqlHelper.GetCon(), CommandType.StoredProcedure, "[AU_UserOrganizations]", param);
                return FetchUserOrganizations();
            }
            catch (Exception ex)
            { throw new Exception(ex.Message); }
        }

        private List<UserOrganizationsModel> LoopinData(DataTable dt)
        {
            List<UserOrganizationsModel> UserOrganizationsL = new List<UserOrganizationsModel>();

            foreach (DataRow r in dt.Rows)
            {
                UserOrganizationsL.Add(RowOfUserOrganizations(r));
            }
            return UserOrganizationsL;
        }

        public List<UserOrganizationsModel> FetchUserOrganizations(UserOrganizationsModel mod)
        {
            try
            {
                DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_UserOrganizations", Common.GetParams(mod)).Tables[0];
                return LoopinData(dt);
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }

        public List<UserOrganizationsModel> FetchUserOrganizations()
        {
            try
            {
                DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_UserOrganizations").Tables[0];
                return LoopinData(dt);
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }

        public List<UserOrganizationsModel> FetchUserOrganizations(bool InActive)
        {
            try
            {
                DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_UserOrganizations", new SqlParameter("@InActive", InActive)).Tables[0];
                return LoopinData(dt);
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }

        public int BatchInsert(List<UserOrganizationsModel> ls, int @BatchFkey, int CurUserID)
        {
            SqlParameter[] param = new SqlParameter[3];

            param[0] = new SqlParameter("@Json", JsonConvert.SerializeObject(ls));
            param[1] = new SqlParameter("@UserID", @BatchFkey);
            param[2] = new SqlParameter("@CurUserID", CurUserID);
            return SqlHelper.ExecuteNonQuery(SqlHelper.GetCon(), CommandType.StoredProcedure, "[BAU_UserOrganizations]", param);
        }

        public List<UserOrganizationsModel> GetByUserID(int UserID)
        {
            try
            {
                DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_UserOrganizations", new SqlParameter("@UserID", UserID)).Tables[0];
                return LoopinData(dt);
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }
             public List<UserOrganizationsModel> GetByUserIDForSSP(int UserID)
        {
            try
            {
                DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "[RD_SSPUserOrganizations]", new SqlParameter("@UserID", UserID)).Tables[0];
                return LoopinData(dt);
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }

        public List<UserOrganizationsModel> GetByOID(int OID)
        {
            try
            {
                DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_UserOrganizations", new SqlParameter("@OID", OID)).Tables[0];
                return LoopinData(dt);
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }

        public void ActiveInActive(int Srno, bool? InActive, int CurUserID)
        {
            SqlParameter[] PLead = new SqlParameter[3];
            PLead[0] = new SqlParameter("@Srno", Srno);
            PLead[1] = new SqlParameter("@InActive", InActive);
            PLead[2] = new SqlParameter("@CurUserID", CurUserID);
            SqlHelper.ExecuteNonQuery(SqlHelper.GetCon(), CommandType.StoredProcedure, "[AI_UserOrganizations]", PLead);
        }

        private UserOrganizationsModel RowOfUserOrganizations(DataRow r)
        {
            UserOrganizationsModel UserOrganizations = new UserOrganizationsModel();
            UserOrganizations.Srno = Convert.ToInt32(r["Srno"]);
            UserOrganizations.UserID = Convert.ToInt32(r["UserID"]);
            UserOrganizations.UserName = r["UserName"].ToString();
            UserOrganizations.OID = Convert.ToInt32(r["OID"]);
            UserOrganizations.OName = r["OName"].ToString();
            UserOrganizations.InActive = Convert.ToBoolean(r["InActive"]);
            UserOrganizations.CreatedUserID = Convert.ToInt32(r["CreatedUserID"]);
            UserOrganizations.ModifiedUserID = Convert.ToInt32(r["ModifiedUserID"]);
            UserOrganizations.CreatedDate = r["CreatedDate"].ToString().GetDate();
            UserOrganizations.ModifiedDate = r["ModifiedDate"].ToString().GetDate();

            return UserOrganizations;
        }
    }
}