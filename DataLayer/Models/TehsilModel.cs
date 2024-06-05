using System;

namespace DataLayer.Models
{
    [Serializable]
    public class TehsilModel : ModelBase
    {
        public TehsilModel() : base()
        {
        }

        public TehsilModel(bool InActive) : base(InActive)
        {
        }

        public int TehsilID { get; set; }
        public string TehsilName { get; set; }
        public int DistrictID { get; set; }
        public string DistrictName { get; set; }
        public string DistrictNameUrdu { get; set; }
    }}