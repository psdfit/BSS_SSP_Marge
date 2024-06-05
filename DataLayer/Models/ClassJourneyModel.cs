using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace DataLayer.Models
{
    public class ClassJourneyModel
    {
        public List<TimeLineChart> ChartData { get; set; }
        public DataTable Finance { get; set; }
        public DataTable Rosi { get; set; }
    }
}
