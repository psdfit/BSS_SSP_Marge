using DataLayer.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataLayer.Interfaces
{
    public interface ISRVSRNDetails
    {
        List<SRNDetailsModel> FetchSRNDetails(SRNDetailsModel model);
        List<SRNDetailsModel> FetchSRNDetailsFiltered(SRNDetailsModel model);
        SRNDetailsModel UpdateSRNDetails(SRNDetailsModel SRN);
        List<SRNDetailsModel> GetSRNExcelExport(SRNDetailsModel SRN);
        public List<SRNDetailsModel> GetSRNExcelExportByIDs(string ids);


    }
}
