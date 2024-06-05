using System;
using System.Collections.Generic;
using System.Text;

namespace DataLayer.Models
{
    public class EmploymentVerificationReportModel : ModelBase
    {

        public EmploymentVerificationReportModel() : base()
        {
        }

        public string DetailString { get; set; }
        public string SchemeName { get; set; }
        public string TSPName { get; set; }
        public string ClassCode { get; set; }
        public string ClassStartDate { get; set; }
        public string ClassEndDate { get; set; }
        public string EmploymentCommitment { get; set; }
        public string CompletedTrainees { get; set; }
        public string EmploymentCommitmentTrainees { get; set; }
        public string EmploymentCommitmentFloor { get; set; }
        public string Reported { get; set; }
        public string Unverified { get; set; }
        public string Verified { get; set; }
        public string VerifiedtoCommitment { get; set; }

    }
}