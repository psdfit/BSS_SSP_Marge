using System;
using System.Collections.Generic;
using System.Text;

namespace DataLayer.Models
{
    public class InfrastructureSaveModel
    {
        public List<InfrastructureModel> InfrastructureArray { get; set; }
    }
    public class InfrastructureModel
    {

        public string Stream { get; set; }
        public string Scheme { get; set; }
        public string TrainingServiceProvider { get; set; }
        public string Trade { get; set; }
        public string Building { get; set; }
        public string Furniture { get; set; }
        public string BackupSourceOfElectricity { get; set; }
        public string ToolsAndequipment { get; set; }
        public string TotalAm { get; set; }
        public string ScoreOnScaleOf5 { get; set; }
        
    }
}
