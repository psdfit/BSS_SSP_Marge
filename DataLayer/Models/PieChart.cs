using System;
using System.Collections.Generic;
using System.Text;

namespace DataLayer.Models
{
    public class PieChart
    {
        public string name { get; set; }
        public int y { get; set; }

        public PieChart(string name, int y) {
            this.name = name;
            this.y = y;
        }
    }
}
