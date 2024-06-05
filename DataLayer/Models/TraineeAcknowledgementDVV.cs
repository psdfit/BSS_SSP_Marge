using System;
using System.Collections.Generic;
using System.Text;

namespace DataLayer.Models
{
    public class TraineeAcknowledgementDVV
    {
        public int AknowledgementTypeID { get; set; }
        public int TraineeID { get; set; }
        public decimal Latitude { get; set; }
        public decimal Longitude { get; set; }
        public string BiometricData1 { get; set; }
        public DateTime TimeStamp { get; set; }
        public int CurUserID { get; set; }

        public int TSPId { get; set; }
    }
}
