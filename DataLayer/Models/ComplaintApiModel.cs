using System;
using System.Collections.Generic;
using System.Text;

namespace DataLayer.Models
{
 public   class ComplaintApiModel
    {
        public string ComplaintNo { get; set; }
        public string ComplaintTypeName { get; set; }
        public string ComplaintSubTypeName { get; set; }
        public string ComplaintStatusType { get; set; }
        public string ComplaintDescription { get; set; }
        public string TSPName { get; set; }
        public string TSPCode { get; set; }
        public string TraineeCode { get; set; }
        public string TraineeName { get; set; }
        public string TraineeCNIC { get; set; }
        public string FatherName { get; set; }

        public class ComplaintHistoryApiModel
        {
            public string ComplaintNo { get; set; }
            public string Status { get; set; }
            public string Comments { get; set; }
            public Nullable<DateTime> CreatedDate { get; set; }
        }

    }
}
