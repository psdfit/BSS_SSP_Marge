using System;
using System.Collections.Generic;
using System.Text;

namespace DataLayer.Models
{
   public class GISModel
    {
        public string Latitude { get; set; }
        public string Longitude { get; set; }
        public int? DistrictID { get; set; }
        public int? ClusterID { get; set; }
        public string ClusterName { get; set; }
        public int? SectorID { get; set; }
        public string SectorName { get; set; }
        public int? TehsilID { get; set; }
        public int? PTypeID { get; set; }
        public int? MaleCount { get; set; }
        public int? FemaleCount { get; set; }
        public int? TransgenderCount { get; set; }
        public int? ClassCount { get; set; }
        public string TSPName { get; set; }

    }
}
