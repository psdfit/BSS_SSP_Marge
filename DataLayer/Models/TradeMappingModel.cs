using System;
using System.Collections.Generic;
using System.Text;

namespace DataLayer.Models
{
   

        public class TradeMappingModel : ModelBase
        {
            public int PBTETradeID { get; set; }
            public string TradeName { get; set; }
            public int TradeID { get; set; }
            public int Duration { get; set; }
        }
}
