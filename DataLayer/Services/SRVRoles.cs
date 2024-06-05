using DataLayer.Classes;
using DataLayer.Interfaces;
using DataLayer.Models;
using Microsoft.Data.SqlClient;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace DataLayer.Services
{
    public class SRVRoles : SRVBase, DataLayer.Interfaces.ISRVRoles
    {
        private readonly ISRVUsers srvUsers;
        public SRVRoles(ISRVUsers srvUsers)
        {
            this.srvUsers = srvUsers;
        }

        public RolesModel GetByRoleID(int RoleID)
        {
            try
            {
                SqlParameter param = new SqlParameter("@RoleID", RoleID);
                DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_Roles", param).Tables[0];
                if (dt.Rows.Count > 0)
                {
                    return RowOfRoles(dt.Rows[0]);
                }
                else
                    return null;
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }

        public List<RolesModel> SaveRoles(RolesModel Roles)
        {
            try
            {
                SqlParameter[] param = new SqlParameter[4];
                param[0] = new SqlParameter("@RoleID", Roles.RoleID);
                param[1] = new SqlParameter("@RoleTitle", Roles.RoleTitle);
                param[2] = new SqlParameter("@CurUserID", Roles.CurUserID);
                param[3] = new SqlParameter("@Ident", SqlDbType.Int)
                {
                    Direction = ParameterDirection.Output
                };
                SqlHelper.ExecuteNonQuery(SqlHelper.GetCon(), CommandType.StoredProcedure, "[AU_Roles]", param);
                new SRVRolesRights().BatchInsert(Roles.RoleRights, Convert.ToInt32(param[3].Value), Roles.CurUserID);
                if (Roles.RoleID>0 && Roles.RoleID!=null) {
                    insertRightsInUserRights(Roles);
                }
                
                return FetchRoles();
            }
            catch (Exception ex)
            { throw new Exception(ex.Message); }
        }
        public void insertRightsInUserRights(RolesModel Roles) {
            //Nasrullah code
            List<UsersModel> UserListRolewise = srvUsers.GetByRoleID(Roles.RoleID);
            string UsersIds = (string.Join(",", UserListRolewise.Select(x => x.UserID.ToString())));
            string[] UserIDs = UsersIds.Split(',').Select(x => x.Trim()).Where(x => !string.IsNullOrWhiteSpace(x)).ToArray();
            string UsersString = string.Join(",", UserIDs);
            string FormIds = (string.Join(",", Roles.DiffRoleRights.Select(x => x.FormID.ToString())));
            string[] FormIDs = FormIds.Split(',').Select(x => x.Trim()).Where(x => !string.IsNullOrWhiteSpace(x)).ToArray();
            string FormString = string.Join(",", FormIDs);

            SqlParameter[] param = new SqlParameter[4];
            param[0] = new SqlParameter("@Rightsjson", JsonConvert.SerializeObject(Roles.DiffRoleRights));
            param[1] = new SqlParameter("@UserIDs", UsersString);
            param[2] = new SqlParameter("@FormIDs", FormString);
            param[3] = new SqlParameter("@CurUserID", Roles.CurUserID);
            SqlHelper.ExecuteNonQuery(SqlHelper.GetCon(), CommandType.StoredProcedure, "[AU_UsersRights_RoleRights]", param);
        }
        private List<RolesModel> LoopinData(DataTable dt)
        {
            List<RolesModel> RolesL = new List<RolesModel>();

            foreach (DataRow r in dt.Rows)
            {
                RolesL.Add(RowOfRoles(r));
            }
            return RolesL;
        }

        public List<RolesModel> FetchRoles(RolesModel mod)
        {
            try
            {
                DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_Roles", Common.GetParams(mod)).Tables[0];
                return LoopinData(dt);
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }

        public List<RolesModel> FetchRoles()
        {
            try
            {
                DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_Roles").Tables[0];
                return LoopinData(dt);
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }

        public List<RolesModel> FetchRoles(bool InActive)
        {
            try
            {
                DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_Roles", new SqlParameter("@InActive", InActive)).Tables[0];
                return LoopinData(dt);
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }

        public int BatchInsert(List<RolesModel> ls, int @BatchFkey, int CurUserID)
        {
            SqlParameter[] param = new SqlParameter[3];

            param[0] = new SqlParameter("@Json", JsonConvert.SerializeObject(ls));
            param[1] = new SqlParameter("@", @BatchFkey);
            param[2] = new SqlParameter("@CurUserID", CurUserID);
            return SqlHelper.ExecuteNonQuery(SqlHelper.GetCon(), CommandType.StoredProcedure, "[BAU_Roles]", param);
        }

        public void ActiveInActive(int RoleID, bool? InActive, int CurUserID)
        {
            SqlParameter[] PLead = new SqlParameter[3];
            PLead[0] = new SqlParameter("@RoleID", RoleID);
            PLead[1] = new SqlParameter("@InActive", InActive);
            PLead[2] = new SqlParameter("@CurUserID", CurUserID);
            SqlHelper.ExecuteNonQuery(SqlHelper.GetCon(), CommandType.StoredProcedure, "[AI_Roles]", PLead);
        }

        private RolesModel RowOfRoles(DataRow r)
        {
            RolesModel Roles = new RolesModel();
            Roles.RoleID = Convert.ToInt32(r["RoleID"]);
            Roles.RoleTitle = r["RoleTitle"].ToString();
            Roles.InActive = Convert.ToBoolean(r["InActive"]);
            Roles.CreatedUserID = Convert.ToInt32(r["CreatedUserID"]);
            Roles.ModifiedUserID = Convert.ToInt32(r["ModifiedUserID"]);
            Roles.CreatedDate = r["CreatedDate"].ToString().GetDate();
            Roles.ModifiedDate = r["ModifiedDate"].ToString().GetDate();

            return Roles;
        }
    }
}