using System;
using System.Collections.Generic;
using System.Data;
using Microsoft.Data.SqlClient;
using DataLayer.Classes;
using DataLayer.Models;
using Newtonsoft.Json;
using DataLayer.Interfaces;
namespace DataLayer.Services
{
    public class SRVPBTEDataSharingTimelines : SRVBase, ISRVPBTEDataSharingTimelines
    {
        public SRVPBTEDataSharingTimelines() { }
        public PBTEDataSharingTimelinesModel GetByID(int ID)
        {
            try
            {
                SqlParameter param = new SqlParameter("@ID", ID);
                DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_PBTEDataSharingTimelines", param).Tables[0];
                if (dt.Rows.Count > 0)
                {
                    return RowOfPBTEDataSharingTimelines(dt.Rows[0]);
                }
                else
                    return null;
            }
            catch (Exception ex) { throw new Exception(ex.Message); }

        }
        public List<PBTEDataSharingTimelinesModel> SavePBTEDataSharingTimelines(PBTEDataSharingTimelinesModel PBTEDataSharingTimelines)
        {
            try
            {

                SqlParameter[] param = new SqlParameter[6];
                param[0] = new SqlParameter("@ID", PBTEDataSharingTimelines.ID);
                param[1] = new SqlParameter("@ClassTimeline", PBTEDataSharingTimelines.ClassTimeline);
                param[2] = new SqlParameter("@TSPTimeline", PBTEDataSharingTimelines.TSPTimeline);
                param[3] = new SqlParameter("@TraineeTimeline", PBTEDataSharingTimelines.TraineeTimeline);
                param[4] = new SqlParameter("@DropOutTraineeTimeline", PBTEDataSharingTimelines.DropOutTraineeTimeline);

                param[5] = new SqlParameter("@CurUserID", PBTEDataSharingTimelines.CurUserID);
                SqlHelper.ExecuteNonQuery(SqlHelper.GetCon(), CommandType.StoredProcedure, "[AU_PBTEDataSharingTimelines]", param);
                return FetchPBTEDataSharingTimelines();
            }
            catch (Exception ex)
            { throw new Exception(ex.Message); }
        }
        private List<PBTEDataSharingTimelinesModel> LoopinData(DataTable dt)
        {
            List<PBTEDataSharingTimelinesModel> PBTEDataSharingTimelinesL = new List<PBTEDataSharingTimelinesModel>();

            foreach (DataRow r in dt.Rows)
            {
                PBTEDataSharingTimelinesL.Add(RowOfPBTEDataSharingTimelines(r));

            }
            return PBTEDataSharingTimelinesL;
        }
        public List<PBTEDataSharingTimelinesModel> FetchPBTEDataSharingTimelines(PBTEDataSharingTimelinesModel mod)
        {
            try
            {
                DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_PBTEDataSharingTimelines", Common.GetParams(mod)).Tables[0];
                return LoopinData(dt);
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }
        public List<PBTEDataSharingTimelinesModel> FetchPBTEDataSharingTimelines()
        {
            try
            {
                DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_PBTEDataSharingTimelines").Tables[0];
                return LoopinData(dt);
            }
            catch (Exception ex) { throw new Exception(ex.Message); }

        }
        public List<PBTEDataSharingTimelinesModel> FetchPBTEDataSharingTimelines(bool InActive)
        {
            try
            {
                DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_PBTEDataSharingTimelines", new SqlParameter("@InActive", InActive)).Tables[0];
                return LoopinData(dt);
            }
            catch (Exception ex) { throw new Exception(ex.Message); }

        }
        public void ActiveInActive(int ID, bool? InActive, int CurUserID)
        {

            SqlParameter[] PLead = new SqlParameter[3];
            PLead[0] = new SqlParameter("@ID", ID);
            PLead[1] = new SqlParameter("@InActive", InActive);
            PLead[2] = new SqlParameter("@CurUserID", CurUserID);
            SqlHelper.ExecuteNonQuery(SqlHelper.GetCon(), CommandType.StoredProcedure, "[AI_PBTEDataSharingTimelines]", PLead);
        }
        private PBTEDataSharingTimelinesModel RowOfPBTEDataSharingTimelines(DataRow r)
        {
            PBTEDataSharingTimelinesModel PBTEDataSharingTimelines = new PBTEDataSharingTimelinesModel();
            PBTEDataSharingTimelines.ID = Convert.ToInt32(r["ID"]);
            PBTEDataSharingTimelines.ClassTimeline = Convert.ToInt32(r["ClassTimeline"]);
            PBTEDataSharingTimelines.TSPTimeline = Convert.ToInt32(r["TSPTimeline"]);
            PBTEDataSharingTimelines.TraineeTimeline = Convert.ToInt32(r["TraineeTimeline"]);
            PBTEDataSharingTimelines.DropOutTraineeTimeline = Convert.ToInt32(r["DropOutTraineeTimeline"]);
            PBTEDataSharingTimelines.InActive = Convert.ToBoolean(r["InActive"]);
            PBTEDataSharingTimelines.CreatedUserID = Convert.ToInt32(r["CreatedUserID"]);
            PBTEDataSharingTimelines.ModifiedUserID = Convert.ToInt32(r["ModifiedUserID"]);
            PBTEDataSharingTimelines.CreatedDate = r["CreatedDate"].ToString().GetDate();
            PBTEDataSharingTimelines.ModifiedDate = r["ModifiedDate"].ToString().GetDate();

            return PBTEDataSharingTimelines;
        }
    }
}
