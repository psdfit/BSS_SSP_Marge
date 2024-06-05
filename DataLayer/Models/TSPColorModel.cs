using System;
using System.Collections.Generic;
using System.Text;

namespace DataLayer.Models
{
  public  class TSPColorModel: ModelBase
    {
        public int? TSPMasterID { get; set; }
        public string TSPName { get; set; }
        public string Address { get; set; }
        public string TSPCode { get; set; }
        public int? TSPColorID { get; set; }
        public string TSPColorName { get; set; }
        public int? TSPColorHistoryID { get; set; }
        public string NTN { get; set; }
        public string TSPColorCode { get; set; }
        public string FullName { get; set; }

    }    
    public  class TSPColorFiltersModel: ModelBase
    {
        public int TSPID { get; set; }
        public int ClassID { get; set; }

    }
    
    public  class BlackListCriteriaModel
    {
        public string ErrorMessage { get; set; }
        public int ErrorTypeID { get; set; }
        public string ErrorTypeName { get; set; }

    }
}
