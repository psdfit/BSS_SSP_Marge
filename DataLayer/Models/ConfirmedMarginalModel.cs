using System;
using System.Collections.Generic;
using System.Text;

namespace DataLayer.Models
{
    [Serializable]
    public class ConfirmedMarginalModel:ModelBase
    {

        public ConfirmedMarginalModel() : base()
        {
        }

        public string SchemeName { get; set; }
        public string DetailString { get; set; }
        public string FirstMonth { get; set; }
        public string SecondMonth { get; set; }
        public string TSPName { get; set; }
        public string ClassCode { get; set; }
        public string ClassDuration { get; set; }
        public string TraineeName { get; set; }
        public string TraineeCode { get; set; }
        public string FatherName { get; set; }
        public string TraineeCNIC { get; set; }
        public string FirstMonthMarginal { get; set; }
        public string SecondMonthMarginal { get; set; }
        //public string Remarks { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime VisitMonth { get; set; }
        public string BSSStatus { get; set; }
        //public string ActionToBeTaken { get; set; }

    }
}
