using System;
using System.Collections.Generic;
using System.Text;

namespace DataLayer.Models
{
    public class PotentialTraineesModel
    {
        public string TraineeName { get; set; }
        public string TraineeEmail { get; set; }
        public string TraineeCNIC { get; set; }
        public string TraineePhone { get; set; }
        public int GenderID { get; set; }
        public string GenderName { get; set; }
        public int EducationID { get; set; }
        public int DistrictID { get; set; }
        public string DistrictName { get; set; }
        public int TehsilID { get; set; }
        public string TehsilName { get; set; }
        public int TradeID { get; set; }
        public string TradeName { get; set; }
        public int ClassID { get; set; }
        public string ClassCode { get; set; }
        public DateTime TimeStamp { get; set; }
    }
}
