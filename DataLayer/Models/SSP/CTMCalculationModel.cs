using System;
using System.Collections.Generic;
using System.Text;

namespace DataLayer.Models.SSP
{
    public class CTMCalculationModel : ModelBase
    {
        public CTMCalculationModel() { }


        public DateTime? ContractAwardStartDate { get; set; }= null;
        public DateTime? ContractAwardEndDate { get; set; }= null;
            public string? FundingSource { get; set; }= null; 
            public string? FundingCategory { get; set; }= null; 
            public string? SchemeType { get; set; }= null; 
            public string? Sector { get; set; }= null; 
            public string? Trade { get; set; }= null; 
            public string? Duration { get; set; }= "0"; 
            public string? Cluster { get; set; }= null; 
            public string? District { get; set; }= null; 
        
    }
}
