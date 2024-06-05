using System;
using System.Collections.Generic;
using System.Text;

namespace DataLayer.Models
{
    [Serializable]
    public class CenterInspectionTradeDetailModel : ModelBase
    {
        public CenterInspectionTradeDetailModel() : base() { }
        public CenterInspectionTradeDetailModel(bool InActive) : base(InActive) { }

        public string tradeName { get; set; }
        public string classesPerBatch { get; set; }
        public string totalContractrualTraineesPerClass { get; set; }
        public string quantitySufficient { get; set; }
        public string noOfItemsMissing { get; set; }
        public string noOfRoomsForLab { get; set; }
        public string isSpaceSufficient { get; set; }
        public string powerBackupAvailability { get; set; }
    }
}
