using System;

namespace DataLayer.Models
{
    [Serializable]
    public class CenterInspectionModel : ModelBase
    {
        public CenterInspectionModel() : base()
        {
        }

        public CenterInspectionModel(bool InActive) : base(InActive)
        {
        }

        public int ClassID { get; set; }
        public string ClassCode { get; set; }
        public string TradeName { get; set; }
        public string TSPName { get; set; }
        public string TrainingCentreName { get; set; }
        public DateTime VisitDateTime { get; set; }
        public string NameOfProject { get; set; }
        public string TrainingCentreAddress { get; set; }
        public int ClassesInspectedCount { get; set; }
        public string CentreInchargeName { get; set; }
        public string CentreInchargeMob { get; set; }
        public string Parameter { get; set; }
        public string Compliance { get; set; }
        public string ObservatoryRemarks { get; set; }
        public string RecommendationRemarks { get; set; }
        public string PSDFCompliance { get; set; }
        public DateTime ExpectedStartDate { get; set; }



        public string LocationAccessSuitablity { get; set; }
        public string LocAccessSuitabilityValue { get; set; }
        public string LocAccessSuitabilityObservRemarks { get; set; }
        public string LocAccessSuitabilityRecomRemarks { get; set; }
        public string SecurityPremises { get; set; }
        public string SecurityPremValue { get; set; }
        public string SecurityPremObservRemarks { get; set; }
        public string SecurityPremRecomRemarks { get; set; }
        public string StructuralIntegrityCompliance { get; set; }
        public string StructIntegValue { get; set; }
        public string StructIntegObservRemarks { get; set; }
        public string StructIntegRecomRemarks { get; set; }
        public string CentreInchargeRoom { get; set; }
        public string CentreInchrgRoomValue { get; set; }
        public string CentreInchrgRoomObservRemarks { get; set; }
        public string CentreInchrgRoomRecomRemarks { get; set; }



    }}