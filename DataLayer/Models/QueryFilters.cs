using System;
using System.Collections.Generic;
using System.Text;

namespace DataLayer.Models
{
    public class QueryFilters
    {
        public int SchemeID { get; set; }
        public int TSPID { get; set; }
        public int ClassID { get; set; }
        public int TraineeID { get; set; }
        public string? TraineeIDs { get; set; }
        public int UserID { get; set; }
        public int OID { get; set; }
        public string ProcessKey { get; set; }
        public DateTime? Month { get; set; }
        public int TradeID { get; set; }
        public int KAMID { get; set; }
        public string? ClassIDs { get; set; }
        public int CertAuthID { get; set; }
        public int Locality { get; set; }
    }
}
