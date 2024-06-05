using System;
using System.Collections.Generic;

namespace DataLayer.Models
{
    [Serializable]
    public class PBTETraineeModel
    {
        public int TraineeID { get; set; }
        public int PBTEID { get; set; }
        public int TradeID { get; set; }
        public int ClassID { get; set; }
        public int ExamID { get; set; }
        public string TradeName { get; set; }
        public int SchemeID { get; set; }
        public int TSPID { get; set; }
        public int CollegeID { get; set; }
        public int DistrictID { get; set; }
        public int GenderID { get; set; }
        public int disableId { get; set; }
        public string ClassCode { get; set; }
        public string TraineeCode { get; set; }
        public string TraineeName { get; set; }
        public string FatherName { get; set; }
        public string StatusName { get; set; }
        public string TraineeCNIC { get; set; }
        public string Education { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public DateTime? DOB { get; set; }
        public int pathWayId { get; set; }
        public int Batch { get; set; }
        public string mobile { get; set; }
        public string address { get; set; }
        public string email { get; set; }
        public int instituteId { get; set; }
        public string TraineeImg { get; set; }
        public string ResultStatusName { get; set; }
        public string psdf_traineeId { get; set; }
        public string psdf_ClassCode { get; set; }
        public string TraineeShift { get; set; }
        public DateTime? ShiftFrom { get; set; }
        public DateTime? ShiftTo { get; set; }
        public int CourseID { get; set; }
        public int CourseCategoryID { get; set; }
        //public string StatusName { get; set; }


    }    public class PBTETraineeExamScriptModel
    {
        public int ExamID { get; set; }
        public string ExamSessionUrdu { get; set; }
        public string ExamSessionenglish { get; set; }
        public int maincategoryid { get; set; }
        public int ExamYear { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public string TraineeDescription { get; set; }
        public int Active { get; set; }
        public string ExamSession { get; set; }
        public int Batch { get; set; }
        public int instituteId { get; set; }
        public int qabId { get; set; }
        public string Education { get; set; }
        public int termId { get; set; }
        public int sessionId { get; set; }
        public int shiftId { get; set; }
        public string psdf_ClassCode { get; set; }


    }


}