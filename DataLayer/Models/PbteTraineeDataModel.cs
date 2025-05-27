using System;
using System.Collections.Generic;
using System.Text;

namespace DataLayer.Models
{
    public class PbteTraineeDataModel:ModelBase
    {
        public PbteTraineeDataModel()
        {
            
        }
        public int Batch { get; set; }
            public string CNIC { get; set; }
            public string CNICVerified { get; set; }
            public string CertificationAuthority { get; set; }
            public string ClassCode { get; set; }
            public string ClassEndDate { get; set; }
            public string ClassStartDate { get; set; }
            public string ClassStatus { get; set; }
            public string ContactNumber { get; set; }
            public int Duration { get; set; }
            public string Education { get; set; }
            public string FatherName { get; set; }
            public string Gender { get; set; }
            public string ResidenceDistrict { get; set; }
            public string ResidenceTehsil { get; set; }
            public string Scheme { get; set; }
            public string SchemeForPBTE { get; set; }
            public string TSP { get; set; }
            public string Trade { get; set; }
            public string TraineeID { get; set; }
            public string TraineeAddress { get; set; }
            public string TraineeName { get; set; }
            public string TraineeStatusName { get; set; }
            public string TrainingLocation { get; set; }
            public string TrainingDistrict { get; set; }

    }
}
