using System;
using System.Collections.Generic;
using System.Text;

namespace DataLayer.Models.AMS
{
    public class FormFourModel
    {
        public List<Details> Details { get; set; }
        public Total Total { get; set; }
    }
    public class Details
    {
        public string SchemeName { get; set; }
        public string TSPName { get; set; }
        public DateTime MonthForReport { get; set; }
        public string NumberOfMonthlyVisits { get; set; }
        public string ClassCode { get; set; }
        public int Minor { get; set; }
        public int Major { get; set; }
        public int Serious { get; set; }
        public int Total { get; set; }
        public int Observation { get; set; }
        public string Remarks { get; set; }
    }
    public class Total
    {
        public int TotalMinor { get; set; }
        public int TotalMajor { get; set; }
        public int TotalSerious { get; set; }
        public int TotalObservation { get; set; }
        public int TotalTotal { get; set; }
    }
}
