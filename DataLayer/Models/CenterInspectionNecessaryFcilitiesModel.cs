using System;
using System.Collections.Generic;
using System.Text;

namespace DataLayer.Models
{
    [Serializable]
    public class CenterInspectionNecessaryFcilitiesModel : ModelBase
    {
        public CenterInspectionNecessaryFcilitiesModel() : base() { }
        public CenterInspectionNecessaryFcilitiesModel(bool InActive) : base(InActive) { }
        public string MonitoringID { get; set; }
        public string ReqID { get; set; }
        public string StructIntegValue { get; set; }
        public string KeyFacMissing { get; set; }
        public string KeyFacBuildingStructInteg { get; set; }
        public string KeyFacElecBackup { get; set; }
        public string KeyFacEquipAval { get; set; }
        public string KeyFacFurnitureAval { get; set; }
        public string FMSubmissionDateTime { get; set; }
        public string TspSignatureImgPath { get; set; }
        public string FMSignatureImgPath { get; set; }
        public string TspRepImgPath { get; set; }
        public string DistrictInchargeName { get; set; }
        public string SignOffTspName { get; set; }
        public string FMName { get; set; }
        public string SignOffTspRemarks { get; set; }
        public string SignOffFmRemarks { get; set; }
    }
}
