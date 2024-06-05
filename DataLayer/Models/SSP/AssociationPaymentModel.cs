using System.Collections.Generic;

namespace DataLayer.Models.SSP
{
    public class AssociationPaymentModel : ModelBase
    {
        public int UserID { get; set; }
        public int NoOFClasses { get; set; }
        public int PerLocationAssociationFee { get; set; }
        public int TotalAssociationFee { get; set; }
        public List<AssociatedTradeLot> AssociatedTradeLot { get; set; }
    }

    

    public class AssociatedTradeLot
    {
  
        public int Tradelot { get; set; }
        public int TrainingLocation { get; set; }
        public int TspAssociationMasterID { get; set; }
        public string TrainingLocationName { get; set; }
        public string EvaluationStatus { get; set; }
        public string IsChecked { get; set; }
        public int NoOfClass { get; set; }
        public string TradeLotTitle { get; set; }
    }
}
