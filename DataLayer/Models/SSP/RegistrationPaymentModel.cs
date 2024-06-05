using System.Collections.Generic;

namespace DataLayer.Models.SSP
{
    public class RegistrationPaymentModel : ModelBase
    {
        public int UserID { get; set; }
        public int NumberOfLocations { get; set; }
        public int PerLocationRegistrationFee { get; set; }
        public int TotalRegistrationFee { get; set; }
        public List<RegisteredLocation> RegisteredLocations { get; set; }
    }

    public class RegisteredLocation
    {
        public int TrainingLocationID { get; set; }
        public string TrainingLocation { get; set; }
        public string TrainingLocationAddress { get; set; }
        public int TotalTrade { get; set; }
    }
}
