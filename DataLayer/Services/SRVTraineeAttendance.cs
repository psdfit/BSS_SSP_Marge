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
    public class SRVTraineeAttendance : SRVBase, ISRVTraineeAttendance
    {
        public SRVTraineeAttendance() { }
        public TraineeAttendanceModel GetByTraineeAttendanceID(int TraineeAttendanceID)
        {
            try
            {
                SqlParameter param = new SqlParameter("@TraineeAttendanceID", TraineeAttendanceID);
                DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_TraineeAttendance", param).Tables[0];
                if (dt.Rows.Count > 0)
                {
                    return RowOfTraineeAttendance(dt.Rows[0]);
                }
                else
                    return null;
            }
            catch (Exception ex) { throw new Exception(ex.Message); }

        }
        public List<TraineeAttendanceModel> SaveTraineeAttendance(TraineeAttendanceModel TraineeAttendance)
        {
            try
            {

                SqlParameter[] param = new SqlParameter[7];
                param[0] = new SqlParameter("@TraineeAttendanceID", TraineeAttendance.TraineeAttendanceID);
                param[1] = new SqlParameter("@TraineeProfileID", TraineeAttendance.TraineeProfileID);
                param[2] = new SqlParameter("@IsManual", TraineeAttendance.IsManual);
                param[3] = new SqlParameter("@GeoLocation", TraineeAttendance.GeoLocation);
                param[4] = new SqlParameter("@AttendanceDate", TraineeAttendance.AttendanceDate);
                param[5] = new SqlParameter("@IsAbsent", TraineeAttendance.IsAbsent);

                param[6] = new SqlParameter("@CurUserID", TraineeAttendance.CurUserID);
                SqlHelper.ExecuteNonQuery(SqlHelper.GetCon(), CommandType.StoredProcedure, "[AU_TraineeAttendance]", param);
                return FetchTraineeAttendance();
            }
            catch (Exception ex)
            { throw new Exception(ex.Message); }
        }
        private List<TraineeAttendanceModel> LoopinData(DataTable dt)
        {
            List<TraineeAttendanceModel> TraineeAttendanceL = new List<TraineeAttendanceModel>();

            foreach (DataRow r in dt.Rows)
            {
                TraineeAttendanceL.Add(RowOfTraineeAttendance(r));

            }
            return TraineeAttendanceL;
        }
        public List<TraineeAttendanceModel> FetchTraineeAttendance(TraineeAttendanceModel mod)
        {
            try
            {
                DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_TraineeAttendance", Common.GetParams(mod)).Tables[0];
                return LoopinData(dt);
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }
        public List<TraineeAttendanceModel> FetchTraineeAttendanceByMonth(TraineeAttendanceModel model)
        {
            try
            {
                SqlParameter[] param = new SqlParameter[5];
                param[0] = new SqlParameter("@TraineeProfileID", model.TraineeProfileID);
                param[1] = new SqlParameter("@IsManual", true);
                param[2] = new SqlParameter("@AttendanceDate", model.AttendanceDate);
                DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_TraineeAttendance_ByMonth", param).Tables[0];
                return LoopinData(dt);
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }
        public List<TraineeAttendanceModel> FetchTraineeAttendance()
        {
            try
            {
                DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_TraineeAttendance").Tables[0];
                return LoopinData(dt);
            }
            catch (Exception ex) { throw new Exception(ex.Message); }

        }
        public List<TraineeAttendanceModel> FetchTraineeAttendance(bool InActive)
        {
            try
            {
                DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_TraineeAttendance", new SqlParameter("@InActive", InActive)).Tables[0];
                return LoopinData(dt);
            }
            catch (Exception ex) { throw new Exception(ex.Message); }

        }
        public void ActiveInActive(int TraineeAttendanceID, bool? InActive, int CurUserID)
        {

            SqlParameter[] PLead = new SqlParameter[3];
            PLead[0] = new SqlParameter("@TraineeAttendanceID", TraineeAttendanceID);
            PLead[1] = new SqlParameter("@InActive", InActive);
            PLead[2] = new SqlParameter("@CurUserID", CurUserID);
            SqlHelper.ExecuteNonQuery(SqlHelper.GetCon(), CommandType.StoredProcedure, "[AI_TraineeAttendance]", PLead);
        }
        private TraineeAttendanceModel RowOfTraineeAttendance(DataRow r)
        {
            TraineeAttendanceModel TraineeAttendance = new TraineeAttendanceModel();
            TraineeAttendance.TraineeAttendanceID = Convert.ToInt32(r["TraineeAttendanceID"]);
            TraineeAttendance.TraineeProfileID = Convert.ToInt32(r["TraineeProfileID"]);
            TraineeAttendance.IsManual = Convert.ToBoolean(r["IsManual"]);
            TraineeAttendance.GeoLocation = r["GeoLocation"].ToString();
            TraineeAttendance.AttendanceDate = r["AttendanceDate"].ToString().GetDate();
            TraineeAttendance.IsAbsent = Convert.ToBoolean(r["IsAbsent"]);
            TraineeAttendance.CreatedDate = r["CreatedDate"].ToString().GetDate();
            TraineeAttendance.CreatedUserID = Convert.ToInt32(r["CreatedUserID"]);
            TraineeAttendance.ModifiedUserID = Convert.ToInt32(r["ModifiedUserID"]);
            TraineeAttendance.ModifiedDate = r["ModifiedDate"].ToString().GetDate();
            TraineeAttendance.InActive = Convert.ToBoolean(r["InActive"]);

            return TraineeAttendance;
        }
    }
}
