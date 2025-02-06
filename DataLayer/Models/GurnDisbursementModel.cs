
using System;

namespace DataLayer.Models
{
    [Serializable]

    public class GurnDisbursementModel
    {
        public int TraineeID { get; set; }
        public string TraineeCode { get; set; }
        public string TraineeName { get; set; }
        public string FatherName { get; set; }
        public string TraineeCNIC { get; set; }
        public string ContactNumber { get; set; }
        public int Batch { get; set; }
        public string DistrictName { get; set; }
        public string Comments { get; set; }
        public decimal Amount { get; set; }
        public int NumberOfMonths { get; set; }
        public string TradeName { get; set; }
        public string ClassCode { get; set; }
        public string SchemeName { get; set; }
        public string TSPName { get; set; }
        public string TokenNumber { get; set; }
        public string TransactionNumber { get; set; }
        public string Redeem { get; set; }
        public DateTime Month { get; set; }
    }
    public class GurnDisbursementFiltersModel
    {
        public DateTime? Month { get; set; }
        public int SchemeID { get; set; }
        public int TSPID { get; set; }
        public int ClassID { get; set; }
 
    }}
