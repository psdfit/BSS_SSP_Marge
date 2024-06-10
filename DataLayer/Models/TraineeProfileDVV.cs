using System;
using System.Collections.Generic;
using System.Text;

namespace DataLayer.Models
{
    public class TraineeProfileDVV
    {
        public int TraineeID { get; set; }
        public string TraineeName { get; set; }
        public string FatherName { get; set; }
        public string TraineeCode { get; set; }
        public string TraineeCNIC { get; set; }
        public int DistrictID { get; set; }
        public DateTime DateOfBirth { get; set; }
        public DateTime CNICIssueDate { get; set; }
        public string MobileNumber { get; set; }
        public decimal Latitude { get; set; }
        public decimal Longitude { get; set; }
        public string BiometricData1 { get; set; }
        public string BiometricData2 { get; set; }
        public string BiometricData3 { get; set; }
        public string BiometricData4 { get; set; }
        public int ClassID { get; set; }
        public int TSPID { get; set; }
        public int GenderID { get; set; }
        public int EducationID { get; set; }
        public int ReligionID { get; set; }
        public int TraineeStatusTypeID { get; set; }
        public string StatusName { get; set; }
        public DateTime? TimeStampOfVerification { get; set; }
        public int CurUserID { get; set; }
        public string GenderName { get; set; }
        public bool IsExtra { get; set; }
        public int PermanentDistrict { get; set; }
        public int PermanentTehsil { get; set; }
        public string PermanentResidence { get; set; }
        public int TemporaryDistrict { get; set; }
        public int TemporaryTehsil { get; set; }
        public string TemporaryResidence { get; set; }
    }
}
