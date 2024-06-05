using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace PSDF_BSS.Reports.Interfaces
{
    public interface ISRVReports
    {
        DataTable GetPlacementReport(string type, int? ClusterID, int? DistrictID, int? TradeID, int? SectorID, int? SchemeID, int? ProgramTypeID, int? ProgramCategoryID);
        DataTable GetSectorTraineeReport(int? sectorID, int? schemeID, int? schemeTypeID, int? ProgramCategoryID);
        DataTable GetFinancialComparisonOfSchemes(string reportType, int? ID, int scheme1ID, int scheme2ID, int scheme3ID);
        DataTable GetLocations(string financialYear);
        DataSet GetNewTradesTSPIndustryQuarterly(int SchemeID, string StartDate, string EndDate, int? ProgramTypeID, int? ProgramCategoryID);
        DataTable GetTSPMasterReport(string type, int? schemeID, int? genderID, int? ProgramTypeID, int? ProgramCategoryID);
        DataTable GetTradeDataReport(string type, int? TradeID, int? genderID, int? schemeID, int? ProgramTypeID, int? ProgramCategoryID);
        DataTable GetTraineeStatusReport(string type, int? sectorID, int? clusterID, int? schemeID, int? ProgramTypeID, int? ProgramCategoryID);
        DataTable GetTSPCurriculumReport(int? sourceofCurriculumID, int? durationID, int? educationTypeID, int? certAuthID, int? schemeID, int? ProgramTypeID, int? ProgramCategoryID);
        DataTable GetChangeofInstructorReport(int? schemeID, int? tspID, int? ProgramTypeID, int? ProgramCategoryID);
        DataTable GetChangeinAppendixReport(int? schemeID, int? ProgramTypeID, int? ProgramCategoryID);
        DataTable GetFactSheetReport(int? schemeID);
        DataTable GetInvoiceStatusReport(int? kamID, int? schemeID, int? tspID, string Month);
        DataTable GetTraineeComplaintsReport(string type, int? kamID, int? schemeID, int? tspID);
        DataTable GetManagementReport(string type, int? SchemeID, int? TSPID, DateTime? Month);
        DataTable GetStipendDisbursementReport(int? kamID, int? schemeID, int? tspID, string month);
        DataTable GetSDPMonthlyReport(int? ProjectTypeID, DateTime? Month);
        DataTable GetTSPTraineeStatusReport(DateTime Month, int? TSPID, int? ClassID);
        DataTable GetUnverifiedTraineeReport(int? TSPID, int? ClassID);
        DataTable GetTraineeCostReport(int? ProgramTypeID, int? SchemeID, int? ClassID);
        DataTable GetTPMSummariesViolationsReport(int? schemeID, int? tspID, string month);
        DataTable GetTPMSummariesTraineesReport(int? schemeID, int? tspID, string month);
        DataTable GetSchemeUniqueTSPReport(int? SchemeID);
        DataTable GetPDTraineeStatusReport(int? ProgramTypeID, int?TradeID, int?SchemeID ,int? TSPID, int? Quarter, int? Year, DateTime? Month);
        DataTable GetIncomeOfGraduateReport(int? SchemeID, int? TSPID, string EmploymentStatus, int? TraineeID);
        DataTable GetTraineeTypePercentageReport(int? ProgramTypeID, int? ClusterID, int? SectorID);
        DataTable GetPDTop10TradesReport(string Type, int? SchemeID, int? SectorID);
        DataTable GetPDPlacmentReport(string Type, int? SchemeID, int? SectorID, int?TradeID);
        DataTable GetPDPPlayersUnderTrainingReport(int? SchemeID);
        DataTable GetPDPCostSavingReport(DateTime? StartDate, DateTime? EndDate);
        DataTable GetCostSharingPartnersReport(int? SchemeID);
        DataTable GetBusinessPartnerInvoiceStatusReport(string Month);
        DataTable GeTspInvoiceStatus(int? schemeID, int? tspID);
    }
}