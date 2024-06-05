using System;
using System.Collections.Generic;
using System.Text;

namespace DataLayer.Models
{
    public class GuageChart
    {
        public string name { get; set; }
        public List<GuageSeriesData> data { get; set; }

        public GuageChart(string name, List<GuageSeriesData> data) {
            this.name = name;
            this.data = data;
        }
    }

    public class GuageSeriesData {
        public string color { get; set; }
        public string radius { get; set; }
        public string innerRadius { get; set; }
        public decimal y { get; set; }

        public GuageSeriesData(string color, string radius, string innerRadius, decimal y) {
            this.color = color;
            this.radius = radius;
            this.innerRadius = innerRadius;
            this.y = y;
        }
    }
}
