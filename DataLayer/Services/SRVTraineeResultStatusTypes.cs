using DataLayer.Classes;
using DataLayer.Models;
using Microsoft.Data.SqlClient;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;

namespace DataLayer.Services
{
    public class SRVTraineeResultStatusTypes : SRVBase, DataLayer.Interfaces.ISRVTraineeResultStatusTypes
    {
        public SRVTraineeResultStatusTypes()
        {
        }

        public TraineeResultStatusTypesModel GetByResultStatusID(int ResultStatusID)
        {
            try
            {
                SqlParameter param = new SqlParameter("@ResultStatusID", ResultStatusID);
                DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_TraineeResultStatusTypes", param).Tables[0];
                if (dt.Rows.Count > 0)
                {
                    return RowOfTraineeResultStatusTypes(dt.Rows[0]);
                }
                else
                    return null;
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }

        public List<TraineeResultStatusTypesModel> SaveTraineeResultStatusTypes(TraineeResultStatusTypesModel TraineeResultStatusTypes)
        {
            try
            {
                SqlParameter[] param = new SqlParameter[3];
                param[0] = new SqlParameter("@ResultStatusID", TraineeResultStatusTypes.ResultStatusID);
                param[1] = new SqlParameter("@ResultStatusName", TraineeResultStatusTypes.ResultStatusName);

                param[2] = new SqlParameter("@CurUserID", TraineeResultStatusTypes.CurUserID);
                SqlHelper.ExecuteNonQuery(SqlHelper.GetCon(), CommandType.StoredProcedure, "[AU_TraineeResultStatusTypes]", param);
                return FetchTraineeResultStatusTypes();
            }
            catch (Exception ex)
            { throw new Exception(ex.Message); }
        }

        private List<TraineeResultStatusTypesModel> LoopinData(DataTable dt)
        {
            List<TraineeResultStatusTypesModel> TraineeResultStatusTypesL = new List<TraineeResultStatusTypesModel>();

            foreach (DataRow r in dt.Rows)
            {
                TraineeResultStatusTypesL.Add(RowOfTraineeResultStatusTypes(r));
            }
            return TraineeResultStatusTypesL;
        }

        public List<TraineeResultStatusTypesModel> FetchTraineeResultStatusTypes(TraineeResultStatusTypesModel mod)
        {
            try
            {
                DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_TraineeResultStatusTypes", Common.GetParams(mod)).Tables[0];
                return LoopinData(dt);
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }

        public List<TraineeResultStatusTypesModel> FetchTraineeResultStatusTypes()
        {
            try
            {
                DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_TraineeResultStatusTypes").Tables[0];
                return LoopinData(dt);
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }

        public List<TraineeResultStatusTypesModel> FetchTraineeResultStatusTypes(bool InActive)
        {
            try
            {
                DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_TraineeResultStatusTypes", new SqlParameter("@InActive", InActive)).Tables[0];
                return LoopinData(dt);
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }

        public int BatchInsert(List<TraineeResultStatusTypesModel> ls, int @BatchFkey, int CurUserID)
        {
            SqlParameter[] param = new SqlParameter[3];

            param[0] = new SqlParameter("@Json", JsonConvert.SerializeObject(ls));
            param[1] = new SqlParameter("@", @BatchFkey);
            param[2] = new SqlParameter("@CurUserID", CurUserID);
            return SqlHelper.ExecuteNonQuery(SqlHelper.GetCon(), CommandType.StoredProcedure, "[BAU_TraineeResultStatusTypes]", param);
        }

        public void ActiveInActive(int ResultStatusID, bool? InActive, int CurUserID)
        {
            SqlParameter[] PLead = new SqlParameter[3];
            PLead[0] = new SqlParameter("@ResultStatusID", ResultStatusID);
            PLead[1] = new SqlParameter("@InActive", InActive);
            PLead[2] = new SqlParameter("@CurUserID", CurUserID);
            SqlHelper.ExecuteNonQuery(SqlHelper.GetCon(), CommandType.StoredProcedure, "[AI_TraineeResultStatusTypes]", PLead);
        }

        private TraineeResultStatusTypesModel RowOfTraineeResultStatusTypes(DataRow r)
        {
            TraineeResultStatusTypesModel TraineeResultStatusTypes = new TraineeResultStatusTypesModel();
            TraineeResultStatusTypes.ResultStatusID = Convert.ToInt32(r["ResultStatusID"]);
            TraineeResultStatusTypes.ResultStatusName = r["ResultStatusName"].ToString();
            TraineeResultStatusTypes.InActive = Convert.ToBoolean(r["InActive"]);
            TraineeResultStatusTypes.CreatedUserID = Convert.ToInt32(r["CreatedUserID"]);
            TraineeResultStatusTypes.ModifiedUserID = Convert.ToInt32(r["ModifiedUserID"]);
            TraineeResultStatusTypes.CreatedDate = r["CreatedDate"].ToString().GetDate();
            TraineeResultStatusTypes.ModifiedDate = r["ModifiedDate"].ToString().GetDate();

            return TraineeResultStatusTypes;
        }
    }
}