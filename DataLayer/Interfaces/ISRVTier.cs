using DataLayer.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DataLayer.Interfaces
{

    public interface ISRVTier
    {
        TierModel GetByTierID(int TierID);
        List<TierModel> SaveTier(TierModel Tier);
        List<TierModel> FetchTier(TierModel mod);
        List<TierModel> FetchTier();
        List<TierModel> FetchTier(bool InActive);
        void ActiveInActive(int TierID, bool? InActive, int CurUserID);
        Task<List<VoilationSummaryModel>>  GetVoilationSummaryReport();
        Task<List<CompletionReportModel>> GetCompletionReport();
        Task<List<EmploymentRatioReportModel>> GetEmploymentRatioReport();
        Task<List<PerformanceAnalysisReportModel>> GetPerformanceAnalysisReport();
        List<GetTSPPerformanceModel> TSPPerformanceDatePeriod();
        List<GetTSPPerformanceModel> GetTSPPerformance(GetTSPPerformanceModel model);
        List<GetTSPPerformanceModel> FetchTradeDetailByTSP(GetTSPPerformanceModel model);
    }
}
