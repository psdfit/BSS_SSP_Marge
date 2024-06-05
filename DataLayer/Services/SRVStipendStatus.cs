using DataLayer.Classes;
using DataLayer.Models;
using Microsoft.Data.SqlClient;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;

namespace DataLayer.Services
{
    public class SRVStipendStatus : SRVBase, DataLayer.Interfaces.ISRVStipendStatus
    {
        public SRVStipendStatus()
        {
        }

        public StipendStatusModel GetByStipendStatusID(int StipendStatusID)
        {
            try
            {
                SqlParameter param = new SqlParameter("@StipendStatusID", StipendStatusID);
                DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_StipendStatus", param).Tables[0];
                if (dt.Rows.Count > 0)
                {
                    return RowOfStipendStatus(dt.Rows[0]);
                }
                else
                    return null;
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }

        public List<StipendStatusModel> SaveStipendStatus(StipendStatusModel StipendStatus)
        {
            try
            {
                SqlParameter[] param = new SqlParameter[3];
                param[0] = new SqlParameter("@StipendStatusID", StipendStatus.StipendStatusID);
                param[1] = new SqlParameter("@StipendStatusName", StipendStatus.StipendStatusName);

                param[2] = new SqlParameter("@CurUserID", StipendStatus.CurUserID);
                SqlHelper.ExecuteNonQuery(SqlHelper.GetCon(), CommandType.StoredProcedure, "[AU_StipendStatus]", param);
                return FetchStipendStatus();
            }
            catch (Exception ex)
            { throw new Exception(ex.Message); }
        }

        private List<StipendStatusModel> LoopinData(DataTable dt)
        {
            List<StipendStatusModel> StipendStatusL = new List<StipendStatusModel>();

            foreach (DataRow r in dt.Rows)
            {
                StipendStatusL.Add(RowOfStipendStatus(r));
            }
            return StipendStatusL;
        }

        public List<StipendStatusModel> FetchStipendStatus(StipendStatusModel mod)
        {
            try
            {
                DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_StipendStatus", Common.GetParams(mod)).Tables[0];
                return LoopinData(dt);
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }

        public List<StipendStatusModel> FetchStipendStatus()
        {
            try
            {
                DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_StipendStatus").Tables[0];
                return LoopinData(dt);
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }

        public List<StipendStatusModel> FetchStipendStatus(bool InActive)
        {
            try
            {
                DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_StipendStatus", new SqlParameter("@InActive", InActive)).Tables[0];
                return LoopinData(dt);
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }

        public int BatchInsert(List<StipendStatusModel> ls, int @BatchFkey, int CurUserID)
        {
            SqlParameter[] param = new SqlParameter[3];

            param[0] = new SqlParameter("@Json", JsonConvert.SerializeObject(ls));
            param[1] = new SqlParameter("@", @BatchFkey);
            param[2] = new SqlParameter("@CurUserID", CurUserID);
            return SqlHelper.ExecuteNonQuery(SqlHelper.GetCon(), CommandType.StoredProcedure, "[BAU_StipendStatus]", param);
        }

        public void ActiveInActive(int StipendStatusID, bool? InActive, int CurUserID)
        {
            SqlParameter[] PLead = new SqlParameter[3];
            PLead[0] = new SqlParameter("@StipendStatusID", StipendStatusID);
            PLead[1] = new SqlParameter("@InActive", InActive);
            PLead[2] = new SqlParameter("@CurUserID", CurUserID);
            SqlHelper.ExecuteNonQuery(SqlHelper.GetCon(), CommandType.StoredProcedure, "[AI_StipendStatus]", PLead);
        }

        private StipendStatusModel RowOfStipendStatus(DataRow r)
        {
            StipendStatusModel StipendStatus = new StipendStatusModel();
            StipendStatus.StipendStatusID = Convert.ToInt32(r["StipendStatusID"]);
            StipendStatus.StipendStatusName = r["StipendStatusName"].ToString();
            StipendStatus.InActive = Convert.ToBoolean(r["InActive"]);
            StipendStatus.CreatedUserID = Convert.ToInt32(r["CreatedUserID"]);
            StipendStatus.ModifiedUserID = Convert.ToInt32(r["ModifiedUserID"]);
            StipendStatus.CreatedDate = r["CreatedDate"].ToString().GetDate();
            StipendStatus.ModifiedDate = r["ModifiedDate"].ToString().GetDate();

            return StipendStatus;
        }
    }
}