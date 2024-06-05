
using System;

namespace DataLayer.Models
{
    [Serializable]

    public class RTPModel : ModelBase
    {
        public RTPModel() : base() { }
        public RTPModel(bool InActive) : base(InActive) { }

        public int RTPID { get; set; }
        public bool RTPValue { get; set; }
        public int ClassID { get; set; }
        public int DistrictID { get; set; }
        public int TehsilID { get; set; }
        //public string TradeName { get; set; }
        public string ClassCode { get; set; }
        public string Name { get; set; }    // source of curriculum name
        public string CPName { get; set; }
        public string CPLandline { get; set; }
        public string TradeName { get; set; }
        public string DistrictName { get; set; }
        public string TehsilName { get; set; }
        public int TraineesPerClass { get; set; }
        public double Duration { get; set; }
        public string AddressOfTrainingLocation { get; set; }
        public string Comments { get; set; }
        public bool IsApproved { get; set; }
        public bool IsRejected { get; set; }
        public bool NTP { get; set; }
        public bool CenterInspection { get; set; }
        public string CenterInspectionValue { get; set; }

        public string SchemeName { get; set; }
        public string TSPName { get; set; }        public DateTime StartDate { get; set; }        public DateTime EndDate { get; set; }

        public int UserID { get; set; }
        public int KAMID { get; set; }
        public int OID { get; set; }
       




    }
    public class RTPByKAMModel
    {

        public int UserID { get; set; }   // it is the KAMID 
        public int OID { get; set; }
        public int SchemeID { get; set; }
        public int TSPID { get; set; }
        public int ClassID { get; set; }

    }    public class NTPByUserModel
    {

        public int UserID { get; set; }   // Azhar  
        public int SchemeID { get; set; }
        public int TSPID { get; set; }
        public int ClassID { get; set; }
        public int StatusID { get; set; }

    }}
