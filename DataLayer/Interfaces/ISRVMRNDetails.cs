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


        //Internet note for Approval 
        List<MRNDetailsModel> FetchIRNDetails(MRNDetailsModel model);
        List<MRNDetailsModel> GetIRNExcelExport(MRNDetailsModel MRN);
        public List<MRNDetailsModel> GetIRNExcelExportByIDs(string ids);

        // Internet Device Note
        List<MRNDetailsModel> FetchIDPDetails(MRNDetailsModel model);
        List<MRNDetailsModel> GetIDPExcelExport(MRNDetailsModel MRN);
        public List<MRNDetailsModel> GetIDPExcelExportByIDs(string ids);


    }
}
