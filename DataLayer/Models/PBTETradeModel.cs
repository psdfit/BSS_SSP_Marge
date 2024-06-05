
using System;
using System.Collections.Generic;

namespace DataLayer.Models
{
    [Serializable]

    public class PBTETradeModel : ModelBase
    {
        public PBTETradeModel() : base() { }
        public PBTETradeModel(bool InActive) : base(InActive) { }

        public int TradeID { get; set; }
        public int Duration { get; set; }
        public string TradeName { get; set; }      
        public string TradeCode { get; set; }      
        public bool IsApproved { get; set; }
        


    }}
