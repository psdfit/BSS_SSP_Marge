using System;
using System.Collections.Generic;
using System.Text;

namespace DataLayer.Models
{
    public class InstructorDVV
    {
        public int InstructorID { get; set; }
        public string Name { get; set; }
        public string CNIC { get; set; }
        public decimal Latitude { get; set; }
        public decimal Longitude { get; set; }
        public string BiometricData1 { get; set; }
        public string BiometricData2 { get; set; }
        public string BiometricData3 { get; set; }
        public string BiometricData4 { get; set; }
        public int ClassID { get; set; }
        public int GenderID { get; set; }
        public int CurUserID { get; set; }
        public DateTime TimeStampOfVerification { get; set; }
        public string QualificationHighest { get; set; }
        public string TotalExperience { get; set; }
        public bool IsVerifiedByDVV { get; set; }
        public string LocationAddress { get; set; }
    }
}
