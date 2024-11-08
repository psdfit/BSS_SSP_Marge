using System;
using System.Collections.Generic;
using System.Text;

namespace DataLayer.Models
{
    public class CheckRegistrationCriteriaModel
    {
        public string ErrorMessage { get; set; }
        public int ErrorTypeID { get; set; }
        public string ErrorTypeName { get; set; }
        public int TSPCapacity { get; set; }
        public int TradeCapicity { get; set; }
        public int EnrolledTraineesTSP { get; set; }
        
    }
}
