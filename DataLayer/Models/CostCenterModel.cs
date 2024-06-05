using System;
using System.Collections.Generic;
using System.Text;

namespace DataLayer.Models
{

    public class CostCenterModel
    {
        public string U_BSS_Id { get; set; }
        public string PrcName { get; set; }
        public string GrpCode { get; set; }
        public string CCTypeCode { get; set; }
        public string ValidFrom { get; set; }
        public string ValidTo { get; set; }
        public string CCOwner { get; set; }
        public string Active { get; set; }
        public string U_Cost_Cntr_Name { get; set; }
        public string Type { get; set; }
        public string TradeType { get; set; }
    }

}
