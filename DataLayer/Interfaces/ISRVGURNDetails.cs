using DataLayer.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataLayer.Interfaces
{
    public interface ISRVGURNDetails
    {
        List<GURNDetailsModel> FetchGURNDetails(GURNDetailsModel model);
        List<GURNDetailsModel> FetchGURNDetailsFiltered(GURNDetailsModel model);
        GURNDetailsModel UpdateGURNDetails(GURNDetailsModel GURN);
        List<GURNDetailsModel> GetGURNExcelExport(GURNDetailsModel GURN);
        public List<GURNDetailsModel> GetGURNExcelExportByIDs(string ids);

    }
}
