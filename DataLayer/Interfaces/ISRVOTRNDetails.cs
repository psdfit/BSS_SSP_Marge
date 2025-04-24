using DataLayer.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataLayer.Interfaces
{
    public interface ISRVOTRNDetails
    {
        List<OTRNDetailsModel> FetchOTRNDetails(OTRNDetailsModel model);
        List<OTRNDetailsModel> FetchOTRNDetailsFiltered(OTRNDetailsModel model);
        OTRNDetailsModel UpdateOTRNDetails(OTRNDetailsModel OTRN);
        List<OTRNDetailsModel> GetOTRNExcelExport(OTRNDetailsModel OTRN);
        public List<OTRNDetailsModel> GetOTRNExcelExportByIDs(string ids);

    }
}
