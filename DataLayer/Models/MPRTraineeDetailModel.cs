
using System;

namespace DataLayer.Models
{
[Serializable]
	public MPRTraineeDetailModel():base() { }
    public MPRTraineeDetailModel(bool InActive) : base(InActive) { }
        public string TraineeStatusName { get; set; }
		public string ClassCode { get; set; }
		public string CNICVerificationStatus { get; set; }
		public string TraineeCNIC { get; set; }
		public string ExtraStatus { get; set; }
		public string UID { get; set; }
		public string IsMarginal { get; set; }
		public string mprMonth { get; set; }
		public string SchemeName { get; set; }

	}