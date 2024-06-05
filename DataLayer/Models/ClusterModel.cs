using System;

namespace DataLayer.Models
{
    [Serializable]
    public class ClusterModel : ModelBase
    {
        public ClusterModel() : base()
        {
        }

        public ClusterModel(bool InActive) : base(InActive)
        {
        }

        public Int32 ClusterID { get; set; }
        public Int32 ProvinceID { get; set; }
        public String ClusterName { get; set; }
    }}