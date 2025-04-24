using System;
using System.Collections.Generic;
using System.Text;

namespace DataLayer.Models.IP
{
    public class IPTraineesModel : ModelBase
    {
        public string Scheme { get; set; }
        public string TSPName { get; set; }
        public int TSPID { get; set; }
        public string ClassCode { get; set; }
        public string TraineeCode { get; set; }
        public string TraineeName { get; set; }
        public string TraineeCNIC { get; set; }
        public string TraineeStatusName { get; set; }
        public string District { get; set; }
    }
}
