using System;

namespace DataLayer.Models
{
    [Serializable]
    public class DistrictModel : ModelBase
    {
        public DistrictModel() : base()
        {
        }

        public DistrictModel(bool InActive) : base(InActive)
        {
        }

        public int DistrictID { get; set; }
        public string DistrictName { get; set; }
        public string DistrictNameUrdu { get; set; }
        public int? TehsilID { get; set; }
        public string TehsilName { get; set; }
        public string TehsilNameUrdu { get; set; }
        public int ClusterID { get; set; }
        public string ClusterName { get; set; }

        public int RegionID { get; set; }
        public string RegionName { get; set; }

        public int ProvinceID { get; set; }
        public string ProvinceName { get; set; }
    }}