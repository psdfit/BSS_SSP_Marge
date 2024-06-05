using DataLayer.Classes;
using DataLayer.Models;
using Microsoft.Data.SqlClient;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;

namespace DataLayer.Services
{
    public class SRVModules : SRVBase, DataLayer.Interfaces.ISRVModules
    {
        public SRVModules()
        {
        }

        public ModulesModel GetByModuleID(int ModuleID)
        {
            try
            {
                SqlParameter param = new SqlParameter("@ModuleID", ModuleID);
                DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_Modules", param).Tables[0];
                if (dt.Rows.Count > 0)
                {
                    return RowOfModules(dt.Rows[0]);
                }
                else
                    return null;
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }

        public List<ModulesModel> SaveModules(ModulesModel Modules)
        {
            try
            {
                SqlParameter[] param = new SqlParameter[5];
                param[0] = new SqlParameter("@ModuleID", Modules.ModuleID);
                param[1] = new SqlParameter("@ModuleTitle", Modules.ModuleTitle);
                param[2] = new SqlParameter("@modpath", Modules.modpath);
                param[3] = new SqlParameter("@SortOrder", Modules.SortOrder);

                param[4] = new SqlParameter("@CurUserID", Modules.CurUserID);
                SqlHelper.ExecuteNonQuery(SqlHelper.GetCon(), CommandType.StoredProcedure, "[AU_Modules]", param);
                return FetchModules();
            }
            catch (Exception ex)
            { throw new Exception(ex.Message); }
        }

        private List<ModulesModel> LoopinData(DataTable dt)
        {
            List<ModulesModel> ModulesL = new List<ModulesModel>();

            foreach (DataRow r in dt.Rows)
            {
                ModulesL.Add(RowOfModules(r));
            }
            return ModulesL;
        }

        public List<ModulesModel> FetchModules(ModulesModel mod)
        {
            try
            {
                DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_Modules", Common.GetParams(mod)).Tables[0];
                return LoopinData(dt);
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }

        public List<ModulesModel> FetchModules()
        {
            try
            {
                DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_Modules").Tables[0];
                return LoopinData(dt);
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }

        public List<ModulesModel> FetchModules(bool InActive)
        {
            try
            {
                DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_Modules", new SqlParameter("@InActive", InActive)).Tables[0];
                return LoopinData(dt);
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }

        public int BatchInsert(List<ModulesModel> ls, int @BatchFkey, int CurUserID)
        {
            SqlParameter[] param = new SqlParameter[3];

            param[0] = new SqlParameter("@Json", JsonConvert.SerializeObject(ls));
            param[1] = new SqlParameter("@", @BatchFkey);
            param[2] = new SqlParameter("@CurUserID", CurUserID);
            return SqlHelper.ExecuteNonQuery(SqlHelper.GetCon(), CommandType.StoredProcedure, "[BAU_Modules]", param);
        }

        public void ActiveInActive(int ModuleID, bool? InActive, int CurUserID)
        {
            SqlParameter[] PLead = new SqlParameter[3];
            PLead[0] = new SqlParameter("@ModuleID", ModuleID);
            PLead[1] = new SqlParameter("@InActive", InActive);
            PLead[2] = new SqlParameter("@CurUserID", CurUserID);
            SqlHelper.ExecuteNonQuery(SqlHelper.GetCon(), CommandType.StoredProcedure, "[AI_Modules]", PLead);
        }

        private ModulesModel RowOfModules(DataRow r)
        {
            ModulesModel Modules = new ModulesModel();
            Modules.ModuleID = Convert.ToInt32(r["ModuleID"]);
            Modules.ModuleTitle = r["ModuleTitle"].ToString();
            Modules.modpath = r["modpath"].ToString();
            Modules.SortOrder = Convert.ToInt32(r["SortOrder"]);
            Modules.InActive = Convert.ToBoolean(r["InActive"]);
            Modules.CreatedUserID = Convert.ToInt32(r["CreatedUserID"]);
            Modules.ModifiedUserID = Convert.ToInt32(r["ModifiedUserID"]);
            Modules.CreatedDate = r["CreatedDate"].ToString().GetDate();
            Modules.ModifiedDate = r["ModifiedDate"].ToString().GetDate();

            return Modules;
        }
    }
}