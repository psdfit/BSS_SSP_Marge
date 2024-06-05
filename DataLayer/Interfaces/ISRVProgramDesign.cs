using DataLayer.Models;
using DataLayer.Models.SSP;
using Microsoft.Data.SqlClient;
using System.Collections.Generic;
using System.Data;

namespace DataLayer.Interfaces
{
    public interface ISRVProgramDesign
    {
        DataTable FetchProgramDesign(bool InActive);
        DataTable FetchAnalysisReportFilters(int[] filters);
        DataTable FetchAnalysisReport();
        void SaveProgramDesign(ProgramDesignModel programDesign);
        public DataTable FetchDropDownList(string Param);
        public DataTable SaveTradeDesign(TradeLotDesignModel data);
        public DataTable UpdateSchemeInitialization(ProgramDesignModel data);
        //public DataTable FetchCTM(CTMCalculationModel data);
        public DataTable FetchCTMTradeWise(CTMCalculationModel data);
        public DataTable FetchCTMBulkReport(CTMCalculationModel data);
        public DataTable FetchHistoryReport(HistoricalReportModel data);

        public bool SaveProgramWorkflowHistory(ProgramWorkflowHistoryModel data);
        public bool SaveProgramStatusHistory(ProgramStatusHistoryModel data);
        public bool SaveProgramCriteriaHistory(ProgramCriteriaHistoryModel data);

        bool ProgramApproveReject(ProgramDesignModel model, SqlTransaction transaction = null);
        bool ProgramDesignFinalApproval(int FormId, SqlTransaction transaction = null);



        public DataTable LoopinData(DataTable dt, string[] attachmentColumns);

    }
}
