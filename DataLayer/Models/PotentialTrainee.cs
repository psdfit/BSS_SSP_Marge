using System;
using System.Collections.Generic;
using System.Text;

namespace DataLayer.Models
{
    public class PotentialTrainee
    {
        public string TraineeName { get; set; }
        public string TraineeEmail { get; set; }
        public string TraineeCNIC { get; set; }
        public string TraineePhone { get; set; }
        public int GenderID { get; set; }
        public int EducationID { get; set; }
        public int DistrictID { get; set; }
        public int TradeID { get; set; }
        public DateTime DateTime { get; set; }
    }
}
