using System.Collections.Generic;
using PSDF_BSS.Reports.Models;

namespace PSDF_BSS.Reports.Interfaces
{
    public interface ISRVTSPMasterData
    {
        List<TSPMasterDataModel> GetTSPMasterDataReportList(int? schemeId, int? tspId, int? classId, string date, string reportType);
    }
}