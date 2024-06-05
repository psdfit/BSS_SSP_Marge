using DataLayer.Classes;
using DataLayer.Models;
using Microsoft.Data.SqlClient;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;

namespace DataLayer.Services
{
    public class SRVRolesRights : SRVBase, DataLayer.Interfaces.ISRVRolesRights
    {
        public SRVRolesRights()
        {
        }

        public RolesRightsModel GetByRoleRightID(int RoleRightID)
        {
            try
            {
                SqlParameter param = new SqlParameter("@RoleRightID", RoleRightID);
                DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_RolesRights", param).Tables[0];
                if (dt.Rows.Count > 0)
                {
                    return RowOfRolesRights(dt.Rows[0]);
                }
                else
                    return null;
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }

        public List<RolesRightsModel> SaveRolesRights(RolesRightsModel RolesRights)
        {
            try
            {
                SqlParameter[] param = new SqlParameter[8];
                param[0] = new SqlParameter("@RoleRightID", RolesRights.RoleRightID);
                param[1] = new SqlParameter("@FormID", RolesRights.FormID);
                param[2] = new SqlParameter("@RoleID", RolesRights.RoleID);
                param[3] = new SqlParameter("@CanAdd", RolesRights.CanAdd);
                param[4] = new SqlParameter("@CanEdit", RolesRights.CanEdit);
                param[5] = new SqlParameter("@CanDelete", RolesRights.CanDelete);
                param[6] = new SqlParameter("@CanView", RolesRights.CanView);

                param[7] = new SqlParameter("@CurUserID", RolesRights.CurUserID);
                SqlHelper.ExecuteNonQuery(SqlHelper.GetCon(), CommandType.StoredProcedure, "[AU_RolesRights]", param);
                return FetchRolesRights();
            }
            catch (Exception ex)
            { throw new Exception(ex.Message); }
        }

        private List<RolesRightsModel> LoopinData(DataTable dt)
        {
            List<RolesRightsModel> RolesRightsL = new List<RolesRightsModel>();

            foreach (DataRow r in dt.Rows)
            {
                RolesRightsL.Add(RowOfRolesRights(r));
            }
            return RolesRightsL;
        }

        public List<RolesRightsModel> FetchRolesRights(RolesRightsModel mod)
        {
            try
            {
                DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_RolesRights", Common.GetParams(mod)).Tables[0];
                return LoopinData(dt);
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }

        public List<RolesRightsModel> FetchRolesRights()
        {
            try
            {
                DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_RolesRights").Tables[0];
                return LoopinData(dt);
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }

        public List<RolesRightsModel> FetchRolesRights(bool InActive)
        {
            try
            {
                DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_RolesRights", new SqlParameter("@InActive", InActive)).Tables[0];
                return LoopinData(dt);
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }

        public int BatchInsert(List<RolesRightsModel> ls, int @BatchFkey, int CurUserID)
        {
            SqlParameter[] param = new SqlParameter[3];

            param[0] = new SqlParameter("@Json", JsonConvert.SerializeObject(ls));
            param[1] = new SqlParameter("@RoleID", @BatchFkey);
            param[2] = new SqlParameter("@CurUserID", CurUserID);
            return SqlHelper.ExecuteNonQuery(SqlHelper.GetCon(), CommandType.StoredProcedure, "[BAU_RolesRights]", param);
        }

        public List<RolesRightsModel> GetByFormID(int FormID)
        {
            try
            {
                DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_RolesRights", new SqlParameter("@FormID", FormID)).Tables[0];
                return LoopinData(dt);
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }

        public List<RolesRightsModel> GetByRoleID(int RoleID)
        {
            try
            {
                DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "GetRolesRightsByRole", new SqlParameter("@RoleID", RoleID)).Tables[0];
                return LoopinData(dt);
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }

        public List<UsersRightsModel> GetByRoleIDForDefaultTSP(int RoleID)
        {
            try
            {
                DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "GetRolesRightsByRole", new SqlParameter("@RoleID", RoleID)).Tables[0];
                return  Helper.ConvertDataTableToModel<UsersRightsModel>(dt);
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }

        public void ActiveInActive(int RoleRightID, bool? InActive, int CurUserID)
        {
            SqlParameter[] PLead = new SqlParameter[3];
            PLead[0] = new SqlParameter("@RoleRightID", RoleRightID);
            PLead[1] = new SqlParameter("@InActive", InActive);
            PLead[2] = new SqlParameter("@CurUserID", CurUserID);
            SqlHelper.ExecuteNonQuery(SqlHelper.GetCon(), CommandType.StoredProcedure, "[AI_RolesRights]", PLead);
        }

        private RolesRightsModel RowOfRolesRights(DataRow r)
        {
            RolesRightsModel RolesRights = new RolesRightsModel();
            RolesRights.RoleRightID = Convert.ToInt32(r["RoleRightID"]);
            RolesRights.FormID = Convert.ToInt32(r["FormID"]);
            RolesRights.FormName = r["FormName"].ToString();
            RolesRights.RoleID = Convert.ToInt32(r["RoleID"]);
            RolesRights.RoleTitle = r["RoleTitle"].ToString();
            RolesRights.ModuleTitle = r["ModuleTitle"].ToString();
            RolesRights.CanAdd = Convert.ToBoolean(r["CanAdd"]);
            RolesRights.CanEdit = Convert.ToBoolean(r["CanEdit"]);
            RolesRights.CanDelete = Convert.ToBoolean(r["CanDelete"]);
            RolesRights.CanView = Convert.ToBoolean(r["CanView"]);
            RolesRights.InActive = Convert.ToBoolean(r["InActive"]);
            RolesRights.CreatedUserID = Convert.ToInt32(r["CreatedUserID"]);
            RolesRights.ModifiedUserID = Convert.ToInt32(r["ModifiedUserID"]);
            RolesRights.CreatedDate = r["CreatedDate"].ToString().GetDate();
            RolesRights.ModifiedDate = r["ModifiedDate"].ToString().GetDate();
            RolesRights.IsAddible = Convert.ToBoolean(r["IsAddible"]);
            RolesRights.IsEditable = Convert.ToBoolean(r["IsEditable"]);
            RolesRights.IsDeletable = Convert.ToBoolean(r["IsDeletable"]);
            RolesRights.IsViewable = Convert.ToBoolean(r["IsViewable"]);
            return RolesRights;
        }
    }
}