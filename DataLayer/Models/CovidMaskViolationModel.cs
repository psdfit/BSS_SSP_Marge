using System;
using System.Collections.Generic;
using System.Text;

namespace DataLayer.Models
{
    [Serializable]
    public class CovidMaskViolationModel : ModelBase
    {

        public CovidMaskViolationModel() : base()
        {
        }

        public string DetailString { get; set; }
		public string SchemeName { get; set; }
        public string TSPName { get; set; }
        public string ClassCode { get; set; }
        public string Name { get; set; }
        public string FatherName { get; set; }
        public string Cnic { get; set; }
        public int Duration { get; set; }
        public string TraineeDoesNotWearMask { get; set; }
        public string Action { get; set; }
    }
}
