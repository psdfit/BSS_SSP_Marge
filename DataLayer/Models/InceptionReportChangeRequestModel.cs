
using System;

namespace DataLayer.Models
{
    [Serializable]

    public class InceptionReportChangeRequestModel : ModelBase
    {
        public InceptionReportChangeRequestModel() : base() { }
        public InceptionReportChangeRequestModel(bool InActive) : base(InActive) { }

        public int InceptionReportChangeRequestID { get; set; }
        public int IncepReportID { get; set; }
        public int ClassID { get; set; }
        public int KamID { get; set; }
        public int SchemeID { get; set; }
        public int TSPID { get; set; }
        public string ClassCode { get; set; }
        public string TradeName { get; set; }
        public DateTime ClassStartTime { get; set; }
        public DateTime ClassEndTime { get; set; }
        public string ClassTotalHours { get; set; }
        public string Shift { get; set; }
        public bool Monday { get; set; }
        public bool Tuesday { get; set; }
        public bool Wednesday { get; set; }
        public bool Thursday { get; set; }
        public bool Friday { get; set; }
        public bool Saturday { get; set; }
        public bool Sunday { get; set; }
        public bool IsApproved { get; set; }
        public bool IsRejected { get; set; }
        public string InstrIDs { get; set; }
        public string Process_Key { get; set; }
        public string TSPName { get; set; }
        public string SchemeName { get; set; }

    }}
