using System;
using System.Collections.Generic;
using System.Text;

namespace DataLayer.Models
{
    [Serializable]
    public class POSummaryModel
    {
        public string SchemeName { get; set; }
        public string TSPName { get; set; }
        public string Trades { get; set; }
        public int ClassCount { get; set; }
        public double SchemeCost { get; set; }
        public double TotalCostPerClassInTax { get; set; }
        public double TaxRate { get; set; }
        public double SalesTax { get; set; }
        public int PST { get; set; }
        public double FinalAmount { get; set; }
        public DateTime Month { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int LineTotal { get; set; }
        public int SchemeID { get; set; }
        public int TSPID { get; set; }
        public string ProcessKey { get; set; }
        public int TraineesPerClass { get; set; }
        public string ClassCode { get; set; }
        public string TradeName { get; set; }

    }
}
