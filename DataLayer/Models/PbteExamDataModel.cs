using System;
using System.Collections.Generic;
using System.Text;

namespace DataLayer.Models
{
  

    public class PbteExamDataModel : ModelBase
    {
        public int Batch { get; set; }
        public int Duration { get; set; }
        public string ClassStartDate { get; set; }
        public string ClassEndDate { get; set; }
        public string ExamYear { get; set; }
        public string SchemeForPBTE { get; set; }

    }
}
