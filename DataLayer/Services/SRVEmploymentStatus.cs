using DataLayer.Classes;
using DataLayer.Models;
using Microsoft.Data.SqlClient;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;

namespace DataLayer.Services
{
    public class SRVEmploymentStatus : SRVBase, DataLayer.Interfaces.ISRVEmploymentStatus
    {
        public SRVEmploymentStatus()
        {
        }

        public EmploymentStatusModel GetByEmploymentStatusID(int EmploymentStatusID)
        {
            try
            {
                SqlParameter param = new SqlParameter("@EmploymentStatusID", EmploymentStatusID);
                DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_EmploymentStatus", param).Tables[0];
                if (dt.Rows.Count > 0)
                {
                    return RowOfEmploymentStatus(dt.Rows[0]);
                }
                else
                    return null;
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }

        public List<EmploymentStatusModel> SaveEmploymentStatus(EmploymentStatusModel EmploymentStatus)
        {
            try
            {
                SqlParameter[] param = new SqlParameter[3];
                param[0] = new SqlParameter("@EmploymentStatusID", EmploymentStatus.EmploymentStatusID);
                param[1] = new SqlParameter("@EmploymentStatusName", EmploymentStatus.EmploymentStatusName);

                param[2] = new SqlParameter("@CurUserID", EmploymentStatus.CurUserID);
                SqlHelper.ExecuteNonQuery(SqlHelper.GetCon(), CommandType.StoredProcedure, "[AU_EmploymentStatus]", param);
                return FetchEmploymentStatus();
            }
            catch (Exception ex)
            { throw new Exception(ex.Message); }
        }

        private List<EmploymentStatusModel> LoopinData(DataTable dt)
        {
            List<EmploymentStatusModel> EmploymentStatusL = new List<EmploymentStatusModel>();

            foreach (DataRow r in dt.Rows)
            {
                EmploymentStatusL.Add(RowOfEmploymentStatus(r));
            }
            return EmploymentStatusL;
        }

        public List<EmploymentStatusModel> FetchEmploymentStatus(EmploymentStatusModel mod)
        {
            try
            {
                DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_EmploymentStatus", Common.GetParams(mod)).Tables[0];
                return LoopinData(dt);
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }

        public List<EmploymentStatusModel> FetchEmploymentStatus()
        {
            try
            {
                DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_EmploymentStatus").Tables[0];
                return LoopinData(dt);
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }

        public List<EmploymentStatusModel> FetchEmploymentStatus(bool InActive)
        {
            try
            {
                DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_EmploymentStatus", new SqlParameter("@InActive", InActive)).Tables[0];
                return LoopinData(dt);
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }

        public int BatchInsert(List<EmploymentStatusModel> ls, int @BatchFkey, int CurUserID)
        {
            SqlParameter[] param = new SqlParameter[3];

            param[0] = new SqlParameter("@Json", JsonConvert.SerializeObject(ls));
            param[1] = new SqlParameter("@", @BatchFkey);
            param[2] = new SqlParameter("@CurUserID", CurUserID);
            return SqlHelper.ExecuteNonQuery(SqlHelper.GetCon(), CommandType.StoredProcedure, "[BAU_EmploymentStatus]", param);
        }

        public void ActiveInActive(int EmploymentStatusID, bool? InActive, int CurUserID)
        {
            SqlParameter[] PLead = new SqlParameter[3];
            PLead[0] = new SqlParameter("@EmploymentStatusID", EmploymentStatusID);
            PLead[1] = new SqlParameter("@InActive", InActive);
            PLead[2] = new SqlParameter("@CurUserID", CurUserID);
            SqlHelper.ExecuteNonQuery(SqlHelper.GetCon(), CommandType.StoredProcedure, "[AI_EmploymentStatus]", PLead);
        }

        private EmploymentStatusModel RowOfEmploymentStatus(DataRow r)
        {
            EmploymentStatusModel EmploymentStatus = new EmploymentStatusModel();
            EmploymentStatus.EmploymentStatusID = Convert.ToInt32(r["EmploymentStatusID"]);
            EmploymentStatus.EmploymentStatusName = r["EmploymentStatusName"].ToString();
            EmploymentStatus.InActive = Convert.ToBoolean(r["InActive"]);
            EmploymentStatus.CreatedUserID = Convert.ToInt32(r["CreatedUserID"]);
            EmploymentStatus.ModifiedUserID = Convert.ToInt32(r["ModifiedUserID"]);
            EmploymentStatus.CreatedDate = r["CreatedDate"].ToString().GetDate();
            EmploymentStatus.ModifiedDate = r["ModifiedDate"].ToString().GetDate();

            return EmploymentStatus;
        }
    }
}