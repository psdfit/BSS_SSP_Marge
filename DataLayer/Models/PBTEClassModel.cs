using System;
using System.Collections.Generic;

namespace DataLayer.Models
{
    [Serializable]
    public class PBTEClassModel
    {
        //public PBTEModel
        //{
        //}

        //public int PBTEID { get; set; }
        //public List<PBTEClass> PBTEClass { get; set; }
        public int ClassID { get; set; }
        public int SchemeID { get; set; }
        public string ClassCode { get; set; }
        public string SchemeName { get; set; }
        public string PBTESchemeName { get; set; }
        public int Batch { get; set; }

        public string TrainingAddressLocation { get; set; }
        public string PBTEAddress { get; set; }
        public int TraineesPerClass { get; set; }

        public string GenderName { get; set; }
        public string DistrictName { get; set; }
        public string TehsilName { get; set; }
        public string CertAuthName { get; set; }
        public string ClassStatusName { get; set; }
        public int Duration { get; set; }

        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }


        public int TradeID { get; set; }
        public int DistrictID { get; set; }
        public int TSPID { get; set; }
        public string TSPName { get; set; }
        public string TradeName { get; set; }
        public string PBTETradeName { get; set; }



    }




}