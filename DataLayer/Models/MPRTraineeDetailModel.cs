
using System;

namespace DataLayer.Models
{
[Serializable]public class MPRTraineeDetailModel :ModelBase {
	public MPRTraineeDetailModel():base() { }
    public MPRTraineeDetailModel(bool InActive) : base(InActive) { }	public int MPRDetailID	{ get; set; }	public int MPRID	{ get; set; }	public string TSPName { get; set; }	public int TraineeID	{ get; set; }	public string TraineeName { get; set; }	public string TraineeCode	{ get; set; }	public bool AttendanceMet	{ get; set; }	public string Reason	{ get; set; }	public int TraineeStatus	{ get; set; }	public double StipendAmount	{ get; set; }
        public string TraineeStatusName { get; set; }
		public string ClassCode { get; set; }
		public string CNICVerificationStatus { get; set; }
		public string TraineeCNIC { get; set; }
		public string ExtraStatus { get; set; }
		public string UID { get; set; }
		public string IsMarginal { get; set; }
		public string mprMonth { get; set; }
		public string SchemeName { get; set; }

	}}
