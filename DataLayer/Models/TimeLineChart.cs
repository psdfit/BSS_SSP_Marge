using System;
using System.Collections.Generic;
using System.Text;

namespace DataLayer.Models
{
    public class TimeLineChart
    {
        public int x { get; set; }
        public string name { get; set; }
        public string label { get; set; }
        public string description { get; set; }

        public TimeLineChart(int x, string name, string label, string description)
        {
            this.x = x;
            this.name = name;
            this.label = label;
            this.description = description;
        }

    }
}
