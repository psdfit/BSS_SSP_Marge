using DataLayer.Models;
using DataLayer.Models.SSP;
using Microsoft.Data.SqlClient;
using System.Collections.Generic;
using System.Data;

namespace DataLayer.Interfaces
{
    public interface ISRVCriteriaTemplate
    {
        public DataTable SaveCriteriaTemplate(CriteriaTemplateModel data);
        public bool RemoveMainCategory(CriteriaMainCategory mainCategory);
        public bool RemoveSubCategory(CriteriaSubCategory subCategory);
        public DataTable FetchDataListBySPName(string Param);

        public DataTable LoopinData(DataTable dt, string[] attachmentColumns);

        bool CriteriaApproveReject(CriteriaTemplateModel model, SqlTransaction transaction = null);
        bool CriteriaFinalApproval(int FormId, SqlTransaction transaction = null);

    }
}
