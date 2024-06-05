using DataLayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DataLayer.Models
{
    public class UnverifiedTraineeModel
    {
        public Table1 ClassDetail { get; set; }
        public List<Table2> UserDetails { get; set; }
    }
    public class Table1
    {
        public string ServiceProviderName { get; set; }
        public string TrainingScheme { get; set; }
        public string TrainingCentreAddress { get; set; }
        public string DistrictofTrainingLocation { get; set; }
        public string ClassCode { get; set; }
        public string Trade { get; set; }
    }
    public class Table2
    {
        public string TraineeName { get; set; }
        public string FathersName { get; set; }
        public string CNIC { get; set; }
        public string CNICVerified { get; set; }
        public string Remarks { get; set; }
    }
}
