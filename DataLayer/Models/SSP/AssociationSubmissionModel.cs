using System;
using System.Collections.Generic;
using System.Text;

namespace DataLayer.Models.SSP
{
    public class AssociationSubmissionModel : ModelBase
    {
        public AssociationSubmissionModel() { }

        public int UserID { get; set; }
        public int TspAssociationMasterID { get; set; } = 0;
        public int TrainingLocation { get; set; } = 0;
        public string TradeLotTitle { get; set; } 
        public string TSPName { get; set; } 
        public int TradeLot { get; set; } = 0;
        public int ProgramID { get; set; } = 0;
        public int TrainerDetailID { get; set; } = 0;
        public List<AssociationDetail> associationDetail { get; set; } = new List<AssociationDetail>();

    }

    public class AssociationDetail
    {
        public int TspAssociationDetailID { get; set; } = 0;
        public int TspAssociationMasterID { get; set; } = 0;
        public int UserID { get; set; } = 0;
        public int CriteriaMainCategoryID { get; set; } = 0;
        public string CategoryTitle { get; set; } 
        public string Evidence { get; set; } 
        public string Remarks { get; set; } 

      
    }
}
