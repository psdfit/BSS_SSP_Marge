using DataLayer.Classes;
using DataLayer.Models;
using Microsoft.Data.SqlClient;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;

namespace DataLayer.Services
{
    public class SRVAppForms : SRVBase, DataLayer.Interfaces.ISRVAppForms
    {
        public SRVAppForms()
        {
        }

        public AppFormsModel GetByFormID(int FormID)
        {
            try
            {
                SqlParameter param = new SqlParameter("@FormID", FormID);
                DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_AppForms", param).Tables[0];
                if (dt.Rows.Count > 0)
                {
                    return RowOfAppForms(dt.Rows[0]);
                }
                else
                    return null;
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }

        public List<AppFormsModel> SaveAppForms(AppFormsModel AppForms)
        {
            try
            {
                SqlParameter[] param = new SqlParameter[8];
                param[0] = new SqlParameter("@FormID", AppForms.FormID);
                param[1] = new SqlParameter("@FormName", AppForms.FormName);
                param[2] = new SqlParameter("@Path", AppForms.Path);
                param[3] = new SqlParameter("@Icon", AppForms.Icon);
                param[4] = new SqlParameter("@Controller", AppForms.Controller);
                param[5] = new SqlParameter("@ModuleID", AppForms.ModuleID);
                param[6] = new SqlParameter("@Sortorder", AppForms.Sortorder);

                param[7] = new SqlParameter("@CurUserID", AppForms.CurUserID);
                SqlHelper.ExecuteNonQuery(SqlHelper.GetCon(), CommandType.StoredProcedure, "[AU_AppForms]", param);
                return FetchAppForms();
            }
            catch (Exception ex)
            { throw new Exception(ex.Message); }
        }

        private List<AppFormsModel> LoopinData(DataTable dt)
        {
            List<AppFormsModel> AppFormsL = new List<AppFormsModel>();

            foreach (DataRow r in dt.Rows)
            {
                AppFormsL.Add(RowOfAppForms(r));
            }
            return AppFormsL;
        }

        public List<AppFormsModel> FetchAppForms(AppFormsModel mod)
        {
            try
            {
                DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_AppForms", Common.GetParams(mod)).Tables[0];
                return LoopinData(dt);
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }

        public List<AppFormsModel> FetchAppForms()
        {
            try
            {
                DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_AppForms").Tables[0];
                return LoopinData(dt);
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }

        public List<AppFormsModel> FetchAppForms(bool InActive)
        {
            try
            {
                DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_AppForms", new SqlParameter("@InActive", InActive)).Tables[0];
                return  Helper.ConvertDataTableToModel<AppFormsModel>(dt);
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }

        public int BatchInsert(List<AppFormsModel> ls, int @BatchFkey, int CurUserID)
        {
            SqlParameter[] param = new SqlParameter[3];

            param[0] = new SqlParameter("@Json", JsonConvert.SerializeObject(ls));
            param[1] = new SqlParameter("@", @BatchFkey);
            param[2] = new SqlParameter("@CurUserID", CurUserID);
            return SqlHelper.ExecuteNonQuery(SqlHelper.GetCon(), CommandType.StoredProcedure, "[BAU_AppForms]", param);
        }

        public List<AppFormsModel> GetByModuleID(int ModuleID)
        {
            try
            {
                DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_AppForms", new SqlParameter("@ModuleID", ModuleID)).Tables[0];
                return LoopinData(dt);
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }

        public void ActiveInActive(int FormID, bool? InActive, int CurUserID)
        {
            SqlParameter[] PLead = new SqlParameter[3];
            PLead[0] = new SqlParameter("@FormID", FormID);
            PLead[1] = new SqlParameter("@InActive", InActive);
            PLead[2] = new SqlParameter("@CurUserID", CurUserID);
            SqlHelper.ExecuteNonQuery(SqlHelper.GetCon(), CommandType.StoredProcedure, "[AI_AppForms]", PLead);
        }

        private AppFormsModel RowOfAppForms(DataRow r)
        {
            AppFormsModel AppForms = new AppFormsModel
            {
                FormID = Convert.ToInt32(r["FormID"]),
                FormName = r["FormName"].ToString(),
                Path = r["Path"].ToString(),
                Icon = r["Icon"].ToString(),
                Controller = r["Controller"].ToString(),
                ModuleID = Convert.ToInt32(r["ModuleID"]),
                ModuleTitle = r["ModuleTitle"].ToString(),
                Sortorder = Convert.ToInt32(r["Sortorder"]),
                InActive = Convert.ToBoolean(r["InActive"]),
                CreatedUserID = Convert.ToInt32(r["CreatedUserID"]),
                ModifiedUserID = Convert.ToInt32(r["ModifiedUserID"]),
                CreatedDate = r["CreatedDate"].ToString().GetDate(),
                ModifiedDate = r["ModifiedDate"].ToString().GetDate()
            };

            return AppForms;
        }
    }
}