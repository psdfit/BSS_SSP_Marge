using DataLayer.Classes;
using DataLayer.Dapper;
using DataLayer.Models;
using Microsoft.Data.SqlClient;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace DataLayer.Services
{
    public class SRVTier : SRVBase, DataLayer.Interfaces.ISRVTier
    {
        public IDapperConfig _dapper { get; set; }
        public SRVTier(IDapperConfig dapper)
        {
            _dapper = dapper;
        }

        public TierModel GetByTierID(int TierID)
        {
            try
            {
                SqlParameter param = new SqlParameter("@TierID", TierID);
                DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_Tier", param).Tables[0];
                if (dt.Rows.Count > 0)
                {
                    return RowOfTier(dt.Rows[0]);
                }
                else
                    return null;
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }

        public List<TierModel> SaveTier(TierModel Tier)
        {
            try
            {
                SqlParameter[] param = new SqlParameter[3];
                param[0] = new SqlParameter("@TierID", Tier.TierID);
                param[1] = new SqlParameter("@TierName", Tier.TierName);

                param[2] = new SqlParameter("@CurUserID", Tier.CurUserID);
                SqlHelper.ExecuteNonQuery(SqlHelper.GetCon(), CommandType.StoredProcedure, "[AU_Tier]", param);
                return FetchTier();
            }
            catch (Exception ex)
            { throw new Exception(ex.Message); }
        }

        private List<TierModel> LoopinData(DataTable dt)
        {
            List<TierModel> TierL = new List<TierModel>();

            foreach (DataRow r in dt.Rows)
            {
                TierL.Add(RowOfTier(r));
            }
            return TierL;
        }

        public List<TierModel> FetchTier(TierModel mod)
        {
            try
            {
                DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_Tier", Common.GetParams(mod)).Tables[0];
                return LoopinData(dt);
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }

        public List<TierModel> FetchTier()
        {
            try
            {
                DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_Tier").Tables[0];
                return LoopinData(dt);
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }

        public List<TierModel> FetchTier(bool InActive)
        {
            try
            {
                DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "RD_Tier", new SqlParameter("@InActive", InActive)).Tables[0];
                return LoopinData(dt);
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }

        public int BatchInsert(List<TierModel> ls, int @BatchFkey, int CurUserID)
        {
            SqlParameter[] param = new SqlParameter[3];

            param[0] = new SqlParameter("@Json", JsonConvert.SerializeObject(ls));
            param[1] = new SqlParameter("@", @BatchFkey);
            param[2] = new SqlParameter("@CurUserID", CurUserID);
            return SqlHelper.ExecuteNonQuery(SqlHelper.GetCon(), CommandType.StoredProcedure, "[BAU_Tier]", param);
        }

        public void ActiveInActive(int TierID, bool? InActive, int CurUserID)
        {
            SqlParameter[] PLead = new SqlParameter[3];
            PLead[0] = new SqlParameter("@TierID", TierID);
            PLead[1] = new SqlParameter("@InActive", InActive);
            PLead[2] = new SqlParameter("@CurUserID", CurUserID);
            SqlHelper.ExecuteNonQuery(SqlHelper.GetCon(), CommandType.StoredProcedure, "[AI_Tier]", PLead);
        }

        private TierModel RowOfTier(DataRow r)
        {
            TierModel Tier = new TierModel();
            Tier.TierID = Convert.ToInt32(r["TierID"]);
            Tier.TierName = r["TierName"].ToString();
            Tier.InActive = Convert.ToBoolean(r["InActive"]);
            Tier.CreatedUserID = Convert.ToInt32(r["CreatedUserID"]);
            Tier.ModifiedUserID = Convert.ToInt32(r["ModifiedUserID"]);
            Tier.CreatedDate = r["CreatedDate"].ToString().GetDate();
            Tier.ModifiedDate = r["ModifiedDate"].ToString().GetDate();

            return Tier;
        }
        public async Task<List<VoilationSummaryModel>> GetVoilationSummaryReport()
        {
            try
            {
                var list = await _dapper.QueryAsync<VoilationSummaryReportModel>("dbo.RD_VoilationSummaryReport", new { }, CommandType.StoredProcedure).ConfigureAwait(true);
                if (list.Any())
                {
                    var summaryList = new List<VoilationSummaryModel>();
                    foreach (var item in list)
                    {
                        var obj = new VoilationSummaryModel();
                        obj.Stream = item.Stream;
                        obj.SchemeName = item.SchemeName;
                        obj.TSPName = item.TSPName;
                        obj.TradeName = item.TradeName;
                        var startMonth = new DateTime(2020, 7, 1).Month;
                        var endMonth = new DateTime(2020, 12, 31).Month;
                        list.Where(x => x.Month >= startMonth && x.Month <= endMonth);
                        var finList = new List<VoilationSummaryReportModel>();
                        for (int i = startMonth; i < endMonth + 1; i++)
                        {
                            //var item = list.Where(x => x.Month == i).First();
                            //var summary = new VoilationSummaryReportModel();
                            //summary.Stream = "";
                            //summary.SchemeName = item.SchemeName;
                            //summary.TSPName = item.TSPName;
                            //summary.TradeName = item.TradeName;
                            //summary.CenterRelocation = item.CenterRelocation == null ? 0 : item.CenterRelocation;
                            //summary.FakeGhost = item.FakeGhost == null ? 0 : item.FakeGhost;
                            //summary.FeeCharge = item.FeeCharge == null ? 0 : item.FeeCharge;
                            //summary.CFFTotal = summary.CenterRelocation + summary.FakeGhost + summary.FeeCharge;
                            //summary.NonFunctional = item.NonFunctional;
                            //summary.MonthName = DateTimeFormatInfo.CurrentInfo.GetAbbreviatedMonthName(i);

                            var summary = new VoilationSummaryReportModel();
                            summary.CenterRelocation = 1;
                            summary.FakeGhost = 1;
                            summary.FeeCharge = 1;
                            summary.CFFTotal = summary.CenterRelocation + summary.FakeGhost + summary.FeeCharge;
                            summary.NonFunctional = 1;
                            summary.MonthName = DateTimeFormatInfo.CurrentInfo.GetAbbreviatedMonthName(i);
                            summary.NonFunctionalTotal += summary.NonFunctionalTotal;
                            finList.Add(summary);
                        }
                        obj.AvgMonthlyScore = finList.Sum(x => x.CFFTotal);
                        if (obj.AvgMonthlyScore >= 5)
                        {
                            obj.CFFOnScaleOfFive = 5;
                        }
                        else if (obj.AvgMonthlyScore >= 4)
                        {
                            obj.CFFOnScaleOfFive = 4;
                        }
                        else if (obj.AvgMonthlyScore >= 85)
                        {
                            obj.CFFOnScaleOfFive = 2;
                        }
                        else if (obj.AvgMonthlyScore >= 3)
                        {
                            obj.CFFOnScaleOfFive = 3;
                        }
                        else if (obj.AvgMonthlyScore >= 2)
                        {
                            obj.CFFOnScaleOfFive = 2;
                        }
                        else if (obj.AvgMonthlyScore >= 1)
                        {
                            obj.CFFOnScaleOfFive = 1;
                        }
                        else if (obj.AvgMonthlyScore < 1)
                        {
                            obj.CFFOnScaleOfFive = 0;
                        }
                        obj.SeventyPercentWeightage = obj.CFFOnScaleOfFive * 0.7;
                        obj.AvgMonthlyScoreNonFunc = finList.Sum(x => x.NonFunctional);
                        if (obj.AvgMonthlyScoreNonFunc >= 5)
                        {
                            obj.NonFuncOnScaleOfFive = 5;
                        }
                        else if (obj.AvgMonthlyScoreNonFunc >= 4)
                        {
                            obj.NonFuncOnScaleOfFive = 4;
                        }
                        else if (obj.AvgMonthlyScoreNonFunc >= 85)
                        {
                            obj.NonFuncOnScaleOfFive = 2;
                        }
                        else if (obj.AvgMonthlyScoreNonFunc >= 3)
                        {
                            obj.NonFuncOnScaleOfFive = 3;
                        }
                        else if (obj.AvgMonthlyScoreNonFunc >= 2)
                        {
                            obj.NonFuncOnScaleOfFive = 2;
                        }
                        else if (obj.AvgMonthlyScoreNonFunc >= 1)
                        {
                            obj.NonFuncOnScaleOfFive = 1;
                        }
                        else if (obj.AvgMonthlyScoreNonFunc < 1)
                        {
                            obj.NonFuncOnScaleOfFive = 0;
                        }
                        obj.ThirtyPercentWeightage = obj.NonFuncOnScaleOfFive * 0.3;
                        obj.FinalScore = obj.SeventyPercentWeightage + obj.ThirtyPercentWeightage;
                        finList.OrderByDescending(x => x.Month);
                        obj.MonthsList = finList.OrderByDescending(x => x.Month).Select(x => x.MonthName).ToList();
                        obj.ItemsList = finList.ToList();
                        summaryList.Add(obj);
                    }
                    return summaryList;
                }
                else
                {
                    return new List<VoilationSummaryModel>();
                }
            }
            catch (Exception ex)
            {
                return new List<VoilationSummaryModel>();
            }
        }
        public async Task<List<CompletionReportModel>> GetCompletionReport()
        {
            try
            {
                var list = await _dapper.QueryAsync<CompletionReportModel>("dbo.RD_CompletionReport", new { }, CommandType.StoredProcedure).ConfigureAwait(true);
                return list.ToList();
            }
            catch (Exception ex)
            {
                return new List<CompletionReportModel>();
            }
        }
        public async Task<List<EmploymentRatioReportModel>> GetEmploymentRatioReport()
        {
            try
            {
                var list = await _dapper.QueryAsync<EmploymentRatioReportModel>("dbo.RD_EmploymentRatioReport", new { }, CommandType.StoredProcedure).ConfigureAwait(true);
                
                if (list.Any())
                {
                    var li = new List<EmploymentRatioReportModel>();
                    double total = 0.00;
                    foreach (var item in list)
                    {
                        var obj = new EmploymentRatioReportModel();
                        obj.CompletedTrainees = item.CompletedTrainees;
                        obj.EmploymentCommitmentPercentage = item.EmploymentCommitmentPercentage;
                        obj.SchemeName = item.SchemeName;
                        obj.Stream = item.Stream;
                        obj.TradeName = item.TradeName;
                        obj.TraineesPerClass = item.TraineesPerClass;
                        obj.TraineesReported = item.TraineesReported;
                        obj.TSPName = item.TSPName;
                        if (Math.Min((double)item.TraineesPerClass, (double)item.TraineesReported) > 0)
                        {
                            var subTotal = Math.Min((double)item.TraineesPerClass, (double)item.TraineesReported) * item.EmploymentCommitmentPercentage;
                            if (subTotal >  0)
                            {
                                total = Math.Round((double)subTotal / 100);
                            }
                        }
                        li.Add(obj);
                    }
                }
                //var li = list.Select(x => new EmploymentRatioReportModel { 
                //    CompletedTrainees = x.CompletedTrainees,
                //    EmploymentCommitmentPercentage = x.EmploymentCommitmentPercentage,
                //    SchemeName = x.SchemeName,
                //    Stream = x.Stream,
                //    TradeName = x.TradeName,
                //    TraineesPerClass = x.TraineesPerClass,
                //    TraineesReported = x.TraineesReported,
                //    TSPName = x.TSPName,
                //    EmploymentCommitmentNumber = ((Math.Min((double)x.TraineesPerClass, (double)x.TraineesReported)) * x.EmploymentCommitmentPercentage) == 0 ? 0 : ((Math.Min((double)x.TraineesPerClass, (double)x.TraineesReported)) * x.EmploymentCommitmentPercentage) / 100,

                //});
                
                return list.ToList();
            }
            catch (Exception ex)
            {
                return new List<EmploymentRatioReportModel>();
            }
        }
        public async Task<List<PerformanceAnalysisReportModel>> GetPerformanceAnalysisReport()
        {
            try
            {
                var list = await _dapper.QueryAsync<PerformanceAnalysisReportModel>("dbo.RD_PerformanceAnalysisReport", new { }, CommandType.StoredProcedure).ConfigureAwait(true);
                return list.ToList();
            }
            catch (Exception ex)
            {
                return new List<PerformanceAnalysisReportModel>();
            }
        }
        public List<GetTSPPerformanceModel> TSPPerformanceDatePeriod()
        {
            try
            {
                DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "[RD_TSPPerformanceDatePeriod]").Tables[0];
                return Helper.ConvertDataTableToModel<GetTSPPerformanceModel>(dt);
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }
        public List<GetTSPPerformanceModel> GetTSPPerformance(GetTSPPerformanceModel model)
        {
            try
            {
                DateTime startDate = new DateTime();
                DateTime endDate = new DateTime();
                if (!string.IsNullOrEmpty(model.Date))
                {
                    string[] splitdate = model.Date.Split("_");
                     startDate = Convert.ToDateTime(splitdate[0]);
                     endDate = Convert.ToDateTime(splitdate[1]);
                }
                SqlParameter[] param = new SqlParameter[6];
                param[0] = new SqlParameter("@SchemeID", model.SchemeID);
                param[1] = new SqlParameter("@TSPID", model.TSPID);
                param[2] = new SqlParameter("@TradeID", model.TradeID);
                param[3] = new SqlParameter("@StartDate", startDate.ToString("yyyy-MM-dd"));
                param[4] = new SqlParameter("@EndDate", endDate.ToString("yyyy-MM-dd"));
                DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "[RD_TSPPerformance]", param).Tables[0];
                 return Helper.ConvertDataTableToModel<GetTSPPerformanceModel>(dt);
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }
        public List<GetTSPPerformanceModel> FetchTradeDetailByTSP(GetTSPPerformanceModel model)
        {
            try
            {
                DateTime startDate = new DateTime();
                DateTime endDate = new DateTime();
                if (!string.IsNullOrEmpty(model.Date))
                {
                    string[] splitdate = model.Date.Split("_");
                    startDate = Convert.ToDateTime(splitdate[0]);
                    endDate = Convert.ToDateTime(splitdate[1]);
                }
                SqlParameter[] param = new SqlParameter[5];
                param[0] = new SqlParameter("@TSPID", model.TSPID);
                param[1] = new SqlParameter("@StartDate", startDate.ToString("yyyy-MM-dd"));
                param[2] = new SqlParameter("@EndDate", endDate.ToString("yyyy-MM-dd"));
                DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "[TSPPerformanceLookUpTrade]", param).Tables[0];
                return Helper.ConvertDataTableToModel<GetTSPPerformanceModel>(dt);
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }
    }
}