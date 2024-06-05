using DataLayer.Classes;
using DataLayer.Models;
using Microsoft.Data.SqlClient;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;

namespace DataLayer.Services
{
    public class SRVTraineeStatusTypes : SRVBase, DataLayer.Interfaces.ISRVTraineeStatusTypes
    {
        public SRVTraineeStatusTypes()
        {
        }

        public TraineeStatusTypesModel GetByStatusID(int StatusID)
        {
            try
            {
                SqlParameter param = new SqlParameter("@TraineeStatusTypeID", StatusID);
                DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_TraineeStatusTypes", param).Tables[0];
                if (dt.Rows.Count > 0)
                {
                    return RowOfTraineeStatusTypes(dt.Rows[0]);
                }
                else
                    return null;
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }

        public List<TraineeStatusTypesModel> SaveTraineeStatusTypes(TraineeStatusTypesModel TraineeStatusTypes)
        {
            try
            {
                SqlParameter[] param = new SqlParameter[3];
                param[0] = new SqlParameter("@TraineeStatusTypeID", TraineeStatusTypes.TraineeStatusTypeID);
                param[1] = new SqlParameter("@StatusName", TraineeStatusTypes.StatusName);

                param[2] = new SqlParameter("@CurUserID", TraineeStatusTypes.CurUserID);
                SqlHelper.ExecuteNonQuery(SqlHelper.GetCon(), CommandType.StoredProcedure, "[AU_TraineeStatusTypes]", param);
                return FetchTraineeStatusTypes();
            }
            catch (Exception ex)
            { throw new Exception(ex.Message); }
        }
        private List<TraineeStatusTypesModel> LoopinDataTraineeReason(DataTable dt)
        {
            List<TraineeStatusTypesModel> TraineeStatusTypesL = new List<TraineeStatusTypesModel>();

            foreach (DataRow r in dt.Rows)
            {
                TraineeStatusTypesL.Add(RowOfTraineeStatusReason(r));
            }
            return TraineeStatusTypesL;
        }

        private List<TraineeStatusTypesModel> LoopinData(DataTable dt)
        {
            List<TraineeStatusTypesModel> TraineeStatusTypesL = new List<TraineeStatusTypesModel>();

            foreach (DataRow r in dt.Rows)
            {
                TraineeStatusTypesL.Add(RowOfTraineeStatusTypes(r));
            }
            return TraineeStatusTypesL;
        }

        public List<TraineeStatusTypesModel> FetchTraineeStatusTypes(TraineeStatusTypesModel mod)
        {
            try
            {
                DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_TraineeStatusTypes", Common.GetParams(mod)).Tables[0];
                return LoopinData(dt);
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }

        public List<TraineeStatusTypesModel> FetchTraineeStatusTypes()
        {
            try
            {
                DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_TraineeStatusTypes").Tables[0];
                return LoopinData(dt);
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }

        public List<TraineeStatusTypesModel> FetchTraineeStatusTypes(bool InActive)
        {
            try
            {
                DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_TraineeStatusTypes", new SqlParameter("@InActive", InActive)).Tables[0];
                return LoopinData(dt);
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }

        public List<TraineeStatusTypesModel> FetchTraineeStatusReason(bool InActive)
        {
            try
            {
                DataSet ds = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.Text, "Select TraineeStatusReasonID,Reason from TraineeStatusReason where InActive = 0");
                DataTable dt = ds.Tables[0];
                return LoopinDataTraineeReason(dt);
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }

        public int BatchInsert(List<TraineeStatusTypesModel> ls, int @BatchFkey, int CurUserID)
        {
            SqlParameter[] param = new SqlParameter[3];

            param[0] = new SqlParameter("@Json", JsonConvert.SerializeObject(ls));
            param[1] = new SqlParameter("@", @BatchFkey);
            param[2] = new SqlParameter("@CurUserID", CurUserID);
            return SqlHelper.ExecuteNonQuery(SqlHelper.GetCon(), CommandType.StoredProcedure, "[BAU_TraineeStatusTypes]", param);
        }

        public void ActiveInActive(int StatusID, bool? InActive, int CurUserID)
        {
            SqlParameter[] PLead = new SqlParameter[3];
            PLead[0] = new SqlParameter("@TraineeStatusTypeID", StatusID);
            PLead[1] = new SqlParameter("@InActive", InActive);
            PLead[2] = new SqlParameter("@CurUserID", CurUserID);
            SqlHelper.ExecuteNonQuery(SqlHelper.GetCon(), CommandType.StoredProcedure, "[AI_TraineeStatusTypes]", PLead);
        }

        private TraineeStatusTypesModel RowOfTraineeStatusTypes(DataRow r)
        {
            TraineeStatusTypesModel TraineeStatusTypes = new TraineeStatusTypesModel();
            TraineeStatusTypes.TraineeStatusTypeID = Convert.ToInt32(r["TraineeStatusTypeID"]);
            TraineeStatusTypes.StatusName = r["StatusName"].ToString();
            TraineeStatusTypes.InActive = Convert.ToBoolean(r["InActive"]);
            TraineeStatusTypes.CreatedUserID = Convert.ToInt32(r["CreatedUserID"]);
            TraineeStatusTypes.ModifiedUserID = Convert.ToInt32(r["ModifiedUserID"]);
            TraineeStatusTypes.CreatedDate = r["CreatedDate"].ToString().GetDate();
            TraineeStatusTypes.ModifiedDate = r["ModifiedDate"].ToString().GetDate();

            return TraineeStatusTypes;
        }

        private TraineeStatusTypesModel RowOfTraineeStatusReason(DataRow r)
        {
            TraineeStatusTypesModel TraineeStatusTypes = new TraineeStatusTypesModel();
            TraineeStatusTypes.TraineeStatusReasonID = Convert.ToInt32(r["TraineeStatusReasonID"]);
            TraineeStatusTypes.TraineeReason = r["Reason"].ToString();

            return TraineeStatusTypes;
        }
    }
}