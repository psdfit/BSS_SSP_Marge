using System;
using System.Collections.Generic;
using System.Text;

namespace DataLayer.Models
{
    public class CenterInspectionTradeToolModel: ModelBase
    {
        public CenterInspectionTradeToolModel() : base() { }
        public CenterInspectionTradeToolModel(bool InActive) : base(InActive) { }

        public string TradeID { get; set; }
        public string CentreMonitoringID { get; set; }
        public string TradeName { get; set; }
        public string TradeDuration { get; set; }
        public string headCount { get; set; }
        public string ToolName { get; set; }
        public string QuantityTotal { get; set; }
        public string QuantityFound { get; set; }

    }
}
