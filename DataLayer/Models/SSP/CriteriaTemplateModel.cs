using System;
using System.Collections.Generic;
using System.Text;

namespace DataLayer.Models.SSP
{




    public class CriteriaTemplateModel : ModelBase
    {
        public int CriteriaTemplateID { get; set; }
        public string CriteriaTemplateTitle { get; set; }
        public string Description { get; set; }
        public string MarkingRequired { get; set; }
        public string MaximumMarks { get; set; }

        public bool? IsApproved { get; set; }
        public bool? IsSubmitted { get; set; } = false;
        public bool? IsRejected { get; set; }
        public int UserID { get; set; }

        public List<CriteriaMainCategory> mainCategory { get; set; } = new List<CriteriaMainCategory>();
    }

    public class CriteriaMainCategory
    {
        public string CategoryTitle { get; set; }
        public int CriteriaMainCategoryID { get; set; }
        public int CriteriaTemplateID { get; set; }
        public string Description { get; set; }
        public string TotalMarks { get; set; }
        public int UserID { get; set; }

        public List<CriteriaSubCategory> subCategory { get; set; } = new List<CriteriaSubCategory>();
    }

    public class CriteriaSubCategory
    {
        public string Criteria { get; set; }
        public int CriteriaMainCategoryID { get; set; }
        public int CriteriaSubCategoryID { get; set; }
        public string Description { get; set; }
        public string Mandatory { get; set; }
        public string MarkedCriteria { get; set; }
        public string MaxMarks { get; set; }
        public string SubCategoryTitle { get; set; }
        public string Attachment { get; set; }
        public int UserID { get; set; }
    }

}