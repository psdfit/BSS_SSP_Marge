using System.Collections.Generic;
using PSDF_BSS.Reports.Models;

namespace PSDF_BSS.Reports.Interfaces
{
    public interface ISRVTradeData
    {
        List<TradeDataModel> GetTradeDataReportList(int? schemeId, int? tspId, int? classId, string date, string reportType);
    }
}
