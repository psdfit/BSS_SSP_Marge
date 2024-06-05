using System;
using System.Collections.Generic;
using System.Data;
using Microsoft.Data.SqlClient;
using DataLayer.Classes;
using DataLayer.Models;
using DataLayer.Interfaces;
using Newtonsoft.Json;
namespace DataLayer.Services
{
    public class SRVHomeStats : SRVBase, ISRVHomeStats
    {
        public SRVHomeStats() { }
        public HomeStatsModel GetByID(int ID)
        {
            try
            {
                SqlParameter param = new SqlParameter("@ID", ID);
                DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_Stats1", param).Tables[0];
                if (dt.Rows.Count > 0)
                {
                    return RowOfHomeStats(dt.Rows[0]);
                }
                else
                    return null;
            }
            catch (Exception ex) { throw new Exception(ex.Message); }

        }
        private List<HomeStatsModel> LoopinData(DataTable dt)
        {
            List<HomeStatsModel> HomeStatsL = new List<HomeStatsModel>();

            foreach (DataRow r in dt.Rows)
            {
                HomeStatsL.Add(RowOfHomeStats(r));

            }
            return HomeStatsL;
        }
        private List<HomeStatsModel> LoopinPendingStatsData(DataTable dt)
        {
            List<HomeStatsModel> HomeStatsL = new List<HomeStatsModel>();

            foreach (DataRow r in dt.Rows)
            {
                HomeStatsL.Add(RowOfPendingHomeStats(r));

            }
            return HomeStatsL;
        }
        public List<HomeStatsModel> FetchHomeStats(HomeStatsModel mod)
        {
            try
            {
                DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_Stats1", Common.GetParams(mod)).Tables[0];
                return LoopinData(dt);
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }
        public List<HomeStatsModel> FetchHomeStats()
        {
            try
            {
                DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_Stats1").Tables[0];
                return LoopinData(dt);
            }
            catch (Exception ex) { throw new Exception(ex.Message); }

        }
        public List<HomeStatsModel> FetchHomeStats(bool InActive)
        {
            try
            {
                DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_Stats1", new SqlParameter("@InActive", InActive)).Tables[0];
                return LoopinData(dt);
            }
            catch (Exception ex) { throw new Exception(ex.Message); }

        }
        public List<HomeStatsModel> GetBySchemeID(int SchemeID)
        {
            try
            {
                DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_Stats", new SqlParameter("@SchemeID", SchemeID)).Tables[0];
                return LoopinData(dt);
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }
        public List<HomeStatsModel> GetByTSPID(int TSPID)
        {
            try
            {
                DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_Stats", new SqlParameter("@TSPID", TSPID)).Tables[0];
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
            SqlHelper.ExecuteNonQuery(SqlHelper.GetCon(), CommandType.StoredProcedure, "[AI_HomeStats]", PLead);
        }
        public List<HomeStatsModel> FetchHomeStatsByClass(int ClassID)
        {
            try
            {
                SqlParameter param = new SqlParameter("@ClassID", ClassID);
                DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "[RD_Stats1]", param).Tables[0];
                if (dt.Rows.Count > 0)
                {
                    return LoopinData(dt);
                }
                else
                    return null;
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }
        public List<HomeStatsModel> FetchHomeStatsByFilters(int[] filters)
        {
            List<HomeStatsModel> list = new List<HomeStatsModel>();
            if (filters.Length > 0)
            {
                int schemeId = filters[0];
                int tspId = filters[1];
                int classId = filters[2];
                int traineeId = filters[3];
                try
                {
                    SqlParameter[] param = new SqlParameter[10];
                    param[0] = new SqlParameter("@SchemeID", schemeId);
                    param[1] = new SqlParameter("@TSPID", tspId);
                    param[2] = new SqlParameter("@ClassID", classId);
                    param[3] = new SqlParameter("@TraineeID", traineeId);
                    DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_Stats1", param).Tables[0];
                    list = LoopinData(dt);
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            return list;
        }
        private HomeStatsModel RowOfHomeStats(DataRow r)
        {
            HomeStatsModel HomeStats = new HomeStatsModel();

            HomeStats.Schemes = Convert.ToInt32(r["Schemes"]);
            HomeStats.Classes = Convert.ToInt32(r["Classes"]);
            HomeStats.TSPs = Convert.ToInt32(r["TSPs"]);
            HomeStats.Trainees = Convert.ToInt32(r["Trainees"]);
            HomeStats.SavedTrainees = Convert.ToInt32(r["SavedTrainees"]);

            return HomeStats;
        }
        private HomeStatsModel RowOfPendingHomeStats(DataRow r)
        {
            HomeStatsModel HomeStats = new HomeStatsModel();

            HomeStats.PendingInceptionReports = Convert.ToInt32(r["PendingInceptionReports"]);
            HomeStats.PendingRegisterations = Convert.ToInt32(r["PendingRegisterations"]);
            HomeStats.PendingRTPs = Convert.ToInt32(r["PendingRTPs"]);
            HomeStats.PendingEmployments = Convert.ToInt32(r["PendingEmployments"]);
            HomeStats.InceptionReportDeadline = r.Field<DateTime?>("InceptionReportDeadline");
            HomeStats.TraineeRegistrationDeadline = r.Field<DateTime?>("TraineeRegistrationDeadline");
            HomeStats.RTPDeadline = r.Field<DateTime?>("RTPDeadline");
            HomeStats.EmploymentDeadline = r.Field<DateTime?>("EmploymentDeadline");
            HomeStats.Planned = Convert.ToInt32(r["Planned"]);
            HomeStats.Active = Convert.ToInt32(r["Active"]);
            HomeStats.Completed = Convert.ToInt32(r["Completed"]);
            HomeStats.Cancelled = Convert.ToInt32(r["Cancelled"]);
            HomeStats.Abandoned = Convert.ToInt32(r["Abandoned"]);
            HomeStats.Ready = Convert.ToInt32(r["Ready"]);
            HomeStats.Suspended = Convert.ToInt32(r["Suspended"]);

            return HomeStats;
        }
        public List<HomeStatsModel> FetchHomeStatsByUser(QueryFilters filters)
        {
            try
            {
                List<SqlParameter> param = new List<SqlParameter>();
                param.Add(new SqlParameter("@SchemeID", filters.SchemeID));
                param.Add(new SqlParameter("@TSPID", filters.TSPID));
                param.Add(new SqlParameter("@ClassID", filters.ClassID));
                param.Add(new SqlParameter("@UserID", filters.UserID));
                param.Add(new SqlParameter("@OID", filters.OID));
                DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_StatsByUser", param.ToArray()).Tables[0];
                return LoopinData(dt);
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }
        public List<HomeStatsModel> FetchPendingHomeStatsByUser(QueryFilters filters)
        {
            try
            {
                List<SqlParameter> param = new List<SqlParameter>();
                param.Add(new SqlParameter("@SchemeID", filters.SchemeID));
                param.Add(new SqlParameter("@TSPID", filters.TSPID));
                param.Add(new SqlParameter("@ClassID", filters.ClassID));
                param.Add(new SqlParameter("@UserID", filters.UserID));
                param.Add(new SqlParameter("@OID", filters.OID));
                DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_StatsByTSPUser_Pending", param.ToArray()).Tables[0];
                return LoopinPendingStatsData(dt);
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }
    }
}
