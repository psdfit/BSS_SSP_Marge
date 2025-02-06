using DataLayer.Interfaces;
using DataLayer.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;

namespace DataLayer.Services
{
    public class SRVDashboard : SRVBase, ISRVDashboard
    {
        public SRVDashboard() { }
        #region filter
        public DataTable FetchTrades()
        {
            try
            {
                DataSet ds = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.Text, "Select TradeID, TradeName from trade where InActive=0 order by tradeName");
                return ds.Tables[0];
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }
        public DataTable FetchClusters()
        {
            try
            {
                DataSet ds = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.Text, "Select ClusterID, ClusterName from Cluster where InActive=0 order by ClusterName");
                return ds.Tables[0];
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }
        public DataTable FetchDistricts()
        {
            try
            {
                DataSet ds = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.Text, "Select DistrictID, DistrictName from District where InActive=0 order by DistrictID");
                return ds.Tables[0];
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }
        public DataTable FetchSchemes()
        {
            try
            {
                DataSet ds = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.Text, "Select SchemeID, SchemeName from Scheme where IsApproved=1 and InActive=0 and IsMigrated = 0 order by SchemeID");
                return ds.Tables[0];
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }
        public DataTable FetchSchemesGSR()
        {
            try
            {
                string query = @"
            SELECT SchemeID, SchemeName
            FROM Scheme
            WHERE IsApproved = 1
              AND InActive = 0
              AND IsMigrated = 0
              AND ProgramTypeID = 7
             ORDER BY SchemeID";

                // Execute query
                DataSet ds = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.Text, query);
                return ds.Tables[0];
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }


        public DataTable FetchTSPs()
        {
            try
            {
                DataSet ds = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.Text, "Select TSPMasterID, TSPName from TSPMaster where InActive=0 order by TSPMasterID");
                return ds.Tables[0];
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }
        public DataTable FetchTSPDetail()
        {
            try
            {
                DataSet ds = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.Text, "SELECT td.TSPID, TSPName FROM dbo.TSPDetail td INNER JOIN dbo.Scheme s ON s.SchemeID = td.SchemeID WHERE td.InActive = 0 AND s.IsApproved = 1 ORDER BY TSPID;");
                return ds.Tables[0];
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }
        public DataTable FetchTSPsByScheme(int SchemeID)
        {
            try
            {
                DataSet ds = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.Text, commandText: "Select TSPID, TSPName from TSPDetail where (SchemeID=" + SchemeID + ")  and IsMigrated = 0 order by TSPID");
                return ds.Tables[0];
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }
        public DataTable FetchClassesBySchemeTSP(int SchemeID, int TspID)
        {
            try
            {
                DataSet ds = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.Text, "Select ClassID, ClassCode from Class where SchemeID=" + SchemeID + " and TSPID=" + TspID + "order by ClassID");
                return ds.Tables[0];
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }
        public DataTable FetchClassesByTSP(int TspID)
        {
            try
            {
                DataSet ds = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.Text, "Select ClassID, ClassCode from Class where TSPID=" + TspID + "order by ClassID");
                return ds.Tables[0];
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }
        public DataTable FetchPrograms()
        {
            try
            {
                DataSet ds = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.Text, "Select PTypeID, PTypeName from ProgramType order by PTypeID");
                return ds.Tables[0];
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }
        public DataTable FetchSchemesByKam(int KamID)
        {
            try
            {
                DataSet ds = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.Text, "SELECT s.SchemeID, s.SchemeName FROM Scheme s INNER JOIN dbo.TSPDetail td ON s.SchemeID = td.SchemeID INNER JOIN dbo.TSPMaster tm ON td.TSPMasterID = tm.TSPMasterID WHERE s.IsApproved=1 AND tm.KamID = "+ KamID + "GROUP BY s.SchemeID, s.SchemeName;");
                return ds.Tables[0];
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }
        public DataTable FetchTSPsByKamScheme(int KamID, int SchemeID)
        {
            try
            {
                DataSet ds = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.Text, "SELECT td.TSPID, td.TSPName FROM dbo.TSPDetail td INNER JOIN dbo.TSPMaster tm ON td.TSPMasterID = tm.TSPMasterID WHERE tm.KamID = " + KamID + " AND td.SchemeID = "+ SchemeID +" GROUP BY td.TSPID, td.TSPName;");
                return ds.Tables[0];
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }
        public DataTable FetchSchemesByUsers(int UserID)
        {
            try
            {
                DataSet ds = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.Text, "SELECT s.SchemeID, s.SchemeName FROM Scheme s INNER JOIN dbo.TSPDetail td ON s.SchemeID = td.SchemeID INNER JOIN dbo.TSPMaster tm ON td.TSPMasterID = tm.TSPMasterID WHERE s.IsApproved=1 AND s.IsMigrated = 0 AND tm.UserID = " + UserID + "GROUP BY s.SchemeID, s.SchemeName;");
                return ds.Tables[0];
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }

