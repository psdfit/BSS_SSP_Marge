using System;
using System.Collections.Generic;
using System.Text;

namespace DataLayer.Models.SSP
{
    public class TrainerProfileDetailModel : ModelBase
    {

        public TrainerProfileDetailModel() { }
        public int UserID { get; set; }
        public int TrainerDetailID { get; set; }
        public int TrainerProfileID { get; set; }
        public int TrainerTradeID { get; set; }
        public string ProfQualification { get; set; }
        public string CertificateBody { get; set; }
        public string ProfQualEvidence { get; set; }
        public float RelExpYear { get; set; }
        public string RelExpLetter { get; set; }


    }
}
