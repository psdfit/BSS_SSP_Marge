using System;
using System.Collections.Generic;
using System.Text;

namespace DataLayer.Models.SSP
{
    public class HistoricalReportModel : ModelBase
    {
        public HistoricalReportModel() { }


        public DateTime? ClassStartDate { get; set; } = null;
        public DateTime? ClassEndDate { get; set; } = null;
        public string? FundingSource { get; set; } = null;
        public string? ProgramFocus { get; set; } = null;
        public string? ProgramType { get; set; } = null;
        public string? Sector { get; set; } = null;
        public string? SubSector { get; set; } = null;
        public string? Trade { get; set; } = null;
        public string? TSPMaster { get; set; } =null;
        public string? Cluster { get; set; } = null;
        public string? District { get; set; } = null;

   

    }
}
