using DataLayer.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Interfaces
{
    public interface ISRVTrn
    {
        Task AddTRN();
        List<TRNMasterModel> FetchTRN(QueryFilters mod);
        List<TRNModel> FetchTRNDetails(int trnMasterID);
        public bool GenerateTRN(QueryFilters model);
        public List<TRNModel> GetTRNExcelExportByIDs(string ids);
        List<TRNModel> GetTRNExcelExport(TRNMasterModel model);
    }
}
