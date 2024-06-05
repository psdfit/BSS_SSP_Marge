using System;
using System.Collections.Generic;
using System.Text;

namespace DataLayer.Models.SSP
{
    public class TrainerProfileModel : ModelBase
    {

        public TrainerProfileModel() { }
        public int UserID { get; set; }
        public int TrainerID { get; set; }

        public string TrainerName { get; set; }
        public string TrainerMobile { get; set; }
        public string TrainerEmail { get; set; }
        public int Gender { get; set; }
        public string TrainerCNIC { get; set; }
        public string CnicFrontPhoto { get; set; }
        public string CnicBackPhoto { get; set; }
        public string Qualification { get; set; }
        public string QualEvidence { get; set; }
        public string TrainerCV { get; set; }

        public List<TrainerProfileDetailModel> trainerDetails { get; set; }



    }
}
