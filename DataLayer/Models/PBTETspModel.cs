using System;
using System.Collections.Generic;

namespace DataLayer.Models
{
    [Serializable]
    public class PBTETspModel
    {
        public int TSPID { get; set; }
        public int TSPMasterID { get; set; }
        public int SchemeID { get; set; }
        public string TSPName { get; set; }
        public string TSPCode { get; set; }
        public int ClassID { get; set; }
        public int TradeID { get; set; }
        public int DistrictID { get; set; }
        public string Address { get; set; }
        public string HeadName { get; set; }
        public string HeadDesignation { get; set; }
        public string HeadEmail { get; set; }
        public string HeadLandline { get; set; }
        public string OrgLandline { get; set; }

        public string Website { get; set; }
        public string CPName { get; set; }
        public string CPDesignation { get; set; }
        //public int CPMobile	{ get; set; }
        public string CPLandline { get; set; }
        public string CPEmail { get; set; }

    }







}