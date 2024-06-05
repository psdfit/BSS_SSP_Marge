using System;
using System.Collections.Generic;
using System.Text;

namespace DataLayer.Models
{
    public class ChartData
    {
        public string name { get; set; }
        public decimal y { get; set; }
        public string color { get; set; }

        public ChartData(string name, decimal y, string color)
        {
            this.name = name;
            this.y = y;
            this.color = color;
        }
    }
}
