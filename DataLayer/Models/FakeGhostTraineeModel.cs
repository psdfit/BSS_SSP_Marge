using System;
using System.Collections.Generic;
using System.Text;

namespace DataLayer.Models
{
    [Serializable]
    public class FakeGhostTraineeModel: ModelBase
    {

        public FakeGhostTraineeModel() : base()
        {
        }

        public string DetailString { get; set; }
        public string SchemeName { get; set; }
        public string TSPName { get; set; }
        public string ClassCode { get; set; }
        public string Name { get; set; }
        public string FatherName { get; set; }
        public string Cnic { get; set; }
        public string PreviousMonth { get; set; }
        public string SelectedMonth { get; set; }
        public string Visit1SelectMonth { get; set; }
        public string Visit2SelectMonth { get; set; }
        public string Visit1PreviousMonth { get; set; }
        public string Visit2PreviousMonth { get; set; }
    }
}
