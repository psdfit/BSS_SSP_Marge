using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using PSDF_BSS.Reports.Models;
using PSDF_BSS.Reports.Interfaces;

namespace PSDF_BSS.Reports.Services
{
    public class SRVTradeData : ISRVTradeData
    {
        public List<TradeDataModel> GetTradeDataReportList(int? schemeId, int? tspId, int? classId, string dateTime, string reportType)
        {
            try
            {
                DateTime? date = null;
                if (!string.IsNullOrEmpty(dateTime))
                    date = DateTime.ParseExact(dateTime, "dd/MM/yyyy", null);
                schemeId = schemeId > 0 ? schemeId : 0;
                tspId = tspId > 0 ? tspId : 0;
                classId = classId > 0 ? classId : 0;
                SqlParameter[] param = new SqlParameter[5];
                param[0] = new SqlParameter("@SchemeId", schemeId);
                param[1] = new SqlParameter("@TspId", tspId);
                param[2] = new SqlParameter("@ClassId", classId);
                param[3] = new SqlParameter("@Month", date);
                param[4] = new SqlParameter("@ReportType", reportType);
                DataTable dt = SqlHelper.ExecuteDataset(SqlHelper.GetCon(), CommandType.StoredProcedure, "dbo.RD_TradeData", param).Tables[0];
                if (dt.Rows.Count > 0)
                {
                    return LoopinData(dt);
                }
                else
                    return null;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        private List<TradeDataModel> LoopinData(DataTable dt)
        {
            List<TradeDataModel> List = new List<TradeDataModel>();

            foreach (DataRow r in dt.Rows)
            {
                List.Add(RowOfTradeData(r));
            }
            return List;
        }
        private TradeDataModel RowOfTradeData(DataRow r)
        {
            TradeDataModel tradeData = new TradeDataModel
            {
                ReportName = r["ReportName"].ToString(),
                SchemeName = r["SchemeName"].ToString(),
                Sector = r["SectorName"].ToString(),
                TotalTraineesContracted = Convert.ToInt32(r["TotalTraineesContracted"]),
                ValueofContract = Convert.ToInt32(r["ValueofContract"]),
                PerofContributionInSchemeTarget = Math.Round(Convert.ToDouble(r["PerofContributionInSchemeTarget"]), 2),
                PerofContributionInOverallFYtarget = Math.Round(Convert.ToDouble(r["PerofContributionInOverallFYtarget"]), 2),
                TSPName = r["TSPName"].ToString(),
                ProgramType = r["ProgramType"].ToString(),
                GenderName = r["GenderName"].ToString()
            };
            return tradeData;
        }
    }
}