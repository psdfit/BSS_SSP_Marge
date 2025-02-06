using System;
using System.Collections.Generic;
using System.Text;

namespace DataLayer.Models
{
    public class SearchFilter
    {
        public int SchemeID { get; set; }
        public int TSPID { get; set; }
        public int ClassID { get; set; }
        public int TraineeID { get; set; }
        public string Schemes { get; set; }
        public string TSPs { get; set; }
        public string Classes { get; set; }
        public string Trainees { get; set; }
        public int OID { get; set; }
        public int UserID { get; set; }
        public int TradeID { get; set; }
        public int DistrictID { get; set; } 
        public int TehsilID { get; set; }
        public List<string> SelectedColumns { get; set; }

        // New fields for filtering by Month and Year
        public int? Month { get; set; }
        public int? Year { get; set; }
    }
}
