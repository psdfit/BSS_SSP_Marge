using System;
using System.Data;
using System.IO;
using System.Net;
using System.Runtime.Caching;
using System.Web.Mvc;
using CrystalDecisions.Shared;
using PSDF_BSS.Reports.Interfaces;
using PSDF_BSS.Reports.Models;
using CrystalDecisions.CrystalReports.Engine;

namespace PSDF_BSS.Reports.Controllers
{
    public class ReportsController : Controller
    {
        private readonly ISRVReports srvRPT;

        public ReportsController(ISRVReports srvRPT)
        {
            this.srvRPT = srvRPT;
        }
        //1.5: Placement Report
        [HttpGet]
        public void GetPlacement(string reportType, int? clusterID, int? districtID, int? tradeID, int? sectorID, int? schemeID, int? programTypeID, int? programCategoryID, string downloadType)
        {
            if (String.IsNullOrEmpty(reportType)) { Response.Redirect("~/ReportViewer"); }
            var model = srvRPT.GetPlacementReport(reportType, districtID, clusterID, tradeID, sectorID, schemeID, programTypeID, programCategoryID);
            //(Path.Combine(Server.MapPath("~/Crystal_Reports/Reports"), "Placement.rpt"));
            string fileName = "Placement.rpt";
            string key = "Placement";
            ParameterModel pm = new ParameterModel
            {
                MyType = reportType,
                Scheme = schemeID == null ? 0 : schemeID,
                ProgramType = programTypeID == null ? 0 : programTypeID,
                ProgramCategory = programCategoryID == null ? 0 : programCategoryID
            };
            var cache = MemoryCache.Default;
            CacheItemPolicy cacheItemPolicy = new CacheItemPolicy();
            if (model != null)
            {
                cache.Set(key + "_rptName", fileName, cacheItemPolicy);
                cache.Set(key + "_rptSource", model, cacheItemPolicy);
            }
            else
                cache.Set(key + "_rptName", "", cacheItemPolicy);
            cache.Set(key + "_rptParam", pm, cacheItemPolicy);
            Response.Redirect("~/ReportViewer?" + key);
        }
        //1.6: Sector Wise Trainee Report
        [HttpGet]
        public void GetSectorTrainee(int? sectorID, int? schemeID, int? programTypeID, int? programCategoryID, string downloadType)
        {
            var model = srvRPT.GetSectorTraineeReport(sectorID, schemeID, programTypeID, programCategoryID);
            //(Path.Combine(Server.MapPath("~/Crystal_Reports/Reports"), "SectorTrainee.rpt"));
            string fileName = "SectorTrainee.rpt";
            string key = "SectorTrainee";
            ParameterModel pm = new ParameterModel
            {
                MyschemeID = schemeID == null ? 0 : schemeID,
                MySchemeTypeID = programTypeID == null ? 0 : programTypeID,
                ProgramCategory = programCategoryID == null ? 0 : programCategoryID
            };
            var cache = MemoryCache.Default;
            CacheItemPolicy cacheItemPolicy = new CacheItemPolicy();
            if (model != null)
            {
                cache.Set(key + "_rptName", fileName, cacheItemPolicy);
                cache.Set(key + "_rptSource", model, cacheItemPolicy);
            }
            else
                cache.Set(key + "_rptName", "", cacheItemPolicy);
            cache.Set(key + "_rptParam", pm, cacheItemPolicy);
            Response.Redirect("~/ReportViewer?" + key);
        }
        //1.7: Financial Comparison Of Schemes
        [HttpGet]
        public void GetFinancialComparisonOfSchemes(string reportType, int? ID, int scheme1ID, int scheme2ID, int scheme3ID, string downloadType)
        {
            var model = srvRPT.GetFinancialComparisonOfSchemes(reportType, ID, scheme1ID, scheme2ID, scheme3ID);
            //(Path.Combine(Server.MapPath("~/Crystal_Reports/Reports"), "FinancialComparisonOfSchemes.rpt"));
            string fileName = "FinancialComparisonOfSchemes.rpt";
            string key = "FinancialComparisonOfSchemes";
            ParameterModel pm = new ParameterModel
            {
                MyReportType = reportType,
            };
            var cache = MemoryCache.Default;
            CacheItemPolicy cacheItemPolicy = new CacheItemPolicy();
            if (model != null)
            {
                cache.Set(key + "_rptName", fileName, cacheItemPolicy);
                cache.Set(key + "_rptSource", model, cacheItemPolicy);
            }
            else
                cache.Set(key + "_rptName", "", cacheItemPolicy);
            cache.Set(key + "_rptParam", pm, cacheItemPolicy);
            Response.Redirect("~/ReportViewer?" + key);
        }
        //1.8: Total Locations Report
        [HttpGet]
        public void GetLocations(string financialYear, int? schemeID, int? programTypeID, int? programCategoryID, string downloadType)
        {
            if (String.IsNullOrEmpty(financialYear)) { financialYear = "2020-2021"; }
            var model = srvRPT.GetLocations(financialYear);
            //(Path.Combine(Server.MapPath("~/Crystal_Reports/Reports"), "Locations.rpt"));
            string fileName = "Locations.rpt";
            string key = "Locations";
            var cache = MemoryCache.Default;
            CacheItemPolicy cacheItemPolicy = new CacheItemPolicy();
            if (model != null)
            {
                cache.Set(key + "_rptName", fileName, cacheItemPolicy);
                cache.Set(key + "_rptSource", model, cacheItemPolicy);
            }
            else
                cache.Set(key + "_rptName", "", cacheItemPolicy);
            Response.Redirect("~/ReportViewer?" + key);
        }
        //1.11: New Trades, Tsp, Industry Quarterly
        [HttpGet]
        public void NewTradesTSPIndustryQuarterly(int schemeID, string StartDate, string EndDate, int? programTypeID, int? programCategoryID, string downloadType)
        {
            //DateTime? date = null;
            //if (!string.IsNullOrEmpty(Year))
            //    date = DateTime.ParseExact(Year, "yyyy/MM/dd", null);
            //int year = 0;
            //if (date != null)
            //{
            //    year = date.Value.Year;
            //    Quarter = (date.Value.Month - 1) / 3 + 1;
            //}

            DataSet model = srvRPT.GetNewTradesTSPIndustryQuarterly(schemeID, StartDate, EndDate, programTypeID, programCategoryID);
            //(Path.Combine(Server.MapPath("~/Crystal_Reports/Reports"), "NewTradesTspIndustryQuarterly.rpt"));
            string fileName = "NewTradesTspIndustryQuarterly.rpt";
            string key = "NewTradesTSPIndustryQuarterly";
            var cache = MemoryCache.Default;
            CacheItemPolicy cacheItemPolicy = new CacheItemPolicy();
            if (model != null)
            {
                cache.Set(key + "_rptName", fileName, cacheItemPolicy);
                cache.Set(key + "_rptSource", model.Tables[0], cacheItemPolicy);
                cache.Set(key + "_rptSource1", model.Tables[1], cacheItemPolicy);
                cache.Set(key + "_rptSource2", model.Tables[2], cacheItemPolicy);
            }
            else
                cache.Set(key + "_rptName", "", cacheItemPolicy);
            Response.Redirect("~/ReportViewer?" + key);
        }
        //1.1:	TSP Master Data
        [HttpGet]
        public void GetTSPMasterReport(string reportType, int? schemeID, int? genderID, int? programTypeID, int? programCategoryID, string downloadType)
        {
            if (string.IsNullOrEmpty(reportType))
            {
                Response.Redirect("~/ReportViewer");
            }
            var model = srvRPT.GetTSPMasterReport(reportType, schemeID, genderID, programTypeID, programCategoryID);
            //(Path.Combine(Server.MapPath("~/Crystal_Reports/Reports"), "TSPMasterData.rpt"));
            string fileName = "TSPMasterData.rpt";
            string key = "TSPMasterReport";
            ParameterModel pm = new ParameterModel
            {
                MyType = reportType,
                Scheme = schemeID == null ? 0 : schemeID,
                ProgramType = programTypeID == null ? 0 : programTypeID,
                ProgramCategory = programCategoryID == null ? 0 : programCategoryID
            };
            var cache = MemoryCache.Default;
            CacheItemPolicy cacheItemPolicy = new CacheItemPolicy();
            if (model != null)
            {
                cache.Set(key + "_rptName", fileName, cacheItemPolicy);
                cache.Set(key + "_rptSource", model, cacheItemPolicy);
            }
            else
                cache.Set(key + "_rptName", "", cacheItemPolicy);
            cache.Set(key + "_rptParam", pm, cacheItemPolicy);
            Response.Redirect("~/ReportViewer?" + key);
        }
        //1.2:  Trade
        [HttpGet]
        public void GetTradeDataReport(string reportType, int? tradeID, int? genderID, int? schemeID, int? programTypeID, int? programCategoryID, string downloadType)
        {
            if (string.IsNullOrEmpty(reportType)) { Response.Redirect("~/ReportViewer"); }
            var model = srvRPT.GetTradeDataReport(reportType, tradeID, genderID, schemeID, programTypeID, programCategoryID);
            //(Path.Combine(Server.MapPath("~/Crystal_Reports/Reports"), "TradeData.rpt"));
            string fileName = "TradeData.rpt";
            ParameterModel pm = new ParameterModel
            {
                MyType = reportType,
                Scheme = schemeID == null ? 0 : schemeID,
                ProgramType = programTypeID == null ? 0 : programTypeID,
                ProgramCategory = programCategoryID == null ? 0 : programCategoryID
            };
            string key = "TradeDataReport";
            var cache = MemoryCache.Default;
            CacheItemPolicy cacheItemPolicy = new CacheItemPolicy();
            if (model != null)
            {
                cache.Set(key + "_rptName", fileName, cacheItemPolicy);
                cache.Set(key + "_rptSource", model, cacheItemPolicy);
            }
            else
                cache.Set(key + "_rptName", "", cacheItemPolicy);
            cache.Set(key + "_rptParam", pm, cacheItemPolicy);
            Response.Redirect("~/ReportViewer?" + key);
        }
        //1.3:  Trainee Status
        [HttpGet]
        public void GetTraineeStatusReport(string reportType, int? sectorID, int? clusterID, int? schemeID, int? programTypeID, int? programCategoryID, string downloadType)
        {
            if (String.IsNullOrEmpty(reportType)) { Response.Redirect("~/ReportViewer"); }
            var model = srvRPT.GetTraineeStatusReport(reportType, sectorID, clusterID, schemeID, programTypeID, programCategoryID);
            //rd.Load(Path.Combine(Server.MapPath("~/Crystal_Reports/Reports"), "TraineeStatusReport.rpt"));
            string fileName = "TraineeStatusReport.rpt";
            ParameterModel pm = new ParameterModel
            {
                MyType = reportType,
                Scheme = schemeID == null ? 0 : schemeID,
                ProgramType = programTypeID == null ? 0 : programTypeID,
                ProgramCategory = programCategoryID == null ? 0 : programCategoryID
            };
            string key = "TraineeStatusReport";
            var cache = MemoryCache.Default;
            CacheItemPolicy cacheItemPolicy = new CacheItemPolicy();
            if (model != null)
            {
                cache.Set(key + "_rptName", fileName, cacheItemPolicy);
                cache.Set(key + "_rptSource", model, cacheItemPolicy);
            }
            else
                cache.Set(key + "_rptName", "", cacheItemPolicy);
            cache.Set(key + "_rptParam", pm, cacheItemPolicy);
            Response.Redirect("~/ReportViewer?" + key);
        }
        //1.4:  TSP Curriculum
        [HttpGet]
        public void GetTSPCurriculumReport(string reportType, int? sourceofCurriculumID, int? durationID, int? educationTypeID, int? certAuthID, int? schemeID, int? programTypeID, int? programCategoryID, string downloadType)
        {
            if (String.IsNullOrEmpty(reportType)) { Response.Redirect("~/ReportViewer"); }
            var model = srvRPT.GetTSPCurriculumReport(sourceofCurriculumID, durationID, educationTypeID, certAuthID, schemeID, programTypeID, programCategoryID);
            //rd.Load(Path.Combine(Server.MapPath("~/Crystal_Reports/Reports"), "TSPCurriculum.rpt"));
            string fileName = "TSPCurriculum.rpt";
            string key = "TSPCurriculumReport";
            ParameterModel pm = new ParameterModel
            {
                MyType = reportType,
                Scheme = schemeID == null ? 0 : schemeID,
                ProgramType = programTypeID == null ? 0 : programTypeID,
                ProgramCategory = programCategoryID == null ? 0 : programCategoryID
            };
            var cache = MemoryCache.Default;
            CacheItemPolicy cacheItemPolicy = new CacheItemPolicy();
            if (model != null)
            {
                cache.Set(key + "_rptName", fileName, cacheItemPolicy);
                cache.Set(key + "_rptSource", model, cacheItemPolicy);
            }
            else
                cache.Set(key + "_rptName", "", cacheItemPolicy);
            cache.Set(key + "_rptParam", pm, cacheItemPolicy);
            Response.Redirect("~/ReportViewer?" + key);
        }
        //1.12:  Change of Instructor
        [HttpGet]
        public void GetChangeofInstructorReport(int? schemeID, int? tspID, int? programTypeID, int? programCategoryID, string downloadType)
        {
            var model = srvRPT.GetChangeofInstructorReport(schemeID, tspID, programTypeID, programCategoryID);
            //(Path.Combine(Server.MapPath("~/Crystal_Reports/Reports"), "ChangeofInstructor.rpt"));
            string fileName = "ChangeofInstructor.rpt";
            string key = "ChangeofInstructorReport";
            ParameterModel pm = new ParameterModel
            {
                Scheme = schemeID == null ? 0 : schemeID,
                ProgramType = programTypeID == null ? 0 : programTypeID,
                ProgramCategory = programCategoryID == null ? 0 : programCategoryID
            };
            var cache = MemoryCache.Default;
            CacheItemPolicy cacheItemPolicy = new CacheItemPolicy();
            if (model != null)
            {
                cache.Set(key + "_rptName", fileName, cacheItemPolicy);
                cache.Set(key + "_rptSource", model, cacheItemPolicy);
            }
            else
                cache.Set(key + "_rptName", "", cacheItemPolicy);
            cache.Set(key + "_rptParam", pm, cacheItemPolicy);
            Response.Redirect("~/ReportViewer?" + key);
        }
        //1.13:Change in Appendix Report
        [HttpGet]
        public void GetChangeinAppendixReport(int? schemeID, int? programTypeID, int? programCategoryID, string downloadType)
        {
            var model = srvRPT.GetChangeinAppendixReport(schemeID, programTypeID, programCategoryID);
            //(Path.Combine(Server.MapPath("~/Crystal_Reports/Reports"), "ChangeInAppendixReport.rpt"));
            string fileName = "ChangeInAppendixReport.rpt";
            string key = "ChangeinAppendixReport";
            var cache = MemoryCache.Default;
            CacheItemPolicy cacheItemPolicy = new CacheItemPolicy();
            if (model != null)
            {
                cache.Set(key + "_rptName", fileName, cacheItemPolicy);
                cache.Set(key + "_rptSource", model, cacheItemPolicy);
            }
            else
                cache.Set(key + "_rptName", "", cacheItemPolicy);
            Response.Redirect("~/ReportViewer?" + key);

        }
        //1.14:  Feet Sheet
        [HttpGet]
        public void GetFactSheetReport(int? schemeID, string downloadType)
        {
            var model = srvRPT.GetFactSheetReport(schemeID);
            //(Path.Combine(Server.MapPath("~/Crystal_Reports/Reports"), "FactSheet.rpt"));
            string fileName = "FactSheet.rpt";
            string key = "FactSheetReport";
            var cache = MemoryCache.Default;
            CacheItemPolicy cacheItemPolicy = new CacheItemPolicy();
            if (model != null)
            {
                cache.Set(key + "_rptName", fileName, cacheItemPolicy);
                cache.Set(key + "_rptSource", model, cacheItemPolicy);
            }
            else
                cache.Set(key + "_rptName", "", cacheItemPolicy);
            Response.Redirect("~/ReportViewer?" + key);
        }
        //2.1:  Invoice Status Report
        [HttpGet]
        public void GetInvoiceStatusReport(int? kamID, int? schemeID, int? tspID, string Month, string downloadType)
        {
            var model = srvRPT.GetInvoiceStatusReport(kamID, schemeID, tspID, Month);
            //(Path.Combine(Server.MapPath("~/Crystal_Reports/Reports"), "InvoiceStatusReport.rpt"));
            string fileName = "InvoiceStatusReport.rpt";
            string key = "InvoiceStatusReport";
            var cache = MemoryCache.Default;
            CacheItemPolicy cacheItemPolicy = new CacheItemPolicy();
            if (model != null)
            {
                cache.Set(key + "_rptName", fileName, cacheItemPolicy);
                cache.Set(key + "_rptSource", model, cacheItemPolicy);
            }
            else
                cache.Set(key + "_rptName", "", cacheItemPolicy);
            Response.Redirect("~/ReportViewer?" + key);
        }
        //2.3 Trainee Complaints
        [HttpGet]
        public void GetTraineeComplaintsReport(string reportType, int? kamID, int? schemeID, int? tspID, string downloadType)
        {
            //if (String.IsNullOrEmpty(reportType)) { Response.Redirect("~/ReportViewer"); }
            var model = srvRPT.GetTraineeComplaintsReport(reportType, kamID, schemeID, tspID);
            //(Path.Combine(Server.MapPath("~/Crystal_Reports/Reports"), "TraineeComplaint.rpt"));
            string fileName = "TraineeComplaint.rpt";
            string key = "TraineeComplaintsReport";
            ParameterModel pm = new ParameterModel
            {
                MyType = reportType
            };
            var cache = MemoryCache.Default;
            CacheItemPolicy cacheItemPolicy = new CacheItemPolicy();
            if (model != null)
            {
                cache.Set(key + "_rptName", fileName, cacheItemPolicy);
                cache.Set(key + "_rptSource", model, cacheItemPolicy);
            }
            else
                cache.Set(key + "_rptName", "", cacheItemPolicy);
            cache.Set(key + "_rptParam", pm, cacheItemPolicy);
            Response.Redirect("~/ReportViewer?" + key);
        }
        //2.6 Management Report
        [HttpGet]
        public void GetManagementReport(string reportType, int? schemeID, int? tspID, DateTime? Month, string downloadType)
        {
            //if (String.IsNullOrEmpty(reportType)) { Response.Redirect("~/ReportViewer"); }
            var model = srvRPT.GetManagementReport(reportType, schemeID, tspID, Month);
            //(Path.Combine(Server.MapPath("~/Crystal_Reports/Reports"), "Management.rpt"));
            string fileName = "Management.rpt";
            string key = "ManagementReport";
            ParameterModel pm = new ParameterModel
            {
                MyType = reportType
            };
            var cache = MemoryCache.Default;
            CacheItemPolicy cacheItemPolicy = new CacheItemPolicy();
            if (model != null)
            {
                cache.Set(key + "_rptName", fileName, cacheItemPolicy);
                cache.Set(key + "_rptSource", model, cacheItemPolicy);
            }
            else
                cache.Set(key + "_rptName", "", cacheItemPolicy);
            cache.Set(key + "_rptParam", pm, cacheItemPolicy);
            Response.Redirect("~/ReportViewer?" + key);
        }
        //2.2: Stipend Disbursement Report
        [HttpGet]
        public void GetStipendDisbursementReport(int? kamID, int? schemeID, int? tspID, string month, string downloadType)
        {
            var model = srvRPT.GetStipendDisbursementReport(kamID, schemeID, tspID, month);
            //Load(Path.Combine(Server.MapPath("~/Crystal_Reports/Reports"), "StipendDisbursementReport.rpt"));
            string fileName = "StipendDisbursementReport.rpt";
            string key = "StipendDisbursementReport";
            var cache = MemoryCache.Default;
            CacheItemPolicy cacheItemPolicy = new CacheItemPolicy();
            if (model != null)
            {
                cache.Set(key + "_rptName", fileName, cacheItemPolicy);
                cache.Set(key + "_rptSource", model, cacheItemPolicy);
            }
            else
                cache.Set(key + "_rptName", "", cacheItemPolicy);
            Response.Redirect("~/ReportViewer?" + key);
        }
        //2.8 SDP Monhtly Report
        [HttpGet]
        public void GetSDPMonthlyReport(int? projectTypeID, DateTime? Month, string downloadType)
        {
            //if (String.IsNullOrEmpty(reportType)) { Response.Redirect("~/ReportViewer"); }
            var model = srvRPT.GetSDPMonthlyReport(projectTypeID, Month);
            //(Path.Combine(Server.MapPath("~/Crystal_Reports/Reports"), "SDPMonthly.rpt"));
            string fileName = "SDPMonthly.rpt";
            string key = "SDPMonthlyReport";
            var cache = MemoryCache.Default;
            CacheItemPolicy cacheItemPolicy = new CacheItemPolicy();
            if (model != null)
            {
                cache.Set(key + "_rptName", fileName, cacheItemPolicy);
                cache.Set(key + "_rptSource", model, cacheItemPolicy);
            }
            else
                cache.Set(key + "_rptName", "", cacheItemPolicy);
            Response.Redirect("~/ReportViewer?" + key);
        }
        //3.1 TSP Trainee Status
        [HttpGet]
        public void GetTSPTraineeStatusReport(DateTime Month, int? tspID, int? classID, string downloadType)
        {
            var model = srvRPT.GetTSPTraineeStatusReport(Month, tspID, classID);
            //(Path.Combine(Server.MapPath("~/Crystal_Reports/Reports"), "TSPTraineeStatus.rpt"));
            string fileName = "TSPTraineeStatus.rpt";
            string key = "TSPTraineeStatusReport";
            var cache = MemoryCache.Default;
            CacheItemPolicy cacheItemPolicy = new CacheItemPolicy();
            if (model != null)
            {
                cache.Set(key + "_rptName", fileName, cacheItemPolicy);
                cache.Set(key + "_rptSource", model, cacheItemPolicy);
            }
            else
                cache.Set(key + "_rptName", "", cacheItemPolicy);
            Response.Redirect("~/ReportViewer?" + key);

        }
        //3.2 Unverified Trainees
        [HttpGet]
        public void GetUnverifiedTraineeReport(int? tspID, int? classID, string downloadType)
        {
            var model = srvRPT.GetUnverifiedTraineeReport(tspID, classID);
            //(Path.Combine(Server.MapPath("~/Crystal_Reports/Reports"), "UnverifiedTrainee.rpt"));
            string fileName = "UnverifiedTrainee.rpt";
            string key = "UnverifiedTraineeReport";
            var cache = MemoryCache.Default;
            CacheItemPolicy cacheItemPolicy = new CacheItemPolicy();
            if (model != null)
            {
                cache.Set(key + "_rptName", fileName, cacheItemPolicy);
                cache.Set(key + "_rptSource", model, cacheItemPolicy);
            }
            else
                cache.Set(key + "_rptName", "", cacheItemPolicy);
            Response.Redirect("~/ReportViewer?" + key);
        }
        //4.1 Trainee Cost
        [HttpGet]
        public void GetTraineeCostReport(int? programTypeID, int? schemeID, int? ClassID, string downloadType)
        {
            var model = srvRPT.GetTraineeCostReport(programTypeID, schemeID, ClassID);
            //(Path.Combine(Server.MapPath("~/Crystal_Reports/Reports"), "TraineeCostMonthly.rpt"));
            string fileName = "TraineeCostMonthly.rpt";
            string key = "TraineeCostReport";
            var cache = MemoryCache.Default;
            CacheItemPolicy cacheItemPolicy = new CacheItemPolicy();
            if (model != null)
            {
                cache.Set(key + "_rptName", fileName, cacheItemPolicy);
                cache.Set(key + "_rptSource", model, cacheItemPolicy);
            }
            else
                cache.Set(key + "_rptName", "", cacheItemPolicy);
            Response.Redirect("~/ReportViewer?" + key);
        }
        //2.4 TPM Summaries - Violations
        [HttpGet]
        public void GetTPMSummariesViolationsReport(int? schemeID, int? tspID, string month, string downloadType)
        {
            var model = srvRPT.GetTPMSummariesViolationsReport(schemeID, tspID, month);
            //(Path.Combine(Server.MapPath("~/Crystal_Reports/Reports"), "TPMSummariesViolations.rpt"));
            string fileName = "TPMSummariesViolations.rpt";
            string key = "TPMSummariesViolationsReport";
            var cache = MemoryCache.Default;
            CacheItemPolicy cacheItemPolicy = new CacheItemPolicy();
            if (model != null)
            {
                cache.Set(key + "_rptName", fileName, cacheItemPolicy);
                cache.Set(key + "_rptSource", model, cacheItemPolicy);
            }
            else
                cache.Set(key + "_rptName", "", cacheItemPolicy);
            Response.Redirect("~/ReportViewer?" + key);
        }
        //4.2 Scheme Unique TSPs
        [HttpGet]
        public void GetSchemeUniqueTSPReport(int? schemeID, string downloadType)
        {
            var model = srvRPT.GetSchemeUniqueTSPReport(schemeID);
            //(Path.Combine(Server.MapPath("~/Crystal_Reports/Reports"), "SchemeUniqueTSP.rpt"));
            string fileName = "SchemeUniqueTSP.rpt";
            string key = "SchemeUniqueTSPReport";
            var cache = MemoryCache.Default;
            CacheItemPolicy cacheItemPolicy = new CacheItemPolicy();
            if (model != null)
            {
                cache.Set(key + "_rptName", fileName, cacheItemPolicy);
                cache.Set(key + "_rptSource", model, cacheItemPolicy);
            }
            else
                cache.Set(key + "_rptName", "", cacheItemPolicy);
            Response.Redirect("~/ReportViewer?" + key);
        }
        //4.3 PD Trainee Status
        [HttpGet]
        public void GetPDTraineeStatusReport(int? programTypeID, int? tradeID, int? schemeID, int? tspID, int? quarter, int? year, DateTime? Month, string downloadType)
        {
            if (Month != null)
            {
                year = Month.Value.Year;
                quarter = (Month.Value.Month - 1) / 3 + 1;
            }

            var model = srvRPT.GetPDTraineeStatusReport(programTypeID, tradeID, schemeID, tspID, quarter, year, Month);

            string ReportName = "OverAll Stats in the System";
            if (Month != null) { ReportName = "Month Wise stats"; }
            else if (quarter != null) { ReportName = "Quarter " + quarter + " of year " + year; }
            else if (tspID != null) { ReportName = "TSP Wise stats"; }
            else if (schemeID != null) { ReportName = "Scheme Wise stats"; }
            else if (tradeID != null) { ReportName = "Trade Wise stats"; }
            else if (programTypeID != null) { ReportName = "Program Wise stats"; }

            //(Path.Combine(Server.MapPath("~/Crystal_Reports/Reports"), "PDTraineeStatus.rpt"));
            string fileName = "PDTraineeStatus.rpt";
            string key = "PDTraineeStatusReport";
            ParameterModel pm = new ParameterModel
            {
                ReportName = ReportName,
            };
            var cache = MemoryCache.Default;
            CacheItemPolicy cacheItemPolicy = new CacheItemPolicy();
            if (model != null)
            {
                cache.Set(key + "_rptName", fileName, cacheItemPolicy);
                cache.Set(key + "_rptSource", model, cacheItemPolicy);
            }
            else
                cache.Set(key + "_rptName", "", cacheItemPolicy);
            cache.Set(key + "_rptParam", pm, cacheItemPolicy);
            Response.Redirect("~/ReportViewer?" + key);
        }
        //4.4 Income of Graduate
        [HttpGet]
        public void GetIncomeOfGraduateReport(int? schemeID, int? TSPID, string EmploymentStatus, int? TraineeID, string downloadType)
        {
            var model = srvRPT.GetIncomeOfGraduateReport(schemeID, TSPID, EmploymentStatus, TraineeID);
            //(Path.Combine(Server.MapPath("~/Crystal_Reports/Reports"), "IncomeOfGraduate.rpt"));
            string fileName = "IncomeOfGraduate.rpt";
            string key = "IncomeOfGraduateReport";
            var cache = MemoryCache.Default;
            CacheItemPolicy cacheItemPolicy = new CacheItemPolicy();
            if (model != null)
            {
                cache.Set(key + "_rptName", fileName, cacheItemPolicy);
                cache.Set(key + "_rptSource", model, cacheItemPolicy);
            }
            else
                cache.Set(key + "_rptName", "", cacheItemPolicy);
            Response.Redirect("~/ReportViewer?" + key);

        }
        //2.5 TPM Summaries - Trainees
        [HttpGet]
        public void GetTPMSummariesTraineesReport(int? schemeID, int? tspID, string month, string downloadType)
        {
            var model = srvRPT.GetTPMSummariesTraineesReport(schemeID, tspID, month);
            //(Path.Combine(Server.MapPath("~/Crystal_Reports/Reports"), "TPMSummariesTrainees.rpt"));
            string fileName = "TPMSummariesTrainees.rpt";
            string key = "TPMSummariesTraineesReport";
            var cache = MemoryCache.Default;
            CacheItemPolicy cacheItemPolicy = new CacheItemPolicy();
            if (model != null)
            {
                cache.Set(key + "_rptName", fileName, cacheItemPolicy);
                cache.Set(key + "_rptSource", model, cacheItemPolicy);
            }
            else
                cache.Set(key + "_rptName", "", cacheItemPolicy);
            Response.Redirect("~/ReportViewer?" + key);
        }
        // 4.5 Trainee Type Percentage
        [HttpGet]
        public void GetTraineeTypePercentageReport(int? programTypeID, int? clusterID, int? sectorID, string downloadType)
        {
            var model = srvRPT.GetTraineeTypePercentageReport(programTypeID, clusterID, sectorID);
            //(Path.Combine(Server.MapPath("~/Crystal_Reports/Reports"), "TraineeTypePercentage.rpt"));
            string fileName = "TraineeTypePercentage.rpt";
            string key = "TraineeTypePercentage";
            var cache = MemoryCache.Default;
            CacheItemPolicy cacheItemPolicy = new CacheItemPolicy();
            if (model != null)
            {
                cache.Set(key + "_rptName", fileName, cacheItemPolicy);
                cache.Set(key + "_rptSource", model, cacheItemPolicy);
            }
            else
                cache.Set(key + "_rptName", "", cacheItemPolicy);
            Response.Redirect("~/ReportViewer?" + key);
        }
        // 4.7 PD Top 10 Trades
        [HttpGet]
        public void GetPDTop10TradesReport(string Type, int? schemeID, int? sectorID, string downloadType)
        {
            if (String.IsNullOrEmpty(Type)) { Type = "Scheme"; }
            var model = srvRPT.GetPDTop10TradesReport(Type, schemeID, sectorID);
            //(Path.Combine(Server.MapPath("~/Crystal_Reports/Reports"), "PDTop10Trades.rpt"));
            string fileName = "PDTop10Trades.rpt";
            string key = "PDTop10TradesReport";
            ParameterModel pm = new ParameterModel
            {
                MyType = Type
            };
            var cache = MemoryCache.Default;
            CacheItemPolicy cacheItemPolicy = new CacheItemPolicy();
            if (model != null)
            {
                cache.Set(key + "_rptName", fileName, cacheItemPolicy);
                cache.Set(key + "_rptSource", model, cacheItemPolicy);
            }
            else
                cache.Set(key + "_rptName", "", cacheItemPolicy);
            cache.Set(key + "_rptParam", pm, cacheItemPolicy);
            Response.Redirect("~/ReportViewer?" + key);
        }
        // 4.8 PD Placement Report
        [HttpGet]
        public void GetPDPlacmentReport(string Type, int? schemeID, int? sectorID, int? tradeID, string downloadType)
        {
            if (String.IsNullOrEmpty(Type)) { Type = "Scheme"; }
            var model = srvRPT.GetPDPlacmentReport(Type, schemeID, sectorID, tradeID);
            //(Path.Combine(Server.MapPath("~/Crystal_Reports/Reports"), "PDPlacement.rpt"));
            string fileName = "PDPlacement.rpt";
            string key = "PDPlacmentReport";
            ParameterModel pm = new ParameterModel
            {
                MyType = Type
            };
            var cache = MemoryCache.Default;
            CacheItemPolicy cacheItemPolicy = new CacheItemPolicy();
            if (model != null)
            {
                cache.Set(key + "_rptName", fileName, cacheItemPolicy);
                cache.Set(key + "_rptSource", model, cacheItemPolicy);
            }
            else
                cache.Set(key + "_rptName", "", cacheItemPolicy);
            cache.Set(key + "_rptParam", pm, cacheItemPolicy);
            Response.Redirect("~/ReportViewer?" + key);
        }
        // 5.2 PDP Players under training
        [HttpGet]
        public void GetPDPPlayersUnderTrainingReport(int? schemeID, string downloadType)
        {
            var model = srvRPT.GetPDPPlayersUnderTrainingReport(schemeID);
            //(Path.Combine(Server.MapPath("~/Crystal_Reports/Reports"), "PDPPlayersUnderTraining.rpt"));
            string fileName = "PDPPlayersUnderTraining.rpt";
            string key = "PDPPlayersUnderTrainingReport";
            var cache = MemoryCache.Default;
            CacheItemPolicy cacheItemPolicy = new CacheItemPolicy();
            if (model != null)
            {
                cache.Set(key + "_rptName", fileName, cacheItemPolicy);
                cache.Set(key + "_rptSource", model, cacheItemPolicy);
            }
            else
                cache.Set(key + "_rptName", "", cacheItemPolicy);
            Response.Redirect("~/ReportViewer?" + key);
        }
        // 5.5 PDP Cost Saving
        [HttpGet]
        public void GetPDPCostSavingReport(DateTime? StartDate, DateTime? EndDate, string downloadType)
        {
            var model = srvRPT.GetPDPCostSavingReport(StartDate, EndDate);
            //(Path.Combine(Server.MapPath("~/Crystal_Reports/Reports"), "PDPCostSaving.rpt"));
            string fileName = "PDPCostSaving.rpt";
            string key = "PDPCostSavingReport";
            var cache = MemoryCache.Default;
            CacheItemPolicy cacheItemPolicy = new CacheItemPolicy();
            if (model != null)
            {
                cache.Set(key + "_rptName", fileName, cacheItemPolicy);
                cache.Set(key + "_rptSource", model, cacheItemPolicy);
            }
            else
                cache.Set(key + "_rptName", "", cacheItemPolicy);
            Response.Redirect("~/ReportViewer?" + key);
        }
        // 5.1 Cost sharing partners
        [HttpGet]
        public void GetCostSharingPartnersReport(int? SchemeID, string downloadType)
        {
            var model = srvRPT.GetCostSharingPartnersReport(SchemeID);
            //(Path.Combine(Server.MapPath("~/Crystal_Reports/Reports"), "CostSharingPartners.rpt"));
            string fileName = "CostSharingPartners.rpt";
            string key = "CostSharingPartnersReport";
            var cache = MemoryCache.Default;
            CacheItemPolicy cacheItemPolicy = new CacheItemPolicy();
            if (model != null)
            {
                cache.Set(key + "_rptName", fileName, cacheItemPolicy);
                cache.Set(key + "_rptSource", model, cacheItemPolicy);
            }
            else
                cache.Set(key + "_rptName", "", cacheItemPolicy);
            Response.Redirect("~/ReportViewer?" + key);
        }
        // 6.1 Business Partner Invoice Status
        [HttpGet]
        public void GetBusinessPartnerInvoiceStatus(string Month, string downloadType)
        {
            var model = srvRPT.GetBusinessPartnerInvoiceStatusReport(Month);
            //(Path.Combine(Server.MapPath("~/Crystal_Reports/Reports"), "CostSharingPartners.rpt"));
            string fileName = "BusinessPartnerInvoiceStatus.rpt";
            string key = "BusinessPartnerInvoiceStatus";
            var cache = MemoryCache.Default;
            CacheItemPolicy cacheItemPolicy = new CacheItemPolicy();
            if (model != null)
            {
                cache.Set(key + "_rptName", fileName, cacheItemPolicy);
                cache.Set(key + "_rptSource", model, cacheItemPolicy);
            }
            else
                cache.Set(key + "_rptName", "", cacheItemPolicy);
            Response.Redirect("~/ReportViewer?" + key);
        }
        // 6.2 TSP Invoice Status
        [HttpGet]
        public void GeTspInvoiceStatus(int? schemeID, int? tspID, string downloadType)
        {
            var model = srvRPT.GeTspInvoiceStatus(schemeID, tspID);
            //(Path.Combine(Server.MapPath("~/Crystal_Reports/Reports"), "CostSharingPartners.rpt"));
            string fileName = "TspInvoiceStatusReport.rpt";
            string key = "TspInvoiceStatusReport";
            var cache = MemoryCache.Default;
            CacheItemPolicy cacheItemPolicy = new CacheItemPolicy();
            if (model != null)
            {
                cache.Set(key + "_rptName", fileName, cacheItemPolicy);
                cache.Set(key + "_rptSource", model, cacheItemPolicy);
            }
            else
                cache.Set(key + "_rptName", "", cacheItemPolicy);
            Response.Redirect("~/ReportViewer?" + key);
        }
        private FileStreamResult Converter(ReportDocument rd, string downloadType, string reportName)
        {
            try
            {
                Response.Buffer = false;
                Response.ClearContent();
                Response.ClearHeaders();
                Stream stream;
                if (downloadType == "xls")
                {
                    reportName += ".xls";
                    stream = rd.ExportToStream(ExportFormatType.Excel);
                    stream.Seek(0, SeekOrigin.Begin);
                    return File(stream, "application/application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", reportName);
                }
                else if (downloadType == "pdf")
                {
                    reportName += ".pdf";
                    //rd.PrintOptions.PaperOrientation = PaperOrientation.Landscape;
                    //rd.PrintOptions.PaperSize = PaperSize.PaperA4;
                    stream = rd.ExportToStream(ExportFormatType.PortableDocFormat);
                    stream.Seek(0, SeekOrigin.Begin);
                    return File(stream, "application/pdf", reportName);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return null;
        }
    }
}