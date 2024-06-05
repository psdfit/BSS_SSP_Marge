using System;
using System.Collections.Generic;
using System.Text;

namespace DataLayer.Models
{
    public class TraineeAttendanceDVV
    {
        public int TraineeID { get; set; }
        public bool CheckIn { get; set; }
        public bool CheckOut { get; set; }
        public DateTime TimeStamp { get; set; }
        public decimal Latitude { get; set; }
        public decimal Longitude { get; set; }
        public string BiometricData1 { get; set; }
        public int CurUserID { get; set; }
        public int TSPId { get; set; }
    }
}
