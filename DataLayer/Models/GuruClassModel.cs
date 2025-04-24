using System;
using System.Collections.Generic;
using System.Text;

namespace DataLayer.Models
{


    public class GuruClassModel : ModelBase
    {
        public int SchemeID { get; set; } = 0;
        public int TSPID { get; set; } = 0;
        public DateTime? Month { get; set; } = null;
        public string ProgramName { get; set; }
        public string SchemeName { get; set; }
        public string TSPName { get; set; }
        public DateTime? ClassStartDate { get; set; }
        public DateTime? ClassEndDate { get; set; }
        public string GuruName { get; set; }
        public string GuruCNIC { get; set; }
        public string GuruContactNumber { get; set; }
        public string ClassCode { get; set; }
        public string TraineeCode { get; set; }
        public string TraineeName { get; set; }
        public string FatherName { get; set; }
        public string TraineeCNIC { get; set; }
        public string TraineeContactNumber { get; set; }
    }

    public class GuruRecommendationNoteRequest
    {
        public int SchemeID { get; set; }
        public int TSPID { get; set; }
        public int ClassID { get; set; }
        public int TraineeID { get; set; }
        public int OID { get; set; }
        public DateTime? Month { get; set; }
    }


}
