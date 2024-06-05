using System;
using System.Collections.Generic;
using System.Text;

namespace DataLayer.Models
{
    public class AMS_Visit
    {
        public int VisitorId{ get; set; }
        public DateTime VisitDate { get; set; }
        public int[] AbsentTrainees{ get; set; }
    }
}
