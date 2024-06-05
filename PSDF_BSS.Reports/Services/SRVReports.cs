using PSDF_BSS.Reports.Interfaces;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace PSDF_BSS.Reports.Services
{
    public class SRVReports : ISRVReports
    {
        public DataTable GetPlacementReport(string type, int? ClusterID, int? DistrictID, int? TradeID, int? SectorID, int? SchemeID, int? ProgramTypeID, int? ProgramCategoryID)
        {
            try
            {
                List<SqlParameter> param = new List<SqlParameter>();
                param.Add(new SqlParameter("@Type", type));
                param.Add(new SqlParameter("@ClusterID", ClusterID == null ? 0 : ClusterID));
                param.Add(new SqlParameter("@DistrictID", DistrictID == null ? 0 : DistrictID));
                param.Add(new SqlParameter("@TradeID", TradeID == null ? 0 : TradeID));
                param.Add(new SqlParameter("@SectorID", SectorID == null ? 0 : SectorID));
                param.Add(new SqlParameter("@SchemeID", SchemeID == null ? 0 : SchemeID));
                param.Add(new SqlParameter("@ProgramTypeID", ProgramTypeID == null ? 0 : ProgramTypeID));
                param.Add(new SqlParameter("@ProgramCategoryID", ProgramCategoryID == null ? 0 : ProgramCategoryID));
                DataSet ds = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "dbo.RT_Placement", param.ToArray());
                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    return ds.Tables[0];
                }
                else
                    return null;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public DataTable GetSectorTraineeReport(int? SectorID, int? SchemeID, int? SchemeTypeID, int? ProgramCategoryID)
        {
            try
            {
                SqlParameter[] param = new SqlParameter[4];
                param[0] = new SqlParameter("@SectorID", SectorID == null ? 0 : SectorID);
                param[1] = new SqlParameter("@SchemeID", SchemeID == null ? 0 : SchemeID);
                param[2] = new SqlParameter("@SchemeTypeID", SchemeTypeID == null ? 0 : SchemeTypeID);
                param[3] = new SqlParameter("@ProgramCategoryID", ProgramCategoryID == null ? 0 : ProgramCategoryID);
                DataSet ds = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "dbo.RT_Sector_Trainee", param);
                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    return ds.Tables[0];
                }
                else
                    return null;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public DataTable GetFinancialComparisonOfSchemes(string reportType, int? ID, int scheme1ID, int scheme2ID, int scheme3ID)
        {
            try
            {
                SqlParameter[] param = new SqlParameter[5];
                param[0] = new SqlParameter("@ReportType", reportType);
                param[1] = new SqlParameter("@ID", ID);
                param[2] = new SqlParameter("@Scheme1ID", scheme1ID);
                param[3] = new SqlParameter("@Scheme2ID", scheme2ID);
                param[4] = new SqlParameter("@Scheme3ID", scheme3ID);
                DataSet ds = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "dbo.RT_Financial_Comparison_of_Schemes", param);
                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    return ds.Tables[0];
                }
                else
                    return null;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public DataTable GetLocations(string financialYear)
        {
            try
            {
                SqlParameter[] param = new SqlParameter[1];
                param[0] = new SqlParameter("@FinancialYear", financialYear);
                DataSet ds = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "dbo.RT_Locations", param);
                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    return ds.Tables[0];
                }
                else
                    return null;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public DataSet GetNewTradesTSPIndustryQuarterly(int SchemeID, string StartDate, string EndDate, int? ProgramTypeID, int? ProgramCategoryID)
        {
            try
            {
                List<SqlParameter> param = new List<SqlParameter>();
                param.Add(new SqlParameter("@SchemeID", SchemeID));
                param.Add(new SqlParameter("@FromDate", StartDate));
                param.Add(new SqlParameter("@ToDate", EndDate));
                param.Add(new SqlParameter("@ProgramTypeID", ProgramTypeID == null ? 0 : ProgramTypeID));
                param.Add(new SqlParameter("@ProgramCategoryID", ProgramCategoryID == null ? 0 : ProgramCategoryID));

                DataSet ds = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "dbo.RT_NewTrade_TSP_Industry_Quarterly", param.ToArray());
                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    return ds;
                }
                else
                    return null;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public DataTable GetTSPMasterReport(string type, int? SchemeID, int? GenderID, int? ProgramTypeID, int? ProgramCategoryID)
        {
            try
            {
                List<SqlParameter> param = new List<SqlParameter>();
                param.Add(new SqlParameter("@Type", type));
                param.Add(new SqlParameter("@SchemeID", SchemeID == null ? 0 : SchemeID));
                param.Add(new SqlParameter("@GenderID", GenderID == null ? 0 : GenderID));
                param.Add(new SqlParameter("@ProgramTypeID", ProgramTypeID == null ? 0 : ProgramTypeID));
                param.Add(new SqlParameter("@ProgramCategoryID", ProgramCategoryID == null ? 0 : ProgramCategoryID));
                DataSet ds = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "dbo.RT_TspMasterData", param.ToArray());
                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    return ds.Tables[0];
                }
                else
                    return null;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public DataTable GetTradeDataReport(string type, int? TradeID, int? GenderID, int? SchemeID, int? ProgramTypeID, int? ProgramCategoryID)
        {
            try
            {
                List<SqlParameter> param = new List<SqlParameter>();
                param.Add(new SqlParameter("@Type", type));
                param.Add(new SqlParameter("@TradeID", TradeID == null ? 0 : TradeID));
                param.Add(new SqlParameter("@GenderID", GenderID == null ? 0 : GenderID));
                param.Add(new SqlParameter("@SchemeID", SchemeID == null ? 0 : SchemeID));
                param.Add(new SqlParameter("@ProgramTypeID", ProgramTypeID == null ? 0 : ProgramTypeID));
                param.Add(new SqlParameter("@ProgramCategoryID", ProgramCategoryID == null ? 0 : ProgramCategoryID));
                DataSet ds = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "dbo.RT_TradeData", param.ToArray());
                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    return ds.Tables[0];
                }
                else
                    return null;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public DataTable GetTraineeStatusReport(string type, int? sectorID, int? clusterID, int? SchemeID, int? ProgramTypeID, int? ProgramCategoryID)
        {
            try
            {
                List<SqlParameter> param = new List<SqlParameter>();
                param.Add(new SqlParameter("@Type", type));
                param.Add(new SqlParameter("@SectorID", sectorID == null ? 0 : sectorID));
                param.Add(new SqlParameter("@ClusterID", clusterID == null ? 0 : clusterID));
                param.Add(new SqlParameter("@SchemeID", SchemeID == null ? 0 : SchemeID));
                param.Add(new SqlParameter("@ProgramTypeID", ProgramTypeID == null ? 0 : ProgramTypeID));
                param.Add(new SqlParameter("@ProgramCategoryID", ProgramCategoryID == null ? 0 : ProgramCategoryID));
                DataSet ds = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "dbo.RT_TraineeStatusReport", param.ToArray());
                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    return ds.Tables[0];
                }
                else
                    return null;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public DataTable GetTSPCurriculumReport(int? sourceofCurriculumID, int? durationID, int? educationTypeID, int? certAuthID, int? SchemeID, int? ProgramTypeID, int? ProgramCategoryID)
        {
            try
            {
                List<SqlParameter> param = new List<SqlParameter>();
                param.Add(new SqlParameter("@SourceofCurriculumID", sourceofCurriculumID == null ? 0 : sourceofCurriculumID));
                param.Add(new SqlParameter("@DurationID", durationID == null ? 0 : durationID));
                param.Add(new SqlParameter("@EducationTypeID", educationTypeID == null ? 0 : educationTypeID));
                param.Add(new SqlParameter("@CertAuthID", certAuthID == null ? 0 : certAuthID));
                param.Add(new SqlParameter("@SchemeID", SchemeID == null ? 0 : SchemeID));
                param.Add(new SqlParameter("@ProgramTypeID", ProgramTypeID == null ? 0 : ProgramTypeID));
                param.Add(new SqlParameter("@ProgramCategoryID", ProgramCategoryID == null ? 0 : ProgramCategoryID));
                DataSet ds = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "dbo.RT_TSPCurriculum", param.ToArray());
                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    return ds.Tables[0];
                }
                else
                    return null;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public DataTable GetChangeofInstructorReport(int? SchemeID, int? tspID, int? ProgramTypeID, int? ProgramCategoryID)
        {
            try
            {
                List<SqlParameter> param = new List<SqlParameter>();
                param.Add(new SqlParameter("@SchemeID", SchemeID == null ? 0 : SchemeID));
                param.Add(new SqlParameter("@TSPID", tspID == null ? 0 : tspID));
                param.Add(new SqlParameter("@ProgramTypeID", ProgramTypeID == null ? 0 : ProgramTypeID));
                param.Add(new SqlParameter("@ProgramCategoryID", ProgramCategoryID == null ? 0 : ProgramCategoryID));
                DataSet ds = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "dbo.RT_ChangeofInstructor", param.ToArray());
                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    return ds.Tables[0];
                }
                else
                    return null;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public DataTable GetChangeinAppendixReport(int? SchemeID, int? ProgramTypeID, int? ProgramCategoryID)
        {
            try
            {
                List<SqlParameter> param = new List<SqlParameter>();
                param.Add(new SqlParameter("@SchemeID", SchemeID == null ? 0 : SchemeID));
                param.Add(new SqlParameter("@ProgramTypeID", ProgramTypeID == null ? 0 : ProgramTypeID));
                param.Add(new SqlParameter("@ProgramCategoryID", ProgramCategoryID == null ? 0 : ProgramCategoryID));
                DataSet ds = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "dbo.RT_ChangeInAppendixReport", param.ToArray());
                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    return ds.Tables[0];
                }
                else
                    return null;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public DataTable GetFactSheetReport(int? schemeID)
        {
            try
            {
                SqlParameter[] param = new SqlParameter[1];
                param[0] = new SqlParameter("@SchemeID", schemeID == null ? 0 : schemeID);
                DataSet ds = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "dbo.SP_FactSheet", param);
                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    return ds.Tables[0];
                }
                else
                    return null;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public DataTable GetInvoiceStatusReport(int? kamID, int? schemeID, int? tspID, string Month)
        {
            try
            {
                List<SqlParameter> param = new List<SqlParameter>();
                param.Add(new SqlParameter("@KAMID", kamID == null ? 0 : kamID));
                param.Add(new SqlParameter("@SchemeID", schemeID == null ? 0 : schemeID));
                param.Add(new SqlParameter("@TSPID", tspID == null ? 0 : tspID));
                if (Month != null)
                    param.Add(new SqlParameter("@Month", Month));
                DataSet ds = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "dbo.RT_InvoiceStatusReport", param.ToArray());
                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    return ds.Tables[0];
                }
                else
                    return null;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public DataTable GetTraineeComplaintsReport(string type, int? kamID, int? schemeID, int? tspID)
        {
            try
            {
                List<SqlParameter> param = new List<SqlParameter>();
                param.Add(new SqlParameter("@Type", type == null ? "A" : type));
                param.Add(new SqlParameter("@KAMID", kamID == null ? 0 : kamID));
                param.Add(new SqlParameter("@SchemeID", schemeID == null ? 0 : schemeID));
                param.Add(new SqlParameter("@TSPID", tspID == null ? 0 : tspID));
                DataSet ds = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "dbo.RT_TraineeComplaint", param.ToArray());
                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    return ds.Tables[0];
                }
                else
                    return null;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public DataTable GetManagementReport(string type, int? SchemeID, int? TSPID, DateTime? Month)
        {
            try
            {
                SqlParameter[] param = new SqlParameter[3];
                param[0] = new SqlParameter("@SchemeID", SchemeID == null ? 0 : SchemeID);
                param[1] = new SqlParameter("@TSPID", TSPID == null ? 0 : TSPID);
                param[2] = new SqlParameter("@Type", type == null ? "A" : type);
                //if (Month != null)
                //{
                //    param[2] = new SqlParameter("@Month", Month);
                //}
                DataSet ds = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "dbo.RT_Management", param);
                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    return ds.Tables[0];
                }
                else
                    return null;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public DataTable GetStipendDisbursementReport(int? kamID, int? schemeID, int? tspID, string month)
        {
            try
            {
                List<SqlParameter> param = new List<SqlParameter>();
                param.Add(new SqlParameter("@KAMID", kamID == null ? 0 : kamID));
                param.Add(new SqlParameter("@SchemeID", schemeID == null ? 0 : schemeID));
                param.Add(new SqlParameter("@TSPID", tspID == null ? 0 : tspID));
                if (month != "null" && month != null)
                    param.Add(new SqlParameter("@Month", month));
                DataSet ds = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "dbo.RT_StipendDisbursementReport", param.ToArray());
                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    return ds.Tables[0];
                }
                else
                    return null;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public DataTable GetSDPMonthlyReport(int? ProjectTypeID, DateTime? Month)
        {
            try
            {
                SqlParameter[] param = new SqlParameter[2];
                param[0] = new SqlParameter("@ProjectTypeID", ProjectTypeID == null ? 0 : ProjectTypeID);
                if (Month != null)
                {
                    param[1] = new SqlParameter("@Month", Month);
                }
                DataSet ds = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "dbo.RT_SDP_Monthly_Report", param);
                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    return ds.Tables[0];
                }
                else
                    return null;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public DataTable GetTSPTraineeStatusReport(DateTime Month, int? TSPID, int? ClassID)
        {
            try
            {
                SqlParameter[] param = new SqlParameter[3];
                param[0] = new SqlParameter("@Month", Month);
                param[1] = new SqlParameter("@TSPID", TSPID == null ? 0 : TSPID);
                param[2] = new SqlParameter("@ClassID", ClassID == null ? 0 : ClassID);
                DataSet ds = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "dbo.RT_TSP_Trainee_Status", param);
                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    return ds.Tables[0];
                }
                else
                    return null;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public DataTable GetUnverifiedTraineeReport(int? TSPID, int? ClassID)
        {
            try
            {
                SqlParameter[] param = new SqlParameter[2];
                param[0] = new SqlParameter("@TSPID", TSPID == null ? 0 : TSPID);
                param[1] = new SqlParameter("@ClassID", ClassID == null ? 0 : ClassID);
                DataSet ds = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "dbo.RT_Unverified_Trainees", param);
                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    return ds.Tables[0];
                }
                else
                    return null;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public DataTable GetTraineeCostReport(int? ProgramTypeID, int? SchemeID, int? ClassID)
        {
            try
            {
                SqlParameter[] param = new SqlParameter[3];
                param[0] = new SqlParameter("@ProgramTypeID", ProgramTypeID == null ? 0 : ProgramTypeID);
                param[1] = new SqlParameter("@SchemeID", SchemeID == null ? 0 : SchemeID);
                param[2] = new SqlParameter("@ClassID", ClassID == null ? 0 : ClassID);
                DataSet ds = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "dbo.RT_Trainee_Average_Cost", param);
                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    return ds.Tables[0];
                }
                else
                    return null;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public DataTable GetTPMSummariesViolationsReport(int? schemeID, int? tspID, string month)
        {
            try
            {
                List<SqlParameter> param = new List<SqlParameter>();
                param.Add(new SqlParameter("@SchemeID", schemeID == null ? 0 : schemeID));
                param.Add(new SqlParameter("@TSPID", tspID == null ? 0 : tspID));
                if (month != "null" && month != null)
                    param.Add(new SqlParameter("@Month", month));
                DataSet ds = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "dbo.RT_TPMSummariesViolations", param.ToArray());
                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    return ds.Tables[0];
                }
                else
                    return null;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public DataTable GetSchemeUniqueTSPReport(int? SchemeID)
        {
            try
            {
                SqlParameter[] param = new SqlParameter[1];
                param[0] = new SqlParameter("@SchemeID", SchemeID == null ? 0 : SchemeID);
                DataSet ds = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "dbo.RT_Scheme_Unique_TSP", param);
                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    return ds.Tables[0];
                }
                else
                    return null;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public DataTable GetPDTraineeStatusReport(int? ProgramTypeID, int? TradeID, int? SchemeID, int? TSPID, int? Quarter, int? Year, DateTime? Month)
        {
            try
            {
                SqlParameter[] param = new SqlParameter[7];
                param[0] = new SqlParameter("@ProgramTypeID", ProgramTypeID == null ? 0 : ProgramTypeID);
                param[1] = new SqlParameter("@TradeID", TradeID == null ? 0 : TradeID);
                param[2] = new SqlParameter("@SchemeID", SchemeID == null ? 0 : SchemeID);
                param[3] = new SqlParameter("@TSPID", TSPID == null ? 0 : TSPID);
                param[4] = new SqlParameter("@Quarter", Quarter == null ? 0 : Quarter);
                param[5] = new SqlParameter("@Year", Year == null ? 0 : Year);
                if (Month != null)
                {
                    param[6] = new SqlParameter("@Month", Month);
                }
                DataSet ds = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "dbo.RT_PD_Trainee_Status", param);
                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    return ds.Tables[0];
                }
                else
                    return null;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public DataTable GetIncomeOfGraduateReport(int? SchemeID, int? TSPID, string EmploymentStatus, int? TraineeID)
        {
            try
            {
                List<SqlParameter> param = new List<SqlParameter>();
                param.Add(new SqlParameter("@SchemeID", SchemeID == null ? 0 : SchemeID));
                param.Add(new SqlParameter("@TSPID", TSPID == null ? 0 : TSPID));
                param.Add(new SqlParameter("@EmploymentStatus", EmploymentStatus == null ? "" : EmploymentStatus));
                param.Add(new SqlParameter("@TraineeID", TraineeID == null ? 0 : TraineeID));
                DataSet ds = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "dbo.RT_Income_Of_Graduate", param.ToArray());
                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    return ds.Tables[0];
                }
                else
                    return null;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public DataTable GetTPMSummariesTraineesReport(int? schemeID, int? tspID, string month)
        {
            try
            {
                List<SqlParameter> param = new List<SqlParameter>();
                param.Add(new SqlParameter("@SchemeID", schemeID == null ? 0 : schemeID));
                param.Add(new SqlParameter("@TSPID", tspID == null ? 0 : tspID));
                if (month != "null" && month != null)
                    param.Add(new SqlParameter("@Month", month));
                DataSet ds = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "dbo.RT_TPMSummariesTrainees", param.ToArray());
                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    return ds.Tables[0];
                }
                else
                    return null;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public DataTable GetTraineeTypePercentageReport(int? ProgramTypeID, int? ClusterID, int? SectorID)
        {
            try
            {
                List<SqlParameter> param = new List<SqlParameter>();
                param.Add(new SqlParameter("@ProgramTypeID", ProgramTypeID == null ? 0 : ProgramTypeID));
                param.Add(new SqlParameter("@ClusterID", ClusterID == null ? 0 : ClusterID));
                param.Add(new SqlParameter("@SectorID", SectorID == null ? 0 : SectorID));
                DataSet ds = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "dbo.RT_Trainee_Type_Percentage", param.ToArray());
                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    return ds.Tables[0];
                }
                else
                    return null;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public DataTable GetPDTop10TradesReport(string Type, int? SchemeID, int? SectorID)
        {
            try
            {
                List<SqlParameter> param = new List<SqlParameter>();
                param.Add(new SqlParameter("@Type", Type == null ? "Scheme" : Type));
                param.Add(new SqlParameter("@SchemeID", SchemeID == null ? 0 : SchemeID));
                param.Add(new SqlParameter("@SectorID", SectorID == null ? 0 : SectorID));
                DataSet ds = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "dbo.RT_Top_10_Trades", param.ToArray());
                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    return ds.Tables[0];
                }
                else
                    return null;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public DataTable GetPDPlacmentReport(string Type, int? SchemeID, int? SectorID, int? TradeID)
        {
            try
            {
                List<SqlParameter> param = new List<SqlParameter>();
                param.Add(new SqlParameter("@Type", Type == null ? "Scheme" : Type));
                param.Add(new SqlParameter("@SchemeID", SchemeID == null ? 0 : SchemeID));
                param.Add(new SqlParameter("@SectorID", SectorID == null ? 0 : SectorID));
                param.Add(new SqlParameter("@TradeID", TradeID == null ? 0 : TradeID));
                DataSet ds = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "dbo.RT_PD_Placement_Report", param.ToArray());
                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    return ds.Tables[0];
                }
                else
                    return null;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public DataTable GetPDPPlayersUnderTrainingReport(int? SchemeID)
        {
            try
            {
                List<SqlParameter> param = new List<SqlParameter>();
                param.Add(new SqlParameter("@SchemeID", SchemeID == null ? 0 : SchemeID));
                DataSet ds = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "dbo.RT_Players_Under_Training", param.ToArray());
                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    return ds.Tables[0];
                }
                else
                    return null;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public DataTable GetPDPCostSavingReport(DateTime? StartDate, DateTime? EndDate)
        {
            try
            {
                List<SqlParameter> param = new List<SqlParameter>();
                if (StartDate != null)
                {
                    param.Add(new SqlParameter("@StartDate", StartDate));
                    param.Add(new SqlParameter("@EndDate", EndDate));
                }

                DataSet ds = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "dbo.RT_Cost_Saving", param.ToArray());
                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    return ds.Tables[0];
                }
                else
                    return null;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public DataTable GetCostSharingPartnersReport(int? SchemeID)
        {
            try
            {
                List<SqlParameter> param = new List<SqlParameter>();
                param.Add(new SqlParameter("@SchemeID", SchemeID == null ? 0 : SchemeID));
                DataSet ds = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "dbo.RT_Cost_Sharing_Partners", param.ToArray());
                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    return ds.Tables[0];
                }
                else
                    return null;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public DataTable GetBusinessPartnerInvoiceStatusReport(string Month)
        {
            try
            {
                List<SqlParameter> param = new List<SqlParameter>();
                param.Add(new SqlParameter("@Month", Month));
                DataSet ds = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "dbo.RT_BusinessPartnerInvoiceStatus", param.ToArray());
                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    return ds.Tables[0];
                }
                else
                    return null;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public DataTable GeTspInvoiceStatus(int? SchemeID, int? TSPID)
        {
            try
            {
                List<SqlParameter> param = new List<SqlParameter>();
                param.Add(new SqlParameter("@SchemeID", SchemeID));
                param.Add(new SqlParameter("@TSPID", TSPID));
                DataSet ds = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "dbo.RT_TspInvoiceStatusReport", param.ToArray());
                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    return ds.Tables[0];
                }
                else
                    return null;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}