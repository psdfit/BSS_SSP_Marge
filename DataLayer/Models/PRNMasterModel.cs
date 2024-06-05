using System;
using System.Collections.Generic;
using System.Text;

namespace DataLayer.Models
{
    [Serializable]
    public class PRNMasterModel:ModelBase
    {
        public int PRNMasterID { get; set; }
        public string ProcessKey { get; set; }
        public string ClassCode { get; set; }
        public string ApprovalProcessName { get; set; }
        public string TSPName { get; set; }
        public string TSPColorName { get; set; }
        public string TSPColorCode { get; set; }
        public int InvoiceNumber { get; set; }
        public int TSPID { get; set; }
        public int TSPMasterID { get; set; }
        public bool IsApproved { get; set; }
        public bool IsRejected { get; set; }
        public DateTime? Month { get; set; }
        public int OID { get; set; }
        public int KAMID { get; set; }
        public int? UserID { get; set; }
        public int SchemeID { get; set; }
        //======================Azhar Iqbal PRN Approval =====================
        public int StatusID { get; set; }
        //=================================================
        public string SchemeName { get; set; }
        public int ApprovalStepID { get; set; }   // gives the step at which the approval process lies

    }
}
