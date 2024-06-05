using System;

namespace DataLayer.Models
{
    [Serializable]
    public class RegionModel : ModelBase
    {
        public RegionModel() : base()
        {
        }

        public RegionModel(bool InActive) : base(InActive)
        {
        }

        public int RegionID { get; set; }
        public string RegionName { get; set; }
    }}