
using System;

namespace DataLayer.Models
{
    [Serializable]

    public class ClassChangeRequestModel : ModelBase
    {
        public ClassChangeRequestModel() : base() { }
        public ClassChangeRequestModel(bool InActive) : base(InActive) { }

        public int ClassChangeRequestID { get; set; }
        public int ClassDatesChangeRequestID { get; set; }
        public int ClassID { get; set; }
        public int TradeID { get; set; }
        public string TradeName { get; set; }
        public int SourceOfCurriculumID { get; set; }
        public string Name { get; set; }
        public int CertAuthID { get; set; }
        public string CertAuthName { get; set; }
        public int ClusterID { get; set; }
        public string ClusterName { get; set; }
        public int TehsilID { get; set; }
        public string TehsilName { get; set; }
        public int DistrictID { get; set; }
        public string DistrictName { get; set; }
        public string ClassCode { get; set; }
        public string TrainingAddressLocation { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public decimal Duration { get; set; }
        public bool IsApproved { get; set; }
        public bool IsRejected { get; set; }
        public bool CrClassLocationIsApproved { get; set; }
        public bool CrClassLocationIsRejected { get; set; }
        public int CrClassLocationID { get; set; }

        public bool CrClassDatesIsApproved { get; set; }
        public bool CrClassDatesIsRejected { get; set; }
        public int CrClassDatesID { get; set; }

    }}
