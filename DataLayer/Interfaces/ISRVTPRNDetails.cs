using DataLayer.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataLayer.Interfaces
{
    public interface ISRVTPRNDetails
    {
        List<TPRNDetailsModel> FetchTPRNDetails(TPRNDetailsModel model);
        List<TPRNDetailsModel> FetchTPRNDetailsFiltered(TPRNDetailsModel model);
        TPRNDetailsModel UpdateTPRNDetails(TPRNDetailsModel TPRN);
        List<TPRNDetailsModel> GetTPRNExcelExport(TPRNDetailsModel TPRN);
        public List<TPRNDetailsModel> GetTPRNExcelExportByIDs(string ids);


    }
}
