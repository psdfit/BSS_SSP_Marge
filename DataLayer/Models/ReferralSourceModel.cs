using System;
using System.Collections.Generic;
using System.Text;

namespace DataLayer.Models
{
    public class ReferralSourceModel
    {
        public int ReferralSourceID { get; set; }
        public string Name { get; set; }
        public string UrduName { get; set; }
        public string Description { get; set; }
        public bool InActive { get; set; }
    }
}
