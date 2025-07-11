using System;
using System.Collections.Generic;
using System.Text;

namespace DataLayer.Models
{
    public class ProgramDesignModel: ModelBase
    {


        public int? ProgramID { get; set; }
        public int UserID { get; set; }
        public string Program { get; set; }
        public int ProgramBudget { get; set; }
        public string ProgramCode { get; set; }
        public int? FinancialYearID { get; set; }
        public int? ProgramTypeID { get; set; }
        public int? ProgramCategoryID { get; set; }
        public int? FundingSourceID { get; set; }
        public int? FundingCategoryID { get; set; }
        public int? PaymentStructureID { get; set; }
        public string Description { get; set; }
        public float? Stipend { get; set; }
        public string StipendMode { get; set; }
        public int[]? ApplicabilityID { get; set; }
        public int SelectionMethodID { get; set; }
        public int? MinEducationID { get; set; }
        public int? MaxEducationID { get; set; }
        public int? MinAge { get; set; }
        public int? MaxAge { get; set; }
        public int? GenderID { get; set; }
        public DateTime? ContractAwardDate { get; set; }
        public string BusinessRuleType { get; set; }
        public int? CreatedUserID { get; set; }
        public int? ModifiedUserID { get; set; }
        public bool? InActive { get; set; }
        public bool? FinalSubmitted { get; set; }
        public int? PlaningTypeID { get; set; }
        public DateTime? TentativeProcessSDate { get; set; }
        public DateTime? ClassStartDate { get; set; }
        public string TraineeSupportItems { get; set; }
        public string EmploymentCommitment { get; set; }
        public string SchemeDesignOn { get; set; }
        public int[]? ProvinceID { get; set; }
        public int[]? ClusterID { get; set; }
        public int[]? DistrictID { get; set; }
        public float TraineeSupportCost { get; set; }
        public string ApprovalRecDetail { get; set; }
        public string ApprovalAttachment { get; set; }
        public string AttachmentTORs { get; set; }
        public string AttachmentCriteria { get; set; }
        
        public bool? IsApproved { get; set; }
        public bool? IsSubmitted { get; set; } = false;
        public bool? IsRejected { get; set; }

      


    }
}
