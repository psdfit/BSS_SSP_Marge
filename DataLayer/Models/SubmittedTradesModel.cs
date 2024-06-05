using System;
using System.Collections.Generic;
using System.Text;

namespace DataLayer.Models
{
    public class SubmittedTradesModel: TradeModel
    {
        public int FormApprovalID { get; set; }
        //public bool? IsApprovedForm { get; set; } // IsApproved field in FormApprovals table.
        public bool? SendBack { get; set; }
        public string Comment { get; set; }
        public int Duration { get; set; }
        public string CertificationCategoryName { get; set; }
        public string CertAuthName { get; set; }
        public string Name { get; set; }
    }
}
