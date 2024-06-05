using System;
using System.Collections.Generic;
using System.Text;

namespace DataLayer.Models
{
    [Serializable]
    public class CenterInspectionComplianceModel: ModelBase
    {
        public CenterInspectionComplianceModel() : base() { }
        public CenterInspectionComplianceModel(bool InActive) : base(InActive) { }

        public string parameter { get; set; }
        public string complaince { get; set; }
        public string observatoryRemarks { get; set; }
        public string psdfStandard { get; set; }
        public string recommendationRemarks { get; set; }
    }
}
