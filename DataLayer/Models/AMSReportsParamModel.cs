using System;
using System.Collections.Generic;
using System.Text;

namespace DataLayer.Models
{
    public class AMSReportsParamModel : ModelBase
    {

        public AMSReportsParamModel() : base()
        {
        }

        public int SchemeID { get; set; }
        public int TSPID { get; set; }
        public int ClassID { get; set; }
        public int UserID { get; set; }
        public DateTime Month { get; set; }
        //public int UserID { get; set; }
        public string? KAMID { get; set; }
        public string? FundingCategoryID { get; set; }


    }
}
