using System;
using System.Collections.Generic;
using System.Text;

namespace DataLayer.Models.DVV
{
    public class BiometricAttendanceTraineeModel : ModelBase
    {
        public int? TraineeID { get; set; }
        public int? SchemeID { get; set; }
        public int? TspID { get; set; }
        public int? ClassID { get; set; }
        public int? UserID { get; set; }
        public int? OID { get; set; }
        public int? PageNo { get; set; }
        public int? PageSize { get; set; }
        public string SortColumn { get; set; }
        public string SortOrder { get; set; } = "desc";

        // Additional properties for returning data from the stored procedure
        public string TraineeName { get; set; }
        public string FatherName { get; set; }
        public int TotalRows { get; set; }



    }
}