        public DataTable FetchSchemesByGSRUsers(int UserID)
        {
            try
            {
                // Validate and ensure UserID is a valid integer
                if (UserID <= 0)
                    throw new ArgumentException("Invalid UserID");

                string query = @"
            SELECT s.SchemeID, s.SchemeName
            FROM Scheme s
            INNER JOIN dbo.TSPDetail td ON s.SchemeID = td.SchemeID
            INNER JOIN dbo.TSPMaster tm ON td.TSPMasterID = tm.TSPMasterID
            WHERE s.IsApproved = 1
              AND s.IsMigrated = 0
              AND tm.UserID = " + UserID + @"
              AND s.ProgramTypeID = 7
            GROUP BY s.SchemeID, s.SchemeName;";

                // Execute query
                DataSet ds = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.Text, query);
                return ds.Tables[0];
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }


        public DataTable FetchClassesBySchemeUser(int SchemeID, int UserID)
        {
            try
            {
                DataSet ds = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.Text, "SELECT c.ClassID, c.ClassCode FROM dbo.Class c INNER JOIN dbo.TSPDetail td ON c.TSPID = td.TSPID INNER JOIN dbo.TSPMaster tm ON td.TSPMasterID = tm.TSPMasterID WHERE tm.UserID = " + UserID + " AND td.SchemeID = " + SchemeID + " GROUP BY c.ClassID, c.ClassCode;");
                return ds.Tables[0];
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }
        public DataTable FetchSchemesByProgramCategory(int PCategoryID)
        {
            try
            {
                DataSet ds = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.Text, "Select SchemeID, SchemeName FROM dbo.Scheme s WHERE s.IsApproved = 1 and s.InActive = 0 AND (s.PCategoryID = " + PCategoryID + " OR " + PCategoryID + " = 0) order by s.SchemeID");
                return ds.Tables[0];
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }
        public DataTable FetchClasses()
        {
            try
            {
                DataSet ds = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.Text, "SELECT c.ClassID, c.ClassCode FROM dbo.Class c INNER JOIN dbo.TSPDetail td ON c.TSPID = td.TSPID INNER JOIN dbo.Scheme s ON s.SchemeID = td.SchemeID WHERE s.IsApproved = 1 ORDER BY c.ClassID, c.ClassCode;");
                return ds.Tables[0];
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }
        public DataTable FetchClassesByUser(int UserID)
        {
            try
            {
                DataSet ds = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.Text, "SELECT c.ClassID, c.ClassCode FROM dbo.Class c INNER JOIN dbo.TSPDetail td ON c.TSPID = td.TSPID INNER JOIN dbo.TSPMaster tm ON td.TSPMasterID = tm.TSPMasterID WHERE tm.UserID = " + UserID + " GROUP BY c.ClassID, c.ClassCode;");
                return ds.Tables[0];
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }
        #endregion filter
        #region Dashborad
        public List<TimeLineChart> FetchTraineeJourney(string type, int ID, string startDate, string endDate)
        {
            try
            {
                SqlParameter[] param = new SqlParameter[8];

                param[0] = new SqlParameter("@ClusterID", 0);
                param[1] = new SqlParameter("@DistrictID", 0);
                param[2] = new SqlParameter("@TradeID", 0);
                param[3] = new SqlParameter("@ProgramID", 0);
                param[4] = new SqlParameter("@SchemeID", 0);
                param[5] = new SqlParameter("@TspID", 0);
                param[6] = new SqlParameter("@StartDate", "");
                param[7] = new SqlParameter("@EndDate", "");

                UpdateTraineeJourneyParam(type, ID, startDate, endDate, ref param);

                DataSet ds = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), "DS_TraineeJourney", param);
                return SetTraineeJourneyData(ds);
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }
        public void UpdateTraineeJourneyParam(string type, int ID, string startDate, string endDate, ref SqlParameter[] param)
        {
            if (type == "Cluster") { param[0] = new SqlParameter("@ClusterID", ID); }
            else if (type == "District") { param[1] = new SqlParameter("@DistrictID", ID); }
            else if (type == "Trade") { param[2] = new SqlParameter("@TradeID", ID); }
            else if (type == "Program") { param[3] = new SqlParameter("@ProgramID", ID); }
            else if (type == "Scheme") { param[4] = new SqlParameter("@SchemeID", ID); }
            else if (type == "TSP") { param[5] = new SqlParameter("@TspID", ID); }
            else if (type == "Duration") { param[6] = new SqlParameter("@StartDate", startDate); param[7] = new SqlParameter("@EndDate", endDate); }
        }
        public List<TimeLineChart> SetTraineeJourneyData(DataSet data)
        {
            List<TimeLineChart> list = new List<TimeLineChart>();

            //1
            DataTable row = data.Tables[0];
            list.Add(new TimeLineChart(1, "Trainee OnBoarding", "<br>Women %: <b>" + row.Rows[0]["WomenPer"].ToString() + "</b>"
                + "<br>Disbaled %: <b>" + row.Rows[0]["DisablePer"].ToString() + "</b>"
                + "<br>Minority %: <b>" + row.Rows[0]["MinorPer"].ToString() + "</b>"
                + "<br>Contracted: <b>" + row.Rows[0]["Contracted"].ToString() + "</b>"
                + "<br>Enrolled: <b>" + row.Rows[0]["Enrolled"].ToString() + "</b>"
                , "Trainee OnBoarding"));
            list.Add(new TimeLineChart(2, "Beginning of Skills Training", "<br>Unverified Trainees: <b>" + row.Rows[0]["UnVerified"].ToString() + "</b>", "Beginning of Skills Training"));

            //2
            row = data.Tables[1];
            list.Add(new TimeLineChart(3, "Monthly Performance", "<br>Dropout Trainees: <b>" + row.Rows[0]["DropOuts"].ToString() + "</b>"
                + "<br>Expelled Trainees: <b>" + row.Rows[0]["Expelled"].ToString() + "</b>"
                + "<br><br><b>Violations</b>"
                + "<br>Minor: <b>" + row.Rows[0]["Minor"].ToString() + "</b>"
                + "<br>Major: <b>" + row.Rows[0]["Major"].ToString() + "</b>"
                + "<br>Serious: <b>" + row.Rows[0]["Serious"].ToString() + "</b>"
                , "Monthly Performance"));

            //3
            row = data.Tables[2];
            list.Add(new TimeLineChart(4, "Monthly Stipend", "<br>Stipend Disbursed: <b>" + Convert.ToInt64(row.Rows[0]["StipendAmount"]).ToString("n") + "</b>", "Monthly Stipend"));

            //4
            row = data.Tables[3];
            list.Add(new TimeLineChart(5, "Training Completion", "<br>Contract to Enroll %: <b>" + row.Rows[0]["ContractToEnrollRatio"].ToString() + "</b>"
                + "<br>Contract to Complete %: <b>" + row.Rows[0]["EnrollToCompleteRatio"].ToString() + "</b>"
                + "<br>Employed Income: <b>" + Convert.ToInt64(row.Rows[0]["EmployeedIncomeBeforeGraduation"]).ToString("n") + "</b>"
                + "<br>UnEmployeed Income: <b>" + Convert.ToInt64(row.Rows[0]["UnEmployeedIncomeBeforeGraduation"]).ToString("n") + "</b>"
                + "<br>Completed Trainees: <b>" + row.Rows[0]["Completed"].ToString() + "</b>"
                , "Training Completion"));

            //5
            row = data.Tables[4];
            list.Add(new TimeLineChart(6, "Exam & Certification", "<br>Passed Trainees: <b>" + row.Rows[0]["Passed"].ToString() + "</b>"
                + "<br>Contract to Passed %: <b>" + row.Rows[0]["ContractToPassedRatio"].ToString() + "</b>"
                + "<br>Complete to Passed %: <b>" + row.Rows[0]["CompleteToPassedRatio"].ToString() + "</b>"
                , "Exam & Certification"));

            //6
            row = data.Tables[5];
            list.Add(new TimeLineChart(7, "Employment / Placement", "<br>Placements: <b>" + row.Rows[0]["Placements"].ToString() + "</b>"
                + "<br>Verified Placements: <b>" + row.Rows[0]["PlacementsVerified"].ToString() + "</b>"
                //+ "<br>Complete to Employment Ratio: <b>" + row.Rows[0]["CompleteToEmpRatio"].ToString() + "</b>"
                //+ "<br>Contract to Employment Ratio: <b>" + row.Rows[0]["ContractToEmpRatio"].ToString() + "</b>"
                //+ "<br>Enroll to Report Ratio: <b>" + row.Rows[0]["EnrollToReportRatio"].ToString() + "</b>"
                + "<br>Contract to Report %: <b>" + row.Rows[0]["ContractToReportRatio"].ToString() + "</b>"
                + "<br>Complete to Report %: <b>" + row.Rows[0]["CompleteToReportRatio"].ToString() + "</b>"
                + "<br>Passed to Report %: <b>" + row.Rows[0]["PassedToReportRatio"].ToString() + "</b>"
                //+ "<br>Enroll to Verified Ratio: <b>" + row.Rows[0]["EnrollToVerifiedRatio"].ToString() + "</b>"
                //+ "<br>Contract to Verified Ratio: <b>" + row.Rows[0]["ContractToVerifiedRatio"].ToString() + "</b>"
                + "<br>Complete to Verified %: <b>" + row.Rows[0]["CompleteToVerifiedRatio"].ToString() + "</b>"
                //+ "<br>Contract to Employment Ratio: <b>" + row.Rows[0]["ContractToEmpRatio"].ToString() + "</b>"
                + "<br>Passed to Verified %: <b>" + row.Rows[0]["PassedToVerifiedRatio"].ToString() + "</b>"
                + "<br>Report to Verified %: <b>" + row.Rows[0]["ReportToVerifiedRatio"].ToString() + "</b>"
                , "Employment / Placement"));

            return list;
        }
        public List<TimeLineChart> FetchTraineeJourneySingle(string traineeCode, string traineeCNIC)
        {
            try
            {
                SqlParameter[] param = new SqlParameter[2];

                param[0] = new SqlParameter("@TraineeCode", traineeCode);
                param[1] = new SqlParameter("@TraineeCNIC", traineeCNIC);
                DataSet ds = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), "DS_TraineeJourneySingle", param);
                return SetTraineeJourneySingleData(ds);
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }
        public List<TimeLineChart> SetTraineeJourneySingleData(DataSet data)
        {
            List<TimeLineChart> list = new List<TimeLineChart>();
            //1
            DataTable row = data.Tables[0];
            string temp = "";
            if (row.Rows.Count <= 0)
            {
                return null;
            }

            temp = "<br>Name: <b>" + row.Rows[0]["TraineeName"].ToString() + "</b>"
                + "<br>CNIC: <b>" + row.Rows[0]["TraineeCNIC"].ToString() + "</b>"
                + "<br>Gender: <b>" + row.Rows[0]["Gender"].ToString() + "</b>"
                + "<br>District: <b>" + row.Rows[0]["District"].ToString() + "</b>"
                + "<br>TSP Name: <b>" + row.Rows[0]["TSPName"].ToString() + "</b>"
                + "<br>District of Training Centre: <b>" + row.Rows[0]["TrainingCenterDistrict"].ToString() + "</b>";

            list.Add(new TimeLineChart(1, "Trainee OnBoarding", temp, "Trainee OnBoarding"));
            //2
            temp = "<br>Verified: <b>" + row.Rows[0]["Verification"].ToString() + "</b>";
            if (row.Rows[0]["Verification"].ToString() == "Yes") { temp = temp + "<br>Verified on: <b>" + row.Rows[0]["VerificationDate"].ToString() + "</b>"; }
            list.Add(new TimeLineChart(2, "Verification Status", temp , "Verification Status"));

            //3
            row = data.Tables[1];

            if (row.Rows.Count > 0)
            {
                temp = "<br>";
                int i = 1;
                foreach (DataRow r in row.Rows)
                {
                    temp = temp + "<b>"+ i + ". " + r["Name"].ToString() + "</b>, Status: <b>" + r["Status"].ToString() + "</b>, IsMarginal: <b>" + r["IsMarginal"].ToString() + "</b>, IsExtra: <b>" + r["IsExtra"].ToString() + "</b>, Stipend: <b>" + r["StipendAmount"].ToString() + "</b><br>";
                    i++;
                }

                list.Add(new TimeLineChart(3, "Monthly Trainee Statuses", temp, "Monthly Trainee Statuses"));
            }
            else
            {
                list.Add(new TimeLineChart(3, "Monthly Trainee Statuses", "<br>NA", "Monthly Trainee Statuses"));
            }

            //4
            row = data.Tables[2];
            if (row.Rows.Count > 0)
            {
                list.Add(new TimeLineChart(4, "Exam & Certification", "<br>Certifying Agency: <b>" + row.Rows[0]["Agency"].ToString() + "</b>"
                    + "<br>Result: <b>" + row.Rows[0]["Status"].ToString() + "</b>"                    
                    , "Exam & Certification"));
            }
            else
            {
                list.Add(new TimeLineChart(4, "Exam & Certification", "<br>Certifying Agency: <b>NA</b>"
                    + "<br>Result: <b>NA</b>"
                    , "Exam & Certification"));
            }

            //5
            row = data.Tables[3];
            if (row.Rows.Count > 0)
            {
                list.Add(new TimeLineChart(5, "Employment", "<br>Employment Status: <b>" + row.Rows[0]["EmploymentStatus"].ToString() + "</b>"
                    + "<br>Employer: <b>" + row.Rows[0]["EmployerName"].ToString() + "</b>"
                    + "<br>Employment Verification: <b>" + row.Rows[0]["Verified"].ToString() + "</b>"
                    , "Employment"));
            }
            else
            {
                list.Add(new TimeLineChart(5, "Employment", "<br>Employment Sattus: <b>NA</b>"
                    + "<br>Employer Name: <b>NA</b>"
                    + "<br>Employment Verification: <b>NA</b>"
                    , "Employment"));
            }

            return list;
        }
        public ClassJourneyModel FetchClassJourney(int ClassID)
        {
            try
            {
                SqlParameter[] param = new SqlParameter[1];
                param[0] = new SqlParameter("@ClassID", ClassID);

                DataSet ds = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), "DS_ClassJourney", param);
                return SetClassJourneyData(ds);
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }
        public ClassJourneyModel SetClassJourneyData(DataSet data)
        {
            ClassJourneyModel journey = new ClassJourneyModel();
            List<TimeLineChart> list = new List<TimeLineChart>();
            int Enrolled = 0, Completed = 0;

            //1 Inception Report
            DataTable row = data.Tables[0];
            list.Add(new TimeLineChart(1, "Inception Report", "<br>Scheme: <b>" + row.Rows[0]["SchemeName"].ToString() + "</b>"
                + "<br>Trade: <b>" + row.Rows[0]["TradeName"].ToString() + "</b>"
                + "<br>Status: <b>" + row.Rows[0]["Status"].ToString() + "</b>"
                + "<br>Contractual Size: <b>" + row.Rows[0]["ContractualSize"].ToString() + "</b>"
                + "<br>Start Date: <b>" + row.Rows[0]["StartDate"].ToString() + "</b>"
                + "<br>Duration: <b>" + row.Rows[0]["Duration"].ToString() + "</b>"
                + "<br>ClassCode: <b>" + row.Rows[0]["ClassCode"].ToString() + "</b>"
                + "<br>Certifying Agency: <b>" + row.Rows[0]["CertifyingAgency"].ToString() + "</b>"
                , "Inception Report"));

            //2 Beginning of Skills Training
            row = data.Tables[1];
            Enrolled = Convert.ToInt32(row.Rows[0]["Enrolled"].ToString());
            list.Add(new TimeLineChart(2, "Beginning of Skills Training", "<br>Male Trainees: <b>" + row.Rows[0]["Male"].ToString() + "</b>"
                + "<br>Female Trainees: <b>" + row.Rows[0]["Female"].ToString() + "</b>"
                + "<br>District of Center: <b>" + row.Rows[0]["District"].ToString() + "</b>", "Beginning of Skills Training"));

            //3 Monthly Performance
            string temp = "", temp1 = "";
            row = data.Tables[2];
            if (row.Rows.Count > 0)
            {
                temp = "<br>";
                int i = 1;
                foreach (DataRow r in row.Rows)
                {
                    temp = temp + "<b>" + i + ". " + r["Month"].ToString() + "</b>, OnRoll: <b>" + r["OnRoll"].ToString() + "</b>, Expelled: <b>" + r["Expelled"].ToString() + "</b>, DropOut: <b>" + r["DropOut"].ToString() + "</b>, Marginal: <b>" + r["Marginal"].ToString() + "</b>, Verified: <b>" + r["Verified"].ToString() + "</b><br>";
                    i++;
                }
            }
            else
            {
                temp = "<br>NA";
            }

            //Violations on Tooltip
            row = data.Tables[3];
            if (row.Rows.Count > 0)
            {
                temp1 = "<br><b>Violation Summary</b><br>";
                int i = 1;
                foreach (DataRow r in row.Rows)
                {
                    temp1 = temp1 + "<b>" + i + ". " + r["Month"].ToString() + "</b>, Minor: <b>" + r["Minor"].ToString() + "</b>, Major: <b>" + r["Major"].ToString() + "</b>, Serious: <b>" + r["Serious"].ToString() + "</b>, Total: <b>" + (Convert.ToInt32(r["Minor"].ToString()) + Convert.ToInt32(r["Major"].ToString()) + Convert.ToInt32(r["Serious"].ToString())) + "</b><br>";
                    i++;
                }
            }
            else
            {
                temp1 = "<br>NA";
            }
            list.Add(new TimeLineChart(3, "Monthly Performance", temp, temp1));

            //4 Trainee Completion
            row = data.Tables[4];
            Completed = Convert.ToInt32(row.Rows[0]["Male"].ToString()) + Convert.ToInt32(row.Rows[0]["Female"].ToString());
            decimal EnrollToComp = 0;
            if (Enrolled > 0) { EnrollToComp = ((decimal)Completed / (decimal)Enrolled) * 100; }
            list.Add(new TimeLineChart(4, "Trainee Completion", "<br>Male: <b>" + row.Rows[0]["Male"].ToString() + "</b>"
                + "<br>Female: <b>" + row.Rows[0]["Female"].ToString() + "</b>"
                + "<br>Enroll to Complete %: <b>" + Math.Round(EnrollToComp,2).ToString() + "</b>"
                + "<br><br><b>Exam</b>"
                + "<br>Pass: <b>" + row.Rows[0]["Pass"].ToString() + "</b>"
                + "<br>Fail: <b>" + row.Rows[0]["Fail"].ToString() + "</b>"
                + "<br>Absent: <b>" + row.Rows[0]["Absent"].ToString() + "</b>"
                , "Trainee Completion"));

            //5 Employment / Placement
            row = data.Tables[5];
            list.Add(new TimeLineChart(5, "Employment / Placement", "<br>Commitment: <b>" + row.Rows[0]["EmploymentCommitment"].ToString() + "</b>"
                + "<br>Employed: <b>" + row.Rows[0]["Employed"].ToString() + "</b>"
                + "<br>UnEmployed: <b>" + row.Rows[0]["UnEmployed"].ToString() + "</b>"
                + "<br>Not Interested: <b>" + row.Rows[0]["NotInterested"].ToString() + "</b>"
                + "<br>Not Reported: <b>" + (Completed - (Convert.ToInt32(row.Rows[0]["Employed"].ToString()) + Convert.ToInt32(row.Rows[0]["UnEmployed"].ToString()) + Convert.ToInt32(row.Rows[0]["NotInterested"].ToString()))) + "</b>"
                + "<br>Employed Verified: <b>" + row.Rows[0]["EmployedVerified"].ToString() + "</b>"
                , "Employment / Placement"));

            // Rosi
            row = data.Tables[7];
            if (row.Rows.Count > 0)
            {
                temp1 = "<br>Actual(Reported): <b>" + row.Rows[0]["ActualRosi"].ToString() + "%</b>"
                    + "<br>Actual(Verified): <b>" + row.Rows[0]["VerifiedActualROSI"].ToString() + "%</b>"
                    + "<br>Contractual: <b>" + row.Rows[0]["ContractualROSI"].ToString() + "%</b>"
                    + "<br>Forcast(Reported): <b>" + row.Rows[0]["ReportedForcastedROSI"].ToString() + "%</b>"
                    + "<br>Forcast(Verified): <b>" + row.Rows[0]["VerifiedForcastedROSI"].ToString() + "%</b>";
            }
            else
            {
                temp1 = "<br>NA";
            }
            list.Add(new TimeLineChart(6, "ROSI", temp1, temp1));

            journey.ChartData = list;
            // Finance
            journey.Finance = data.Tables[6];

            return journey;
        }
        public ManagementDashboard FetchManagementDashboard(string type, int ID, string startDate, string endDate)
        {
            try
            {
                SqlParameter[] param = new SqlParameter[10];

                param[0] = new SqlParameter("@ClusterID", 0);
                param[1] = new SqlParameter("@DistrictID", 0);
                param[2] = new SqlParameter("@TradeID", 0);
                param[3] = new SqlParameter("@ProgramID", 0);
                param[4] = new SqlParameter("@SchemeID", 0);
                param[5] = new SqlParameter("@TspID", 0);
                param[6] = new SqlParameter("@StartDate", "");
                param[7] = new SqlParameter("@EndDate", "");
                param[8] = new SqlParameter("@ActualContractualFlag", false);
                param[9] = new SqlParameter("@VerifiedEmploymentROSIs", false);
                UpdateTraineeJourneyParam(type, ID, startDate, endDate, ref param);

                DataSet ds = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), "DS_Management", param);

                return SetManagementDashboardData(ds);
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }
        public ManagementDashboard SetManagementDashboardData(DataSet data)
        {
            ManagementDashboard md = new ManagementDashboard();

            //Scheme Defination
            //1
            List<GuageChart> GuageList = new List<GuageChart>();
            DataTable row = data.Tables[0];
            GuageList.Add(new GuageChart("Men", new List<GuageSeriesData> { new GuageSeriesData(ChartColors.Colors[0], "112%", "95%", Convert.ToDecimal(row.Rows[0]["MenPer"].ToString())) }));
            GuageList.Add(new GuageChart("Women", new List<GuageSeriesData> { new GuageSeriesData(ChartColors.Colors[1], "94%", "77%", Convert.ToDecimal(row.Rows[0]["WomenPer"].ToString())) }));
            GuageList.Add(new GuageChart("Minority", new List<GuageSeriesData> { new GuageSeriesData(ChartColors.Colors[3], "76%", "59%", Convert.ToDecimal(row.Rows[0]["MinorPer"].ToString())) }));
            GuageList.Add(new GuageChart("Disable", new List<GuageSeriesData> { new GuageSeriesData(ChartColors.Colors[2], "58%", "41%", Convert.ToDecimal(row.Rows[0]["DisablePer"].ToString())) }));
            md.SDGuage = GuageList;

            //2
            List<PieChart> MenWomenPieData = new List<PieChart>();
            MenWomenPieData.Add(new PieChart("Men", Convert.ToInt32(row.Rows[0]["Men"].ToString())));
            MenWomenPieData.Add(new PieChart("Women", Convert.ToInt32(row.Rows[0]["Women"].ToString())));
            md.SDPie = MenWomenPieData;

            md.SchemeDefination = row;

            //Monitoring
            row = data.Tables[1];
            List<PieChart> MnViolations = new List<PieChart>();
            MnViolations.Add(new PieChart("Major", Convert.ToInt32(row.Rows[0]["Major"].ToString())));
            MnViolations.Add(new PieChart("Minor", Convert.ToInt32(row.Rows[0]["Minor"].ToString())));
            MnViolations.Add(new PieChart("Serious", Convert.ToInt32(row.Rows[0]["Serious"].ToString())));
            md.Violations = MnViolations;

            List<PieChart> ExpDrops = new List<PieChart>();
            ExpDrops.Add(new PieChart("Expelled", Convert.ToInt32(row.Rows[0]["Expelled"].ToString())));
            ExpDrops.Add(new PieChart("DropOuts", Convert.ToInt32(row.Rows[0]["DropOuts"].ToString())));
            md.ExpelledDropouts = ExpDrops;

            md.Monitoring = data.Tables[2];

            //Completion & Certificate
            row = data.Tables[3];
            List<ChartData> Payments = new List<ChartData>();
            if (row.Rows.Count > 0)
            {                
                Payments.Add(new ChartData("Total Payable", Convert.ToDecimal(row.Rows[0]["TotalPayable"].ToString()), ChartColors.Colors[0]));
                Payments.Add(new ChartData("Payment Made", Convert.ToDecimal(row.Rows[0]["TotalPaymentMade"].ToString()), ChartColors.Colors[1]));
                Payments.Add(new ChartData("Deductions", Convert.ToDecimal(row.Rows[0]["TotalDeductions"].ToString()), ChartColors.Colors[2]));
            }

            md.Payments = Payments;

            md.ExamCertification = data.Tables[4];
            md.Placement = data.Tables[5];
            md.ROSI = data.Tables[6];

            return md;
        }
        #endregion Dashborad
    }
}
