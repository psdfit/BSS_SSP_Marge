
using System;

namespace DataLayer.Models
{
    [Serializable]

    public class TSPMasterModel : ModelBase
    {
        public TSPMasterModel() : base() { }
        public TSPMasterModel(bool InActive) : base(InActive) { }

        public int TSPMasterID { get; set; }
        public string TSPName { get; set; }
        public string Address { get; set; }
        public string NTN { get; set; }
        public string PNTN { get; set; }
        public string GST { get; set; }
        public string FTN { get; set; }
        public string UID { get; set; }
        public int? UserID { get; set; }
        public int? KAMID { get; set; }
        public int? ClassID { get; set; }
        public string SAPID { get; set; }
    }}
