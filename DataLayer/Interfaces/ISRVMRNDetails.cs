using DataLayer.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataLayer.Interfaces
{
    public interface ISRVMRNDetails
    {
        List<MRNDetailsModel> FetchMRNDetails(MRNDetailsModel model);
        List<MRNDetailsModel> FetchMRNDetailsFiltered(MRNDetailsModel model);
        MRNDetailsModel UpdateMRNDetails(MRNDetailsModel MRN);
        List<MRNDetailsModel> GetMRNExcelExport(MRNDetailsModel MRN);
        public List<MRNDetailsModel> GetMRNExcelExportByIDs(string ids);


    }
}
