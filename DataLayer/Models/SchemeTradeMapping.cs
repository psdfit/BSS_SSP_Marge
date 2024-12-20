/* **** Aamer Rehman Malik *****/

using System;

namespace DataLayer.Models
{
    [Serializable]
    public class SchemeTradeMapping : ModelBase
    {
        
        public int SchemeID { get; set; }
        public int TradeID { get; set; }
        public int PTypeID { get; set; }
        public string TradeTarget { get; set; }
        public string TradeName { get; set; }
        public string ClusterName { get; set; }
        public int ClusterID { get; set; }
        public string DistrictName { get; set; }
        public int DistrictID { get; set; }
    }


}