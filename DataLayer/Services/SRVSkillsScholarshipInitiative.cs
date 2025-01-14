using DataLayer.Classes;
using DataLayer.Interfaces;
using DataLayer.Models;
using Microsoft.Data.SqlClient;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;

namespace DataLayer.Services
{
    public class SRVSkillsScholarshipInitiative : SRVBase, ISRVSkillsScholarshipInitiative
    {
        public SRVSkillsScholarshipInitiative()
        {
        }

        public List<SkillsScholarshipInitiativeModel> GetSkillsScholarshipBySchemeID(int SchemeID, int? TSPId, int Locality, int Cluster, int? District)
        {
            try
            {
                if (TSPId == 0)
                {
                    TSPId = null;
                }
                if (District == 0)
                {
                    District = null;
                }
                SqlParameter[] param = new SqlParameter[5];
                param[0] = new SqlParameter("@SchemeID", SchemeID);
                param[1] = new SqlParameter("@TSPId", TSPId);
                param[2] = new SqlParameter("@Locality", Locality);
                param[3] = new SqlParameter("@ClusterID", Cluster);
                param[4] = new SqlParameter("@DistrictID", District);

                DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "B2C_Dashboard", param).Tables[0];
                return LoopinData(dt);

            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }

        public List<SkillsScholarshipInitiativeModel> GetSkillsScholarshipBySchemeIDReport(int SchemeID, int? TSPId, int Locality, int Cluster)
        {
            try
            {
                if (TSPId == 0)
                {
                    TSPId = null;
                }
                SqlParameter[] param = new SqlParameter[4];
                param[0] = new SqlParameter("@SchemeID", SchemeID);
                param[1] = new SqlParameter("@TSPId", TSPId);
                param[2] = new SqlParameter("@Locality", Locality);
                param[3] = new SqlParameter("@ClusterID", Cluster);

                DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "B2C_DashboardProgress", param).Tables[0];
                
                return LoopinDataReport(dt);


            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }

        private List<SkillsScholarshipInitiativeModel> LoopinDataReport(DataTable dt)
        {
            List<SkillsScholarshipInitiativeModel> SkillsScholarshipL = new List<SkillsScholarshipInitiativeModel>();

            if (dt.Rows.Count > 1)
            {
                foreach (DataRow r in dt.Rows)
                {
                    SkillsScholarshipL.Add(RowOfSkillsScholarshipReport(r));

                }
            }
            return SkillsScholarshipL;
        }


        private List<SkillsScholarshipInitiativeModel> LoopinData(DataTable dt)
        {
            List<SkillsScholarshipInitiativeModel> SkillsScholarshipL = new List<SkillsScholarshipInitiativeModel>();

            foreach (DataRow r in dt.Rows)
            {
                SkillsScholarshipL.Add(RowOfSkillsScholarship(r));

            }
            return SkillsScholarshipL;
        }


        private SkillsScholarshipInitiativeModel RowOfSkillsScholarship(DataRow r)
        {
            SkillsScholarshipInitiativeModel SkillsScholarship = new SkillsScholarshipInitiativeModel();
            
           
            SkillsScholarship.TradeName = r["TradeName"].ToString();
            SkillsScholarship.TradeID = Convert.ToInt32(r["TradeID"]);
            SkillsScholarship.TradeTarget = Convert.ToInt32(r["TradeTarget"]);
            SkillsScholarship.OverallEnrolments= Convert.ToInt32(r["OverallEnrolments"]);
            SkillsScholarship.EnrolmentsCompleted = Convert.ToInt32(r["EnrolmentsCompleted"]);
            SkillsScholarship.RemainingSeats = Convert.ToInt32(r["RemainingSeats"]);
            SkillsScholarship.ClusterID = Convert.ToInt32(r["ClusterID"]);
            SkillsScholarship.DistrictID = Convert.ToInt32(r["DistrictID"]);
            SkillsScholarship.DistrictName = r["DistrictName"].ToString();
            if (r.Table.Columns.Contains("SchemeID"))
            {
                
                SkillsScholarship.HasRaceStarted = Convert.ToBoolean(r["HasRaceStarted"]);
                SkillsScholarship.RaceStopped = Convert.ToBoolean(r["RaceStopped"]);
                SkillsScholarship.SchemeID = Convert.ToInt32(r["SchemeID"]);
                SkillsScholarship.NoOfAssociate = Convert.ToInt32(r["NoOfAssociate"]);
            }
            return SkillsScholarship;
        }

