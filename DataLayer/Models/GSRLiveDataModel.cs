using System;
using System.Collections.Generic;
using System.Text;

namespace DataLayer.Models
{
    public class GSRLiveDataModel
    {
        public GSRLiveDataModel() : base()
        {
        }

        public string GuruName { get; set; }
        public string GuruCNIC { get; set; }
        public string GuruContactNumber { get; set; }
        public string TraineeCode { get; set; }
        public string TraineeName { get; set; }
        public string TraineeCNIC { get; set; }
        public string FatherName { get; set; }
        public string TSPName { get; set; }
        public string ClassCode { get; set; }
    }
}