
using System;

namespace DataLayer.Models
{
    [Serializable]

    public class HomeStatsModel : ModelBase
    {
        public HomeStatsModel() : base() { }
        public HomeStatsModel(bool InActive) : base(InActive) { }

        public int Schemes { get; set; }
        //public string SchemeName { get; set; }
        //public string SchemeCode { get; set; }
        public int Classes { get; set; }
        //public string ClassCode { get; set; }

        public int TSPs { get; set; }
        //public string TSPName { get; set; }

        public int Trainees { get; set; }
        public int SavedTrainees { get; set; }
        public int PendingInceptionReports { get; set; }
        public int PendingRegisterations { get; set; }
        public int PendingRTPs { get; set; }
        public int PendingEmployments { get; set; }
        public DateTime? InceptionReportDeadline { get; set; }
        public DateTime? TraineeRegistrationDeadline { get; set; }
        public DateTime? RTPDeadline { get; set; }
        public DateTime? EmploymentDeadline { get; set; }
        public int Planned { get; set; }
        public int Active { get; set; }
        public int Completed { get; set; }
        public int Abandoned { get; set; }
        public int Cancelled { get; set; }
        public int Ready { get; set; }
        public int Suspended { get; set; }


        //public int OID { get; set; }
        //public int SchemeID { get; set; }
        //public int TSPID { get; set; }
        //public int ClassID { get; set; }
        //public int TraineeID { get; set; }
        //public int UserID { get; set; }

    }}
