using System;
using System.Collections.Generic;
using System.Text;

namespace DataLayer.Models
{
    public class SAPResponseModel
    {
        public string Status { get; set; }
        public string Message { get; set; }
        public string SapObjId { get; set; }
        //public bool StatusBit { get; set; }
    }
}
