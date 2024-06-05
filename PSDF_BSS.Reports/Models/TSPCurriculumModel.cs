using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PSDF_BSS.Reports.Models
{
    [Serializable]
    public class TSPCurriculumModel
    {
        public string ReportName { get; set; }
        public string SchemeName { get; set; }
        public string TypeofScheme { get; set; }
        public string SourceofCurriculum { get; set; }
        public string ValueofContract { get; set; }
        public string DurationofCurriculum { get; set; }
        public string EntryQualification { get; set; }
        public int TotalTraineesContracted { get; set; }
        public string NameofExaminationBody { get; set; }
    }
}