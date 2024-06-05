using System;
using System.Collections.Generic;
using System.Text;

namespace DataLayer.Models
{
    public class TradeCategoryModel
    {
        public int TradeID { get; set; }
        public string TradeName { get; set; }
        public int CertificationCategoryID { get; set; }
        public string CertificationCategoryName { get; set; }
        public string SAPID { get; set; }
    }
}
