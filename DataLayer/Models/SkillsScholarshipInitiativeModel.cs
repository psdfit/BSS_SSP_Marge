using System;

namespace DataLayer.Models
{
    [Serializable]
    public class SkillsScholarshipInitiativeModel :ModelBase
    {

        public SkillsScholarshipInitiativeModel() { }

        public string TradeName { get; set; }
        public int TradeID { get; set; }
        public int TradeTarget { get; set; }
        public int NoOfAssociate { get; set; }
        public int OverallEnrolments { get; set; }
        public int EnrolmentsCompleted { get; set; }
        public int RemainingSeats { get; set; }
        public string ClusterName { get; set; }
        public int ClusterID { get; set; }
        public Boolean HasRaceStarted { get; set; }
        public Boolean RaceStopped { get; set; }
        public int SchemeID { get; set; }
        public string TSPName { get; set; }
        public string UserName { get; set; }
        public string LoginDate { get; set; }
        public int TSPID { get; set; }
        public int SessionID { get; set; }
        public string IPAddress { get; set; }
        public double ageCompleted { get; set; }
        public int DistrictID { get; set; }
        public string DistrictName { get; set; }


    }
}
