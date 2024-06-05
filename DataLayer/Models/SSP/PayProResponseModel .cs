using System;
using System.Collections.Generic;
using System.Text;

namespace DataLayer.Models.SSP
{
   

    public class PayProResponseModel
    {
        public string Click2Pay { get; set; }
        public string Status { get; set; }
        public string Description { get; set; }
        public string ConnectPayId { get; set; }
        public string ConnectPayFee { get; set; }
        public string OrderNumber { get; set; }
        public string OrderAmount { get; set; }
        public string IsFeeApplied { get; set; }
        public DateTime OrderDueDate { get; set; }
        public string OrderType { get; set; }
        public DateTime IssueDate { get; set; }
        public int OrderExpireAfterSeconds { get; set; }
        public string OrderAmountWithinDueDate { get; set; }
        public string OrderAmountAfterDueDate { get; set; }
    }

}
