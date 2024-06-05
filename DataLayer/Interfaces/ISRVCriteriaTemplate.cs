using DataLayer.Models.SSP;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace DataLayer.Interfaces
{
    public interface ISRVCriteriaTemplate
    {
        public DataTable SaveCriteriaTemplate(CriteriaTemplateModel data);
        public bool RemoveMainCategory(CriteriaMainCategory mainCategory);
        public bool RemoveSubCategory(CriteriaSubCategory subCategory);
        public DataTable FetchDataListBySPName(string Param);

        public DataTable LoopinData(DataTable dt, string[] attachmentColumns);

    }
}
