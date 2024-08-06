using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace DataLayer.Models
{
    public class PBTECenterLocationMappingModel:ModelBase
    {
            public int PBTECollegeID { get; set; }   
            public string TSPName { get; set; }
            public string TSPCenterLocation { get; set; }
            public string TSPCenterDistrict { get; set; }
        
    }
}
