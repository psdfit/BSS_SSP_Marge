using System;
using System.Collections.Generic;
using System.Text;

namespace DataLayer.Models
{
    public class SubmitedPOsModel: POHeaderModel
    {
        public int FormApprovalID { get; set; }
        //public bool? IsApprovedForm { get; set; } // IsApproved field in FormApprovals table.
        public bool? SendBack { get; set; }
        public string Comment { get; set; }
    }
}

