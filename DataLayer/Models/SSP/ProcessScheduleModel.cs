using System;
using System.Collections.Generic;
using System.Text;

namespace DataLayer.Models.SSP
{
    public class ProcessScheduleModel : ModelBase
    {
        public ProcessScheduleModel() { }
        public int ProcessScheduleMasterID { get; set; } = 0;

        public int UserID { get; set; }
        public int ProgramID { get; set; }
        public DateTime ProgramStartDate { get; set; }
        public int TotalDays { get; set; }
        public int TotalProcess { get; set; }
        public List<ProcessDetail> processDetails { get; set; } = new List<ProcessDetail>();

    }

    public class ProcessDetail
    {
        public int ProcessScheduleDetailID { get; set; } = 0;
        public int UserID { get; set; }

        public int ProcessID { get; set; }
        public DateTime ProcessStartDate { get; set; }
        public DateTime ProcessEndDate { get; set; }
        public int ProcessDays { get; set; } = 0;
        public int IsLocked { get; set; } = 0;
    }
}
