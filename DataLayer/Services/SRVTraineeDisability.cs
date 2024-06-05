using DataLayer.Classes;
using DataLayer.Models;
using Microsoft.Data.SqlClient;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;

namespace DataLayer.Services
{
    public class SRVTraineeDisability : SRVBase, DataLayer.Interfaces.ISRVTraineeDisability
    {
        public SRVTraineeDisability()
        {
        }

        public TraineeDisabilityModel GetById(int Id)
        {
            try
            {
                SqlParameter param = new SqlParameter("@Id", Id);
                DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_TraineeDisability", param).Tables[0];
                if (dt.Rows.Count > 0)
                {
                    return RowOfTraineeDisability(dt.Rows[0]);
                }
                else
                    return null;
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }

        public List<TraineeDisabilityModel> SaveTraineeDisability(TraineeDisabilityModel TraineeDisability)
        {
            try
            {
                SqlParameter[] param = new SqlParameter[3];
                param[0] = new SqlParameter("@Id", TraineeDisability.Id);
                param[1] = new SqlParameter("@Disability", TraineeDisability.Disability);

                param[2] = new SqlParameter("@CurUserID", TraineeDisability.CurUserID);
                SqlHelper.ExecuteNonQuery(SqlHelper.GetCon(), CommandType.StoredProcedure, "[AU_TraineeDisability]", param);
                return FetchTraineeDisability();
            }
            catch (Exception ex)
            { throw new Exception(ex.Message); }
        }

        private List<TraineeDisabilityModel> LoopinData(DataTable dt)
        {
            List<TraineeDisabilityModel> TraineeDisabilityL = new List<TraineeDisabilityModel>();

            foreach (DataRow r in dt.Rows)
            {
                TraineeDisabilityL.Add(RowOfTraineeDisability(r));
            }
            return TraineeDisabilityL;
        }

        public List<TraineeDisabilityModel> FetchTraineeDisability(TraineeDisabilityModel mod)
        {
            try
            {
                DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_TraineeDisability", Common.GetParams(mod)).Tables[0];
                return LoopinData(dt);
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }

        public List<TraineeDisabilityModel> FetchTraineeDisability()
        {
            try
            {
                DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_TraineeDisability").Tables[0];
                return LoopinData(dt);
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }

        public List<TraineeDisabilityModel> FetchTraineeDisability(bool InActive)
        {
            try
            {
                DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_TraineeDisability", new SqlParameter("@InActive", InActive)).Tables[0];
                return LoopinData(dt);
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }

        public int BatchInsert(List<TraineeDisabilityModel> ls, int @BatchFkey, int CurUserID)
        {
            SqlParameter[] param = new SqlParameter[3];

            param[0] = new SqlParameter("@Json", JsonConvert.SerializeObject(ls));
            param[1] = new SqlParameter("@", @BatchFkey);
            param[2] = new SqlParameter("@CurUserID", CurUserID);
            return SqlHelper.ExecuteNonQuery(SqlHelper.GetCon(), CommandType.StoredProcedure, "[BAU_TraineeDisability]", param);
        }

        public void ActiveInActive(int Id, bool? InActive, int CurUserID)
        {
            SqlParameter[] PLead = new SqlParameter[3];
            PLead[0] = new SqlParameter("@Id", Id);
            PLead[1] = new SqlParameter("@InActive", InActive);
            PLead[2] = new SqlParameter("@CurUserID", CurUserID);
            SqlHelper.ExecuteNonQuery(SqlHelper.GetCon(), CommandType.StoredProcedure, "[AI_TraineeDisability]", PLead);
        }

        private TraineeDisabilityModel RowOfTraineeDisability(DataRow r)
        {
            TraineeDisabilityModel TraineeDisability = new TraineeDisabilityModel();
            TraineeDisability.Id = Convert.ToInt32(r["Id"]);
            TraineeDisability.Disability = r["Disability"].ToString();
            TraineeDisability.InActive = Convert.ToBoolean(r["InActive"]);
            TraineeDisability.CreatedUserID = Convert.ToInt32(r["CreatedUserID"]);
            TraineeDisability.ModifiedUserID = Convert.ToInt32(r["ModifiedUserID"]);
            TraineeDisability.CreatedDate = r["CreatedDate"].ToString().GetDate();
            TraineeDisability.ModifiedDate = r["ModifiedDate"].ToString().GetDate();

            return TraineeDisability;
        }
    }
}