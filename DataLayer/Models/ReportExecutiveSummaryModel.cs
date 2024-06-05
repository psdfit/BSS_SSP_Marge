using System;
using System.Collections.Generic;
using System.Text;

namespace DataLayer.Models
{
    [Serializable]
    public class ReportExecutiveSummaryModel:ModelBase
    {

        public ReportExecutiveSummaryModel() : base()
        {
        }

        public string SchemeName { get; set; }
        public string TSPName { get; set; }
        public string ClassCode { get; set; }
        public string TotalNumberOfRegisteredTrainees { get; set; }
        public string TotalTraineesPresentInClass { get; set; }
    }
}
