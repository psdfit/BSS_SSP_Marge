using System;
using System.Collections.Generic;
using System.Text;

namespace DataLayer.Models.DVV
{
    public class BiometricTraineeDataModel:ModelBase
    {
        public int TraineeID { get; set; }
        public int ClassID { get; set; }
        public string RightIndexFinger { get; set; }
        public string RightMiddleFinger { get; set; }
        public string LeftIndexFinger { get; set; }
        public string LeftMiddleFinger { get; set; }
        public DateTime Date { get; set; }
        public string Time { get; set; }
        public string AttendanceType { get; set; }

        public string FingerImpression { get; set; }

    }
}
