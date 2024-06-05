using System;
using System.Collections.Generic;
using System.Text;

namespace DataLayer.Models.SSP
{
    public class TradeMapModel : ModelBase
    {

        public TradeMapModel() { }
        public int UserID { get; set; }
        public int TradeManageID { get; set; }
        public string TrainingDuration { get; set; }
        public string tradeManageIds { get; set; }
        public int TrainingLocationID { get; set; }
        public int CertificateID { get; set; }
        public int TradeID { get; set; }
        public string TradeAsPerCer { get; set; }
        public string ProcurementRemarks { get; set; }
        public int ApprovalLevel { get; set; }
        public int Status { get; set; }
        public string Remarks { get; set; }
        public int NoOfClassMor { get; set; }
        public int ClassCapacityMor { get; set; }
        public int NoOfClassEve { get; set; }
        public int ClassCapacityEve { get; set; }


    }
}