        private SkillsScholarshipInitiativeModel RowOfSkillsScholarshipReport(DataRow r)
        {
            SkillsScholarshipInitiativeModel SkillsScholarship = new SkillsScholarshipInitiativeModel();


            SkillsScholarship.TradeName = r["TradeName"].ToString();
            SkillsScholarship.ClusterName = r["ClusterName"].ToString();
            SkillsScholarship.TradeID = Convert.ToInt32(r["TradeID"]);
            SkillsScholarship.TradeTarget = Convert.ToInt32(r["TradeTarget"]);
            SkillsScholarship.EnrolmentsCompleted = Convert.ToInt32(r["EnrolmentsCompleted"]);
            SkillsScholarship.RemainingSeats = Convert.ToInt32(r["RemainingSeats"]);
            SkillsScholarship.ageCompleted = Convert.ToDouble(r["AgeCompleted"]);
            if (r.Table.Columns.Contains("RaceStopped"))
            {
                SkillsScholarship.RaceStopped = Convert.ToBoolean(r["RaceStopped"]);
                SkillsScholarship.HasRaceStarted = Convert.ToBoolean(r["HasRaceStarted"]);
            }

            SkillsScholarship.SchemeID = Convert.ToInt32(r["SchemeID"]);
            SkillsScholarship.NoOfAssociate = Convert.ToInt32(r["NoOfAssociate"]);

            return SkillsScholarship;
        }


        public List<SkillsScholarshipInitiativeModel> GetFilteredSessionCount(int SchemeID, int? TSPId)
        {
            try
            {
                SqlParameter[] param = new SqlParameter[4];
                param[0] = new SqlParameter("@SchemeID", SchemeID);
                param[1] = new SqlParameter("@TSPId", TSPId);

                DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "GetSessionData", param).Tables[0];
                return LoopinDataSession(dt);

            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }

        private List<SkillsScholarshipInitiativeModel> LoopinDataSession(DataTable dt)
        {
            List<SkillsScholarshipInitiativeModel> SkillsScholarshipL = new List<SkillsScholarshipInitiativeModel>();

            foreach (DataRow r in dt.Rows)
            {
                SkillsScholarshipL.Add(RowOfSession(r));

            }
            return SkillsScholarshipL;
        }

        private SkillsScholarshipInitiativeModel RowOfSession(DataRow r)
        {
            SkillsScholarshipInitiativeModel SkillsScholarship = new SkillsScholarshipInitiativeModel();


            SkillsScholarship.TSPName = r["TSPName"].ToString();
            SkillsScholarship.UserName = r["UserName"].ToString();
            SkillsScholarship.LoginDate = r["LoginDate"].ToString();
            SkillsScholarship.TSPID = Convert.ToInt32(r["TSPID"]);
            SkillsScholarship.SessionID = Convert.ToInt32(r["SessionID"]);
            SkillsScholarship.IPAddress = r["IPAddress"].ToString();
            return SkillsScholarship;
        }

        public List<SkillsScholarshipInitiativeModel> FetchClustersByLocality(int Locality)
        {
            try
            {
                SqlParameter[] param = new SqlParameter[2];
                param[0] = new SqlParameter("@Locality", Locality);

                DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "LocalityProvider", param).Tables[0];
                return LoopinDataCluster(dt);

            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }
        public List<SkillsScholarshipInitiativeModel> FetchDistrictsByCluster(int Cluster)
        {
            try
            {
                SqlParameter[] param = new SqlParameter[2];
                param[0] = new SqlParameter("@Cluster", Cluster);

                DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "DistrictProvider", param).Tables[0];
                return LoopinDataDistrict(dt);

            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }

