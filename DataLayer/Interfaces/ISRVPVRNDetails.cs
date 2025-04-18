using DataLayer.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataLayer.Interfaces
{
    public interface ISRVPVRNDetails
    {
        List<PVRNDetailsModel> FetchPVRNDetails(PVRNDetailsModel model);
        List<PVRNDetailsModel> FetchPVRNDetailsFiltered(PVRNDetailsModel model);
        PVRNDetailsModel UpdatePVRNDetails(PVRNDetailsModel PVRN);
        List<PVRNDetailsModel> GetPVRNExcelExport(PVRNDetailsModel PVRN);
        public List<PVRNDetailsModel> GetPVRNExcelExportByIDs(string ids);

    }
}
