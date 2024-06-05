using System;
using System.Collections.Generic;
using System.Text;

namespace DataLayer.Models
{
    public class TRNMasterModel
    {
        public int TRNMasterID { get; set; }
        public string ProcessKey { get; set; }
        public int? CertAuthID { get; set; }
        public int? SchemeID { get; set; }
        public bool? IsApproved { get; set; }
        public bool? IsRejected { get; set; }
        public bool? InActive { get; set; }
        public int? CreatedUserID { get; set; }
        public int? ModifiedUserID { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public string CertAuthName { get; set; }
        public string SchemeName { get; set; }
        public string SchemeCode { get; set; }
        public DateTime Month { get; set; }
        public int? ApprovalStepID { get; set; }
    }
}