        public void GetStartRace(int SchemeID, int ClusterID, int DistrictID, int TradeID, int UserID)
        {
            try
            {
                List<SqlParameter> param = new List<SqlParameter>();
                param.Add(new SqlParameter("@SchemeID", SchemeID));
                param.Add(new SqlParameter("@ClusterID", ClusterID));
                param.Add(new SqlParameter("@DistrictID", DistrictID));
                param.Add(new SqlParameter("@TradeID", TradeID));
                param.Add(new SqlParameter("@StartRaceUserID", UserID));
                SqlHelper.ExecuteNonQuery(SqlHelper.GetCon(), CommandType.StoredProcedure, "StartRace", param.ToArray());
                
                //param.Add(new SqlParameter("@HasRaceStarted", 1));
                //SqlHelper.ExecuteNonQuery(SqlHelper.GetCon(), CommandType.StoredProcedure, "UpdateStartRace", param.ToArray());

            }
            catch (Exception e)
            { throw new Exception(e.Message); }

        }
        public void GetStopRace(int SchemeID, int ClusterID, int DistrictID, int TradeID, int UserID)
        {
            try
            {
                List<SqlParameter> param = new List<SqlParameter>();
                param.Add(new SqlParameter("@SchemeID", SchemeID));
                param.Add(new SqlParameter("@ClusterID", ClusterID));
                param.Add(new SqlParameter("@DistrictID", DistrictID));
                param.Add(new SqlParameter("@TradeID", TradeID));
                param.Add(new SqlParameter("@StopRaceUserID", UserID));
                SqlHelper.ExecuteNonQuery(SqlHelper.GetCon(), CommandType.StoredProcedure, "StopRace", param.ToArray());

            }
            catch (Exception e)
            { throw new Exception(e.Message); }

        }

        public void GetDeleteSession(int SchemeID, int TSPID, int SessionID)
        {
            try
            {
                List<SqlParameter> param = new List<SqlParameter>();
                param.Add(new SqlParameter("@SchemeID", SchemeID));
                param.Add(new SqlParameter("@TSPID", TSPID));
                param.Add(new SqlParameter("@SessionID", SessionID));
                SqlHelper.ExecuteNonQuery(SqlHelper.GetCon(), CommandType.StoredProcedure, "DeleteSession", param.ToArray());

            }
            catch (Exception e)
            { throw new Exception(e.Message); }

        }

        private List<SkillsScholarshipInitiativeModel> LoopinDataCluster(DataTable dt)
        {
            List<SkillsScholarshipInitiativeModel> SkillsScholarshipL = new List<SkillsScholarshipInitiativeModel>();

            foreach (DataRow r in dt.Rows)
            {
                SkillsScholarshipL.Add(RowOfSkillsScholarshipClusters(r));

            }
            return SkillsScholarshipL;
        }

        private SkillsScholarshipInitiativeModel RowOfSkillsScholarshipClusters(DataRow r)
        {
            SkillsScholarshipInitiativeModel SkillsScholarship = new SkillsScholarshipInitiativeModel();

            SkillsScholarship.ClusterName = r["ClusterName"].ToString();
            SkillsScholarship.ClusterID = Convert.ToInt32(r["ClusterID"]);

            return SkillsScholarship;
        }

        private List<SkillsScholarshipInitiativeModel> LoopinDataDistrict(DataTable dt)
        {
            List<SkillsScholarshipInitiativeModel> SkillsScholarshipL = new List<SkillsScholarshipInitiativeModel>();

            foreach (DataRow r in dt.Rows)
            {
                SkillsScholarshipL.Add(RowOfSkillsScholarshipDistrict(r));

            }
            return SkillsScholarshipL;
        }
        private SkillsScholarshipInitiativeModel RowOfSkillsScholarshipDistrict(DataRow r)
        {
            SkillsScholarshipInitiativeModel SkillsScholarship = new SkillsScholarshipInitiativeModel();

            SkillsScholarship.DistrictName = r["DistrictName"].ToString();
            SkillsScholarship.DistrictID = Convert.ToInt32(r["DistrictID"]);

            return SkillsScholarship;
        }
    }
}