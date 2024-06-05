
using System;
using System.Collections.Generic;

namespace DataLayer.Models
{
    [Serializable]

    public class VisitPlanModel : ModelBase
    {
        public VisitPlanModel() : base() { }
        public VisitPlanModel(bool InActive) : base(InActive) { }

        public int VisitPlanID { get; set; }
        public string VisitType { get; set; }
        public string VisitTypeName { get; set; }
        public int UserID { get; set; }
        public int ClusterID { get; set; }
        public int RegionID { get; set; }
        public int DistrictID { get; set; }
        //public int? SchemeID { get; set; }
        public int? ClassID { get; set; }
        //public string UserName { get; set; }
        public DateTime? VisitStartDate { get; set; }
        public DateTime? VisitEndDate { get; set; }
        public DateTime? VisitStartTime { get; set; }
        public DateTime? VisitEndTime { get; set; }
        public string Attachment { get; set; }
        public string Comments { get; set; }
        public string Venue { get; set; }
        public string TSPName { get; set; }
        public string DistrictName { get; set; }
        public string ClusterName { get; set; }
        public string RegionName { get; set; }
        public string UserStatus { get; set; }
        public string CPAdmissionsName { get; set; }
        public string HeadName { get; set; }
        public string CPLandline { get; set; }

        public bool IsVisited { get; set; }
        public bool LinkWithCRM { get; set; }

        public string UserStatusByCallCenter { get; set; }
        public string NominatedPersonName { get; set; }
        public string NominatedPersonContactNumber { get; set; }
        public string editor { get; set; }

        public List<UserEventMapModel> EventUsers { get; set; }
        public List<ClassEventMapModel> EventClasses { get; set; }
        public List<SchemeEventMapModel> EventSchemes { get; set; }


    }}
