using System;
using System.Collections.Generic;
using System.Text;

namespace DataLayer.Models
{
    public class SRNDataModel : ModelBase
    {
        public SRNDataModel() : base() { }
        public int SrnId { get; set; }
        public string ReportId { get; set; }
        public int TraineeId { get; set; }
        public decimal Amount { get; set; }
        public string TokenNumber { get; set; }
        public string TransactionNumber { get; set; }
        public string Comments { get; set; }
        public bool IsPaid { get; set; }
        public bool IsVarified { get; set; }
        public DateTime Month { get; set; }
        public int NumberOfMonths { get; set; }

        public string TraineeCode { get; set; }
        public string TraineeName { get; set; }
        public string FatherName { get; set; }
        public string TraineeCNIC { get; set; }
        public int SchemeID { get; set; }
        public int TspID { get; set; }
        public int ClassID { get; set; }
        public string ContactNumber1 { get; set; }
        public string ClassCode { get; set; }
        public string TrainingAddressLocation { get; set; }
        public int Batch { get; set; }
        public string TSPName { get; set; }
        public int TSPID { get; set; }
        public string TSPCode { get; set; }
        public string DistrictName { get; set; }
        public string TradeName { get; set; }
    }
}
