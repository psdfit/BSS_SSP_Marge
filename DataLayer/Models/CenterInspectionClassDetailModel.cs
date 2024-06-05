using System;
using System.Collections.Generic;
using System.Text;

namespace DataLayer.Models
{
    [Serializable]
    public class CenterInspectionClassDetailModel : ModelBase
    {
        public CenterInspectionClassDetailModel() : base() { }
        public CenterInspectionClassDetailModel(bool InActive) : base(InActive) { }

        public string ClassCode { get; set; }
        public string TradeName { get; set; }
        public string ExpectedStartDate { get; set; }
        public string BoardAval { get; set; }
        public string SufficientFurniture { get; set; }
        public string LightAval { get; set; }
        public string VentFanAval { get; set; }
        public string ClassSpaceSufficient { get; set; }
    }
}
