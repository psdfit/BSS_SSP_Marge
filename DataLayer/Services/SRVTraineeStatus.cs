using DataLayer.Classes;
using DataLayer.Models;
using Microsoft.Data.SqlClient;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace DataLayer.Services
{
    public class SRVTraineeStatus : SRVBase, DataLayer.Interfaces.ISRVTraineeStatus
    {
        public SRVTraineeStatus()
        {
        }
        public bool SaveTraineeStatus(TraineeStatusModel TraineeStatus)
        {
            try
            {
                SqlParameter[] param = new SqlParameter[6];
                param[0] = new SqlParameter("@TraineeStatusID", TraineeStatus.TraineeStatusID);
                param[1] = new SqlParameter("@TraineeProfileID", TraineeStatus.TraineeProfileID);
                param[2] = new SqlParameter("@TraineeStatusTypeID", TraineeStatus.TraineeStatusTypeID);
                param[3] = new SqlParameter("@CreatedUserID", TraineeStatus.CreatedUserID);
                param[4] = new SqlParameter("@Comments", TraineeStatus.Comments);
                SqlHelper.ExecuteNonQuery(SqlHelper.GetCon(), CommandType.StoredProcedure, "[AU_TraineeStatus]", param);
                return true;
            }
            catch (Exception ex)
            { throw new Exception(ex.Message); }
        }
        public List<TraineeStatusModel> FetchTraineeStatus(TraineeStatusModel model)
        {
            try
            {
                //SqlParameter[] param = new SqlParameter[5];
                //param[0] = new SqlParameter("@TraineeProfileID", model.TraineeProfileID);
                DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_TraineeStatus", Common.GetParams(model)).Tables[0];
                return LoopinData(dt);
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }

        public List<TraineeStatusModel> FetchTraineeStatusByTraineeID(int TraineeID)
        {
            try
            {
                
                DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_TraineeStatus", new SqlParameter("@TraineeProfileID",TraineeID)).Tables[0];
                return LoopinData(dt);
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }

        public List<TraineeStatusModel> FetchTraineeStatus()
        {
            try
            {
                DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_TraineeStatus").Tables[0];
                return LoopinData(dt);
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }
        private List<TraineeStatusModel> LoopinData(DataTable dt)
        {
            List<TraineeStatusModel> TraineeStatusL = new List<TraineeStatusModel>();

            foreach (DataRow r in dt.Rows)
            {
                TraineeStatusL.Add(RowOfTraineeStatus(r));
            }
            return TraineeStatusL;
        }
        private TraineeStatusModel RowOfTraineeStatus(DataRow r)
        {
            TraineeStatusModel TraineeStatus = new TraineeStatusModel();
            TraineeStatus.TraineeStatusID = Convert.ToInt32(r["TraineeStatusID"]);
            TraineeStatus.TraineeProfileID = Convert.ToInt32(r["TraineeProfileID"]);
            TraineeStatus.TraineeStatusTypeID = Convert.ToInt32(r["TraineeStatusTypeID"]);
            TraineeStatus.CreatedUserID = String.IsNullOrEmpty(r["CreatedUserID"].ToString())? 0 :Convert.ToInt32(r["CreatedUserID"]);
            TraineeStatus.CreatedDate = r["CreatedDate"].ToString().GetDate();
            TraineeStatus.Comments = r["Comments"].ToString();
            TraineeStatus.StatusName = r["StatusName"].ToString();
            TraineeStatus.UserName = r["UserName"].ToString();

            return TraineeStatus;
        }
        public TraineeStatusModel GetTraineeStatusByMonth(TraineeStatusModel model)
        {
            try
            {
                SqlParameter[] param = new SqlParameter[5];
                param[0]=  new SqlParameter("@TraineeProfileID", model.TraineeProfileID);
                param[1]=  new SqlParameter("@CreatedDate", model.CreatedDate);
                DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_TraineeStatus_ByMonth", param).Tables[0];
                if (dt.Rows.Count > 0)
                {
                    return RowOfTraineeStatus(dt.Rows[0]);
                }
                else
                    return null;
            }
            catch (Exception ex) { throw new Exception(ex.Message); }

        }

        public string GetTSPUserByClassID_NADRA_Notification(List<int> TspIDs)
        {
            try
            {
                var IDs = String.Join(",", TspIDs);
                List<string> Distinct_uniqueValues = IDs.ToLower().Split(',').Distinct().ToList();
                string TSPIDs = "";
                foreach (var item in Distinct_uniqueValues)
                {
                    List<TSPMasterModel> model = new List<TSPMasterModel>();
                    SqlParameter[] obj = new SqlParameter[3];
                    obj[0] = new SqlParameter("@TspID",Convert.ToInt32(item));
                    DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "[RD_GetTSPUserByTSPID_Notification]", obj).Tables[0];
                    model = Helper.ConvertDataTableToModel<TSPMasterModel>(dt);
                    if (model.Count > 0)
                    {
                        TSPIDs = model[0].UserID.ToString() + ",";
                    }
                }
                List<string> Distinct_uniqueVal = TSPIDs.ToLower().Split(',').Distinct().ToList();
                var tspIDs = String.Join(",", Distinct_uniqueVal);
                return tspIDs;
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }
    }
}