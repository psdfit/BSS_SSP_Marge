using System;
using System.Collections.Generic;

namespace DataLayer.Models
{
    [Serializable]
    public class PBTEModel
    {
        //public PBTEModel
        //{
        //}

        //public int PBTEID { get; set; }
        //public List<PBTEClass> PBTEClass { get; set; }
        public int ClassID { get; set; }
        public string ClassCode { get; set; }
        public string TSPCode { get; set; }
        public string TraineeCode { get; set; }
        public int PBTEID { get; set; }
        public int NAVTTCID { get; set; }
        public int PBTEDistrictID { get; set; }
        public int PBTETradeID { get; set; }
        public int CollegeID { get; set; }
        public int PBTEStudentID { get; set; }
        //public int ExamID { get; set; }
        public int TSPID { get; set; }
        public int TraineeID { get; set; }
        public int TradeID { get; set; }
        public string TradeCode { get; set; }
        public string ResultStatusName { get; set; }
        public string Batch { get; set; }


        public int ClassesCount { get; set; }
        public int TSPsCount { get; set; }
        public int TraineesCount { get; set; }
        public int DropOutTraineesCount { get; set; }
        public string PBTECollegeID { get; set; }
        public string PBTESchemeID { get; set; }
        public string NAVTTCCollegeID { get; set; }
        public string CourseID { get; set; }
        public string Course_CategoryID { get; set; }
        public string ExamID { get; set; }


    }


    //public class PBTEClass
    //{
    //    public List<int> ClassID { get; set; }
    //    public List<int> PBTEID { get; set; }
    //}




}