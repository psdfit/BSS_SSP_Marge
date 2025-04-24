using DataLayer.Models;
using System.Collections.Generic;

namespace DataLayer.Interfaces
{
    public interface ISRVPCRNDetails
    {
        List<PCRNDetailsModel> FetchPCRNDetails(PCRNDetailsModel model);
        List<PCRNDetailsModel> FetchPCRNDetailsFiltered(PCRNDetailsModel model);
        PCRNDetailsModel UpdatePCRNDetails(PCRNDetailsModel PCRN);
        List<PCRNDetailsModel> GetPCRNExcelExport(PCRNDetailsModel PCRN);
        public List<PCRNDetailsModel> GetPCRNExcelExportByIDs(string ids);

    }
}
