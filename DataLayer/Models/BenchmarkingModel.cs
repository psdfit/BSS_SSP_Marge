
using System;

namespace DataLayer.Models
{
[Serializable]
	public BenchmarkingModel():base() { }
    public BenchmarkingModel(bool InActive) : base(InActive) { }

        public int BenchmarkingID { get; set; }
        public string TradeName	{ get; set; }
     public int OfferedAmount { get; set; }
     public int CalculatedAmount { get; set; }
     public int CalculatedAmount70 { get; set; }
        
     public int ProposedAmount50 { get; set; }
        //public int DistrictID { get; set; }
        //public int ClusterID { get; set; }
        //public int RegionID { get; set; }
        //public bool InRecentSchemes { get; set; }
        public int TotalClasses { get; set; }
        //public float Inflation { get; set; }
        public DateTime SchemeDate { get; set; }
        public string PTypeName { get; set; }


    }