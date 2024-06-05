using System;
using System.Collections.Generic;
using System.Text;

namespace DataLayer.Models.SSP
{
    public class CertificateModel : ModelBase
    {

        public CertificateModel() { }
        public int UserID { get; set; }
        public int TrainingCertificationID { get; set; }

        public int TrainingLocationID { get; set; }
        public int RegistrationAuthority { get; set; }
        public string RegistrationStatus { get; set; }
        public string RegistrationCerNum { get; set; }
        public DateTime IssuanceDate { get; set; }
        public DateTime ExpiryDate { get; set; }
        public string RegistrationCerEvidence { get; set; }


    }
}
