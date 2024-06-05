using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace DataLayer.Models
{
    public class ManagementDashboard
    {
        public List<GuageChart> SDGuage { get; set; }
        public List<PieChart> SDPie { get; set; }

        public DataTable SchemeDefination { get; set; }

        public List<PieChart> Violations { get; set; }
        public List<PieChart> ExpelledDropouts { get; set; }

        public DataTable Monitoring { get; set; }

        public List<ChartData> Payments { get; set; }

        public DataTable ExamCertification { get; set; }
        public DataTable Placement { get; set; }
        public DataTable ROSI { get; set; }
    }
}
