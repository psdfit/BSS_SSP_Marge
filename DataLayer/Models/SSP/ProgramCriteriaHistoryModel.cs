using System;
using System.Collections.Generic;
using System.Text;

namespace DataLayer.Models.SSP
{
    public class ProgramCriteriaHistoryModel:ModelBase
    {
        public int ID { get; set; }
        public int ProgramID { get; set; }
        public int CriteriaID { get; set; }
        public DateTime StartDate { get; set; } 
        public DateTime EndDate { get; set; }  
        public string Remarks { get; set; }
        public int UserID { get; set; }
    }
}
