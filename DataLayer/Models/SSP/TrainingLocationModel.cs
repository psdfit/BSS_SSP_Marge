using System;
using System.Collections.Generic;
using System.Text;

namespace DataLayer.Models.SSP
{
    public class TrainingLocationModel : ModelBase
    {

        public TrainingLocationModel() { }
        public int UserID { get; set; }
        public int TrainingLocationID { get; set; }
        public string TrainingLocationName { get; set; }
        public string TrainingLocationAddress { get; set; }
        public string GeoTagging { get; set; }
        public int RegistrationAuthority { get; set; }
        public int Province { get; set; }
        public int Cluster { get; set; }
        public int District { get; set; }
        public int Tehsil { get; set; }
        public string FrontMainEntrancePhoto { get; set; }
        public string ClassroomPhoto { get; set; }
        public string ComputerLabPhoto { get; set; }
        public string PracticalAreaPhoto { get; set; }
        public string ToolsAndEquipmentsPhoto { get; set; }

    }
}
