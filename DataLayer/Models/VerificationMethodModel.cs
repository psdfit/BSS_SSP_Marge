using System;
using System.Collections.Generic;
using System.Text;

namespace DataLayer.Models
{
    public class VerificationMethodModel
    {
        public int VerificationMethodID { get; set; }
        public string VerificationMethodType { get; set; }
        public int PlacementTypeID { get; set; }
    }
}
